using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class arrow_rotation : MonoBehaviour {

	private enum ControlMode
	{
		Keys,
		EEG,
	}

	private enum GameMode
	{
		MiniGame2,
		Level1,
	}

	[SerializeField] private ControlMode m_controlMode = ControlMode.Keys;
	[SerializeField] private GameMode m_gameMode = GameMode.MiniGame2;
	public Button arrow;
	public GameObject player;
	public int direction = 0;
	Renderer rd;
	private float otherangle;
	private bool blinked;
	private bool blinked2;
	private float timer = 0;

	// Use this for initialization
	void Start () {
		arrow.GetComponentInChildren<Text> ().text = "Move";
		arrow.GetComponent<Image> ().color = Color.green;
	}
	
	// Update is called once per frame
	void Update() {
		switch(m_controlMode)
		{
		case ControlMode.Keys:
			KeyUpdate();
			break;

		case ControlMode.EEG:
			EEGUpdate ();
			if (blinked)
				timer += Time.deltaTime;
			break;

		default:
			Debug.LogError("Unsupported state");
			break;
		}
	}

	void KeyUpdate () {
		if ( Input.GetAxis("Horizontal") == -1){
			arrow.transform.Rotate(new Vector3(0, 0, Time.deltaTime*50));

		}
		if ( Input.GetAxis("Horizontal") == 1){
			arrow.transform.Rotate(new Vector3(0, 0, -Time.deltaTime*50));
		}
		//float Angle = Quaternion.Angle(Quaternion.Euler(new Vector3(0,0,360)), arrow.transform.rotation);
		otherangle = arrow.transform.eulerAngles.z;
		//Debug.Log (otherangle);

		if (otherangle > 225 && otherangle <= 325) {
			arrow.GetComponentInChildren<Text> ().text = "Go right";
			//Debug.Log ("right");
			if (Input.GetKeyDown (KeyCode.Space)) 
				movement(2);
		}
		if (otherangle > 135 && otherangle <= 225) {
			arrow.GetComponentInChildren<Text> ().text = "Go down";
			//Debug.Log ("down");
			if (Input.GetKeyDown (KeyCode.Space))
				movement(4);
		}

		if (otherangle >= 0 && otherangle <= 45 || otherangle <= 360 && otherangle > 325) {
			arrow.GetComponentInChildren<Text> ().text = "Go up";
			//Debug.Log ("up");
			if (Input.GetKeyDown (KeyCode.Space))
				movement(3);
		}

		if (otherangle > 45 && otherangle <= 135) {
			arrow.GetComponentInChildren<Text> ().text = "Go left";
			//Debug.Log ("left");
			if (Input.GetKeyDown (KeyCode.Space))
				movement(1);
		}
		OnPress ();
		OnRelease ();
	}

	void EEGUpdate() {
		arrow.transform.Rotate (new Vector3 (0, 0, direction * Time.deltaTime * 50));
		otherangle = arrow.transform.eulerAngles.z;

		int i = 0;
		if (otherangle > 225 && otherangle <= 325) {
			arrow.GetComponentInChildren<Text> ().text = "Go right";
			i = 2;
		}
		if (otherangle > 135 && otherangle <= 225) {
			arrow.GetComponentInChildren<Text> ().text = "Go down";
			i = 4;
		}

		if (otherangle >= 0 && otherangle <= 45 || otherangle <= 360 && otherangle > 325) {
			arrow.GetComponentInChildren<Text> ().text = "Go up";
			i = 3;
		}

		if (otherangle > 45 && otherangle <= 135) {
			arrow.GetComponentInChildren<Text> ().text = "Go left";
			i = 1;
		}
		if (blinked) {
			movement (i);
		}
	}

	private void movement(int i){
		switch (m_gameMode) {
			case GameMode.MiniGame2:
				if (blinked2) {
					switch (i) {
					case 1:
						player.transform.position += Vector3.left;
						break;
					case 2:
						player.transform.position += Vector3.right;
						break;
					case 3:
						player.transform.position += Vector3.forward;
						break;
					case 4:
						player.transform.position += Vector3.back;
						break;
					}
					blinked = false;
					blinked2 = false;
					timer = 0;
				}
				break;
			case GameMode.Level1:
				if (player.GetComponent<CharacterControl> ().move != 1 || blinked2) {
					player.GetComponent<CharacterControl> ().GridMove (i);
					blinked = false;
					blinked2 = false;
					timer = 0;
				}
				break;
			default:
				Debug.Log ("Error game mode");
				break;
		}
	}

	public void Blinked() {
		if (blinked) {
			if (timer < .5)
				blinked2 = true;
			else {
				blinked = false;
				timer = 0;
			}
		} else
			blinked = true;
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
