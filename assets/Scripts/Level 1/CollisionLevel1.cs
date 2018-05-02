using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionLevel1 : Collision1 {

	//public int[,] objects = new int[21, 21];
	[SerializeField] private GameObject[] obj;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 21; i++) {
			for (int j = 0; j < 21; j++) {
				if (i == 0 || j == 0 || i == 20 || j == 20)
					objects [i,j] = -1;
				else
					objects [i,j] = 0;
			}
		}
		objects [2,2] = 1;
		objects [4,6] = -1;
		objects [1,12] = -1;
		objects [1,14] = -1;
		objects [1,17] = -1;
        objects [2,12] = -1;
		objects [5,12] = -1;
		objects [7,11] = -1;
		objects [8,13] = -1;
		objects [8,19] = -1;
		objects [9,6] = -1;
		objects [10,17] = -1;
		objects [11,12] = -1;
		objects [13,19] = -1;
		objects [14,15] = -1;
		objects [15,19] = -1;
		objects [8,16] = -1;
		objects [17,12] = -1;
		objects [19,16] = -1;
		objects [19,18] = -1;
	}

	public override void callEvent(int x, int y) {
		Event[] eventscripts = obj [objects [x, y]-1].GetComponents<Event> ();
		foreach (Event script in eventscripts) {
			script.OnTriggerEnter (null);
		}
	}

	public override void endEvent(int x, int y) {
		Event[] eventscripts = obj [objects [x, y]-1].GetComponents<Event> ();
		foreach (Event script in eventscripts) {
			script.OnTriggerExit (null);
		}
	}

	// Update is called once per frame
	void Update () {
	}
}
