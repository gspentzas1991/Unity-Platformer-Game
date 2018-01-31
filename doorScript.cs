using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//Contains the sript that opens or locks the level entrance for each level
//It checks in the save file wether the player has unlocked it or not
public class doorScript : MonoBehaviour {
	SpriteRenderer spriteRend;
	public Sprite opened;
	public Sprite closed;

	public KeyCode openDoor;
	public int levelNumber;
	bool isOpen=false;
    
	void Start () {
        //Since scene 1 is the main menu, we have to add one to the levelNumber to get the correct one
        levelNumber = levelNumber + 1;
		spriteRend=GetComponent<SpriteRenderer> ();
	
	}
	
	void Update () {
		if (PlayerPrefs.GetInt ("level"+(levelNumber-1)+"IsUnlocked"+gameManagerScript.gameManager.savefile) == 1)
			isOpen = true;

		if (isOpen)
			spriteRend.sprite = opened;
		else
			spriteRend.sprite = closed;
	
	}

	void OnTriggerStay2D(Collider2D coll) 
	{
		if(isOpen)
		{
			if (coll.name == "Player")
			{
				if (Input.GetKey (openDoor))
				{
                    gameManagerScript.gameManager.ChangeMusic(levelNumber);
                    SceneManager.LoadScene(levelNumber);
				}
			}
		}


	}
}
