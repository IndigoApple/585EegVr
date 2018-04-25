using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//work in progress, but basically you can set an initial event to the event sequence by setting initial to true, hide the object(if you're picking it up for ex) after its been executed, and create dialogue which updates the text UI on the canvas. References the event before too to know what sequence its on. I'm gonna be making this class easier to use so we can just attach events onto gameobjects and write dialogue in the components tab.
public class Event : MonoBehaviour
{
    public GameObject previousEvent;
    public ObjectDialogue dialogue;
    public DialogueManager manager;
    public Text initial_Text;
    public Text NPCName;
    public Image textBox;
    public Status status;
    private bool started = false;


    private bool check()
    {
        return ((status.eventNumber == 0 || 
            (previousEvent.GetComponent<Event>().status.eventNumber == status.eventNumber-1 && previousEvent.GetComponent<Event>().hasStarted())) && 
            (started == false));
    }
    public void initializer()
    {
        NPCName.text = "";
        initial_Text.text = "";
        textBox.enabled = false;
    }

    public void TriggerDialogue()
    {
        Debug.Log("triggered");
        manager.StartDialogue(dialogue);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (check())
        {
            NPCName.text = dialogue.name;
            textBox.enabled = true;
            TriggerDialogue();
        }

    }
    public void OnTriggerExit(Collider other)
    {
        if (check())
        {
            initializer();
           if (status.Dstatus == "destroy")
            {
                gameObject.GetComponent<Renderer>().enabled = false; 
            }
            started = true;
        }
    }

    public void updateText(string text)
    {
        initial_Text.text = text;
    }

    public bool hasStarted()
    {
        return started;
    }

    void Start()
    {
            initializer();
    }

}
