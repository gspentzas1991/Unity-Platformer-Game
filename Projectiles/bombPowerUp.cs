using UnityEngine;
using System.Collections;

//When the player touches the powerUp, his ranged weapon changes
public class bombPowerUp : MonoBehaviour {

	public GameObject powerUp;
	

	void OnTriggerEnter2D(Collider2D targetCollider)
	{
			if(targetCollider.gameObject.name=="Player")
			{
				targetCollider.gameObject.SendMessage("GotPowerUp",powerUp);
				Destroy(this.gameObject);
			}
	}
}
