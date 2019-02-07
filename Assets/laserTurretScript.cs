using UnityEngine;
using System.Collections;

public class laserTurretScript : MonoBehaviour {

	AudioSource audioMan;
	public float animationSpeed;
	public int maxHealth;
	int currentHealth;
	public bool facingLeft;
	public GameObject lazer;

	public float maxRangedCooldown;
	float currentRangedCooldown;

	Animator characterAnim;

	// Use this for initialization
	void Start () {
		audioMan = GetComponent<AudioSource> ();
		currentHealth = maxHealth;
		characterAnim = GetComponent<Animator> ();
		currentRangedCooldown = maxRangedCooldown;
		characterAnim.speed = animationSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		currentRangedCooldown=cooldown(currentRangedCooldown);
		if (currentRangedCooldown <= 0)
			rangedAttack ();
	
	}


	
	//aferei kata ena kathe defterolepto to value, mexri na ginei 0
	float cooldown(float value)
	{
		if (value > 0)
			value -= 0.0167f;
		else
			value = 0;
		return value;
	}

	void rangedAttack()
	{
		audioMan.Play ();
			//dimiourgei ena object gia to projectile, ke tou dinei onoma analoga pros ta pou paei
		if(facingLeft)
		{
				Object bulletInstance=Instantiate (lazer, new Vector2(transform.position.x - 2f, transform.position.y),transform.rotation);
				bulletInstance.name="bullet(left)";
		}
		else
		{
			Object bulletInstance=Instantiate (lazer, new Vector2(transform.position.x + 2f, transform.position.y),transform.rotation);
			bulletInstance.name="bullet(right)";
		}

			//ksekinaei to cooldown gia tin epomeni ranged epithesi
			currentRangedCooldown=maxRangedCooldown;
	}

	void gotHit(int damage)
	{
		//otan o adipalos xtipiete, menei akinitos gia 0.2 defterolepta
		currentHealth-=damage;
		if (currentHealth <= 0)
			Destroy (this.gameObject);
	}
}
