using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SharpBCI;

public class EEGToSlider : MonoBehaviour {

	public static int UP_ID = 1;
	public static int DOWN_ID = 2;

	public Slider slider;

	private bool downQueue;
	private bool upQueue;

	// Use this for initialization
	void Start () {
		//add trained events so SharpBCI will call TrainedEvent
		SharpBCIController.BCI.AddTrainedHandler(DOWN_ID, TrainedEvent);
		SharpBCIController.BCI.AddTrainedHandler(UP_ID, TrainedEvent);
	}
	
	// Update is called once per frame
	void Update () {
		if (downQueue) {
			slider.value = 1;
		}
		else if (upQueue) {
			slider.value = -1;
		}
	}

	void TrainedEvent(TrainedEvent evt)
	{
		downQueue = evt.id == DOWN_ID;
		upQueue = evt.id == UP_ID;

	}
}
