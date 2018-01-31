using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/* When the player enters the trigger collider, it unlocks the next level
 * and loads the "Level Select" level
 * If the goal unlocks the credits level (scene 7) , it also loads the credits
 */
public class goalScript : MonoBehaviour {

	public int levelToUnlock;
	
	void OnTriggerEnter2D(Collider2D targetCollider)
	{
		if(targetCollider.name=="Player")
		{
            gameManagerScript.gameManager.Unlock(levelToUnlock);
            if (levelToUnlock == 7)
            {

                gameManagerScript.gameManager.ChangeMusic(7);
                SceneManager.LoadScene(7);
            }
            else
            {
                gameManagerScript.gameManager.ChangeMusic(6);
                SceneManager.LoadScene(6);
            }


		}
	}
}
