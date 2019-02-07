using UnityEngine;
using System.Collections;

public class shopkeep : MonoBehaviour {
	public KeyCode shopKey;
	public GameObject shopDialogue;
	public bool playerIsReading=false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	void OnTriggerStay2D(Collider2D other) 
	{
		if ((other.name == "Player")&&(Input.GetKeyDown(shopKey))&&(!playerIsReading))
		{
			playerIsReading = true;
			shopDialogue.SendMessage("dialogueTriggered");
		}
	}
	/*
	void dialogueStart()
	{
		playerIsReading = true;
	}*/
	
	void dialogueEnd()
	{
		playerIsReading = false;
	}
}
