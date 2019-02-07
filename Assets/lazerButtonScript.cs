using UnityEngine;
using System.Collections;

public class lazerButtonScript : MonoBehaviour {
	public GameObject mainLazerBeam;

	AudioSource audioMan;
	public int maxHealth;
	int currentHealth;
	bool isOff=false;

	// Use this for initialization
	void Start () {
		audioMan = GetComponent<AudioSource> ();
		currentHealth = maxHealth;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void gotHit(int damage)
	{
		currentHealth-=damage;
		if ((currentHealth <= 0)&&(!isOff))
		{
			
			audioMan.Play();
			isOff=true;
			transform.localScale=new Vector3(-1,1,1);
			Destroy (mainLazerBeam);
		}
	}
}
