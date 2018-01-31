using UnityEngine;
using System.Collections;

/* Contains the collision detection for the Ghost enemy
 * It inherits its behaviour from the class "FlyingEnemies" 
 */
public class ghostBehaviour : FlyingEnemies
{
   void Update()
   {
        flyingEnemyAnimator.SetBool("escaping", touchedPlayer);
   }

    void OnTriggerEnter2D(Collider2D targetCollider)
    {
        if (targetCollider.gameObject.tag == "Player")
        {
            touchedPlayer = true;
            targetCollider.SendMessage("GotHit", damage);
        }
    }
}
	