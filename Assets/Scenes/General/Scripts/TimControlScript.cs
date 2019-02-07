 using UnityEngine;
using System.Collections;

public class TimControlScript : MonoBehaviour {

	AudioSource audioMan;
	//taxitita tou xaraktira
	public float Speed;
	//taxitita pou anevenei skales
	public float climbingSpeed;
	//taxitita almatos
	public float jumpSpeed;
	//to megethos tou kiklou pou elenxei an patame edafos
	public float feetRadius;
	//poso damage kanei i melee
	public int meleeDamage;
	//to range tou melee weapon
	public float meleeRange;

	//gia poso eise aititos meta apo xtipima
	public float maxInvissibilityTime;
	float currentInvissibilityTime;

	//posi ora pernei prin ksanakaneis ranged epithesi
	public float maxRangedCooldown;
	float currentRangedCooldown;

	//posi zoi exei o pextis
	public int maxHealth;
	int currentHealth;

	//oi podoi pou exei mazepsei
	//int score;

	//to gameobject pou dixnei pou eine to kedro tou kiklou pou tsekarei an patame sto edafos
	public Transform feet;
	//ta gameobject pou dixnoun pou ksekinaei ke pou telionei to tetragono tou melee attack
	public Transform swordStart;
	public Transform swordEnd;
	//ta gameobject pou dixnoun pou tha sxediastei i grami pou elenxei an iparxei tpt brosta sou	
	public Transform frontCheckStart;
	public Transform frontCheckEnd;
	//ta gameobject pou dixnoun pou tha sxediastei i grami pou elenxei an iparxei tpt piso sou	
	//public Transform backCheckStart;
	//public Transform backCheckEnd;
	//posi ora na perimeneis prin peis oti teliose to attack animation
	public float attackAnimationCooldown;
	float currentAttackAnimationCooldown;

	public Texture heartEmpty;
	public Texture heartHalf1;
	public Texture heartHalf2;


	public LayerMask platformLayer;
	public LayerMask enemyLayer;

	//kathe pote tha anavosvinei otan ton xtipane
	public float hurtBlinkingSpeed;
	//xrisimopiite gia na tsekarei an irthe i ora na kanei blink o xaraktiras
	float currentBlinkingTimer;

	//to prefab gia tin sfera
	public GameObject bullet;

	//apothikevei tin taxitita otan ginete pause, ke tin ksanadinei meta
	Vector2 currentVelocity;

	bool isPaused=false;
	bool facingRight = true;
	bool grounded=false;
	bool isAttacking=false;
	bool isClimbingLadder=false;
	bool inDialogue=false;

	bool touchingFront=false;
	//bool touchingBack=false;

	bool inFrontofLadder=false;

	GameObject meter;


	//o collider tou stoxou pou petixe i melee
	Collider2D meleeTarget;

	bool canDoubleJump = false;
	//to koubi gia na paei deksia
	public KeyCode rightKey;
	//to koubi gia na paei aristera
	public KeyCode leftKey;
	//to koubi gia na pidaei
	public KeyCode jumpKey;
	//to koubi gia melee
	public KeyCode meleeKey;
	//to koubi gia ranged
	public KeyCode rangedKey;


	public KeyCode climbUpKey;
	public KeyCode climbDownKey;

	public AudioClip swordAttack;
	public AudioClip fireBall;
	public AudioClip jumpSound;

	//i taxitita me tin opia theloume na peksei to animation se afto to frame
	int animationSpeed;

	Animator[] characterAnims;

	// Use this for initialization
	void Start () 
	{
		audioMan = GetComponent<AudioSource> ();
		animationSpeed = 1;
		//an iparxei apothikevmeno score, to pernei. Allios arxizei me score 0
		//if(PlayerPrefs.HasKey("Score"+gameManagerScript.savefile))
			//score = PlayerPrefs.GetInt("Score"+gameManagerScript.savefile);
		//else
			//score=0;
		currentBlinkingTimer = 0;
		currentHealth = maxHealth;
		currentRangedCooldown = 0;
		currentInvissibilityTime = 0;
		//to characterAnim eine o animator tou xaraktira
		characterAnims = GetComponentsInChildren<Animator> ();
	}

