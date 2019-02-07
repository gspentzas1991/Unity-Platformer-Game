using UnityEngine;
using System.Collections;

public class goalScript : MonoBehaviour {

	public int levelUnlock;
	public int nextLevel;
	// Use this for initialization
	void Start () 
	{
		//ta scenes 0 ke 1 den eine alithina levels, opote prosthetoume 1
		nextLevel += 1;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if(coll.name=="Player")
		{
			GameObject.Find ("gameManager").SendMessage("unlock",levelUnlock);
			GameObject.Find("gameManager").SendMessage("changeMusic",nextLevel);
			//coll.SendMessage ("saveScore");
			Application.LoadLevel(nextLevel);
		}
	}
}
