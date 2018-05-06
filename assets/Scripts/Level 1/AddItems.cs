using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddItems : MonoBehaviour
{

    [SerializeField] public GameObject triggeringEvent;
    [SerializeField] public GameObject finishingEvent;
    private Event[] sEvents;
    private Event[] fEvents;
    private bool sEventStartCondition = false;
    private bool fEventStartcondition = false;

    void Start()
    {
        sEvents = triggeringEvent.GetComponents<Event>();
        fEvents = finishingEvent.GetComponents<Event>();
        gameObject.GetComponent<Renderer>().enabled = false;
    }
    // Update is called once per frame
    void Update()
    {   
        foreach(Event e in sEvents)
        {
            if (e.hasStarted()){
                sEventStartCondition = true;
            }
            else
            {
                sEventStartCondition = false;
            }
        }

        if (sEventStartCondition)
        {
            gameObject.GetComponent<Renderer>().enabled = true;
        }

        if (fEvents != null)
        {
            foreach (Event e in fEvents)
            {
                if (e.hasStarted() == false)
                {
                    fEventStartcondition = false;
                }
                else
                {
                    fEventStartcondition = true;
                }
            }
        }
        if (fEventStartcondition)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
    }
}

