using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {

    public GameObject player;
    private Button self;
    public int id;

	// Use this for initialization
	void Start () {
        self = GetComponent<Button>();
        self.onClick.AddListener(() => OnClick(id));
	}

    protected void OnClick(int id)
    {
        Debug.Log(id);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
