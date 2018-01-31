using UnityEngine;
using System.Collections;

/* Contains the behaviour of the bomb projectile
 * After it gets created, a force is applied to it depending on its name (right or left)
 * When it touches a platform, enemy or player, it explodes, hurting everything within its radius
 */
public class bombBehaviour : MonoBehaviour {

	public float radiusOfExplosion;
	public int damage;
	public int bombForceX;
	public int bombForceY;
	Collider2D[] targetList;
	public GameObject explosion;
    Rigidbody2D bombRigidbody;
    bool isPaused;


	void Start () 
	{
        bombRigidbody = GetComponent<Rigidbody2D>();
        if (name == "bullet(right)")
            bombForceX = -bombForceX;
        bombRigidbody.AddForce (new Vector2(-bombForceX,bombForceY));
	}
	
	void OnTriggerEnter2D(Collider2D targetCollider) 
	{
		if((targetCollider.tag=="Platform")||(targetCollider.tag=="Player")||(targetCollider.tag=="Enemy"))
		{
            Object explosionInstance = Instantiate (explosion, transform.position,transform.rotation);
			targetList=Physics2D.OverlapCircleAll(transform.position,radiusOfExplosion);
			foreach (Collider2D collider in targetList) 
			{
			    collider.gameObject.SendMessage("GotHit",damage,SendMessageOptions.DontRequireReceiver);
			}
            Destroy(this.gameObject);
		}
	}

    
}
