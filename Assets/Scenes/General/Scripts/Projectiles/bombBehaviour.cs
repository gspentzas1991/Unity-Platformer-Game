using UnityEngine;
using System.Collections;

public class bombBehaviour : MonoBehaviour {

	public float radiusOfExplosion;
	public int damage;
	public int bombForceX;
	public int bombForceY;
	AudioSource audioMan;
	Collider2D[] targetList;
	public GameObject explosion;

	// Use this for initialization
	void Start () 
	{
		audioMan = GetComponent<AudioSource> ();
		if(name=="bullet(right)")
			GetComponent<Rigidbody2D>().AddForce (new Vector2(bombForceX,bombForceY));
		else
			GetComponent<Rigidbody2D>().AddForce (new Vector2(-bombForceX,bombForceY));

	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (transform.position.y < -200)
						Destroy (this.gameObject);
	
	}

	//ti ginete otan petixenei kapion
	void OnTriggerEnter2D(Collider2D target) 
	{
		if((target.tag=="Platform")||(target.tag=="Player"))
		{
			Object explosionInstance = Instantiate (explosion, transform.position,transform.rotation);
			GetComponent<Renderer>().enabled=false;
			GetComponent<Collider2D>().enabled=false;
			targetList=Physics2D.OverlapCircleAll(transform.position,radiusOfExplosion);
			foreach (Collider2D coll in targetList) 
			{
				if(coll.isTrigger==false);
				{
					coll.gameObject.SendMessage("gotHit",damage,SendMessageOptions.DontRequireReceiver);
				}
			}
			audioMan.Play ();
		}
	}
}
