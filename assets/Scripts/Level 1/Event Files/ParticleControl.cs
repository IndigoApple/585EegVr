using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attaches particle effects to object after quest has been accepted, removes after it is done. Make sure to specify which game object started the quest and which ends it in the components part
public class ParticleControl : MonoBehaviour {
    public GameObject QuestStart;
    public GameObject Item;
	// Use this for initialization
	void Start () {
        gameObject.GetComponent<ParticleSystem>().Stop();
        InvokeRepeating("startEffects", 0,1);
	}
	
    void startEffects()
    {
        if (QuestStart.GetComponent<Event>().hasStarted())
        {
            gameObject.GetComponent<ParticleSystem>().Play();
        }

        if (Item.GetComponent<Event>().hasStarted())
        {
            gameObject.GetComponent<ParticleSystem>().Stop();
            CancelInvoke();
        }
    }
}
