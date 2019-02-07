using UnityEngine;
using System.Collections;

public class scoreScript : MonoBehaviour {
	int score;
	// Use this for initialization
	void Start () 
	{
		score=PlayerPrefs.GetInt ("Score" + gameManagerScript.savefile);
	}
	
	// Update is called once per frame
	void Update () 
	{
		GetComponent<GUIText>().text = "SCORE: " + score;
	}

	void getScore(int newScore)
	{
		score = newScore;
	}

}
