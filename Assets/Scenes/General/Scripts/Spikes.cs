using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour {

	public float spikeDamage;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D(Collider2D target) 
	{
		target.SendMessage ("gotHit", spikeDamage,SendMessageOptions.DontRequireReceiver);
	}
}
