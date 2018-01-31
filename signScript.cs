using UnityEngine;
using System.Collections;

/* The sign text will be invincible, until the player gets close
 * Then it appears on top of the sign
 */
public class signScript : MonoBehaviour {

    Renderer signRenderer;

	void Start ()
    {
        signRenderer = GetComponent<Renderer>();
        signRenderer.enabled = false;
	}
	
	void OnTriggerEnter2D(Collider2D targetCollider) 
	{
		if (targetCollider.name == "Player")
			signRenderer.enabled = true;
	}

	void OnTriggerExit2D(Collider2D targetCollider) 
	{
		if (targetCollider.name == "Player")
            signRenderer.enabled = false;
	}

}
