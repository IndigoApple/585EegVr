using SharpBCI;
using UnityEngine;
using System.Diagnostics;
using SharpOSC;
using System;
using System.Collections.Generic;
using System.Threading;


public enum SharpBCIControllerType {
	MUSE,
	TONE_GENERATOR,
	TWO_TONE_GENERATOR,
	CSV_READER
}

public class UnityLogger : ILogOutput {

	public void Dispose() {
		// no cleanup required
	}

	public void Log(LogLevel level, object message) {
		switch (level) {
			case LogLevel.INFO:
				UnityEngine.Debug.Log(message);
				break;
			case LogLevel.WARNING:
				UnityEngine.Debug.LogWarning(message);
				break;
			case LogLevel.ERROR:
				UnityEngine.Debug.LogError(message);
				break;
			default:
				return;
		}
	}
}

public class SharpBCIController : MonoBehaviour {


	public const int OSC_DATA_PORT = 5000;
	public const string LOG_NAME = "SharpBCI_log.txt";

	public static SharpBCI.SharpBCI BCI;
	public static EEGDeviceAdapter adapter;

	public SharpBCIControllerType bciType;

	public EEGDataType dataType;

	public string CSVReadFilePath;

	static SharpBCIController _inst;

	Process museIOProcess;

	bool isTheHighlander = false;

    public void addBlinkHandler(BlinkAdapter.blinkHandler bh)
    {
        if (adapter != null)
        {
            ((BlinkAdapter) adapter).addHandler(bh);
        }
    }

	// Use this for initialization
	void Awake() {
		if (_inst != null) {
			Destroy (gameObject);
			return;
		}
		_inst = this;
		DontDestroyOnLoad (gameObject);
		isTheHighlander = true;

		// FileLogger requires actual pathnames not Unity
		string logName = System.IO.Path.Combine(Application.persistentDataPath.Replace('/', System.IO.Path.DirectorySeparatorChar), LOG_NAME);
		UnityEngine.Debug.Log("Writing sharpBCI log to: " + logName);
		// configure logging
		SharpBCI.Logger.AddLogOutput (new UnityLogger ());
		//SharpBCI.Logger.AddLogOutput(new FileLogger(logName));

		//EEGDeviceAdapter adapter;
		if (bciType == SharpBCIControllerType.MUSE) {
			// start Muse-IO
			try {
				museIOProcess = new Process ();
				museIOProcess.StartInfo.FileName = System.IO.Path.Combine (Application.streamingAssetsPath, "MuseIO", "muse-io");
				UnityEngine.Debug.Log("filename: " + museIOProcess.StartInfo.FileName);
				// default is osc.tcp://localhost:5000, but we expect udp
				museIOProcess.StartInfo.Arguments = "--osc osc.udp://localhost:5000 --device Muse-4B45";
				museIOProcess.StartInfo.CreateNoWindow = true;
				museIOProcess.StartInfo.UseShellExecute = false;
				museIOProcess.Start ();
				museIOProcess.PriorityClass = ProcessPriorityClass.RealTime;
			} catch (System.Exception e) {
				UnityEngine.Debug.LogError ("Could not open muse-io:");
				UnityEngine.Debug.LogException (e);
			}

            //adapter = new RemoteOSCAdapter(OSC_DATA_PORT);
            adapter = new BlinkAdapter(OSC_DATA_PORT);
        } else if (bciType == SharpBCIControllerType.TONE_GENERATOR) {
			adapter = new DummyAdapter (new DummyAdapterSignal (new double[] { 
				// alpha
				10, 
				// beta
				24, 
				// gamma
				40, 
				// delta
				2, 
				// theta
				6,
				// simulate AC interference
				60,
			}, new double[] {
				512,
				512,
				512,
				512,
				512,
				512
			}), 220, 2);
		} else if (bciType == SharpBCIControllerType.TWO_TONE_GENERATOR) {
			var signals = new DummyAdapterSignal[] { 
				new DummyAdapterSignal (new double[] { 
					// alpha
					10, 
					// beta
					24, 
					// gamma
					40, 
					// delta
					2, 
					// theta
					6,
				}, new double[] {
					512,
					0,
					0,
					0,
					0
				}),
				new DummyAdapterSignal (new double[] { 
					// alpha
					10, 
					// beta
					24, 
					// gamma
					40, 
					// delta
					2, 
					// theta
					6,
				}, new double[] {
					0,
					512,
					0,
					0,
					0
				})
			};
			adapter = new InstrumentedDummyAdapter (signals, 220, 2);
		} else if (bciType == SharpBCIControllerType.CSV_READER) {
			double sampleRate = 220;
			adapter = new CSVReadAdapter (CSVReadFilePath, sampleRate);
		} else {
			throw new System.Exception("Invalid bciType");
		}

		BCI = new SharpBCIBuilder()
			.EEGDeviceAdapter(adapter)
			.PipelineFile(System.IO.Path.Combine(Application.streamingAssetsPath, "default_pipeline.json"))
			.Build();

		if (bciType != SharpBCIControllerType.CSV_READER) {
			BCI.LogRawData(dataType);
		}
	}

	void OnDestroy() {
		if (!isTheHighlander)
			return;
		
		if (bciType == SharpBCIControllerType.MUSE) {
			if (museIOProcess != null && !museIOProcess.HasExited) {
				museIOProcess.Kill();
				museIOProcess.WaitForExit();
			}
		}
			
		BCI.Close();
		SharpBCI.Logger.Dispose();
	}
}

public class BlinkAdapter : EEGDeviceAdapter
{

    /**
     * How long the RemoteOSCAdapter waits until it assumes the device has hung up
     */
    public const int HANGUP_TIME = 1000;
    public delegate void blinkHandler();

