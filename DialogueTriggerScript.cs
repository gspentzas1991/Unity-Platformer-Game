using UnityEngine;
using System.Collections;

//When the player touches the trigger, it sends a message to the dialogue handler
public class DialogueTriggerScript : MonoBehaviour {
	bool triggered=false;
	public GameObject dialogueObject;

    void OnTriggerEnter2D(Collider2D other) 
	{
		//otan patisei o pextis pano tou, leei se ola ta adikimena oti ksekinise to dialogue
		if ((other.gameObject.tag == "Player")&&(!triggered))
		{
			triggered=true;
			dialogueObject.SendMessage("DialogueTriggered");
		}
	}
}
