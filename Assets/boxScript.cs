using UnityEngine;
using System.Collections;

public class boxScript : MonoBehaviour {

	
	//posi zoi tha exei
	public int maxHealth;
	int currentHealth;

	AudioSource audioMan;

	public GameObject drop;
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
		//otan o adipalos xtipiete, menei akinitos gia 0.2 defterolepta
		currentHealth-=damage;
		if (currentHealth <= 0)
		{
			Object dropInstance=Instantiate (drop, new Vector2(transform.position.x, transform.position.y),transform.rotation);
			audioMan.Play ();
			GetComponent<Collider2D>().enabled=false;
			GetComponent<Renderer>().enabled=false;
		}
	}
}
