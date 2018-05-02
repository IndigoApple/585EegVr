using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collision1 : MonoBehaviour {
	public int[,] objects = new int[21, 21];
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public abstract void callEvent (int x, int y);
	public abstract void endEvent (int x, int y);
}
