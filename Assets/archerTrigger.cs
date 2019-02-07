using UnityEngine;
using System.Collections;

public class archerTrigger : MonoBehaviour {
	public GameObject archer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.name == "Player")
			archer.SendMessage ("triggered",other);
	}
}
