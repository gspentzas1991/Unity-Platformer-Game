using UnityEngine;
using System.Collections;

/* Contains the behaviour for the robot enemy
 * The robot enemy will walk from left to right until something blocks its way, or the platform ends
 * at that point the robot turns around. It has a child gameObject named robotLOS, that is a trigger 
 * collider in front of the robot. When the player enters the robotLOS, the robot gets notified,
 * stops moving and starts firing at the player. When the player leaves the robotLOS, the robot resumes moving
 * Everytime it finds the player, it creates an Alert icon on top of it
 */
public class robotBehavior : MonoBehaviour {
	
	public float speed;
	public int damage;
    //How long the enemy will wait before attacking again
	public float maxRangedAttackCooldown;
	float currentRangedAttackCooldown;
	//For how long the enemy freezes after being hit
	public float maxFreezeTime;
    float currentFreezeTimer;
    public int maxHealth;
	int currentHealth;
    bool isPaused = false;
    bool foundPlayer = false;
    public LayerMask platformLayer;
	public GameObject alert;
	public GameObject lazer;
    //A position in front of the enemy
	public Transform frontSide;
    //Uses the two transforms to create a Raycast in front of it, to see if its path is blocked
    public Transform frontCheckStart;
	public Transform frontCheckEnd;
	Animator characterAnimator;
	AudioSource audioManager;
	public AudioClip laserSound;
    Rigidbody2D enemyRigidbody;
	
	// Use this for initialization
	void Start () 
	{
        enemyRigidbody = GetComponent<Rigidbody2D>();
        audioManager = GetComponent<AudioSource> ();
        currentFreezeTimer = 0;
        currentRangedAttackCooldown = 0;
		characterAnimator = GetComponent<Animator>();
		currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!isPaused)
		{
            currentFreezeTimer = gameManagerScript.gameManager.Cooldown(currentFreezeTimer);
            currentRangedAttackCooldown = gameManagerScript.gameManager.Cooldown(currentRangedAttackCooldown);
            if(currentFreezeTimer==0)
                CheckForRangedAttack();
            //Checks if there is a platform in front of it. If there isn't it turns around
			bool frontIsGrounded=Physics2D.OverlapCircle(frontSide.position,0.2f,platformLayer);
			if(!frontIsGrounded)
			{
				speed=-speed;
                gameManagerScript.gameManager.FlipTransform(transform);
			}

		}
	}

    private void FixedUpdate()
    {
        if ((currentFreezeTimer == 0) && (!foundPlayer))
            enemyRigidbody.velocity = new Vector2(speed, enemyRigidbody.velocity.y);
        else
            enemyRigidbody.velocity = new Vector2(0, enemyRigidbody.velocity.y);
    }
    
	


    /* If the player touches the enemy, he gets hurt
     */
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.name == "Player")
        {
            collision.collider.gameObject.SendMessage("GotHit", damage/2);
        }
    }


    /* Checks if the enemy needs to make a ranged attack
    * Depending where the character is facing, it will create a "bullet(left)" or "bullet(right)" gameobject in front of him
    * The bullet's script will assign the correct velocity, depending on its name (left or right)
    */
    void CheckForRangedAttack()
    {
        if ((currentRangedAttackCooldown == 0) && (foundPlayer))
        {
            audioManager.clip = laserSound;
            audioManager.Play();
            string lazerName = "bullet(left)";
            if (speed > 0)
                lazerName = "bullet(right)";
            Object lazerInstance = Instantiate(lazer, new Vector2(transform.position.x - 2f, transform.position.y), transform.rotation);
            lazerInstance.name = lazerName;
            currentRangedAttackCooldown = maxRangedAttackCooldown;
        }
    }

    /* Checks if the player has entered or exited its line of sight and creates an alert icon over its head
     * The child gameobject "robotLOS" calls this method with a flag, depending
     * if the player entered, or exited its trigger collider
     */
    public void GetPlayerSearchStatus(bool detectedPlayer)
	{
        foundPlayer = detectedPlayer;
        if (foundPlayer)
        {
            Object alertInstance = Instantiate(alert, new Vector2(transform.position.x, transform.position.y + 2.4f), transform.rotation);
        }
		characterAnimator.SetBool("isShooting",foundPlayer);
	}
    
    /* What happens when the enemy is hurt
     */ 
    void GotHit(int damage)
    {
        currentFreezeTimer = maxFreezeTime;
        currentHealth -= damage;
        if (currentHealth <= 0)
            Destroy(this.gameObject);
    }
    
    void OnPauseGame()
	{
		enemyRigidbody.isKinematic = true;
		isPaused = true;
	}
	
	void OnUnPauseGame()
	{
		enemyRigidbody.isKinematic = false;
		isPaused = false;
	}
	
}
