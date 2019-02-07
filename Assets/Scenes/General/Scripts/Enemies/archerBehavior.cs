using UnityEngine;
using System.Collections;

public class archerBehavior : MonoBehaviour {


	public float bombStartingDistance;

	public float maxReload;
	float currentReload;

	//posi zoi tha exei
	public int maxHealth;
	int currentHealth;

	bool firing;

	Vector2 realTriggerPosition;

	Animator archerAnim;

	Collider2D trigger;

	public Transform triggerPosition;
	public float triggerRadius;
	public GameObject bomb;

	// Use this for initialization
	void Start () 
	{
		archerAnim = GetComponent<Animator>();
		firing = false;
		currentReload = 0;
		currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () 
	{
		currentReload=cooldown(currentReload);
		//trigger = Physics2D.OverlapCircle (triggerPosition.position, triggerRadius);
		//if (trigger)
			//triggered(trigger.collider2D);
		archerAnim.SetBool("firing",firing);
		firing = false;
	
	}

	//otan o pextis eine mesa sto trigger zone tou, ke den kanei reload, tha dimiourgei mia vomva
	void triggered(Collider2D target) 
	{
		Object bombInstance;
		if((target.gameObject.name=="Player")&&(currentReload<=0))
		{
			firing=true;
			bombInstance=Instantiate(bomb,new Vector2(transform.position.x-bombStartingDistance,transform.position.y), transform.rotation);
			currentReload=maxReload;
		}
	

	}

	void gotHit(int damage)
	{
		//otan o adipalos xtipiete, kanei reset to reloading tou
		currentReload = maxReload;
		currentHealth-=damage;
		if (currentHealth <= 0)
			Destroy (this.gameObject);
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
}
