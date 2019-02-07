using UnityEngine;
using System.Collections;

public class IceSpikeScript : MonoBehaviour {
	public int damage;
	bool isFalling;
	public float fallingSpeed;
	AudioSource audioMan;
	// Use this for initialization
	void Start () {
		audioMan = GetComponent<AudioSource> ();
		isFalling = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isFalling)
			transform.position = new Vector2 (transform.position.x, transform.position.y - fallingSpeed);
	
	}

	void OnTriggerEnter2D(Collider2D target) 
	{
		//an petixe adipalo tou stelnei minima me to damage pou ekane
		if (target.tag == "Player") 
		{
			audioMan.Play ();
			isFalling = true;
		}

	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.tag=="Player")
		{
			coll.gameObject.SendMessage ("gotHit", damage);
			Destroy(this.gameObject);
		}
		if(coll.gameObject.tag=="Platform")
			Destroy(this.gameObject);
	}
}
