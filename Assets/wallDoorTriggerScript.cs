using UnityEngine;
using System.Collections;

public class wallDoorTriggerScript : MonoBehaviour {

	public GameObject wallDoor;
	public GameObject boss;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.gameObject.tag == "Player")
		{
			wallDoor.SendMessage ("closeDoor", true);
			boss.SendMessage ("closeDoor",true);
		}
	}
}
