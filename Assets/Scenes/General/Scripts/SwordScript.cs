using UnityEngine;
using System.Collections;

public class SwordScript : MonoBehaviour 
{
	bool isPaused=false;

	public KeyCode meleeKey=KeyCode.K;
	public int startingTimer=500;
	int timer;

	// Use this for initialization
	void Start () 
	{
		gameObject.GetComponent<Renderer>().enabled=false;
		timer=startingTimer;
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//FUCK U
		if(!isPaused)
		{
			if (Input.GetKeyDown(meleeKey))
			{
				//gameObject.renderer.enabled=true;
			}
		}
	}

	void FixedUpdate()
	{
		timer--;
		if(timer<=0)
		{
			gameObject.GetComponent<Renderer>().enabled=false;
			timer=startingTimer;

		}

	}

	void OnPauseGame()
	{
		isPaused = true;
	}

	void onUnPauseGame()
	{
		isPaused = false;
	}
}
