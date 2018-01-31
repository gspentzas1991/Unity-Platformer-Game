using UnityEngine;
using System.Collections;

/* Contains the behaviour of the Ice Spike obstacle
 * When the player enters the Spike's trigger collider, the spike begins to fall
 * If it touches the player it hurts him. If it touches any collider it gets destroyed
 */
public class IceSpikeScript : MonoBehaviour
{
	public int damage;
	bool isFalling;
	public float fallingSpeed;
	AudioSource audioManager;
    Rigidbody2D iceSpikeRigidbody;

	void Start ()
    {
		audioManager = GetComponent<AudioSource> ();
        iceSpikeRigidbody = GetComponent<Rigidbody2D>();
		isFalling = false;
	}

    //When the player enters the trigger collider, the spike starts falling
	void OnTriggerEnter2D(Collider2D targetCollider) 
	{
		if ((targetCollider.tag == "Player")&&(!isFalling))
		{
			audioManager.Play ();
			isFalling = true;
            iceSpikeRigidbody.velocity = new Vector2(iceSpikeRigidbody.velocity.x, -fallingSpeed);
        }

    }
    
	void OnCollisionEnter2D(Collision2D targetCollider)
	{
		if(targetCollider.gameObject.tag=="Player")
		{
			targetCollider.gameObject.SendMessage ("GotHit", damage);
		}
        Destroy(this.gameObject);
    }
}
