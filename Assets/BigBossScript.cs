using UnityEngine;
using System.Collections;

public class BigBossScript : MonoBehaviour {
	public GameObject topLeftPosition;
	public GameObject topRightPosition;
	public GameObject downLeftPosition;
	public GameObject downRightPosition;
	Transform currentPosition;

	public GameObject blink;
	public GameObject doorToOpen;
	public float chargeSpeed;

	public int maxHealth;
	int currentHealth;

	int animationSpeed;
	int random;


	bool closed;
	bool inDialogue;
	bool isPaused;
	bool inBattle;
	bool isCharging;

	public float maxInvTime;
	float currentInvTime;

	public GameObject lazer;
	public GameObject bomb;
	//posi ora pernei prin ksanakaneis ranged epithesi
	public float maxRangedCooldown;
	float currentRangedCooldown;

	public int maxNumberOfBombs;
	int currentNumberOfBombs;

	bool facingRight=false;
	//kathe pote tha anavosvinei otan ton xtipane
	public float hurtBlinkingSpeed;
	//xrisimopiite gia na tsekarei an irthe i ora na kanei blink o xaraktiras
	float currentBlinkingTimer;

	Animator anim;
	public Texture heartEmpty;
	public Texture heartHalf1;
	public Texture heartHalf2;

	AudioSource audioMan;
	public AudioClip laserSound;

	// Use this for initialization
	void Start () 
	{
		audioMan = GetComponent<AudioSource> ();
		currentPosition = downRightPosition.transform;
		inBattle = false;
		inDialogue = false;
		isPaused = false;
		animationSpeed = 1;
		currentHealth = maxHealth;
		currentInvTime = 0;
		closed = false;
		currentBlinkingTimer = 0;
		anim=GetComponent<Animator>();
	
	}
	
	// Update is called once per frame
	void Update () {
		currentInvTime=cooldown (currentInvTime);
		currentRangedCooldown=cooldown (currentRangedCooldown);
		checkBlinking();
		if((transform.position.y<10)&&(GetComponent<Collider2D>().isTrigger==false)&&(inBattle))
			rangedAttack (lazer);
		if((transform.position.y>10)&&(GetComponent<Collider2D>().isTrigger==false)&&(inBattle))
			rangedAttack (bomb);
		if(GetComponent<Collider2D>().isTrigger==true)
		{
			if(facingRight)
			{
				transform.position=new Vector2(transform.position.x+chargeSpeed,transform.position.y);
				if(transform.position.x>50)
					Flip();
			}
			else
			{
				transform.position=new Vector2(transform.position.x-chargeSpeed,transform.position.y);
				if(transform.position.x<10)
					Flip();
			}
		}
		if (currentNumberOfBombs == -1)
			move ();
		anim.speed=animationSpeed;
		anim.SetBool ("isCharging", isCharging);
		anim.SetFloat ("verticalPosition", transform.position.y);
		anim.SetBool ("inBattle", inBattle);
	}

	void closeDoor(bool input)
	{
		closed = input;
	}
	void gotHit(int damage)
	{
		damage = 1;
		//otan o adipalos xtipiete, menei akinitos gia 0.2 defterolepta
		if (currentInvTime == 0) 
		{
			currentInvTime = maxInvTime;
			currentHealth-=damage;
			//otan xtipithei dialegei mia katefthinsi gia na figei
			if(currentHealth>0)
			{
				move();
			}
		}
		if (currentHealth <= 0)
		{
			doorToOpen.SendMessage("closeDoor",false);
			Destroy (this.gameObject);
		}
	}

	void OnGUI () 
	{
		if(closed)
		{
			//kardies
			int offset=0;
			Texture x;
			for(int i=0;i<maxHealth/2;i++)
				GUI.DrawTexture(new Rect(Screen.width-maxHealth*20+i*40,0,30,30),heartEmpty);
			for(int i=0;i<currentHealth;i++)
			{
				if(i%2==0)
					x=heartHalf1;
				else
					x=heartHalf2;
				if((i>1)&&(i%2==0))
					offset+=10;
				GUI.DrawTexture(new Rect((15*i+offset)+Screen.width-maxHealth*20,0,15,30),x);
			}
		}
		
	}

	//aferei kata ena kathe defterolepto to value, mexri na ginei 0
	float cooldown(float colldownItem)
	{
		if (colldownItem > 0)
			colldownItem -= 0.0167f;
		else
			colldownItem  = 0;
		return colldownItem;
	}

