using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowOption : MonoBehaviour {

	Text optionText;
	public GameObject playerObject;

	// Use this for initialization
	void Start () {
		optionText = GetComponent<Text>();
	}

	// Update is called once per frame
	public void textUpdate (float value) {
		if (value == 0) {
			optionText.text = "Select";
		} else if (value == +1) { // right should be positive
			optionText.text = "Go Right";
		} else if (value == -1) { // left should be negative
			optionText.text = "Go Left";
		} 
		if ((value != 0) && (playerObject != null)) {
//			playerObject.transform.position.x += value;
			Vector3 temp = playerObject.transform.position;
			temp.x += value;
			playerObject.transform.position = temp;
		}
	}
}