	void Update()
	{
		//an to pexnidi eine paused, min kaneis tpt
		if((!isPaused)&&(!inDialogue))
		{
			currentAttackAnimationCooldown=cooldown(currentAttackAnimationCooldown);
			if(currentAttackAnimationCooldown<=0)
				isAttacking=false;
			//an o xaraktiras exei xtipithei, ke exei perasei hurtBlinkingSpeed, tote tha anapsei
			//i tha svisei
			checkBlinking();

			checkTouchingSides();
			//dimiourgei ena kiklo sto gameobject feet, megethous feetradius, ke elenxei an akoubaei kapio adikimeno pou eine sto layer "ground"
			grounded=Physics2D.OverlapCircle(feet.position,feetRadius,platformLayer);

			//an o xaraktiras akoubaei to ground, tote exei tin ikanotita gia double jump
			if (grounded==true)
				canDoubleJump=true;

			//kounaei to sprite deksia i aristera
			if (Input.GetKey (rightKey))
			{
				//an kitas deksia ke den se blokarei tpt, i kitas aristera, proxora deksia
				if(((facingRight)&&(!touchingFront))||(!facingRight))
					GetComponent<Rigidbody2D>().velocity = new Vector2 (Speed, GetComponent<Rigidbody2D>().velocity.y);
			}
			else if (Input.GetKey (leftKey))
			{
				//an koitas aristera ke den se blokarei tpt, i kitas deksia,proxora aristera
				if(((!facingRight)&&(!touchingFront))||(facingRight))
					GetComponent<Rigidbody2D>().velocity = new Vector2 (-Speed, GetComponent<Rigidbody2D>().velocity.y);
			}
			else
				GetComponent<Rigidbody2D>().velocity = new Vector2 (0,GetComponent<Rigidbody2D>().velocity.y);

			//kanei to sprite na pidiksei
			if (Input.GetKeyDown(jumpKey)) 
			{
				animationSpeed=1;
				//an itan stin skala otan protopidikse, katevenei apo tin skala ke borei na pidiksei dio fores
				if(isClimbingLadder)
				{
					isClimbingLadder=false;
					canDoubleJump=true;
				}
				jump();
			}

			//molis patisei to W brosta stin skala, ksekinaei to "climb mode"
			if((inFrontofLadder)&&((Input.GetKeyDown(climbUpKey))||(Input.GetKeyDown(climbDownKey))))
			{
				isClimbingLadder=true;
			}
			//an den eine brosta apo skala, tote exei teliosei to "climb mode"
			if(!inFrontofLadder)
				isClimbingLadder=false;
			//kane ito sprite na skarfalonei
			if(isClimbingLadder)
			{
				//oso skarfalonei, den to epireazei i varitita
				GetComponent<Rigidbody2D>().gravityScale=0;
				//oso den pataei koubia, den pezei animation
				if((!Input.GetKey(climbDownKey))&&(!Input.GetKey(climbUpKey))&&(!Input.GetKey(rightKey))&&(!Input.GetKey(leftKey)))
					animationSpeed=0;
				else
					animationSpeed=1;
				//oso exei patimeno to W anevenei. otan to afinei stamataei
				if(Input.GetKey(climbUpKey))
					GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x,climbingSpeed);
				else if(Input.GetKey(climbDownKey))
					GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x,-climbingSpeed);
				else
					GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x,0);
			}
			else
			{
				GetComponent<Rigidbody2D>().gravityScale=7.4f;
			}

			//kanei to sprite na xtipisei
			if (Input.GetKeyDown(meleeKey))
			{
				currentAttackAnimationCooldown=attackAnimationCooldown;
				isAttacking=true;
				meleeAttack();
			}
			//kanei to sprite na xtipisei me ranged
			if (Input.GetKeyDown(rangedKey))
			{
				rangedAttack();
			}

			currentInvissibilityTime=cooldown (currentInvissibilityTime);
			currentRangedCooldown=cooldown(currentRangedCooldown);

			calculateRangedMeter();

			//elenxei an prepi na fliparistei to sprite
			if (GetComponent<Rigidbody2D>().velocity.x > 14 && !facingRight)
				Flip ();
			else if (GetComponent<Rigidbody2D>().velocity.x < -14 && facingRight)
				Flip ();
		}
		foreach (Animator anim in characterAnims)
		{
			if((anim.name=="torso")||(anim.name=="legs"))
			{
				//vazei tin taxitita tou character stin metavliti speed tou animator tou
				anim.SetFloat ("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));
				
				//leei ston animator an o xaraktiras peftei
				anim.SetFloat ("verticalSpeed", GetComponent<Rigidbody2D>().velocity.y);
				
				//an akoubaei o pextis to deksi i aristero side kapiou adikimenou, tha girisei sto idle animation
				anim.SetBool("isGrounded",grounded);
				
				//an akoubaei o pextis to deksi i aristero side kapiou adikimenou, tha girisei sto idle animation
				anim.SetBool("isClimbingLadder",isClimbingLadder);
				
				//kanei melee attack
				anim.SetBool ("isAttacking", isAttacking);
				
				//dinei sto animation tin taxitita pou prepei na exei
				anim.speed=animationSpeed;
			}
		}
	}
	
	void checkBlinking()
	{
		//vriskei ta gameobjects tou torso ke legs, ke an exei xtipithei o pextis, anavosvinoun
		GameObject torso= GameObject.Find ("torso");
		GameObject legs= GameObject.Find ("legs");

		if((currentInvissibilityTime>0)&&(Time.time>=currentBlinkingTimer+hurtBlinkingSpeed))
		{
			currentBlinkingTimer=Time.time;
			if(torso.GetComponent<Renderer>().enabled==true)
			{
				torso.GetComponent<Renderer>().enabled=false;
				legs.GetComponent<Renderer>().enabled=false;
			}
			else
			{
				torso.GetComponent<Renderer>().enabled=true;
				legs.GetComponent<Renderer>().enabled=true;
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

	//stelnei sto meter ta values pou xriazete gia na kserei poso megalo prepei na eine
	void calculateRangedMeter()
	{
		meter=GameObject.FindGameObjectWithTag ("Meter");
		meter.SendMessage ("getRangedCooldown",new Vector2(currentRangedCooldown,maxRangedCooldown),SendMessageOptions.DontRequireReceiver);
	}

	//kanei ton pexti na xoropidiksei
	void jump()
	{
		

		//an pataei stin gi, tote apla xoropidaei
		if (grounded==true)
		{
			audioMan.clip = jumpSound;
			audioMan.Play ();
			GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x,jumpSpeed);
		}
		else 
			//an eine idi ston aera, tote xrisimopoiei to double jump tou
			if (canDoubleJump==true)
			{
				audioMan.clip = jumpSound;
				audioMan.Play ();
				GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x,jumpSpeed);
				canDoubleJump=false;
			}
	}

	//ti ginete otan o pextis kanei mia melee attack
	void meleeAttack()
	{
		audioMan.clip = swordAttack;
		audioMan.Play ();
		//kathorizei to megethos tous spathiou, analoga me to melee range
		swordStart.localPosition = new Vector2 (1f, 0.5f);
		swordEnd.localPosition = new Vector2 (meleeRange, -1);

		//elenxei an o adipalos eine mesa sto spathi, ke an eine tou stelnei message me to damage pou efage
		meleeTarget=Physics2D.OverlapArea(swordStart.position,swordEnd.position,enemyLayer);
		if(meleeTarget)
			meleeTarget.SendMessage("gotHit", meleeDamage);
	}

	//ti ginete otan o pextis kanei mia ranged attack
	void rangedAttack()
	{

		if(currentRangedCooldown==0)
		{
			if(bullet.name=="Fireball")
			{
				audioMan.clip=fireBall;
				audioMan.Play ();
			}
			//dimiourgei ena object gia to projectile, ke tou dinei onoma analoga pros ta pou paei
			if(facingRight)
			{
				Object bulletInstance=Instantiate (bullet, new Vector2(transform.position.x + 3f, transform.position.y+1f),transform.rotation);
				bulletInstance.name="bullet(right)";

			}
			else
			{
				Object bulletInstance=Instantiate (bullet, new Vector2(transform.position.x - 3f, transform.position.y+1f),transform.rotation);
				bulletInstance.name="bullet(left)";
			}
			//ksekinaei to cooldown gia tin epomeni ranged epithesi
			currentRangedCooldown=maxRangedCooldown;
		}
	}
	
	//fliparei to sprite
	void Flip ()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	//elenxei an o pextis eine brosta apo kapia skala
	void laddercheck(bool check)
	{
		inFrontofLadder = check;
	}

	//ti ginete otan o pextis trooei damage
	void gotHit(int damage)
	{
		//an den eine invissible, tote troei damage, allios to agnoei
		if (currentInvissibilityTime == 0) 
		{
			currentInvissibilityTime = maxInvissibilityTime;
			currentHealth-=damage;
			if(currentHealth<=0)
				Application.LoadLevel(Application.loadedLevel);
		} 
	}

	void gotPowerUp(GameObject powerUp)
	{
		bullet = powerUp;

	}

	//elenxei an iparxei kati brosta i piso sou
	void checkTouchingSides()
	{
		//sxediazei ena tetragono brosta tou ke elenxei an iparxoun colliders mesa tou
		Collider2D front=Physics2D.OverlapArea(frontCheckStart.position,frontCheckEnd.position);
		// an iparxei kati brosta sou ke den eine trigger, tote to touchingFront tha eine true
		if(front)
		{
			if ((front.gameObject.tag!="Player")&&(front.isTrigger==false))
				touchingFront = true;
			else
				touchingFront = false;
		}
		else
			touchingFront = false;

		/*//sxediazei ena tetragono piso tou ke elenxei an iparxoun colliders mesa tou
		Collider2D back=Physics2D.OverlapArea(backCheckStart.position,backCheckEnd.position);
		// an iparxei kati piso sou ke den eine trigger, tote to touchingBack tha eine true
		if(back)
		{
			if ((back.gameObject.tag!="Player")&&(back.isTrigger==false))
				touchingBack = true;
			else
				touchingBack = false;
		}
		else
			touchingBack = false;*/
	}

	//tou stelnei minima o levelManager me to pou eine to checkpoint pou tha ksanaemfanistei
	void teleportToCheckPoint(Vector3 position)
	{
		transform.position = position;
	}

	//giatrevei ton pexti
	void gainHealth(int cure) 
	{
		if (currentHealth + cure < maxHealth)
			currentHealth += cure;
		else if(currentHealth<maxHealth)
			currentHealth=maxHealth;

	}

	//prosthetei podous ston pexti
	/*void gainPoints(int extraPoints)
	{
		score += extraPoints;
		//vriskei to gameobject me to gui tou score ke tou dinei to score
		GameObject tempGO = GameObject.FindGameObjectWithTag ("scoreText");
		tempGO.SendMessage("getScore", score);

	}*/
	//apothikevei to score otan akoubaei ena checkpoint
	/*void saveScore()
	{
		PlayerPrefs.SetInt ("Score" + gameManagerScript.savefile,score );
		PlayerPrefs.Save ();
	}*/

	//zografizei to GUI se kathe frame
	void OnGUI () 
	{
		//kardies
		int offset=0;
		Texture x;
		for(int i=0;i<maxHealth/2;i++)
			GUI.DrawTexture(new Rect(0+i*40,0,30,30),heartEmpty);
		for(int i=0;i<currentHealth;i++)
		{
			if(i%2==0)
				x=heartHalf1;
			else
				x=heartHalf2;
			if((i>1)&&(i%2==0))
				offset+=10;
			GUI.DrawTexture(new Rect(15*i+offset,0,15,30),x);
		}

	}

	//otan ginete pause
	void OnPauseGame()
	{
		currentVelocity = GetComponent<Rigidbody2D>().velocity;
		GetComponent<Rigidbody2D>().isKinematic = true;
		animationSpeed=0;
		isPaused=true;
	}

	void dialogueStart()
	{
		inDialogue = true;
		//currentVelocity = rigidbody2D.velocity;
		GetComponent<Rigidbody2D>().isKinematic = true;
		animationSpeed=0;
	}

	void dialogueEnd()
	{
		inDialogue = false;
		GetComponent<Rigidbody2D>().isKinematic = false;
		//rigidbody2D.velocity = currentVelocity;
		animationSpeed=1;
	}
	
	//otan kseginete pause
	void onUnPauseGame()
	{
		GetComponent<Rigidbody2D>().isKinematic = false;
		GetComponent<Rigidbody2D>().velocity = currentVelocity;
		animationSpeed=1;
		isPaused = false;
	}
}
