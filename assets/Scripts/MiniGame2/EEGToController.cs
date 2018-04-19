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
        SharpBCIController.BCI.AddTrainedHandler(DOWN_ID, TrainedEvent);
        SharpBCIController.BCI.AddTrainedHandler(UP_ID, TrainedEvent);
        EEG.GetComponent<SharpBCIController> ().addBlinkHandler (BlinkEvent);
		rotation = arrow.GetComponent<arrow_rotation>();
	}
	
	// Update is called once per frame
	void Update () {
		if (downQueue) {
			arrow.GetComponent<arrow_rotation>().direction = -1;
		}
		else if (upQueue) {
			arrow.GetComponent<arrow_rotation>().direction = 1;
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
