using UnityEngine;
using System.Collections;

public class zombieBehavior : MonoBehaviour {

	public float speed;
	public int damage;
	public float legRadius;
	bool isPaused=false;
	bool backSideIsGrounded=true;
	bool frontSideIsGrounded=true;
	//For how long the enemy is stunned after getting hurt (in seconds)
	public float maxFreezeTime;
	float currentFreezeTime=0;
	public int maxHealth;
	int currentHealth;
    Rigidbody2D enemyRigidbody;
    public LayerMask platformLayer;
	public Transform frontSide; 
	public Transform backSide;
    //Creates a raycast with the two transforms
    public Transform frontCheckRaycastStart;
    public Transform frontCheckRaycastEnd;

    void Start () 
	{
        enemyRigidbody = GetComponent<Rigidbody2D>();
		currentHealth = maxHealth;
	}
	
	void Update () 
	{
		if(!isPaused)
        {
            CheckIfFrontIsGrounded();
            CheckIfPathIsBlocked();           
            currentFreezeTime = gameManagerScript.gameManager.Cooldown(currentFreezeTime);
            
        }
    }

    private void FixedUpdate()
    {
        if (currentFreezeTime > 0)
        {
            enemyRigidbody.velocity = new Vector2(0, 0);
        }
        else
        {
            enemyRigidbody.velocity = new Vector2(speed, enemyRigidbody.velocity.y);
        }
    }

    /* Checks if the front of the enemy is in the air
     * If it is, the enemy turns around
     */
    void CheckIfFrontIsGrounded()
    {
        backSideIsGrounded = Physics2D.OverlapCircle(backSide.position, legRadius, platformLayer);
        frontSideIsGrounded = Physics2D.OverlapCircle(frontSide.position, legRadius, platformLayer);
        if ((!frontSideIsGrounded) && (backSideIsGrounded))
        {
            Flip();
            speed = -speed;
        }
    }

    /* Checks if there are colliders in front of the enemy
     * If there are it turns around, and if it touches the player, it hurts it
     */
    void CheckIfPathIsBlocked()
    {
        RaycastHit2D[] hitObjectsArray = Physics2D.RaycastAll(frontCheckRaycastStart.position, frontCheckRaycastEnd.position, 0.1f);
        foreach (RaycastHit2D hitObjects in hitObjectsArray)
        {
            if (!hitObjects.collider.isTrigger)
            {
                gameManagerScript.gameManager.FlipTransform(transform);
                speed = -speed;
                if (hitObjects.collider.name == "Player")
                {
                    hitObjects.collider.gameObject.SendMessage("GotHit", damage);
                }
            }
        }
    }

    /* If the player touches the enemy, he gets hurt
     */ 
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.name == "Player")
        {
            collision.collider.gameObject.SendMessage("GotHit", damage);
        }
    }

    /* Receives the message that it got hit
     */
    void GotHit(int damage)
	{
		currentFreezeTime = maxFreezeTime;
		currentHealth-=damage;
		if (currentHealth <= 0)
			Destroy (this.gameObject);
	}

    //Flips the transform on the X-axis
    void Flip ()
	{
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
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
