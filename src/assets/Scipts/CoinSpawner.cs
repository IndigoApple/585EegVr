using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour {
    public GameObject timerObj;
    public GameObject coin;
    
    // Use this for initialization
    void Start() {
            timerObj.GetComponent<TimerController>().addListener(
            this.notified);

    }

    public void notified(object o, int timerEvent) {
        //Instantiate(coin, new Vector3(Random.Range(-14.0f, 14.0f), 0.5f, 0f), transform.rotation);
        Instantiate(coin, new Vector3(0f, 0.5f, Random.Range(-14.0f, 14.0f)), transform.rotation);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
