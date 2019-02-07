using UnityEngine;
using System.Collections;

public class gemScript : MonoBehaviour {

	public int points;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		
		if(coll.name=="Player")
		{
			coll.gameObject.SendMessage("gainPoints",points);
			Destroy(this.gameObject);
		}
	}
}
