using UnityEngine;
using System.Collections;


/* Contains the behaviour of the laser projectile
 * After it gets created, it gains velocity, depending on its name (right or left)
 * When it touches an the player it hurts him and is destroyed. It's also destroyed if it passes its range value
 */
public class lazerBehavior : MonoBehaviour {
	public float speed;
	public float range;
	public int damage;
	bool isPaused;
    Rigidbody2D lazerRigidbody;
	float startingPosition;
	
	
	void Start () 
	{
        lazerRigidbody = GetComponent<Rigidbody2D>();
		isPaused = false;
		startingPosition = transform.position.x;
        if (name == "bullet(left)")
        {
            speed = -speed;
        }
	}
	
	void FixedUpdate () 
	{
		if(!isPaused)
		{
            lazerRigidbody.velocity = new Vector2(speed, lazerRigidbody.velocity.y);
            if (System.Math.Abs(startingPosition-transform.position.x)> range)
				Destroy (this.gameObject);
		}
	}
	
	void OnTriggerEnter2D(Collider2D target) 
	{
		if (target.tag == "Player") 
			target.gameObject.SendMessage ("GotHit", damage);
        //The projectile is destroyed if it hits something that isn't an enemy or a line of sight object
		if((target.tag!="Enemy")&&(target.tag!="LoS"))
			Destroy (this.gameObject);
	}
	
    //Pause messages received from the gameManager
	void OnPauseGame()
	{
        lazerRigidbody.isKinematic = true;
		isPaused = true;
	}
	
	void OnUnPauseGame()
	{
        lazerRigidbody.isKinematic = false;
		isPaused = false;
	}
}
