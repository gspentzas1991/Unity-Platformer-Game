using UnityEngine;
using System.Collections;

/* Contains the behaviour for the bomber enemy
 * The bomber has a child called "bomberTrigger" that is a trigger collider.
 * The bomberTrigger informs the bomber when the player enters its trigger collider
 * When that happens, the bomber starts creating bombs that have their own velocity
 */
public class bomberBehavior : MonoBehaviour
{
	public float maxBombCooldown;
	float currentBombCooldownTimer;
	public int maxHealth;
	int currentHealth;
	bool isFiringBomb;
	Animator bomberAnimator;
	public GameObject bomb;
    bool isPaused=false;

	void Start () 
	{
		bomberAnimator = GetComponent<Animator>();
		isFiringBomb = false;
		currentBombCooldownTimer = 0;
		currentHealth = maxHealth;
	}
	
	void Update () 
	{
		currentBombCooldownTimer=gameManagerScript.gameManager.Cooldown(currentBombCooldownTimer);
		bomberAnimator.SetBool("firing",isFiringBomb);
		isFiringBomb = false;
	}

    /* The bomber is informed that the player entered the bomberTrigger
     * This method is called by the bomberTrigger
     */
    public void PlayerEnteredTrigger() 
	{
		if((currentBombCooldownTimer==0)&&(!isPaused))
		{
			isFiringBomb=true;
			Object bombInstance=Instantiate(bomb,new Vector2(transform.position.x-2.1f,transform.position.y), transform.rotation);
			currentBombCooldownTimer=maxBombCooldown;
		}
	}

    /* When the bomber gets hit, it takes damage
     */
	void GotHit(int damage)
	{
		currentHealth-=damage;
		if (currentHealth <= 0)
			Destroy (this.gameObject);
	}


    //Pause messages received from the gameManager
    void OnPauseGame()
    {
        isPaused = true;
    }

    void OnUnPauseGame()
    {
        isPaused = false;
    }

}
