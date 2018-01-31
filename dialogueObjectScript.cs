using UnityEngine;
using System.Collections;

/* The script that handles boss dialogue and player choice
 * The pieces of dialogue are inputed on the Unity Editor
 */
public class dialogueObjectScript : MonoBehaviour {

	public KeyCode continueDialogueKey;
	bool inDialogue=false;
    //Points at piece of dialogue we are in the dialogue tree
    int dialogueCursor=0;
    //The total amount of pieces of dialogue the boss has
	public int bossDialogueTreeSize;
    //Contains all the pieces of dialogue the boss has
    public string[] bossPiecesOfDialogue;
    //Marks what pieces of boss dialogue will have the END keyword
	public int[] endDialogueArray;
    //Marks what pieces of boss dialogue will have the CHOICE keyword
    public int[]choiceDialogueArray;
    /*The first column contains all the pieces of dialogue for the boss.
     *The second column has the keyword "END","CHOICE" or null.
     *That shows what type each piece of boss dialogue is*/
    string[,] bossDialogueTree;
    //Contains the player answers that appear when we reach a "CHOICE" boss dialogue
	public string[] answerTree;
    //Points to what answer we have selected
	int answerPointer = -1;

    //Initializes the dialogue tree
	void Start () 
	{
		bossDialogueTree = new string[bossDialogueTreeSize, 2];
		int count;
        //Fills the boss dialogue tree with all pieces of dialogue
		for(count=0;count<bossDialogueTreeSize;count++)
		{
			bossDialogueTree[count,0]=bossPiecesOfDialogue[count];
		}
        //Marks what type each piece of dialogue is
		foreach (int element in endDialogueArray)
		{
			bossDialogueTree[element,1]="END";
		}
		foreach (int element in choiceDialogueArray)
		{
			bossDialogueTree[element,1]="CHOICE";
		}

		
	}

    //When the dialogue starts, it informs every gameobject in the scene
    //This method is called by the "Dialogue Trigger" gameObject
    void DialogueTriggered()
    {
        Object[] objectList = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject gObject in objectList)
            gObject.SendMessage("DialogueStart", SendMessageOptions.DontRequireReceiver);
        inDialogue = true;
    }

    /* When the player presses the continue dialogue button, it progresses the dialogue tree by one
     * When the dialogue ends, it informs every gameobject in the scene
     */
    void Update () 
	{
		if ((Input.GetKeyDown (continueDialogueKey))&&(inDialogue))
		{

			if(bossDialogueTree[dialogueCursor,1]=="END")
            {
                Object[] objectList = FindObjectsOfType(typeof(GameObject));
                inDialogue = false;
                foreach (GameObject gObject in objectList)
                    gObject.SendMessage("DialogueEnd", SendMessageOptions.DontRequireReceiver);
			}
			else
            {
                if (bossDialogueTree[dialogueCursor, 1] != "CHOICE")
                    dialogueCursor++;
            }
        }

	}
	
    //Draws the dialogue box and progresses the dialogue when the player answers
	void OnGUI() 
	{
		if(inDialogue)
		{
            //Draws the dialogue box
			GUI.Box (new Rect (Screen.width*1/8,Screen.height*9/12,Screen.width*3/4,Screen.height*1/5),bossDialogueTree [dialogueCursor,0]  );
            GUI.Label(new Rect(Screen.width * 5 / 9, Screen.height * 16/ 18, Screen.width * 2 / 7, Screen.height * 1 / 10),"Press K to continue");

            if (bossDialogueTree[dialogueCursor,1]=="CHOICE")
			{
				answerPointer = GUI.SelectionGrid(new Rect(Screen.width*1/8,Screen.height*6/12,Screen.width*1/3,Screen.height*1/5), answerPointer, answerTree, 1);
                //If the player picks the first or third option, skip the next piece of dialogue
                if ((answerPointer==0)||(answerPointer==2))
					 dialogueCursor+=2;
                //If the player picks the second option, continue to the next piece of dialoge
				if(answerPointer==1)
					dialogueCursor++;
			}
		}
		
	}
    

}
