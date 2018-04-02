using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SharpBCI;

public class EEGToController : MonoBehaviour {

	public static int UP_ID = 1;
	public static int DOWN_ID = 2;

	public GameObject buttons;

	private bool downQueue;
	private bool upQueue;
	public GameObject EEG;

	// Use this for initialization
	void Start () {
		//add trained events so SharpBCI will call TrainedEvent
		SharpBCIController.BCI.AddTrainedHandler(DOWN_ID, TrainedEvent);
		SharpBCIController.BCI.AddTrainedHandler(UP_ID, TrainedEvent);
		EEG.GetComponent<SharpBCIController> ().addBlinkHandler (BlinkEvent);
	}
	
	// Update is called once per frame
	void Update () {
		if (downQueue) {
			buttons.GetComponent<MovementController>().direction = -1;
		}
		else if (upQueue) {
			buttons.GetComponent<MovementController>().direction = 1;
		}
	}

	void TrainedEvent(TrainedEvent evt)
	{
		downQueue = evt.id == DOWN_ID;
		upQueue = evt.id == UP_ID;

	}

	void BlinkEvent() {
		buttons.GetComponent<MovementController> ().move = true;
	}
}
