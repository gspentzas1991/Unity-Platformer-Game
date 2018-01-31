using UnityEngine;
using System.Collections;

/* Contains the script for moving a gameObjects transform when the player passes the trigger collider
 * This script is used to change the position of the "newCameraPosition" GameObject
 * When this scripts calculates the new transform of the camera, it calls the ChangeCamera method of cameraChangeScript
 * When the player is in the trigger collider, if he passes the cameraPusher, the target transform will move to the right
 * If the player returns to the cameraPusher, the transform is moved to the left
 */
public class CameraPusher : MonoBehaviour {

    public Transform cameraPosition;
    public GameObject player;
    public float cameraPushDistance;
    //The script that applies the changes to the camera
    public cameraChangeScript cameraChanger;
    public bool hasPushedCamera = false;


    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector3 moveCamera = new Vector3(0, 0, 0);
            if ((player.transform.position.x >= transform.position.x) && (!hasPushedCamera))
            {
                moveCamera = new Vector3(cameraPushDistance, 0, 0);
                hasPushedCamera = true;
            }
            if ((player.transform.position.x < transform.position.x) && (hasPushedCamera))
            {
                moveCamera = new Vector3(-cameraPushDistance, 0, 0);
                hasPushedCamera = false;
            }

            cameraPosition.transform.position += moveCamera;
            cameraChanger.ChangeCamera();
        }

    }
    
}
