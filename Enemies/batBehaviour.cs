using UnityEngine;
using System.Collections;

/* Contains the collision detection for the Bat enemy
 * It inherits its behaviour from the class "FlyingEnemies" 
 */
public class batBehaviour : FlyingEnemies
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            touchedPlayer = true;
            collision.gameObject.SendMessage("GotHit", damage);
        }
    }
    

}
