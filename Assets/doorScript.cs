using UnityEngine;
using System.Collections;

public class doorScript : MonoBehaviour {
	SpriteRenderer spriteRend;
	public Sprite opened;
	public Sprite closed;

	public KeyCode openDoor;
	public int levelNumber;
	bool isOpen=false;

	// Use this for initialization
	void Start () {
		//to level 0 eine to initilization, to 1 to main menu, ara to level 1 pragmatika eine to scene 1+1=2
		levelNumber = levelNumber + 1;
		spriteRend=GetComponent<SpriteRenderer> ();
	
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerPrefs.GetInt ("level"+(levelNumber-1)+"IsUnlocked"+gameManagerScript.savefile) == 1)
			isOpen = true;

		if (isOpen)
			spriteRend.sprite = opened;
		else
			spriteRend.sprite = closed;
	
	}

	void OnTriggerStay2D(Collider2D coll) 
	{
		//an to levelxIsUnlocked eine 1, tote mono i porta tha eine anixti
		if(isOpen)
		{
			if (coll.name == "Player")
			{
				if (Input.GetKey (openDoor))
				{
					GameObject.Find("gameManager").SendMessage("changeMusic",levelNumber);
					Application.LoadLevel (levelNumber);
				}
			}
		}


	}
}