	//o boss allazei topothesia
	void move()
	{
		currentNumberOfBombs = maxNumberOfBombs;
		random=Random.Range (0,3);
		//periptoseis gia an o boss eine se mia apo tis pano platformes
		if(transform.position.y>10)
		{
			//an eine pano theloume to random na ginei 1 i 2 gia na paei kapou kato
			if(random==0)
			{
				random=Random.Range(1,3);
				/*if(currentPosition==topLeftPosition.transform)
					teleport (topRightPosition);
				if(currentPosition==topRightPosition.transform)
					teleport(topLeftPosition);*/
			}
			//tha katevei kato deksia i aristera
			if(random==1)
				teleport(downLeftPosition);
			if(random==2)
				teleport(downRightPosition);
		}
		//periptoseis gia an eine kato
		else
		{
			//tha kanei charge apenadi
			if(random==0)
			{
				if(currentPosition==downLeftPosition.transform)
					charge (downRightPosition);
				if(currentPosition==downRightPosition.transform)
					charge(downLeftPosition);
			}
			//tha anevei se mia pano
			if(random==1)
				teleport(topLeftPosition);
			if(random==2)
				teleport(topRightPosition);
			
		}
	}
	void checkBlinking()
	{
		
		if((currentInvTime>0)&&(Time.time>=currentBlinkingTimer+hurtBlinkingSpeed))
		{
			currentBlinkingTimer=Time.time;
			if(GetComponent<Renderer>().enabled==true)
				GetComponent<Renderer>().enabled=false;
			else
				GetComponent<Renderer>().enabled=true;
		}
		if (currentInvTime == 0)
			GetComponent<Renderer>().enabled = true;
	}

	void teleport (GameObject destination)
	{
		currentRangedCooldown = 0.2f;
		Object bulletInstance = Instantiate (blink, transform.position,transform.rotation);
		currentPosition = destination.transform;
		transform.position = destination.transform.position;
		if (((transform.position.x < 40)&&(!facingRight))||((transform.position.x > 20)&&(facingRight)))
			Flip ();
	}
	void charge(GameObject destination)
	{
		isCharging = true;
		GetComponent<Collider2D>().isTrigger = true;
		GetComponent<Rigidbody2D>().isKinematic = true;
	}

	void rangedAttack(GameObject bullet)
	{
		if(currentRangedCooldown==0)
		{
			if(bullet.name=="Bomb")
				currentNumberOfBombs--;
			if(bullet.name=="BossLazer")
			{
				audioMan.clip=laserSound;
				audioMan.Play ();
			}
			//dimiourgei ena object gia to projectile, ke tou dinei onoma analoga pros ta pou paei
			if(facingRight)
			{
				Object bulletInstance=Instantiate (bullet, new Vector2(transform.position.x + 2f, transform.position.y),transform.rotation);
				bulletInstance.name="bullet(right)";
			}
			else
			{
				Object bulletInstance=Instantiate (bullet, new Vector2(transform.position.x - 2f, transform.position.y),transform.rotation);
				bulletInstance.name="bullet(left)";
			}
			//ksekinaei to cooldown gia tin epomeni ranged epithesi
			currentRangedCooldown=maxRangedCooldown;
		}
	}

	void Flip ()
	{
		if (isCharging)
			isCharging = false;
		if (GetComponent<Collider2D>().isTrigger == true)
			GetComponent<Collider2D>().isTrigger = false;
		if (GetComponent<Rigidbody2D>().isKinematic == true)
			GetComponent<Rigidbody2D>().isKinematic = false;
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	//otan ginete pause
	void OnPauseGame()
	{
		animationSpeed=0;
		isPaused=true;
	}
	
	void dialogueStart()
	{
		inDialogue = true;
		animationSpeed=0;
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		//otan akoubisei ton pexti, petagete piso, ke kanei disable to collider tou
		if(other.gameObject.tag=="Player")
		{
			other.SendMessage ("gotHit",1);
		}
	}
	
	void dialogueEnd()
	{
		inDialogue = false;
		//rigidbody2D.velocity = currentVelocity;
		animationSpeed=1;
		inBattle = true;
	}
	
	//otan kseginete pause
	void onUnPauseGame()
	{
		animationSpeed=1;
		isPaused = false;
	}
}
