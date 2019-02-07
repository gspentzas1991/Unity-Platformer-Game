using UnityEngine;
using System.Collections;

public class ladderScript : MonoBehaviour {

	bool check=false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	void OnTriggerStay2D(Collider2D other) 
	{
		if (other.gameObject.tag == "Player")
		{
			if (check==false)
			{
				check=true;
				other.gameObject.SendMessage ("laddercheck",check);
			}
		}
	}
	
	void OnTriggerExit2D(Collider2D other) 
	{
		if (other.gameObject.tag == "Player")
		{
			check=false;
			other.gameObject.SendMessage ("laddercheck",check);
		}
	}
}
