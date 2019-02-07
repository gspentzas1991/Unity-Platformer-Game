using UnityEngine;
using System.Collections;

public class spinnerBehavior : MonoBehaviour {

	public int damage;

	bool isPaused=false;

	//gia posi ora tha stunarei apo xtipimata
	public float maxFreezeTime;
	float currentFreezeTime=0;

	//posi zoi tha exei
	public int maxHealth;
	int currentHealth;

	Animator characterAnim;



	// Use this for initialization
	void Start () 
	{
		characterAnim = GetComponent<Animator>();
		currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//an eine paused min kaneis tpt
		if(isPaused)
			characterAnim.speed=0;
		else
			characterAnim.speed=1;
	}


	//leei ti ginete otan kapios collider tou character akoubaei kapion allo
	//gia kathe frame sto opio akoubane
	void OnCollisionEnter2D(Collision2D collision)
	{

		if(!isPaused)
		{
			if(collision.gameObject.name=="Player")
			{
				print (collision.gameObject.name);
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

	
	//otan ginete pause
	void OnPauseGame()
	{
		isPaused = true;
	}
	
	//otan kseginete apo pause
	void onUnPauseGame()
	{
		isPaused = false;
	}

}
