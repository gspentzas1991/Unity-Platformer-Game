using UnityEngine;
using System.Collections;

/* Contains the behaviour of the moving platforms
 * The platform will either start moving immediately, or when the player first touches them (bool "startMoving)
 * When the platform passes surpases its range in either axis, it either stops moving or turns around (bool platformReturnsToStart)
 * When the player touches the platform he becomes its child object in order to follow its movement
 * When the player leaves the platform, he stops being its child object
 */
public class movingPlatformBehavior : MonoBehaviour {

    //if true, the platform starts moving before the player touches the platform
	public bool startMoving;
    //If true, the platform turns around after it passes its range in either axis
    public bool platformReturnsToStart;
    //How far the platform will travel in either Axis before reaching the destination
    public float platformRange;
    public float verticalSpeed;
	public float horizontalSpeed;
	public GameObject startingPoint;
	

	// Update is called once per frame
	void FixedUpdate () 
	{
        if(startMoving)
		{
			if((System.Math.Abs(transform.position.x-startingPoint.transform.position.x)>platformRange)||
			   (System.Math.Abs(transform.position.y-startingPoint.transform.position.y)>platformRange))
            {
                //the speed will either become negative or 0
                horizontalSpeed = (platformReturnsToStart ? 1 : 0) * -horizontalSpeed;
                verticalSpeed = (platformReturnsToStart ? 1 : 0) * -verticalSpeed;
			}
			transform.position=new Vector3(transform.position.x+horizontalSpeed,transform.position.y+verticalSpeed,0);
		}
	}

    //When the player enters the trigger collider on top of the platform, it becomes a child of the platform
    void OnTriggerEnter2D(Collider2D targetCollider) 
	{
		if (targetCollider.tag == "Player")
		{
			targetCollider.gameObject.transform.parent=transform;
			startMoving = true;
		}
	
	}

    //When the player leaves the platform, he is no longer a child
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            transform.DetachChildren();
        }
    }
}
