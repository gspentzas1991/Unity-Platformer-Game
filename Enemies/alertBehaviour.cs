using UnityEngine;
using System.Collections;

//After a few seconds the alert gameobject will be destroyed
public class alertBehaviour : MonoBehaviour {

	public float timeToLive;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		timeToLive -= Time.deltaTime;
		if (timeToLive <= 0)
			Destroy (this.gameObject);
	}
}
