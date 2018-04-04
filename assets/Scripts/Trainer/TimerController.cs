using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimerController : MonoBehaviour {
    public Text timeRemaining;
    public bool IsTiming { get { return isTiming; } }

    public float timerTime = 10;

    private float timer;
    private float time;

    private bool isTiming;

    public static timerChangeHandler tch;

    private enum Events { stopped_timing };

    private IEnumerator coroutine;

    // Use this for initialization
    void Start() {
        isTiming = true;
        timer = timerTime;
        timeRemaining.text = timerTime.ToString();
    }

    // Update is called once per frame
    void Update() {
        if (isTiming)
        {
            timer -= Time.deltaTime;
            timeRemaining.text = "Start in " + ((int)timer).ToString() + " seconds";
            if (timer < 1)
            {
                //Debug.Log(timer.ToString() + " time");
                isTiming = false;
                timer = 0;
                timeRemaining.text = "Go!";
                notify((int)Events.stopped_timing);
                coroutine = ToggleText(timeRemaining, 2f);
                StartCoroutine(coroutine);
            }
        } else
        {
            timer += Time.deltaTime;
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

    public string getTime()
    {
        return ((int)timer).ToString();
    }

    public void reset()
    {
        //StartCoroutine(ToggleText(timeRemaining, 0f));
        StopCoroutine(coroutine);
        timeRemaining.enabled = true;
        timer = timerTime;
        isTiming = true;
    }

    private void notify(int Event)
    {
        if (tch != null)
        {
            tch(new object(), Event);
        }
    }

    public void addListener(Action<object, int> m)
    {
        Debug.Log(m);
        tch += new timerChangeHandler(m);
    }
}

public delegate void timerChangeHandler(object o, int timerEvent);

