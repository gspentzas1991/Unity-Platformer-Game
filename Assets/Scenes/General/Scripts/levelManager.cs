using UnityEngine;
using System.Collections;

public class levelManager : MonoBehaviour {

	public KeyCode pauseKey;

	public float guiPlacementy;

	public GUIStyle emptyTexture;
	public int levelNumber;
	public int checkPointNumber=0;

	public bool isPaused=false;
	bool inDialogue=false;

	// Use this for initialization
	void Start () 
	{/*
		dialogueTree = new string[6, 2];
		//gemizei to dialogueTree me ta strings pou prepei na exei
		int count;
		for(count=0;count<=4;count++)
		{
			dialogueTree[count,0]=setDialogue[count];
		}

		//osa dialogue exoun choice dipla tous, tote otan ftanoume se afta vgenei to select grid
		dialogueTree [2,1] = "CHOICE";
		//osa exoun end dipla tous, otan ftanoume se afta ke patisoume end, telionei o dialogos
		dialogueTree[3,1]="END";
		dialogueTree[4,1]="END";*/

		load ();
		
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (Input.GetKeyDown (pauseKey)) 
		{
			Object[] objectList = FindObjectsOfType (typeof(GameObject));
			if(!isPaused)
			{
				Time.timeScale=0;
				isPaused=true;
				foreach (GameObject gObject in objectList)
					gObject.SendMessage ("OnPauseGame", SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				Time.timeScale=1;
				isPaused=false;
				foreach (GameObject gObject in objectList)
					gObject.SendMessage ("onUnPauseGame", SendMessageOptions.DontRequireReceiver);
			}
		}/*
		if (Input.GetKeyDown (dialogueKey))
		{
			Object[] objectList = FindObjectsOfType (typeof(GameObject));
			if(dialogueTree[dialogueNumber,1]!="END")
			{
				if(dialogueTree[dialogueNumber,1]!="CHOICE")
					dialogueNumber++;
			}
			else
			{
				inDialogue=false;
				foreach (GameObject gObject in objectList)
					gObject.SendMessage ("dialogueEnd", SendMessageOptions.DontRequireReceiver);
			}
		}*/
	}


	void load()
	{
		//an to apothikevmeno levelNumber, eine iso me to levelNumber tou levelManager,
		//afto simenei oti o pextis exei ksanarthei se afto to level, opote prosthese sto checkpoint
		//ton apothikevmeno arithmo checkpoint. allios pigene stin arxi tou level, diladi sto checkpoint0
		if(PlayerPrefs.GetInt("levelNumber"+gameManagerScript.savefile)==levelNumber)
		{
			if(PlayerPrefs.HasKey("checkPointNumber"+gameManagerScript.savefile))
				checkPointNumber+= PlayerPrefs.GetInt ("checkPointNumber"+gameManagerScript.savefile);

		}
		GameObject player = GameObject.Find ("Player");
		player.SendMessage("teleportToCheckPoint",GameObject.Find("checkPoint"+checkPointNumber).transform.position);

	}

	//data = se pio checkpoint tou level eimaste
	void save(int data)
	{
		PlayerPrefs.SetInt ("checkPointNumber"+gameManagerScript.savefile,data);
		PlayerPrefs.Save ();
	}


	
	void OnPauseGame()
	{
		isPaused = true;

	}

	void onUnPauseGame()
	{
		isPaused = false;
	}

	void OnGUI() 
	{
		if (isPaused)
		{
			if(Application.loadedLevel!=6)
			{
				if(GUI.Button (new Rect (Screen.width * 0.37f, Screen.height * guiPlacementy, Screen.width * 0.25f, Screen.height * 0.1f), "Return to Town", emptyTexture))
				{
					Time.timeScale=1;
					isPaused=false;
					GameObject.Find("gameManager").SendMessage("changeMusic",6);
					Application.LoadLevel (6);
				}
			}
			if(GUI.Button (new Rect (Screen.width * 0.37f, Screen.height * (guiPlacementy+0.2f), Screen.width * 0.25f, Screen.height * 0.1f), "Quit Game", emptyTexture))
			{
				if(Application.isEditor)
				{
					
					Time.timeScale=1;
					isPaused=false;
					GameObject.Find("gameManager").SendMessage("changeMusic",1);
					Application.LoadLevel (1);
				}
				else
					Application.Quit ();
			}
		}
		/*
		if(inDialogue)
		{
			GUI.Box (new Rect (Screen.width*1/8,Screen.height*9/12,Screen.width*3/4,Screen.height*1/5),dialogueTree [dialogueNumber,0]  );
			if(dialogueNumber==2)
			{
				selGridInt = GUI.SelectionGrid(new Rect(Screen.width*1/8,Screen.height*8/12,Screen.width*1/4,Screen.height*1/20), selGridInt, answerTree, 3);
				if((selGridInt==0)||(selGridInt==2))
					dialogueNumber++;
				if(selGridInt==1)
					dialogueNumber+=2;
			}
		}*/

	}

	void dialogueStart()
	{
		inDialogue = true;
	}
}
