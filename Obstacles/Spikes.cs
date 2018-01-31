using UnityEngine;
using System.Collections;


/*if the player touches the spikes, he gets damaged*/
public class Spikes : MonoBehaviour {

	public float spikeDamage;
	
	void OnTriggerStay2D(Collider2D targetCollider) 
	{
		targetCollider.SendMessage ("GotHit", spikeDamage,SendMessageOptions.DontRequireReceiver);
	}
}
