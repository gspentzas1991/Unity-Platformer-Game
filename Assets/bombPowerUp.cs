using UnityEngine;
using System.Collections;

public class bombPowerUp : MonoBehaviour {

	public GameObject powerUp;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
			if(collision.gameObject.name=="Player")
			{
				collision.gameObject.SendMessage("gotPowerUp",powerUp);
				Destroy(this.gameObject);
			}
	}
}
