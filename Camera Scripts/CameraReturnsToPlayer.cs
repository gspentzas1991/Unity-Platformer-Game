using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script returns the camera to "Follow the player" mode when the player enters its Trigger Collider
public class CameraReturnsToPlayer : MonoBehaviour {

    public Camera mainCamera;
    public GameObject player;
    //The camera's original localposition in comparison to the player
    Vector3 cameraOriginalLocalPosition;

    void Start()
    {
        cameraOriginalLocalPosition = mainCamera.transform.localPosition;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            mainCamera.transform.parent = player.transform;
            mainCamera.transform.localPosition = cameraOriginalLocalPosition;
            mainCamera.orthographicSize = 10;
        }
    }
}
