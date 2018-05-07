using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterControl : MonoBehaviour {
	public GameObject Item;
	public GameObject Teleport;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<ParticleSystem>().Stop();
		InvokeRepeating("startEffects", 0,1);
	}
	
	// Update is called once per frame
	void startEffects () {
		Event events = Item.GetComponents<Event> ()[1];
		if (events.hasStarted())
		{
			gameObject.GetComponent<ParticleSystem>().Play();
		}
		if (Teleport.GetComponent<Event> ().hasStarted()) {
			Teleport.GetComponent<LoadLevel> ().ChangeScene ("Snow Scene");
		}
	}
}
