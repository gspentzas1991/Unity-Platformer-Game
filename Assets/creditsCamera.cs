using UnityEngine;
using System.Collections;

public class creditsCamera : MonoBehaviour {

	public KeyCode returnToTownKey;
	public int nextLevel;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(returnToTownKey))
		{
			GameObject.Find("gameManager").SendMessage("changeMusic",nextLevel);
			Application.LoadLevel(nextLevel);
		}

	
	}

}
