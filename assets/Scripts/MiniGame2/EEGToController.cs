using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SharpBCI;

public class EEGToController : MonoBehaviour {

	public static int UP_ID = 1;
	public static int DOWN_ID = 2;

	public GameObject arrow;

	private bool downQueue;
	private bool upQueue;
	public GameObject EEG;
	private arrow_rotation rotation;

    private float started;

    private bool isTraining;
    private bool trainingUp;
    private bool trainingDown;

    private float _upTrainedTime;
    private float _downTrainedTime;

    public float trainingTime = 2;
    public float timerTime = 10;

    public bool IsTraining { get { return isTraining; } }

    // Use this for initialization
    void Start () {
        //add trained events so SharpBCI will call TrainedEvent
        //SharpBCIController.BCI.AddTrainedHandler(DOWN_ID, TrainedEvent);
        //SharpBCIController.BCI.AddTrainedHandler(UP_ID, TrainedEvent);
        SharpBCIController.BCI.ClearTrainingData();
        started = Time.time;
        isTraining = true;
        trainingUp = false;
        trainingDown = false;
		EEG.GetComponent<SharpBCIController> ().addBlinkHandler (BlinkEvent);
		rotation = arrow.GetComponent<arrow_rotation>();
	}
	
	// Update is called once per frame
	void Update () {
        if (isTraining)
        {
            UpdateTraining();
        } 
		else if (downQueue) {
			arrow.GetComponent<arrow_rotation>().direction = -1;
		}
		else if (upQueue) {
			arrow.GetComponent<arrow_rotation>().direction = 1;
		}
	}

    void UpdateTraining()
    {
        if (trainingUp)
        {
            _upTrainedTime += Time.deltaTime;
            Debug.Log("up trained:" + _upTrainedTime.ToString());
        }
        else if (trainingDown)
        {
            _downTrainedTime += Time.deltaTime;
            Debug.Log("down trained:" + _downTrainedTime.ToString());
        }

        if ((isTraining && _upTrainedTime >= trainingTime) && (_downTrainedTime >= trainingTime))
        {
            Debug.Log("Done Training");
            if (trainingDown)
                SharpBCIController.BCI.StopTraining(DOWN_ID);
            if (trainingUp)
                SharpBCIController.BCI.StopTraining(UP_ID);

            SharpBCIController.BCI.AddTrainedHandler(DOWN_ID, TrainedEvent);
            SharpBCIController.BCI.AddTrainedHandler(UP_ID, TrainedEvent);
            Debug.Log("Added Trained Handlers");
            isTraining = false;
        }


        if (Input.GetAxis("Vertical") < 0)
        {
            if (trainingUp)
            {
                SharpBCIController.BCI.StopTraining(UP_ID);
                trainingUp = false;
            }
            if (!trainingDown)
            {
                SharpBCIController.BCI.StartTraining(DOWN_ID);
                trainingDown = true;
            }
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            if (trainingDown)
            {
                SharpBCIController.BCI.StopTraining(DOWN_ID);
                trainingDown = false;
            }
            else if (!trainingUp)
            {
                SharpBCIController.BCI.StartTraining(UP_ID);
                trainingUp = true;
            }
        }
        else
        {
            if (trainingDown)
            {
                SharpBCIController.BCI.StopTraining(DOWN_ID);
                trainingDown = false;
            }
            else if (trainingUp)
            {
                SharpBCIController.BCI.StopTraining(UP_ID);
                trainingUp = false;
            }

        }

    }

    void TrainedEvent(TrainedEvent evt)
	{
        Debug.Log("trained event");
		downQueue = evt.id == DOWN_ID;
		upQueue = evt.id == UP_ID;
	}

	void BlinkEvent() {
		Debug.Log ("blink event called");
		rotation.Blinked ();
	}
}
