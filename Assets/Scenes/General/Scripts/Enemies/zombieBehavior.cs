using UnityEngine;
using System.Collections;

public class zombieBehavior : MonoBehaviour {

	public float speed;
	public int damage;
	public float legRadius;

	bool isPaused=false;
	bool leftIsGrounded=true;
	bool rightIsGrounded=true;

	//gia posi ora tha stunarei apo xtipimata
	public float maxFreezeTime;
	float currentFreezeTime=0;

	//posi zoi tha exei
	public int maxHealth;
	int currentHealth;

	public LayerMask platformLayer;

	//dio simia gia na ginei elenxos an iparxei tpt brosta tou
	public Transform frontCheckStart;
	public Transform frontCheckEnd;

	//ta gameobject tsekaroun an paei na pesei apo platforma
	public Transform rightLeg; //x=0.7 y=-0.57 radius =0.2
	public Transform leftLeg; //x=-0.7 y=-0.57 radius = 0.2



	// Use this for initialization
	void Start () 
	{
		currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//an eine paused min kaneis tpt
		if(!isPaused)
		{
			//elenxei an to deksi ke to aristero podi akoubane platformes
			leftIsGrounded=Physics2D.OverlapCircle(leftLeg.position,legRadius,platformLayer);
			rightIsGrounded=Physics2D.OverlapCircle(rightLeg.position,legRadius,platformLayer);

			//an to aristero podi stamatisei na akoubaei kato, ke pas aristera, alakse poria
			//an to deksi podi stamatisei na akoubaei kato, ke pas deksia, alakse poria
			if((rightIsGrounded)&&(!leftIsGrounded))
			{
				Flip();
			   speed=-speed;
			}

			//elenxei an iparxei tpt brosta tou, ke an iparxei girnaei anapoda
			RaycastHit2D[] hit =Physics2D.RaycastAll(frontCheckStart.position,frontCheckEnd.position,0.1f);
			
			foreach (RaycastHit2D coll in hit)
			{
				if(!coll.collider.isTrigger)
				{
					Flip ();
					speed=-speed;
					if(coll.collider.name=="Player")
					{
						coll.collider.gameObject.SendMessage("gotHit",damage);
					}
				}
			}

			currentFreezeTime=cooldown(currentFreezeTime);
			//oso o adipalos den iene frozen, kinite pros ta aristera
			if(currentFreezeTime==0)
			{
				//if(rigidbody2D.velocity.x==0)
					//speed=-speed;
				//an i taxitita eine 0 ke den eine frozen, tote malon to blokarei kati ke prepei na alaksei taxitita
				GetComponent<Rigidbody2D>().velocity=new Vector2(speed,GetComponent<Rigidbody2D>().velocity.y);

			}
			else
				GetComponent<Rigidbody2D>().velocity=new Vector2(0,0);

		}
	}

	//aferei kata ena kathe defterolepto to value, mexri na ginei 0
	float cooldown(float value)
	{
		if (value > 0)
			value -= 0.0167f;
		else
			value = 0;
		return value;
	}



	//leei ti ginete otan kapios collider tou character akoubaei kapion allo
	//gia kathe frame sto opio akoubane
	/*void OnCollisionEnter2D(Collision2D collision)
	{
		if(!isPaused)
		{
			if(collision.gameObject.tag=="Player")
			{
				Flip();
				speed=-speed;
				collision.gameObject.SendMessage("gotHit",damage);
			}
		}
	}*/


	void gotHit(int damage)
	{
		//otan ton xtipisei gia proti fora, tou alazei tin taxtita, epidi tha ksanaalaksei otan stamatisei na 
		//eine frozen

		//otan o adipalos xtipiete, menei akinitos gia 0.2 defterolepta
		currentFreezeTime = maxFreezeTime;
		currentHealth-=damage;
		if (currentHealth <= 0)
			Destroy (this.gameObject);
	}

	//fliparei to sprite
	void Flip ()
	{
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
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
