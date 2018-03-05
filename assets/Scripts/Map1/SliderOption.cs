using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderOption : MonoBehaviour {

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
			optionText.text = "Go Backwards";
		} else if (value == -1) { // left should be negative
			optionText.text = "Go Forwards";
		}
	}
}
