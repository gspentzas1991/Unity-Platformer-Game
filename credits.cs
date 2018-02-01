using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//When the player presses the return to town key, it loads the town level
public class credits : MonoBehaviour {

	public KeyCode returnToTownKey;
	public int nextLevel;
	
	
	void Update () {
        if (Input.GetKeyDown(returnToTownKey))
        {
            gameManagerScript.gameManager.ChangeMusic(nextLevel);
            SceneManager.LoadScene(nextLevel);
        }
	
	}

}
