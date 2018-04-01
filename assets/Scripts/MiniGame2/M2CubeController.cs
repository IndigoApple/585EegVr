using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M2CubeController : MonoBehaviour {

    public GameObject spawner;
	// Use this for initialization
	void Start () {
		
	}
	
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Pick Up"))
        {
            Destroy(other.gameObject);
            spawner.GetComponent<M2CoinSpawner>().create();
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
