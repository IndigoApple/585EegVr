using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;


    void LateUpdate()
    {
        float newXPosition = player.transform.position.x - 10;
        float newZPosition = player.transform.position.z - 10;

        transform.position = new Vector3(newXPosition, transform.position.y, newZPosition);
    }
}
