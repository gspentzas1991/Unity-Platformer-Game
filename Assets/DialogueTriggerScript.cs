using UnityEngine;
using System.Collections;

public class DialogueTriggerScript : MonoBehaviour {
	//an tha emfanizete mia fora o dialogos i sinexia
	bool used=false;
	public GameObject dialogueObject;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		//otan patisei o pextis pano tou, leei se ola ta adikimena oti ksekinise to dialogue
		if ((other.gameObject.tag == "Player")&&(!used))
		{
			used=true;
			dialogueObject.SendMessage("dialogueTriggered");
		}
	}
}
