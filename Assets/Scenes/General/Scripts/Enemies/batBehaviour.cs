using UnityEngine;
using System.Collections;

public class batBehaviour : MonoBehaviour {
	//i taxitita pou petaei
	public float speed;
	//to damage pou kanei otan se akoubaei
	public int damage;

	public int maxHealth;
	int currentHealth;

	public float lineOfSightRadius;
	public float nestRadius;

	public LayerMask playerLayer;
	public LayerMask nestLayer;
	//i topothesia tis folias tou
	public Transform home;
	
	Collider2D lineOfSight;
	Collider2D sleepOnNest;

	//an xtipise ton pexti
	bool hitTarget;

	// Use this for initialization
	void Start () {
		hitTarget = false;
		currentHealth = maxHealth;
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//elenxei giro tou an vlepei ton pexti
		lineOfSight=Physics2D.OverlapCircle(transform.position,lineOfSightRadius,playerLayer);
		//an vlepei ton pexti ke den ton exei xtipisei akoma, katefthinete pros afton
		if ((lineOfSight) && (!hitTarget))
			pathfinding (lineOfSight.GetComponent<Collider2D>().gameObject.transform);
		//an exei xtipisei ton pexti, i den ton vlepei, katefthinete pros tin folia tou
		else 
			pathfinding (home);

		sleepOnNest=Physics2D.OverlapCircle(transform.position,nestRadius,nestLayer);
		//otan plisiasei stin folia tou, kanei reset kapia pragmata
		if (sleepOnNest)
		{
			hitTarget = false;
		}
	}

	//ti ginete otan akoubisei kati
	void OnCollisionEnter2D(Collision2D coll)
	{
		//otan akoubisei ton pexti, petagete piso, ke kanei disable to collider tou
		if(coll.gameObject.tag=="Player")
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(-GetComponent<Rigidbody2D>().velocity.x,speed);
			hitTarget = true;
			coll.gameObject.SendMessage ("gotHit",damage);
		}
	}

	void gotHit (int damage)
	{
		currentHealth -= damage;
		if (currentHealth <= 0)
			Destroy (this.gameObject);
	}

	void pathfinding(Transform target)
	{
		//elenxei pou eine o xaraktiras, ke ton plisiazei ston aksona x
		if(target.position.x>transform.position.x)
			GetComponent<Rigidbody2D>().velocity=new Vector2(speed,GetComponent<Rigidbody2D>().velocity.y);
		if(target.position.x<transform.position.x)
			GetComponent<Rigidbody2D>().velocity=new Vector2(-speed,GetComponent<Rigidbody2D>().velocity.y);
		if((target.position.x>transform.position.x-0.5)&&(target.position.x<transform.position.x+0.5))
			GetComponent<Rigidbody2D>().velocity=new Vector2(0,GetComponent<Rigidbody2D>().velocity.y);
		
		//elenxei pou eine o xaraktiras, ke ton plisiazei ston aksona y
		if(target.position.y>transform.position.y)
			GetComponent<Rigidbody2D>().velocity=new Vector2(GetComponent<Rigidbody2D>().velocity.x,speed);
		if(target.position.y<transform.position.y)
			GetComponent<Rigidbody2D>().velocity=new Vector2(GetComponent<Rigidbody2D>().velocity.x,-speed);
		if((target.position.y>transform.position.y-0.5)&&(target.position.y<transform.position.y+0.5))
			GetComponent<Rigidbody2D>().velocity=new Vector2(GetComponent<Rigidbody2D>().velocity.x,0);
	}


}
