using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementController : MonoBehaviour {

	private enum ControlMode
	{
		Keys,
		EEG,
	}
	[SerializeField] private ControlMode m_controlMode = ControlMode.Keys;
    public Button[] buttons = new Button[4];
    public GameObject player;
    public float timeTaken = 0.1f;
    public float distance = 1f;
	public float direction = 1;
	public bool move = false;

    private bool _isLerping;
    private Vector3 _startPos;
    private Vector3 _endPos;
    private float _timeStarted;

    private float bpos = 4.0f;
    private int current = 0;

    private int hAx;
    private int vAx;
	// Use this for initialization
	void Start () {
		for (int i = 0; i<buttons.Length; i++)
        {
            Button btn = buttons[i];
            ButtonController btc = btn.GetComponent<ButtonController>();
            btn.onClick.AddListener(() => notify(btc.id));
        }
    }

    void Update()
	{
		switch(m_controlMode)
		{
		case ControlMode.Keys:
			KeyUpdate();
			break;

		case ControlMode.EEG:
			EEGUpdate();
			break;

		default:
			Debug.LogError("Unsupported state");
			break;
		}
	}

	void EEGUpdate() {
		if (move)
		{
			//Debug.Log("H: " + Input.GetAxis("Horizontal").ToString() + "; V: " + Input.GetAxis("Vertical").ToSring());
			notify(current);
		}
		else
		{
			//Debug.Log("H: " + Input.GetAxis("Horizontal").ToString() + "; V: " + Input.GetAxis("Vertical").ToSring());
			bpos += (Input.GetAxis("Horizontal") * Time.deltaTime);
			if (bpos < 0)
				bpos += 4;
			int btn = (int)((bpos) % 4);
			if (!(current == btn))
			{
				buttons[current].image.color = Color.white;
				current = btn;
				Debug.Log(current);
			}
		}
		buttons[current].image.color = new Color(1 - Mathf.Sin(6 * bpos), 1f, 1 - Mathf.Sin(6 * bpos), 1f);
	}

	void KeyUpdate() {
		if (Input.GetAxis("Vertical") != 0)
		{
			//Debug.Log("H: " + Input.GetAxis("Horizontal").ToString() + "; V: " + Input.GetAxis("Vertical").ToSring());
			notify(current);
		}
        else if (Input.GetAxis("Horizontal") == -1 || Input.GetAxis("Horizontal") == 1)
        {
            //Debug.Log("H: " + Input.GetAxis("Horizontal").ToString() + "; V: " + Input.GetAxis("Vertical").ToSring());
            bpos += (Input.GetAxis("Horizontal") * Time.deltaTime);
			if (bpos < 0)
				bpos += 4;
			int btn = (int)((bpos) % 4);
            if (!(current == btn))
            {
                buttons[current].image.color = Color.white;
                current = btn;
                Debug.Log(current);
            }
        }
        buttons[current].image.color = new Color(1 - Mathf.Sin(6 * bpos), 1f, 1 - Mathf.Sin(6 * bpos), 1f); 
    }

    void StartLerping(Vector3 dir)
    {
        _isLerping = true;
        _timeStarted = Time.time;
        _startPos = player.transform.position;
        _endPos = player.transform.position + dir * distance;
    }

    protected void notify(int id)
    {
        switch (id)
        {
            case 0:
                StartLerping(Vector3.forward);
                break;
            case 1:
                StartLerping(Vector3.right);
                break;
            case 2:
                StartLerping(Vector3.back);
                break;
            case 3:
                StartLerping(Vector3.left);
                break;
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
		if (_isLerping)
        {
            float timeSinceStart = Time.time - _timeStarted;
            float percentageDone = timeSinceStart / timeTaken;

            player.transform.position = Vector3.Lerp(_startPos, _endPos, percentageDone);

            if(percentageDone >= 1.0f)
            {
                _isLerping = false;
            }
        }
	}
}
