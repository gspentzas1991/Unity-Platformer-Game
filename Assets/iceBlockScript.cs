using UnityEngine;
using System.Collections;

public class iceBlockScript : MonoBehaviour {
	public float timeToLive;

	SpriteRenderer spriteRend;
	public Sprite cracked;
	AudioSource audioMan;
	bool playedSound=false;

	// Use this for initialization
	void Start () 
	{
		audioMan = GetComponent<AudioSource> ();
		spriteRend=GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (timeToLive < 0.4)
		{
			if(!playedSound)
			{
				audioMan.Play ();
				playedSound=true;
			}
			spriteRend.sprite = cracked;
		}
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		if(collision.gameObject.name=="Player")
		{
			timeToLive -= Time.deltaTime;
			if (timeToLive <= 0)
				Destroy (this.gameObject);
		}
	}
}
