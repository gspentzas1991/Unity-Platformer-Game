 using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class playerControlScript : MonoBehaviour {

	AudioSource playerAudioSource;
	public float runningSpeed;
	public float climbingSpeed;
	public float jumpSpeed;
	public int meleeDamage;
    //How many seconds the player will be invincible after getting hit
    public float maxInvincibilityTime;
    //The timer counting down the remaining seconds of player invincibility
    float invincibilityTimer;
	//How many seconds is the ranged attack cooldown
	public float maxRangedCooldown;
    //The timer counting down the remaining seconds of the ranged attack cooldown
	float rangedCooldownTimer;
	public int maxHealth;
	int currentHealth;
    //The calculated velocity the player will have after FixedUpdate finishes
    Vector2 newPlayerVelocity;
    public Transform playerFeetPosition;
    //The starting and ending point of the melee attack hitbox
    public Transform meleeHitboxStart;
	public Transform meleeHitboxEnd;
    //Using those two points we detect if there is a wall in front of the player
    public Transform frontCheckStart;
	public Transform frontCheckEnd;
    //How long the melee attack hitbox stays out, after the attack button is pressed
	public float meleeAttackDuration;
	float meleeAttackDurationTimer;
	public Texture emptyHeartContainer;
	public Texture rightHalfOfHeart;
	public Texture leftHalfOfHeart;
	public LayerMask platformLayer;
	public LayerMask enemyLayer;
	//How fast the character sprite blinks when he is hurt (in seconds)
	public float hurtBlinkingSpeed;
	//The time when the previous frame was drawn
	float timeOnPreviousFrame;
	public GameObject rangedWeapon;
    public Renderer playerTorsoRenderer;
    public Renderer playerLegsRenderer;
    public Meter rangedCooldownMeter;
	bool gameIsPaused=false;
	bool facingRight = true;
	bool isTouchingGround=false;
	bool isAttacking=false;
	bool isClimbingLadder=false;
	bool inDialogue=false;
    //Flag, true if another gameobject is in front of the player
	bool wallInFrontOfPlayer=false;
	bool inFrontofLadder=false;
    bool canDoubleJump = false;
	public KeyCode rightKey;
	public KeyCode leftKey;
	public KeyCode jumpKey;
	public KeyCode meleeAttackKey;
	public KeyCode rangedAttackKey;
	public KeyCode climbUpKey;
	public KeyCode climbDownKey;
	public AudioClip swordAttack;
	public AudioClip fireBall;
	public AudioClip jumpSound;
	//The speed in which the sprite's animations play
    //Used to pause and resume the animation (0 or 1)
	int animationSpeed;
    //An array of animators that will contain the torso and leg animator
	Animator[] characterAnimator;
    Rigidbody2D playerRigidbody;
    
	void Start () 
	{

		playerAudioSource = GetComponent<AudioSource>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        characterAnimator = GetComponentsInChildren<Animator>();
        animationSpeed = 1;
		timeOnPreviousFrame = 0;
		currentHealth = maxHealth;
		rangedCooldownTimer = 0;
		invincibilityTimer = 0;
        TeleportToLastActivatedCheckpoint();
    }

	void Update()
	{
		if((!gameIsPaused)&&(!inDialogue))
		{
			meleeAttackDurationTimer=gameManagerScript.gameManager.Cooldown(meleeAttackDurationTimer);
			if(meleeAttackDurationTimer<=0)
				isAttacking=false;
            //Looks for platforms around the Player's feet, to see if the player is touching the ground or not
            isTouchingGround = Physics2D.OverlapCircle(playerFeetPosition.position, 0.2f, platformLayer);
            if (isTouchingGround == true)
                canDoubleJump = true;
			CheckTouchingSides();
            CheckBlinking();
            CheckForMeleeAttack();
            CheckForRangedAttack();
            CheckForMovent();
            CheckForLadderMovement();
            invincibilityTimer = gameManagerScript.gameManager.Cooldown(invincibilityTimer);
			rangedCooldownTimer= gameManagerScript.gameManager.Cooldown(rangedCooldownTimer);
			SendRangedMeter();
            SetAnimatorVariables();
        }

    }

    //The new velocity is prepared from the Update() function, and is applied here
    private void FixedUpdate()
    { 
        playerRigidbody.velocity = newPlayerVelocity;
    }

    /* Checks if a movement key is pressed, and prepares the new velocity
     */
    void CheckForMovent()
    {
        float newPlayerHorizontalVelocity = 0f;
        float newPlayerVerticalVelocity = playerRigidbody.velocity.y;

        if (Input.GetKey(rightKey))
        {
            if (((facingRight) && (!wallInFrontOfPlayer)) || (!facingRight))
                newPlayerHorizontalVelocity = runningSpeed;
        }
        else if (Input.GetKey(leftKey))
        {
            if (((!facingRight) && (!wallInFrontOfPlayer)) || (facingRight))
                newPlayerHorizontalVelocity =- runningSpeed;

        }

        if (Input.GetKeyDown(jumpKey))
        {
            bool playerWillJump = false;
            if (isClimbingLadder)
            {
                playerWillJump = true;
                isClimbingLadder = false;
                
            }
            if (isTouchingGround == true)
            {
                playerWillJump = true;
            }
            else if (Input.GetKeyDown(jumpKey))
            {
                if (canDoubleJump == true)
                {
                    playerWillJump = true;
                    canDoubleJump = false;
                }
            }
            if (playerWillJump)
            {
                playerAudioSource.clip = jumpSound;
                playerAudioSource.Play();
                newPlayerVerticalVelocity = jumpSpeed;
            }
        }
        newPlayerVelocity = new Vector2(newPlayerHorizontalVelocity, newPlayerVerticalVelocity);
        
        //Checks if the player sprite should flip
        if (playerRigidbody.velocity.x > 14 && !facingRight)
            Flip();
        else if (playerRigidbody.velocity.x < -14 && facingRight)
            Flip();
    }


    /* Checks if ladder climb key is pressed, and prepares the new velocity
     * This inFrontOfLadder variable is changed by the ladderScript script
     */
    void CheckForLadderMovement()
    {
        if (!inFrontofLadder)
        {
            isClimbingLadder = false;
        }
        else if ((Input.GetKeyDown(climbUpKey)) || (Input.GetKeyDown(climbDownKey)))
        {
            isClimbingLadder = true;
        }

        if (isClimbingLadder)
        {
            playerRigidbody.gravityScale = 0;
            //The player climbing animation will only play when the player is pressing a climb key
            if ((!Input.GetKey(climbDownKey)) && (!Input.GetKey(climbUpKey)))
                animationSpeed = 0;
            else
                animationSpeed = 1;

            if (Input.GetKey(climbUpKey))
                newPlayerVelocity = new Vector2(playerRigidbody.velocity.x, climbingSpeed);
            else if (Input.GetKey(climbDownKey))
                newPlayerVelocity = new Vector2(playerRigidbody.velocity.x, -climbingSpeed);
            else
                newPlayerVelocity = new Vector2(playerRigidbody.velocity.x, 0);
        }
        else
        {
            playerRigidbody.gravityScale = 7.4f;
            animationSpeed = 1;
        }

    }


    /* Checks for the melee attack input, creates a melee attack hitbox and sends a message to any enemy gameObject it hits
     */
    void CheckForMeleeAttack()
    {
        if (Input.GetKeyDown(meleeAttackKey))
        {
            meleeAttackDurationTimer = meleeAttackDuration;
            isAttacking = true;
            playerAudioSource.clip = swordAttack;
            playerAudioSource.Play();
            Collider2D meleeHitboxTarget = Physics2D.OverlapArea(meleeHitboxStart.position, meleeHitboxEnd.position, enemyLayer);
            if (meleeHitboxTarget)
                meleeHitboxTarget.SendMessage("GotHit", meleeDamage);
        }
    }

    /* Checks for the ranged attack input, creates and creates a bullet GameObject
     * Depending where the character is facing, it will create a "bullet(left)" or "bullet(right)" gameobject in front of him
     * The bullet's script will assign the correct velocity, depending on its name (left or right)
     */
    void CheckForRangedAttack()
    {
        if (Input.GetKeyDown(rangedAttackKey))
        {
            if (rangedCooldownTimer == 0)
            {
                int bulletDirection = 1;
                string bulletName = "bullet(right)";
                if (!facingRight)
                {
                    bulletDirection = -1;
                    bulletName = "bullet(left)";

                }
                Object bulletInstance = Instantiate(rangedWeapon, new Vector2(transform.position.x+bulletDirection*3f, transform.position.y+1f), transform.rotation);
                bulletInstance.name = bulletName;
                rangedCooldownTimer = maxRangedCooldown;
            }
        }
    }


    /* The player's renderer starts blinking after he's hurt
     * The blinking lasts while he has invincibility, and happens at the speed of hurtBlinkingSpeed
     */
    void CheckBlinking()
	{
        //When [hurtBlinkingSpeed] seconds pass from the previous frame, changes the blinking state
		if((invincibilityTimer>0)&&(Time.time>=timeOnPreviousFrame+hurtBlinkingSpeed))
		{
			timeOnPreviousFrame=Time.time;
			if(playerTorsoRenderer.enabled==true)
			{
                playerTorsoRenderer.enabled=false;
				playerLegsRenderer.enabled=false;
			}
			else
			{
                playerTorsoRenderer.enabled=true;
                playerLegsRenderer.enabled=true;
			}
		}
        else if(invincibilityTimer<=0)
        {
            playerTorsoRenderer.enabled = true;
            playerLegsRenderer.enabled = true;
        }
	}


    //Cheks if there is a collider in front of the player
    void CheckTouchingSides()
    {
        Collider2D front = Physics2D.OverlapArea(frontCheckStart.position, frontCheckEnd.position);
        if (front)
        {
            if ((front.gameObject.tag != "Player")&&(front.gameObject.tag!="Pushable") && (front.isTrigger == false))
                wallInFrontOfPlayer = true;
            else
                wallInFrontOfPlayer = false;
        }
        else
            wallInFrontOfPlayer = false;
    }

    //Flips the player's transform on the X-axis
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

    }


    /* Teleports to the last activated checkpoint
     */
    void TeleportToLastActivatedCheckpoint()
    {
        //Since the main menu is the first scene, we must decrese it by 1 to get the real level number
        int levelNumber = SceneManager.GetActiveScene().buildIndex - 1;
        int checkPointNumber = 0;
        /* if there is save data for that level, adds the active checkpoint number on the target checkpoint
         */
        if (PlayerPrefs.GetInt("levelNumber" + gameManagerScript.gameManager.savefile) == levelNumber)
        {
            if (PlayerPrefs.HasKey("checkPointNumber" + gameManagerScript.gameManager.savefile))
                checkPointNumber += PlayerPrefs.GetInt("checkPointNumber" + gameManagerScript.gameManager.savefile);

        }
        //finds the gameobject of the target checkpoint (by name), and deleports to it
        transform.position = GameObject.Find("checkPoint" + checkPointNumber).transform.position;

    }


    /* Draws the health gui
     */
    void OnGUI()
    {
        int spaceBetweenHearts = 0;
        Texture x;
        for (int i = 0; i < maxHealth / 2; i++)
            GUI.DrawTexture(new Rect(0 + i * 40, 0, 30, 30), emptyHeartContainer);
        for (int i = 0; i < currentHealth; i++)
        {
            if (i % 2 == 0)
            {
                x = rightHalfOfHeart;
                //after the first heart, there should be a space between each heart
                if (i > 1)
                    spaceBetweenHearts += 10;
            }
            else
                x = leftHalfOfHeart;
            GUI.DrawTexture(new Rect(15 * i + spaceBetweenHearts, 0, 15, 30), x);
        }

    }
    /* Sets the variables of the torso and legs animators
     */
    void SetAnimatorVariables()
    {
        foreach (Animator anim in characterAnimator)
        {
            if ((anim.name == "torso") || (anim.name == "legs"))
            {
                anim.SetFloat("Speed", Mathf.Abs(playerRigidbody.velocity.x));
                anim.SetFloat("verticalSpeed", playerRigidbody.velocity.y);
                anim.SetBool("isGrounded", isTouchingGround);
                anim.SetBool("isClimbingLadder", isClimbingLadder);
                anim.SetBool("isAttacking", isAttacking);
                anim.speed = animationSpeed;
            }
        }
    }

	//Sends the rangedCooldownTimer to the ranged meter gameobject
	void SendRangedMeter()
	{
        rangedCooldownMeter.GetRangedCooldown(new Vector2(rangedCooldownTimer, maxRangedCooldown));
	}
    
    //The following methods are message receivers from other GameObjects

	/* Gets the message that the player is or isn't in front of a ladder, and changes the flag accordingly
     * The ladder GameObject sends the message to the player, when he enters and exits its trigger collider
     */
	public void LadderCheck(bool check)
	{
		inFrontofLadder = check;
	}

	/* Gets the message that the player got hit by an enemy
     * The enemy or projectile sends the message to the player when they collide with him
     * If the player dies, the scene gets reloaded
     */
	void GotHit(int damage)
	{
		
		if (invincibilityTimer == 0) 
		{
			invincibilityTimer = maxInvincibilityTime;
			currentHealth-=damage;
            if (currentHealth <= 0)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		} 
	}
    /* Gets the message that the player picked up a powerup
     * The powerup sends the message when the player enters its trigger collider
     * Changes his ranged weapon to the picked up GameObject
     */
	void GotPowerUp(GameObject powerUp)
	{
		rangedWeapon = powerUp;
	}


   
	/* Gets a message to heal a specific ammount
     * The healing object or checkpoint when the player enters their trigger collider
     */
	void GainHealth(int cure) 
	{
		if (currentHealth + cure < maxHealth)
			currentHealth += cure;
		else if(currentHealth<maxHealth)
			currentHealth=maxHealth;

	}
    

    /* Gets the message from the GameManager GameObject that the game is paused
     */
    void OnPauseGame()
	{
        //saves the current player velocity, to apply it after the game unpauses
		newPlayerVelocity = playerRigidbody.velocity;
		playerRigidbody.isKinematic = true;
		animationSpeed=0;
		gameIsPaused=true;
	}

    /* Gets the message from the GameManager GameObject that the game is out of pause
     */
    void OnUnPauseGame()
    {
        playerRigidbody.isKinematic = false;
        animationSpeed = 1;
        gameIsPaused = false;
    }

    /* Gets the message from the GameManager GameObject that the player is in Dialogue
     */
    void DialogueStart()
	{
		inDialogue = true;
        newPlayerVelocity = new Vector2(0, 0);
        playerRigidbody.isKinematic = true;
        animationSpeed = 0;
        SetAnimatorVariables();

    }

    /* Gets the message from the GameManager GameObject that the player is out of Dialogue
     */
    void DialogueEnd()
	{
		inDialogue = false;
		playerRigidbody.isKinematic = false;
		animationSpeed=1;
	}
	
	
}
