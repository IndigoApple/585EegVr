﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {

	//public GameObject obj;
	//public BallController trainer;

	// Use this for initialization
	void Start () {
		//trainer = obj.GetComponent<BallController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// changes scene, but only if training is over
	public void ChangeScene(string scenename) {
		//if (!trainer.IsTraining)
		SceneManager.LoadScene (scenename, LoadSceneMode.Single);
	}
}
