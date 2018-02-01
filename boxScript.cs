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
		currentHealth-=damage;
		if (currentHealth <= 0)
		{
			Object dropInstance=Instantiate (powerUp, new Vector2(transform.position.x, transform.position.y),transform.rotation);
			audioMan.Play ();
           		 Destroy(gameObject);
		}
	}
}
