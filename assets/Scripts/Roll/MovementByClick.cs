using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementByClick : MonoBehaviour {

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			transform.position += Vector3.left; ;
		}
		if (Input.GetKeyDown(KeyCode.H))
		{
			transform.position += Vector3.right;
		}
		if (Input.GetKeyDown(KeyCode.T))
		{
			transform.position += Vector3.forward;
		}
		if (Input.GetKeyDown(KeyCode.G))
		{
			transform.position += Vector3.back;
		}
	}
}
