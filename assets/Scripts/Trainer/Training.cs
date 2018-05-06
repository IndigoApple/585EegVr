using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Training : MonoBehaviour {

    public GameObject arrow;
    public GameObject plane;
    private Trainer trainer;
    private stages stage;
    private enum stages
    {
        init,
        up,
        down,
        finish
    }

    // Use this for initialization
    void Start () {
        trainer = GetComponent<Trainer>();
        //plane.GetComponentInChildren<Text>().text = "Think clockwise and press the up key for " + GetComponent<Trainer>().trainingTime.ToString() + " seconds\n Total held time:" +GetComponent<Trainer>;
        stage = stages.init;
    }

   


    // Update is called once per frame
    void Update () {
        if (stage == stages.init)
        {
            plane.SetActive(false);
            if (Input.GetKeyDown("space"))
            {
                plane.SetActive(true);
                stage = stages.up;
            }
        }
        else if (stage == stages.up)
        {
            arrow.GetComponent<arrow_rotation>().direction = -1;
            if (trainer.upTrainedTime < trainer.trainingTime)
                plane.GetComponentInChildren<Text>().text = "Think clockwise and hold the up key for " + trainer.trainingTime.ToString() + " seconds" +
                    "\n\nTotal held time: " + trainer.upTrainedTime.ToString("0");
            else stage = stages.down;
        }
        else if (stage == stages.down)
        {
            arrow.GetComponent<arrow_rotation>().direction = 1;
            if (trainer.downTrainedTime < trainer.trainingTime)
                plane.GetComponentInChildren<Text>().text = "Great! Now think counter-clockwise and hold the down key for " + trainer.trainingTime.ToString() + " seconds. When you're done double-blink to move! " +
                    "\n\nTotal held time: " + trainer.downTrainedTime.ToString("0");
            else stage = stages.finish;
        }
        else if (stage == stages.finish)
        {
            arrow.GetComponent<arrow_rotation>().direction = 0;
            plane.SetActive(false);
            arrow.GetComponent<EEGToController>().Initialize();
        }
    }
}
