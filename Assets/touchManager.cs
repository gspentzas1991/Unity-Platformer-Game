using UnityEngine;
using System.Collections;

public class touchManager : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{

	
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void OnGUI ()
	{
		if (GUI.Button (new Rect (Screen.width / 30, Screen.height / 30, Screen.width / 10, Screen.height / 15), ""))
						print ("1");
	}

}
