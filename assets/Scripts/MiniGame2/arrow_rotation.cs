using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class arrow_rotation : MonoBehaviour {

	public Button arrow;
	public GameObject player;
	Renderer rd;

	// Use this for initialization
	void Start () {
		arrow.GetComponentInChildren<Text> ().text = "Move";
		arrow.GetComponent<Image> ().color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetAxis("Horizontal") == -1){
			arrow.transform.Rotate(new Vector3(0, 0, Time.deltaTime*35));
		}
		if ( Input.GetAxis("Horizontal") == 1){
			arrow.transform.Rotate(new Vector3(0, 0, -Time.deltaTime*35));
		}
		//float Angle = Quaternion.Angle(Quaternion.Euler(new Vector3(0,0,360)), arrow.transform.rotation);
		float otherangle = arrow.transform.eulerAngles.z;
		//Debug.Log (otherangle);

		if (otherangle >= 280 && otherangle <= 350) {
			arrow.GetComponentInChildren<Text> ().text = "Go right";
			//Debug.Log ("right");
			rightmovement();
			OnPress ();
			OnRelease ();
		}
		if (otherangle >= 190 && otherangle <= 260) {
			arrow.GetComponentInChildren<Text> ().text = "Go down";
			//Debug.Log ("down");
			downmovement ();
			OnPress ();
			OnRelease ();
		}

		if (otherangle >= 10 && otherangle <= 80) {
			arrow.GetComponentInChildren<Text> ().text = "Go up";
			//Debug.Log ("up");
			upmovement();
			OnPress ();
			OnRelease ();
		}

		if (otherangle >= 100 && otherangle <= 170) {
			arrow.GetComponentInChildren<Text> ().text = "Go left";
			//Debug.Log ("left");
			leftmovement ();
			OnPress ();
			OnRelease ();
		}


	}

	void rightmovement(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			player.transform.position += Vector3.right;
		}
	}

	void downmovement(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			player.transform.position += Vector3.back;
		}
	}

	void leftmovement(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			player.transform.position += Vector3.left;
		}
	}

	void upmovement(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			player.transform.position += Vector3.forward;
		}
	}

	void OnPress()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			arrow.GetComponent<Image>().color = Color.red;
		}
	}
	void OnRelease()
	{
		if (Input.GetKeyUp (KeyCode.Space)) {
			arrow.GetComponent<Image> ().color = Color.green;
		} 
	}
}
