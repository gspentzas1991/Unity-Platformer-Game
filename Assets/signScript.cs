using UnityEngine;
using System.Collections;

public class signScript : MonoBehaviour {

	bool playerIsReading=false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (playerIsReading)
			GetComponent<Renderer>().enabled = true;
		else
			GetComponent<Renderer>().enabled=false;
	
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.name == "Player")
			playerIsReading = true;
	}

	void OnTriggerExit2D(Collider2D other) 
	{
		if (other.name == "Player")
			playerIsReading = false;
	}

}
