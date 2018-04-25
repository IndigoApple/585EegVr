using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SharpBCI;

// At this point this is MUCH more than just the ball controller...

public class BallController : MonoBehaviour
{

    public GameObject timerObj;
    private Rigidbody rb;
    public float speed;

    public static int UP_ID = 1;
    public static int DOWN_ID = 2;

    public float trainingTime = 30;
    public bool eeg = true;
    public float timerTime = 10;
    private float timer;
    private float time;

    public Text upText;
    public Text downText;
    public Text score;
   

    public bool IsTraining { get { return isTraining; } }

    private bool isTraining;
    private bool trainingUp;
    private bool trainingDown;

    private float _upTrainedTime;
    private float _downTrainedTime;

    private float started;

    private bool downQueue;
    private bool upQueue;

    private Vector3 originalPos;

    private bool moving;

    // Use this for initialization
    void Start()
    {
        originalPos = gameObject.transform.position;
        rb = GetComponent<Rigidbody>();
        if (eeg)
        {
            SharpBCIController.BCI.ClearTrainingData();
            started = Time.time;
            isTraining = true;
        }
        else
        {
            StartCoroutine(ToggleText(upText, 0f));
            StartCoroutine(ToggleText(downText, 0f));
        }
    }

    private int x;
    private int y;
    // FixedUpdate is called once per time frame

    void Update()
    {
        if (timerObj.GetComponent<TimerController>().IsTiming)
        {
            rb.isKinematic = true;
        } else
        {
            rb.isKinematic = false;
        }
        if (isTraining)
        {
            UpdateTraining();
        }
    }

    IEnumerator ToggleText(Text text, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (text.enabled)   
        {
            text.enabled = false;
        }
        else
        {
            text.enabled = true;   
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            score.text = "It took " + timerObj.GetComponent<TimerController>().getTime() + " seconds";
            rb.isKinematic = true;
            Destroy(other.gameObject);
            //gameObject.transform.position = originalPos;
            Vector3 currentPos = gameObject.transform.position;
            gameObject.transform.position = originalPos;
            timerObj.GetComponent<TimerController>().reset();
        }
    }

    SharpBCI.MovingAverageFilter movementFilter = new SharpBCI.MovingAverageFilter(3);

    void FixedUpdate()
    {
        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");

        if (eeg)
        {
            if (downQueue)
            {
                var nextV = (float)movementFilter.Filter(speed);
                rb.AddForce(new Vector3(0, 0, nextV));
                downQueue = false;
            }
            else if (upQueue)
            {
                var nextV = (float)movementFilter.Filter(-speed);
                rb.AddForce(new Vector3(0, 0, nextV));
                upQueue = false;

            }
        }
        else
        {
            rb.AddForce(new Vector3(moveH, 0.0f, moveV) * speed);
        }
    }

    void UpdateTraining()
    {
        if (trainingUp)
        {
            _upTrainedTime += Time.deltaTime;
            upText.text = "Up Count: " + _upTrainedTime.ToString();

        }
        else if (trainingDown)
        {
            _downTrainedTime += Time.deltaTime;
            downText.text = "Down Count: " + _downTrainedTime.ToString();
        }

        if (isTraining && _upTrainedTime >= trainingTime && _downTrainedTime >= trainingTime)
        {
            Debug.Log("Done Training");
            if (trainingDown)
                SharpBCIController.BCI.StopTraining(DOWN_ID);
            if (trainingUp)
                SharpBCIController.BCI.StopTraining(UP_ID);

            SharpBCIController.BCI.AddTrainedHandler(DOWN_ID, OnTrainedEvent);
            SharpBCIController.BCI.AddTrainedHandler(UP_ID, OnTrainedEvent);
            isTraining = false;

            SharpBCIController.BCI.AddTrainedHandler(DOWN_ID, OnTrainedEvent);
            SharpBCIController.BCI.AddTrainedHandler(UP_ID, OnTrainedEvent);
        }


        if (Input.GetAxis("Vertical") < 0)
        {
            if (trainingUp)
            {
                SharpBCIController.BCI.StopTraining(UP_ID);
                trainingUp = false;
            }
            if (!trainingDown)
            {
                SharpBCIController.BCI.StartTraining(DOWN_ID);
                trainingDown = true;
            }

        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            if (trainingDown)
            {
                SharpBCIController.BCI.StopTraining(DOWN_ID);
                trainingDown = false;
            }
            else if (!trainingUp)
            {
                SharpBCIController.BCI.StartTraining(UP_ID);
                trainingUp = true;
            }
        }
        else
        {
            if (trainingDown)
            {
                SharpBCIController.BCI.StopTraining(DOWN_ID);
                trainingDown = false;
            }
            else if (trainingUp)
            {
                SharpBCIController.BCI.StopTraining(UP_ID);
                trainingUp = false;
            }

        }

    }


    void OnTrainedEvent(TrainedEvent evt)
    {
        downQueue = evt.id == DOWN_ID;
        upQueue = evt.id == UP_ID;

    }
}
