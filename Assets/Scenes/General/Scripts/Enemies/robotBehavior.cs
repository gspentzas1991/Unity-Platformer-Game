using UnityEngine;
using System.Collections;

public class robotBehavior : MonoBehaviour {
	
	public float speed;
	public int damage;
	public float legRadius;

	//posi ora pernei prin ksanakaneis ranged epithesi
	public float maxRangedCooldown;
	float currentRangedCooldown;
	
	bool isPaused=false;
	bool frontIsGrounded=true;
	bool isShooting=false;
	bool facingRight;
	
	//gia posi ora tha stunarei apo xtipimata
	public float maxFreezeTime;
	float currentFreezeTime=0;
	
	//posi zoi tha exei
	public int maxHealth;
	int currentHealth;
	
	public LayerMask platformLayer;

	public GameObject alert;
	public GameObject lazer;
	//elenxei an exei vgei to thavmastiko
	bool detected=false;
	
	//ta gameobject tsekaroun an paei na pesei apo platforma
	public Transform frontLeg; //x=-0.7 y=-0.57 radius = 0.2
	
	
	public Transform frontCheckStart;
	public Transform frontCheckEnd;

	Animator characterAnim;
	public float maxAlertTimer;
	float currentAlertTimer;

	AudioSource audioMan;
	public AudioClip laserSound;
	
	// Use this for initialization
	void Start () 
	{
		audioMan = GetComponent<AudioSource> ();
		currentAlertTimer = 0;
		currentRangedCooldown = 0;
		characterAnim = GetComponent<Animator>();
		currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//an eine paused min kaneis tpt
		if(!isPaused)
		{
			currentAlertTimer=cooldown(currentAlertTimer);
			currentRangedCooldown=cooldown(currentRangedCooldown);
			if (speed > 0)
				facingRight = true;
			if (speed < 0)
				facingRight = false;
			if(isShooting)
				rangedAttack ();

			frontIsGrounded=Physics2D.OverlapCircle(frontLeg.position,legRadius,platformLayer);
			
			//an to aristero podi stamatisei na akoubaei kato, ke pas aristera, alakse poria
			//an to deksi podi stamatisei na akoubaei kato, ke pas deksia, alakse poria
			if(!frontIsGrounded)
			{
				speed=-speed;
				Flip();
			}
			//elenxei an iparxei tpt brosta tou, ke an iparxei girnaei anapoda
			RaycastHit2D[] hit =Physics2D.RaycastAll(frontCheckStart.position,frontCheckEnd.position,0.5f);

			foreach (RaycastHit2D coll in hit)
			{
				if(!coll.collider.isTrigger)
				{
					if((coll.collider.name!="Player")&&(coll.collider.name!="robot"))
					{
						speed=-speed;
						Flip();
					}
				}
			}

			currentFreezeTime=cooldown(currentFreezeTime);
			//oso o adipalos den iene frozen, kinite pros ta aristera
			if((currentFreezeTime==0)&&(!isShooting))
				GetComponent<Rigidbody2D>().velocity=new Vector2(speed,GetComponent<Rigidbody2D>().velocity.y);
			else
				GetComponent<Rigidbody2D>().velocity=new Vector2(0,GetComponent<Rigidbody2D>().velocity.y);
			//transform.position=new Vector2(transform.position.x-0.2f,transform.position.y);
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

	//fliparei to sprite
	void Flip ()
	{
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	//leei ti ginete otan kapios collider tou character akoubaei kapion allo
	//gia kathe frame sto opio akoubane
	void OnCollisionStay2D(Collision2D collision)
	{
		if(!isPaused)
		{
			if(collision.gameObject.tag=="Player")
			{
				collision.gameObject.SendMessage("gotHit",damage);
			}
		}
	}
	
	
	void gotHit(int damage)
	{
		//otan o adipalos xtipiete, menei akinitos gia 0.2 defterolepta
		currentFreezeTime = maxFreezeTime;
		currentHealth-=damage;
		if (currentHealth <= 0)
			Destroy (this.gameObject);
	}



	void startShooting()
	{
		if((currentAlertTimer<=0)&&(!detected))
		{
			currentAlertTimer=maxAlertTimer;
			Object alertInstance=Instantiate (alert, new Vector2(transform.position.x, transform.position.y+2.4f),transform.rotation);
			detected=true;
		}
		isShooting = true;
		GetComponent<Rigidbody2D>().velocity=new Vector2(0,GetComponent<Rigidbody2D>().velocity.y);
		characterAnim.SetBool("isShooting",isShooting);
	}

	void stopShooting()
	{
		detected = false;
		isShooting = false;
		GetComponent<Rigidbody2D>().velocity=new Vector2(speed,GetComponent<Rigidbody2D>().velocity.y);
		characterAnim.SetBool("isShooting",isShooting);

	}

	//ti ginete otan o pextis kanei mia ranged attack
	void rangedAttack()
	{
		if(currentRangedCooldown==0)
		{
			audioMan.clip=laserSound;
			audioMan.Play ();
			//dimiourgei ena object gia to projectile, ke tou dinei onoma analoga pros ta pou paei
			if(facingRight)
			{
				Object bulletInstance=Instantiate (lazer, new Vector2(transform.position.x + 2f, transform.position.y),transform.rotation);
				bulletInstance.name="bullet(right)";
			}
			else
			{
				Object bulletInstance=Instantiate (lazer, new Vector2(transform.position.x - 2f, transform.position.y),transform.rotation);
				bulletInstance.name="bullet(left)";
			}
			//ksekinaei to cooldown gia tin epomeni ranged epithesi
			currentRangedCooldown=maxRangedCooldown;
		}
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
