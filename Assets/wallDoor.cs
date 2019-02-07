using UnityEngine;
using System.Collections;

public class wallDoor : MonoBehaviour 
{
	public bool closed;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!closed)
		{
			GetComponent<Collider2D>().enabled = false;
			GetComponent<Renderer>().enabled=false;
		}
		else
		{
			GetComponent<Collider2D>().enabled = true;
			GetComponent<Renderer>().enabled=true;
		}
	
	}



	void closeDoor(bool input)
	{
		closed = input;
	}

}