using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//When the player presses the return to town key, it loads the town level
public class credits : MonoBehaviour {

	public KeyCode returnToTownKey;
	public int nextLevel;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(returnToTownKey))
        {
            gameManagerScript.gameManager.ChangeMusic(nextLevel);
            SceneManager.LoadScene(nextLevel);
        }
	
	}

}
