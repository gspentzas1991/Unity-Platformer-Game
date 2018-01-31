using UnityEngine;
using System.Collections;

//When the fireball is spawned, it gets a velocity depending on its name (left or right)
//When the fireball hits something, it hurts it and is destroyed.
//If the fireball passes its range, it's destroyed
public class fireballBehavior : MonoBehaviour {
	public float speed;
	public float range;
	public int damage;
	bool isPaused;
	Vector2 startingPosition;
    Rigidbody2D fireballRigidbody;

    
	void Start () 
	{
        fireballRigidbody = GetComponent<Rigidbody2D>();
        AudioSource fireballAudioSource = GetComponent<AudioSource>();
        fireballAudioSource.Play();
		isPaused = false;
		startingPosition = transform.position;
        if (name == "bullet(left)")
        {
            transform.Rotate(new Vector3(0f, 0f, 180f));
            speed = -speed;
        }
        fireballRigidbody.velocity = new Vector2(speed, fireballRigidbody.velocity.y);
	}


    //When the fireball passes it's range, it gets destroyed
	void Update () 
	{
		if(!isPaused)
        {
            if (Mathf.Abs(transform.position.x-startingPosition.x) > range)
				Destroy (this.gameObject);
		}
	}
    
	void OnTriggerEnter2D(Collider2D target) 
	{
		if (target.gameObject.tag == "Enemy") 
		{
			target.gameObject.SendMessage ("GotHit", damage);
			Destroy (this.gameObject);
		}
        

	}
}
