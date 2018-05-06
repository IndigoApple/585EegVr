using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionInfoToggle : MonoBehaviour {

	public ConnectionInfoController connectionInfoController;

    void Start()
    {
        Toggle();
    }

	void Update() {
		if (Input.GetKeyDown("space")) {
			connectionInfoController.showStatus = !connectionInfoController.showStatus;
		}
	}

    public void Toggle()
    {
        connectionInfoController.showStatus = !connectionInfoController.showStatus;
    }
}
