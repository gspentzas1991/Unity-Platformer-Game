using UnityEngine;
using System.Collections;

/* The spinner enemy doesn't move, but hurts the player if he touches them
 */
public class spinnerBehavior : MonoBehaviour {

	public int damage;
    bool isPaused = false;  
	public int maxHealth;
	int currentHealth;
	Animator characterAnim;
    
	void Start () 
	{
		characterAnim = GetComponent<Animator>();
		currentHealth = maxHealth;
	}
	
	void Update () 
	{
		if(isPaused)
			characterAnim.speed=0;
		else
			characterAnim.speed=1;
	}
    
	void OnCollisionEnter2D(Collision2D collision)
	{

		if(!isPaused)
		{
			if(collision.gameObject.name=="Player")
			{
				collision.gameObject.SendMessage("GotHit",damage);
			}
		}
	}


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
