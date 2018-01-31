using UnityEngine;
using System.Collections;

/* When the player enters the bomberTrigger's trigger collider, the bomberTrigger
*  informs the bomber through the Triggered() method
*/
public class bomberTrigger : MonoBehaviour {
	public bomberBehavior bomber;

	void OnTriggerStay2D(Collider2D targetCollider)
	{
        if (targetCollider.name == "Player")
            bomber.PlayerEnteredTrigger();
	}
}
