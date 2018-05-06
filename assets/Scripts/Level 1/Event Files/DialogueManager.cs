using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//general dialogue manager, make sure you change the character movement script name in this file to wbatever you named your character control
public class DialogueManager : MonoBehaviour {

    private Queue<string> sentences;
    public string currentSentence = "";
    public GameObject player;


    public void StartDialogue(ObjectDialogue dialogue)
    {
        Debug.Log("Starting conversation" + dialogue.name);
        sentences.Clear();
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {

        Debug.Log(sentences.Count);
        InvokeRepeating("nextSentence", 0, 3);
        
    }

    void EndDialogue()
    {
        Debug.Log("End of Conv");
    }

    private void nextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            CancelInvoke();
            player.GetComponent<CharacterControl>().switchmove = true;
            return;
        }
        player.GetComponent<CharacterControl>().canmove = false;
        currentSentence = sentences.Dequeue();
        Debug.Log(sentences.Count + currentSentence);
        FindObjectOfType<Event>().updateText(currentSentence);

    }

	void Start () {
        sentences = new Queue<string>();
	}


}
