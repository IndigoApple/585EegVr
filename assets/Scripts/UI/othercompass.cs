using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class othercompass : MonoBehaviour {

	public Vector3 NorthDirection;
	public Transform player;
	public Quaternion inputDirection;
	//public Transform guy;
	public Transform player2;

	public RectTransform Northlayer;
	public RectTransform inputLayer;
	
	// Update is called once per frame
	void Update () {
		ChangeNorthDirection ();
		ChangeInputDirection ();
	}

	public void ChangeNorthDirection(){
		NorthDirection.z = player.eulerAngles.y;
		Northlayer.localEulerAngles = NorthDirection;
	}

	public void ChangeInputDirection(){
		

//		inputDirection = Quaternion.LookRotation(dir);
//		inputDirection.z = -inputDirection.y;
//		inputDirection.x = 0;
//		inputDirection.y = 0;

		inputLayer.localRotation = inputDirection * Quaternion.Euler (NorthDirection);

		if (Input.GetKeyDown (KeyCode.D)) {
			//player2.transform.position += Vector3.right;
			Vector3 dir = transform.position;
			inputDirection = Quaternion.LookRotation (new Vector3(0,0,1));
		}

		if (Input.GetKeyDown (KeyCode.A)) {
			//player2.transform.position += Vector3.left;
			Vector3 dir = transform.position;
			inputDirection = Quaternion.LookRotation (new Vector3(0,0,-1));
		}

		if (Input.GetKeyDown (KeyCode.W)) {
			//player2.transform.position += Vector3.forward;
			Vector3 dir = transform.position;
			inputDirection = Quaternion.LookRotation (Vector3.left);
			print (inputDirection.y);
			inputDirection.z = -inputDirection.y;
			inputDirection.x = 0;
			inputDirection.y = 0;
		}

		if (Input.GetKeyDown (KeyCode.S)) {
			//player2.transform.position += Vector3.back;
			Vector3 dir = transform.position;
			inputDirection = Quaternion.LookRotation (Vector3.right);
			inputDirection.z = -inputDirection.y;
			inputDirection.x = 0;
			inputDirection.y = 0;
		}
	}
}
