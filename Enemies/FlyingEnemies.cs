using UnityEngine;
using System.Collections;

/* Contains the behaviour for the flying enemies
 * Flying enemies are always looking for the player on each line of sight
 * When they find the player they start moving towards him, until they touch him, or he escapes their line of sight
 * After they touch the player, they return to their nest, and start looking for the player again
 */
public class FlyingEnemies : MonoBehaviour
{
    public float flyingSpeed;
    public int damage;

    public int maxHealth;
    protected int currentHealth;


    public LayerMask playerLayer;
    public LayerMask nestLayer;
    public Transform nestTransform;

    protected Animator flyingEnemyAnimator;
    protected Rigidbody2D flyingEnemyRigidBody;

    protected Collider2D lineOfSight;
    public float lineOfSightRadius;

    protected Collider2D nestCollider;
    public float nestRadius;

    protected bool touchedPlayer;



    void Start()
    {

        flyingEnemyAnimator = GetComponent<Animator>();
        flyingEnemyRigidBody = GetComponent<Rigidbody2D>();
        touchedPlayer = false;
        currentHealth = maxHealth;

    }

    void FixedUpdate()
    {

        //Checks if the player is in its line of sight
        lineOfSight = Physics2D.OverlapCircle(transform.position, lineOfSightRadius, playerLayer);
        if ((lineOfSight) && (!touchedPlayer))
            Pathfinding(lineOfSight.GetComponent<Collider2D>().gameObject.transform);
        //If it touched the player, or he is not in its line of sight, it returns to its nest
        else
            Pathfinding(nestTransform);

        //Checks if the enemy has reached its nest
        nestCollider = Physics2D.OverlapCircle(transform.position, nestRadius, nestLayer);
        //The enemy resets its behaviour when it reaches its nest
        if (nestCollider)
        {
            touchedPlayer = false;
        }

    }
    
    void GotHit(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Destroy(this.gameObject);
    }

    //Makes the enemy chase the target transform
    void Pathfinding(Transform target)
    {
        //Chases the target on the X-Axis
        if (target.position.x > transform.position.x)
            flyingEnemyRigidBody.velocity = new Vector2(flyingSpeed, flyingEnemyRigidBody.velocity.y);
        if (target.position.x < transform.position.x)
            flyingEnemyRigidBody.velocity = new Vector2(-flyingSpeed, flyingEnemyRigidBody.velocity.y);
        if ((target.position.x > transform.position.x - 0.5) && (target.position.x < transform.position.x + 0.5))
            flyingEnemyRigidBody.velocity = new Vector2(0, flyingEnemyRigidBody.velocity.y);
        //Chases the target on the Y-Axis
        if (target.position.y > transform.position.y)
            flyingEnemyRigidBody.velocity = new Vector2(flyingEnemyRigidBody.velocity.x, flyingSpeed);
        if (target.position.y < transform.position.y)
            flyingEnemyRigidBody.velocity = new Vector2(flyingEnemyRigidBody.velocity.x, -flyingSpeed);
        if ((target.position.y > transform.position.y - 0.5) && (target.position.y < transform.position.y + 0.5))
            flyingEnemyRigidBody.velocity = new Vector2(flyingEnemyRigidBody.velocity.x, 0);
    }


    //Pause messages received from the gameManager
    void OnPauseGame()
    {
        flyingEnemyRigidBody.isKinematic = true;
    }

    void OnUnPauseGame()
    {
        flyingEnemyRigidBody.isKinematic = false;
    }


}
