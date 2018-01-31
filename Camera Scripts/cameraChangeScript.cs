using UnityEngine;
using System.Collections;

/* Contains the Script for changing the camera mode from "Follow Player" to "Show Area"
 * When the player is in the trigger collider, if he passes the cameraChangeMode gameobject, the mode changes
 * When the mode changes, the camera stops being a child of the player and goes to its new position
 * If the player returns to the cameraChangeMode gameobject, the camera returns to him
 */
public class cameraChangeScript : MonoBehaviour {
    public float newCameraZoom;
    public Transform newCameraPosition;
    public GameObject player;
    //The camera's original localposition in comparison to the player
    Vector3 cameraOriginalLocalPosition;
    public  Camera mainCamera;

    void Start()
    {
        cameraOriginalLocalPosition = mainCamera.transform.localPosition;
    }

    /* If the player passes the gameObject, the camera goes to its new position
     * If he returns to the gameObject, the camera returns to following the player
     * This method can also be called by the CameraPusher GameObject
     */
    public void ChangeCamera()
    {
        
        if (transform.position.x <= player.transform.position.x)
        {
            mainCamera.transform.parent = null;
            mainCamera.transform.position = newCameraPosition.position;
            mainCamera.orthographicSize = newCameraZoom;
        }
        else
        {
            mainCamera.transform.parent = player.transform;
            mainCamera.transform.localPosition = cameraOriginalLocalPosition;
            mainCamera.orthographicSize = 10;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            ChangeCamera();
        }
    }
}
