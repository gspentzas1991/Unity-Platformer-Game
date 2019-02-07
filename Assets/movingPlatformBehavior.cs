using UnityEngine;
using System.Collections;

public class movingPlatformBehavior : MonoBehaviour {

	public bool auto;
	//katheti taxitita
	public float verticalSpeed;
	//orizodia taxitita
	public float horizontalSpeed;
	//to simio pou ksekinaei (stin pragmatikotita ksekinaei -movingArea-0,1 makria tou
	//px an to startingPoint eine 5, ksekinaei apo to -4.9
	public GameObject startingPoint;
	//poso makria petaei i platforma
	public float movingArea;
	//an tha stamataei otan ftasei ston prorismo tis i platforma
	public bool loop;

	//epidi den litourgei to onCollisionExit, ke to collisionEnter ginete sinexia, leme oti
	//an gia 0.01 defterolepta o pextis den akoubaei pano stin platforma, tote stamataei na eine
	//child tis
	public float maxTimeToDetach;
	float currentTimeToDetach;


	//tha ginei true ke tha ksekinisei na kinite otan to akoubisei o pextis
	bool startMoving;


	// Use this for initialization
	void Start () 
	{

		currentTimeToDetach = 0;
		if (auto)
			startMoving = true;
		else
			startMoving = false;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (transform.childCount > 0)
		{
			if (currentTimeToDetach <= 0)

				transform.DetachChildren ();
			else
				currentTimeToDetach=currentTimeToDetach-Time.deltaTime;
		}



		if(startMoving)
		{
			if((transform.position.x>=startingPoint.transform.position.x+movingArea)||
			   (transform.position.x<=startingPoint.transform.position.x-movingArea)||
			   (transform.position.y>=startingPoint.transform.position.y+movingArea)||
			   (transform.position.y<=startingPoint.transform.position.y-movingArea))
			{
				if(loop)
				{
					verticalSpeed=-verticalSpeed;
					horizontalSpeed=-horizontalSpeed;
				}
				else
				{
					verticalSpeed=0;
					horizontalSpeed=0;
				}
			}
			transform.position=new Vector3(transform.position.x+horizontalSpeed,
			                               transform.position.y+verticalSpeed,
			                               0);
		}
	}


	//molis to akoubisi o pextis, ginete child tis platformas ke i platforma arxizei tin kinisi
	void OnTriggerStay2D(Collider2D coll) 
	{
		if (coll.tag == "Player")
		{
			currentTimeToDetach=maxTimeToDetach;
			coll.gameObject.transform.parent=transform;
			startMoving = true;
		}
	
	}
}
