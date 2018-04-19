using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SharpBCI;

public class Trainer : MonoBehaviour
{

    public static int UP_ID = 1;
    public static int DOWN_ID = 2;

    private bool downQueue;
    private bool upQueue;
    public GameObject EEG;

    private float started;

    private bool isTraining;
    private bool trainingUp;
    private bool trainingDown;

    private float _upTrainedTime;
    private float _downTrainedTime;

    public float trainingTime = 2;
    public float timerTime = 10;

    public bool IsTraining { get { return isTraining; } }

    // Use this for initialization
    void Start()
    {
        SharpBCIController.BCI.ClearTrainingData();
        started = Time.time;
        isTraining = true;
        trainingUp = false;
        trainingDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTraining)
        {
            UpdateTraining();
        }
    }

    void UpdateTraining()
    {
        if (trainingUp)
        {
            _upTrainedTime += Time.deltaTime;
            Debug.Log("up trained:" + _upTrainedTime.ToString());
        }
        else if (trainingDown)
        {
            _downTrainedTime += Time.deltaTime;
            Debug.Log("down trained:" + _downTrainedTime.ToString());
        }

        if ((isTraining && (_upTrainedTime >= trainingTime)) && (_downTrainedTime >= trainingTime))
        {
            Debug.Log("Done Training");
            if (trainingDown)
                SharpBCIController.BCI.StopTraining(DOWN_ID);
            if (trainingUp)
                SharpBCIController.BCI.StopTraining(UP_ID);

            Debug.Log("Added Trained Handlers");
            isTraining = false;
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
}

