using UnityEngine;
using System.Collections;

public class shopDialogue : MonoBehaviour {
	
	public KeyCode dialogueKey;
	public int numberOfProducts;
	
	public bool isPaused=false;
	bool inDialogue=false;
	public int dialogueNumber=0;
	public int dialogueSize;
	public string[] setDialogue;
	
	public int[] endDialogueArray;
	public int[]choiceDialogueArray;
	
	string[,] dialogueTree;
	public string[] answerTree;
	public int selGridInt = -1;
	
	
	// Use this for initialization
	void Start () 
	{
		dialogueTree = new string[dialogueSize, 2];
		//gemizei to dialogueTree me ta strings pou prepei na exei
		int count;
		for(count=0;count<dialogueSize;count++)
		{
			dialogueTree[count,0]=setDialogue[count];
		}
		
		foreach (int element in endDialogueArray)
		{
			dialogueTree[element,1]="END";
		}
		foreach (int element in choiceDialogueArray)
		{
			dialogueTree[element,1]="CHOICE";
		}
		
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		Object[] objectList = FindObjectsOfType (typeof(GameObject));
		if (Input.GetKeyDown (dialogueKey))
		{
			
			if(dialogueTree[dialogueNumber,1]!="END")
			{
				if(dialogueTree[dialogueNumber,1]!="CHOICE")
					dialogueNumber++;
			}
			else
			{
				inDialogue=false;
				dialogueNumber=0;
				foreach (GameObject gObject in objectList)
					gObject.SendMessage ("dialogueEnd", SendMessageOptions.DontRequireReceiver);
			}
		}
		
	}
	
	
	void OnGUI() 
	{
		if(inDialogue)
		{
			if(dialogueTree[dialogueNumber,1]!="CHOICE")
			GUI.Box (new Rect (Screen.width*1/8,Screen.height*9/12,Screen.width*3/4,Screen.height*1/5),dialogueTree [dialogueNumber,0]  );
			if(dialogueTree[dialogueNumber,1]=="CHOICE")
			{
				selGridInt = GUI.SelectionGrid(new Rect(Screen.width*1/8,Screen.height*1/12,Screen.width*1/4,Screen.height*1/2), selGridInt, answerTree, numberOfProducts);
				if(selGridInt==0)
				{
					print ("choice1");
					selGridInt=-1;
				}
				if(selGridInt==1)
				{
					print ("choice2");
					selGridInt=-1;
				}
				if(selGridInt==2)
					dialogueNumber++;
			}
		}
		
	}
	
	void dialogueTriggered()
	{
		Object[] objectList = FindObjectsOfType (typeof(GameObject));
		foreach (GameObject gObject in objectList)
			gObject.SendMessage ("dialogueStart", SendMessageOptions.DontRequireReceiver);
		inDialogue=true;
		
	}
	
	
	void OnPauseGame()
	{
		isPaused = true;
		
	}
	
	void onUnPauseGame()
	{
		isPaused = false;
	}
	
	
}
