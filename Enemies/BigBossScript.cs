using UnityEngine;
using System.Collections;

/* Contains the behaviour for the boss battle. The boss battle begins when the dialogue ends
 * The boss stands at a corner of the arena and shoots his gun
 * Every time the boss is hurt, he has a random chance to do a Charge attack or a bomb attack
 * For a bomb attack he teleports to the ceiling, throws 3 bombs and teleports back down
 * For a charge attack, his collider becomes a trigger, and he runs to the other side of the map
 */
public class BigBossScript : MonoBehaviour {
    //The places that the boss can teleport to
	public GameObject topLeftPosition;
	public GameObject topRightPosition;
	public GameObject downLeftPosition;
	public GameObject downRightPosition;
    //The blink object that the boss leaves behind when he teleports
	public GameObject blink;
    public float chargeSpeed;
	public int maxHealth;
	int currentHealth;
    //How many bombs the boss can throw before he moves away
    public int maxNumberOfBombs;
    int currentNumberOfBombs;
    public float maxInvincibilityTime;
    float currentInvincibilityTimer;
    public float maxRangedAttackCooldown;
    float rangedAttackCooldownTimer;
    int animationSpeed;
	bool isPaused;
	bool inBattle;
	bool isCharging;
    bool isFacingRight = false;
    //The ranged weapons the boss can use
    public GameObject lazer;
	public GameObject bomb;
	//How fast the boss' sprite blinks when he is hurt
	public float hurtBlinkingSpeed;
    //The time when the previous frame was drawn
    float timeOfPreviousFrame;
	Animator bossAnimator;
	public Texture heartEmpty;
	public Texture heartLeftHalf;
	public Texture heartRightHalf;
	AudioSource audioManager;
	public AudioClip laserSound;
    Renderer bossRenderer;
    Collider2D bossCollider;
    Rigidbody2D bossRigidbody;

	// Use this for initialization
	void Start () 
	{
		audioManager = GetComponent<AudioSource> ();
        bossAnimator = GetComponent<Animator>();
        bossRenderer = GetComponent<Renderer>();
        bossCollider = GetComponent<Collider2D>();
        bossRigidbody = GetComponent<Rigidbody2D>();
		inBattle = false;
		isPaused = false;
		animationSpeed = 1;
		currentHealth = maxHealth;
        currentNumberOfBombs = maxNumberOfBombs;
		currentInvincibilityTimer = 0;
		timeOfPreviousFrame = 0;
	
	}
	
    void Update ()
    {
		currentInvincibilityTimer=gameManagerScript.gameManager.Cooldown(currentInvincibilityTimer);
		rangedAttackCooldownTimer=gameManagerScript.gameManager.Cooldown(rangedAttackCooldownTimer);
		CheckBlinking();
		if((!isCharging)&&(inBattle))
        {
            //If he is on the ground he fires his laser, if he's on the ceiling, he shoots bombs
            if (transform.position.y > 10)
                RangedAttack(bomb);
            else
                RangedAttack(lazer);
        }
        CheckIfCharging();
        SendAnimatorVariables();
	}
    
   /* When the boss is hit, his health decreases. He only takes 1 damage from any attack
    * Everytime he gets hurt, he teleports to a different part of the map
    */
	void GotHit(int damage)
	{
		damage = 1;
		if (currentInvincibilityTimer == 0) 
		{
			currentInvincibilityTimer = maxInvincibilityTime;
			currentHealth-=damage;
			if(currentHealth>0)
			{
				DecideMoveDestination();
			}
		}
		if (currentHealth <= 0)
		{
			Destroy (this.gameObject);
		}
	}


	//The boss randomly decides where to go
    //If the boss is on the ground, will either teleport on a part of the ceiling, or charge across the screen
    //If the boss is on the ceiling, he will either teleport on the left, or right part of the ground
	void DecideMoveDestination()
	{
		currentNumberOfBombs = maxNumberOfBombs;
		int random=Random.Range (0,3);
        //If the boss is on the ceiling
		if(transform.position.y>10)
		{
			//Chooses if the boss will teleport to the left or right of the stage
			if(random==1)
				Teleport(downLeftPosition);
			else
				Teleport(downRightPosition);
		}
        //If the boss is on the ground
		else
		{
			if(random==0)
                isCharging = true;
			if(random==1)
				Teleport(topLeftPosition);
			if(random==2)
				Teleport(topRightPosition);
			
		}
	}

    //When the Boss has invincibility, his sprite blinks every [hurtBlinkingSpeed] seconds
	void CheckBlinking()
	{
		
		if((currentInvincibilityTimer>0)&&(Time.time>=timeOfPreviousFrame+hurtBlinkingSpeed))
		{
			timeOfPreviousFrame=Time.time;
			if(bossRenderer.enabled==true)
				bossRenderer.enabled=false;
			else
				bossRenderer.enabled=true;
		}
		if (currentInvincibilityTimer == 0)
			bossRenderer.enabled = true;
	}

