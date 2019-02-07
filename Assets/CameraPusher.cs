using UnityEngine;
using System.Collections;

public class CameraPusher : MonoBehaviour {

	public Rigidbody2D player;
	public float pushX;
	public float pushY;
	public Camera cam;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerExit2D(Collider2D other) 
	{
		if (other.name == "Player") 
		{
			if((player.velocity.x>0)&&(cam.transform.position.x<transform.position.x))
				cam.transform.localPosition= new Vector3(cam.transform.localPosition.x+pushX,cam.transform.localPosition.y+pushY,-1.77f);
			else if((player.velocity.x<0)&&(cam.transform.position.x>transform.position.x))
				cam.transform.localPosition= new Vector3(cam.transform.localPosition.x-pushX,cam.transform.localPosition.y-pushY,-1.77f);

		}
	}
}
