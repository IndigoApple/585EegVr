using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using SharpBCI;
		

public class BallController : MonoBehaviour {

	private Rigidbody rb;
	public float speed;

	public static int LEFT_ID = 1;
	public static int RIGHT_ID = 2;


	public float trainingTime = 30;

	public Text leftText;
	public Text rightText;

	public bool IsTraining { get { return isTraining; } }

	private bool isTraining;
	private bool trainingLeft;
	private bool trainingRight;

	private float _leftTrainedTime;
	private float _rightTrainedTime;

	private float started;

	private bool rightQueued;
	private bool leftQueued;



	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		SharpBCIController.BCI.ClearTrainingData ();
		Debug.Log ("after clearing data");
		started = Time.time;
		isTraining = true;
	}

	private int x;
	private int y;
	// FixedUpdate is called once per time frame

	void Update() {
        if (isTraining) {
            UpdateTraining();
        }
	}

    //void FixedUpdate()
    //{
    //    x = x + 1;
    //    y = y + 1;
    //    float moveH = Input.GetAxis("Horizontal");
    //    float moveV = Input.GetAxis("Vertical");
    //    Debug.Log("Fixed Update Axis: " + Input.GetAxis("Horizontal").ToString());
    //    if (Input.GetAxis("Horizontal") < 0)
    //    {
    //        _leftTrainedTime += Time.deltaTime;
    //    }
    //    else if (Input.GetAxis("Horizontal") > 0)
    //    {
    //        _rightTrainedTime += Time.deltaTime;
    //    }

    //    leftText.text = "Left Count: " + _leftTrainedTime.ToString();
    //    rightText.text = "Right Count: " + _rightTrainedTime.ToString();

    //    //Vector3 movement = new Vector3 (x, y, z);
    //    Vector3 movement = new Vector3(moveH, 0, moveV);
    //    rb.AddForce(movement * speed);
    //}

    SharpBCI.MovingAverageFilter movementFilter = new SharpBCI.MovingAverageFilter(3);

    void FixedUpdate()
    {
        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");
        if (rightQueued)
        {
            var nextH = (float)movementFilter.Filter(speed);
            rb.AddForce(new Vector3(nextH, 0, moveV));
            rightQueued = false;
        }
        else if (leftQueued)
        {
            var nextH = (float)movementFilter.Filter(-speed);
            rb.AddForce(new Vector3(nextH, 0, moveV));
            leftQueued = false;

        }
    }

    void UpdateTraining()
    {
        if (trainingLeft)
        {
            _leftTrainedTime += Time.deltaTime;
            leftText.text = "Left Count: " + _leftTrainedTime.ToString();

        }
        else if (trainingRight)
        {
            _rightTrainedTime += Time.deltaTime;
            rightText.text = "Right Count: " + _rightTrainedTime.ToString();
        }

        if (isTraining && _rightTrainedTime >= trainingTime && _rightTrainedTime >= trainingTime)
        {
            Debug.Log("Done Training");
            if (trainingRight)
                SharpBCIController.BCI.StopTraining(RIGHT_ID);
            if (trainingLeft)
                SharpBCIController.BCI.StopTraining(LEFT_ID);

            SharpBCIController.BCI.AddTrainedHandler(RIGHT_ID, OnTrainedEvent);
            SharpBCIController.BCI.AddTrainedHandler(LEFT_ID, OnTrainedEvent);
            isTraining = false;

            SharpBCIController.BCI.AddTrainedHandler(RIGHT_ID, OnTrainedEvent);
            SharpBCIController.BCI.AddTrainedHandler(LEFT_ID, OnTrainedEvent);
        }


        if (Input.GetAxis("Horizontal") > 0)
        {
            if (trainingLeft)
            {
                SharpBCIController.BCI.StopTraining(LEFT_ID);
                trainingLeft = false;
            }
            if (!trainingRight)
            {
                SharpBCIController.BCI.StartTraining(RIGHT_ID);
                trainingRight = true;
            }

        } else if (Input.GetAxis("Horizontal") < 0)
        {
            if (trainingRight)
            {
                SharpBCIController.BCI.StopTraining(RIGHT_ID);
                trainingRight = false;
            }
            else if (!trainingLeft)
            {
                SharpBCIController.BCI.StartTraining(LEFT_ID);
                trainingLeft = true;
            }
        } else
        {
            if (trainingRight)
            {
                SharpBCIController.BCI.StopTraining(RIGHT_ID);
                trainingRight = false;
            } else if (trainingLeft)
            {
                SharpBCIController.BCI.StopTraining(LEFT_ID);
                trainingLeft = false;
            }

        }

    }


    void OnTrainedEvent(TrainedEvent evt)
    {
        rightQueued = evt.id == RIGHT_ID;
        leftQueued = evt.id == LEFT_ID;

    }
}