    //Creates a "Blink" gameObject and teleports the boss to the desired destination
	void Teleport (GameObject destination)
	{
        //resents the ranged attack cooldown, so the boss fires as soon as he teleports
		rangedAttackCooldownTimer = 0.2f;
		Object blinkInstance = Instantiate (blink, transform.position,transform.rotation);
		transform.position = destination.transform.position;
        //Flips the boss if necessary
        if (((transform.position.x < 40) && (!isFacingRight)) || ((transform.position.x > 20) && (isFacingRight)))
        {
            isFacingRight = !isFacingRight;
            gameManagerScript.gameManager.FlipTransform(transform); 
        }
	}

    //Performs the charing attack. Turns his collider to trigger mode, and starts running across the stage
    //When he reaches a corner, he enables his collider and Flips around
	void CheckIfCharging()
	{
        if(isCharging)
        {
            bossCollider.isTrigger = true;
            bossRigidbody.isKinematic = true;
            if (!isPaused)
            {
                float movingDirection = chargeSpeed*Time.deltaTime;
                if (!isFacingRight)
                {
                    movingDirection = -chargeSpeed*Time.deltaTime;
                }
                transform.position = new Vector2(transform.position.x + movingDirection, transform.position.y);
                //If the boss reaches his destination, he stops the charge
                if (((isFacingRight) && (transform.position.x > 50)) || ((!isFacingRight) && (transform.position.x < 10)))
                {
                    isCharging = false;
                    bossCollider.isTrigger = false;
                    bossRigidbody.isKinematic = false;
                    isFacingRight = !isFacingRight;
                    gameManagerScript.gameManager.FlipTransform(transform);
                }

            }
        }
	}

    //Performs a ranged attack with the equiped weapon
	void RangedAttack(GameObject bullet)
	{
		if(rangedAttackCooldownTimer==0)
		{
			if(bullet.name=="Bomb")
				currentNumberOfBombs--;
			if(bullet.name=="BossLazer")
			{
				audioManager.clip=laserSound;
				audioManager.Play ();
			}
            //Creates the bullet and names it depending on its direction
            string bulletName="bullet(right)";
            float bulletDistanceFromShooter = 2f;
			if(!isFacingRight)
			{
                bulletName = "bullet(left)";
                bulletDistanceFromShooter = -2f;
		    }
            Object bulletInstance = Instantiate(bullet, new Vector2(transform.position.x + bulletDistanceFromShooter, transform.position.y), bullet.transform.rotation);
            bulletInstance.name = bulletName;
            rangedAttackCooldownTimer = maxRangedAttackCooldown;            
            if (currentNumberOfBombs == 0)
                DecideMoveDestination();
        }
	}

    //Sets the animator values
    void SendAnimatorVariables()
    {

        bossAnimator.speed = animationSpeed;
        bossAnimator.SetBool("isCharging", isCharging);
        bossAnimator.SetFloat("verticalPosition", transform.position.y);
        bossAnimator.SetBool("inBattle", inBattle);
    }


    //Draws the boss's health on the top right
    void OnGUI()
    {
        if (inBattle)
        {
            int offset = 0;
            Texture x;
            for (int i = 0; i < maxHealth / 2; i++)
                GUI.DrawTexture(new Rect(Screen.width - maxHealth * 20 + i * 40, 0, 30, 30), heartEmpty);
            for (int i = 0; i < currentHealth; i++)
            {
                if (i % 2 == 0)
                    x = heartLeftHalf;
                else
                    x = heartRightHalf;
                if ((i > 1) && (i % 2 == 0))
                    offset += 10;
                GUI.DrawTexture(new Rect((15 * i + offset) + Screen.width - maxHealth * 20, 0, 15, 30), x);
            }
        }

    }

    //When the boss touches the player, he hurts him
    void OnTriggerEnter2D(Collider2D targetCollider)
    {
        if (targetCollider.gameObject.tag == "Player")
        {
            targetCollider.SendMessage("GotHit", 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D targetCollision)
    {
        if (targetCollision.gameObject.tag == "Player")
        {
            targetCollision.gameObject.SendMessage("GotHit", 1);
        }
    }

    //Receives messages from the dialogue gameObject when the player is in/out of dialogue

    void DialogueStart()
    {
        animationSpeed = 0;
    }

    void DialogueEnd()
    {
        animationSpeed = 1;
        inBattle = true;
    }

    //Receives messages from the gameManager when the game is paused/unpaused
    void OnPauseGame()
	{
		animationSpeed=0;
		isPaused=true;
	}
	
	void OnUnPauseGame()
	{
		animationSpeed=1;
		isPaused = false;
	}
}