    private blinkHandler bh;
    private bool handled = false;

    public void addHandler(blinkHandler handler)
    {
        this.bh = handler;
        handled = true;
    }

    /**
     * Port number that OSC packets can be retrieved from
     */
    int port;
    UDPListener listener;
    Dictionary<string, EEGDataType> typeMap;
    readonly Converter<object, double> converter = new Converter<object, double>(delegate (object inAdd)
    {
        // TODO fix this horrendous kludge
        return Double.Parse(inAdd.ToString());
    });

    Thread listenerThread;
    bool stopRequested;
    DateTime lastPacketRecieved = DateTime.UtcNow;

    public BlinkAdapter(int port) : base(4, 220)
    {
        this.port = port;
    }

    /**
     * Starts the listener for OSC packets on the specified port number and starts the Run function in a new thread
     */
    public override void Start()
    {
        SharpBCI.Logger.Log("Starting RemoteOSCAdapter");
        typeMap = InitTypeMap();
        listener = new UDPListener(port);
        listenerThread = new Thread(new ThreadStart(Run));
        listenerThread.Start();
    }

    /**
     * Stops the listener and stops triggers the stop of the Run function
     */
    public override void Stop()
    {
        SharpBCI.Logger.Log("Stopping RemoteOSCAdapter");
        stopRequested = true;
        listenerThread.Join();
        listener.Dispose();
    }

    Dictionary<string, EEGDataType> InitTypeMap()
    {
        Dictionary<string, EEGDataType> typeMap = new Dictionary<string, EEGDataType>();

        // raw EEG data
        typeMap.Add("/muse/eeg", EEGDataType.EEG);
        //typeMap.Add("/muse/eeg/quantization", EEGDataType.QUANTIZATION);

        // absolute power bands
        typeMap.Add("/muse/elements/alpha_absolute", EEGDataType.ALPHA_ABSOLUTE);
        typeMap.Add("/muse/elements/beta_absolute", EEGDataType.BETA_ABSOLUTE);
        typeMap.Add("/muse/elements/gamma_absolute", EEGDataType.GAMMA_ABSOLUTE);
        typeMap.Add("/muse/elements/delta_absolute", EEGDataType.DELTA_ABSOLUTE);
        typeMap.Add("/muse/elements/theta_absolute", EEGDataType.THETA_ABSOLUTE);

        // relative power bands
        typeMap.Add("/muse/elements/alpha_relative", EEGDataType.ALPHA_RELATIVE);
        typeMap.Add("/muse/elements/beta_relative", EEGDataType.BETA_RELATIVE);
        typeMap.Add("/muse/elements/gamma_relative", EEGDataType.GAMMA_RELATIVE);
        typeMap.Add("/muse/elements/delta_relative", EEGDataType.DELTA_RELATIVE);
        typeMap.Add("/muse/elements/theta_relative", EEGDataType.THETA_RELATIVE);

        // session scores
        //typeMap.Add(EEGDataType.ALPHA_SCORE, "/muse/elements/alpha_session_score");
        //typeMap.Add(EEGDataType.BETA_SCORE, "/muse/elements/beta_session_score");
        //typeMap.Add(EEGDataType.GAMMA_SCORE, "/muse/elements/gamma_session_score");
        //typeMap.Add(EEGDataType.DELTA_SCORE, "/muse/elements/delta_session_score");
        //typeMap.Add(EEGDataType.THETA_SCORE, "/muse/elements/theta_session_score");

        // headband status
        typeMap.Add("/muse/elements/horseshoe", EEGDataType.CONTACT_QUALITY);

        // DRL-REF
        // typeMap.Add(EEGDataType.DRL_REF, "/muse/drlref");

        return typeMap;
    }

    void Run()
    {
        while (!stopRequested)
        {
            var packet = listener.Receive();
            if (packet != null)
            {
                lastPacketRecieved = DateTime.UtcNow;
                OnOSCMessageReceived(packet);
            }

            if (DateTime.UtcNow.Subtract(lastPacketRecieved).TotalMilliseconds > HANGUP_TIME)
            {
                lastPacketRecieved = DateTime.UtcNow;
                EmitData(
                    new EEGEvent(
                        DateTime.UtcNow,
                        EEGDataType.CONTACT_QUALITY,
                        new double[] { 4, 4, 4, 4 }
                    )
                );
            }
        }
    }

    void OnOSCMessageReceived(OscPacket packet)
    {
        var msg = (OscMessage)packet;
        if (msg.Address == "/muse/elements/blink")
        {
            if (((int)msg.Arguments[0]) == 1)
            {
                UnityEngine.Debug.Log(msg.Address.ToString() + " blinked");
                if (handled)
                {
					bh();
                } else
                {
                    UnityEngine.Debug.Log("No Blink Handler Added Yet");
                }
            }
        }
        if (!typeMap.ContainsKey(msg.Address))
            return;

        //			Debug.Log("Got packet from: " + msg.Address);
        //			Debug.Log("Arguments: ");
        //			foreach (var a in msg.Arguments) {
        //				Debug.Log(a.ToString());
        //			}

        try
        {
            var data = msg.Arguments.ConvertAll<double>(converter).ToArray();
            var type = typeMap[msg.Address];

            //				Debug.Log("EEGType: " + type);
            //				Debug.Log("Converted Args: ");
            //				foreach (var d in data) {
            //					Debug.Log(d.ToString());
            //				}

            EmitData(new EEGEvent(DateTime.UtcNow, type, data));
        }
        catch (Exception e)
        {
            SharpBCI.Logger.Error("Could not convert/emit data from EEGDeviceAdapter: " + e);
        }
    }
}
