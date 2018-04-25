using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M2CoinSpawner : MonoBehaviour {
    public GameObject coin;

	// Use this for initialization
	void Start () {

        create();	
	}
	
    public void create()
    {
        Instantiate(coin, new Vector3(generateRandom(), 0.5f, generateRandom()), transform.rotation);
    }

    public float generateRandom()
    {
        float num = 0;
        while (Mathf.Abs(num) < 3.0) 
            num = Random.Range(-8.0f, 8.0f);
        return num;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
