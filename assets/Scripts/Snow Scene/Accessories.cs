using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accessories : MonoBehaviour {
    GameObject currentChild;
    List<GameObject> children;


    // Use this for initialization
    void Start () {

        foreach (GameObject child in children)
        {
            child.SetActive(false);
        }
        currentChild = children[0];
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("right"))
        {
            for (int i = 0; i <= children.Count; i++)
            {
                if (currentChild == children[i] & i != children.Count)
                {
                    currentChild = children[i + 1];
                }
                else if (currentChild == children[i] & i == children.Count)
                {
                    currentChild = children[i + 1];
                    i = 0;
                    return;
                }
            }
        }
        if (Input.GetKey("left")) {
            for (int i = 0; i <= children.Count; i++)
            {
                if (currentChild == children[i] & i != 0)
                {
                    currentChild = children[i - 1];
                }
                else if (currentChild == children[i] & i == 0)
                {
                    currentChild = children[children.Count];
                    i = 0;
                    return;
                }
            }
        }
    }
}
