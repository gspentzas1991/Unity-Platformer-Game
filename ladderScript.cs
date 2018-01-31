using UnityEngine;
using System.Collections;

/*Contains the behaviour for the player-ladder interaction
 *When the player is in front of the ladder and presses W or S
 *his ladder climbing animation starts playing and he climbs the ladder
 */
public class ladderScript : MonoBehaviour {
    
    public PlayerControlScript player;

	void OnTriggerStay2D(Collider2D targetCollider) 
	{
		if (targetCollider.gameObject.tag == "Player")
		{
            player.LadderCheck(true) ;
		}
	}
	
	void OnTriggerExit2D(Collider2D targetCollider) 
	{
		if (targetCollider.gameObject.tag == "Player")
		{
            player.LadderCheck(false);
		}
	}
}
