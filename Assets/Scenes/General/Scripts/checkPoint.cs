using UnityEngine;
using System.Collections;

public class checkPoint : MonoBehaviour {
	public int currentLevel;
	public int currentCheckPoint;
	bool check=false;

	Animator checkPointAnim;

	AudioSource audioMan;

	// Use this for initialization
	void Start () 
	{
		checkPointAnim = GetComponent<Animator>();
		audioMan = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		checkPointAnim.SetBool ("checked", check);
	}

	void OnTriggerEnter2D(Collider2D coll)
	{

		if((coll.name=="Player")&&(!check))
		{
			coll.gameObject.SendMessage("gainHealth",100);
			check=true;
			audioMan.Play();
			if(GameObject.Find ("gameManager"))
				GameObject.Find ("gameManager").SendMessage("save",currentLevel);
			GameObject.Find ("levelManager").SendMessage("save",currentCheckPoint);
			//coll.SendMessage("saveScore");
		}
	}
}
