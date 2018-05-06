using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour {

    [SerializeField] public GameObject lastEvent;
    [SerializeField] public GameObject particles1;
    [SerializeField] public GameObject particles2;
    [SerializeField] public Text congrats;
    private Event[] sEvents;
    private bool startEnd;

    void Start () {
        sEvents = lastEvent.GetComponents<Event>();
        particles1.GetComponent<ParticleSystem>().Stop();
        particles2.GetComponent<ParticleSystem>().Stop();
        congrats.enabled = false;
    }

	void Update () {
		foreach(Event e in sEvents)
        {
            if (e.hasStarted() == false)
            {
                startEnd = false;
            }
            else
            {
                startEnd = true;
            }
        }

        if (startEnd)
        {
            particles1.GetComponent<ParticleSystem>().Play();
            particles2.GetComponent<ParticleSystem>().Play();

            congrats.enabled = true;
        }
	}
}
