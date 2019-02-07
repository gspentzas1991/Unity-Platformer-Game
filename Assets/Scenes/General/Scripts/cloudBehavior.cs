using UnityEngine;
using System.Collections;

public class cloudBehavior : MonoBehaviour {
	public float speed;
	public float range;
	
	bool isPaused;
	
	Vector2 startingPosition;
	
	
	// Use this for initialization
	void Start () 
	{
		isPaused = false;
		startingPosition = transform.position;
	}
	
	
	// Update is called once per frame
	void Update () 
	{
		if(!isPaused)
		{
			transform.position=new Vector2(transform.position.x-speed,transform.position.y);
						//otan perasei to bullet to range tou, katastrefete
			if ((transform.position.x > startingPosition.x + range)||(transform.position.x < startingPosition.x - range))
				Destroy (this.gameObject);
		}
	}
		
	//otan ginete pause
	void OnPauseGame()
	{
		isPaused = true;
	}
	
	//otan kseginete apo pause
	void onUnPauseGame()
	{
		isPaused = false;
	}
}
