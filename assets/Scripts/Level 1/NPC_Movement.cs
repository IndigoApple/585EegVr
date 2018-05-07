using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC_Movement : MonoBehaviour {

    public GameObject player;
    private float timer = 0;
    private int dir = 2;

	// Use this for initialization
	void Start () {
        player.GetComponent<CharacterControl>().GridMove(1);
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        
        if (timer > 5.0)
        {
            timer = 0;
            player.GetComponent<CharacterControl>().GridMove(dir);
            player.GetComponent<CharacterControl>().GridMove(dir);
            if (dir == 1)
                dir = 2;
            else
                dir = 1;
        }
	}
}
