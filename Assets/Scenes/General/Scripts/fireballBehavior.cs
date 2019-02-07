using UnityEngine;
using System.Collections;

public class fireballBehavior : MonoBehaviour {
	public float speed;
	public float range;
	public int damage;

	bool isPaused;

	Vector2 startingPosition;


	// Use this for initialization
	void Start () 
	{
		isPaused = false;
		startingPosition = transform.position;
		//girnaei 180 mires ta bullets pou legode bullet(left)
		if(name=="bullet(left)")
			transform.Rotate(new Vector3(0f,0f,180f));
	}


	// Update is called once per frame
	void Update () 
	{
		if(!isPaused)
		{
			//to bullet proxoraei se kapia katefthinsi analoga me to onoma tou
			if(name=="bullet(right)")
				GetComponent<Rigidbody2D>().velocity=new Vector2(speed,GetComponent<Rigidbody2D>().velocity.y);
			else
				GetComponent<Rigidbody2D>().velocity=new Vector2(-speed,GetComponent<Rigidbody2D>().velocity.y);

			//otan perasei to bullet to range tou, katastrefete
			if ((transform.position.x > startingPosition.x + range)||(transform.position.x < startingPosition.x - range))
				Destroy (this.gameObject);
		}
	}

	//ti ginete otan petixenei kapion
	void OnTriggerEnter2D(Collider2D target) 
	{
		//an petixe adipalo tou stelnei minima me to damage pou ekane
		if (target.gameObject.tag == "Enemy") 
		{
			target.gameObject.SendMessage ("gotHit", damage);
			Destroy (this.gameObject);
		}

		//an petixei kapion katastrefete (ektos ton player)
		//if(target.gameObject.tag == "Platform")

	}

	//otan ginete pause
	void OnPauseGame()
	{
		GetComponent<Rigidbody2D>().isKinematic = true;
		isPaused = true;
	}
	
	//otan kseginete apo pause
	void onUnPauseGame()
	{
		GetComponent<Rigidbody2D>().isKinematic = false;
		isPaused = false;
	}
}
