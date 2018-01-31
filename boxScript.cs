using UnityEngine;
using System.Collections;

/* Contains the behaviour for the power up container
 * When the container breaks, it creats a powerUp gameobject and is destroyed
 */
public class boxScript : MonoBehaviour {

	public int maxHealth;
	int currentHealth;
	AudioSource audioMan;
	public GameObject powerUp;

	void Start ()
    {
		audioMan = GetComponent<AudioSource> ();
		currentHealth = maxHealth;
	}
	
	void GotHit(int damage)
	{
		//otan o adipalos xtipiete, menei akinitos gia 0.2 defterolepta
		currentHealth-=damage;
		if (currentHealth <= 0)
		{
			Object dropInstance=Instantiate (powerUp, new Vector2(transform.position.x, transform.position.y),transform.rotation);
			audioMan.Play ();
            Destroy(gameObject);
			//GetComponent<Collider2D>().enabled=false;
			//GetComponent<Renderer>().enabled=false;
		}
	}
}
