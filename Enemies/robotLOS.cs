using UnityEngine;
using System.Collections;

//When the player enters its trigger collider, it informs the robot enemy it belongs to
public class robotLOS : MonoBehaviour {

    public robotBehavior robot;
	void OnTriggerEnter2D(Collider2D other) 
	{
        if (other.gameObject.tag == "Player")
            robot.GetPlayerSearchStatus(true);

    }

	void OnTriggerExit2D(Collider2D other) 
	{
		if (other.gameObject.tag == "Player")
            robot.GetPlayerSearchStatus(false);
    }
}
