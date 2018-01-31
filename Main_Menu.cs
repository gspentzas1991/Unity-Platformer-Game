using UnityEngine;
using System.Collections;

/*Creates the Main Menu GUI and its behaviour 
 */
public class Main_Menu : MonoBehaviour 
{
    //The number of available save slots
    public int maxSaveSlots;

    
    public Texture backgroundTexture;

	public GUIStyle emptyButtonTexture;
    
	public GUIStyle backButtonTexture;
    
	public GUIStyle deleteButtonTexture;

    //The gui style for the "Are you sure" deletion prompt
    public GUIStyle areYouSureGuiStyle;

    //The offset on the Y-Axis, of the "Start Game" button
    public float guiPlacementy;

    //Flag , if true draws the save slots
	bool showLoadGames=false;

    //Flag , if true draws the "Are you Sure" screen
	bool areYouSure=false;

    //Shows what save slot we have selected
	int saveslot=0;

    //The position of the "Back" button on the screen
	public float backBtnX1, backBtnX2, backBtnY1, backBtnY2;
    

	void OnGUI()
	{
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),backgroundTexture);

		if((!showLoadGames)&&(!areYouSure))
		{
			//Draws the "Start Game" button. If clicked shows the save slots
			if(GUI.Button (new Rect(Screen.width*0.37f,Screen.height*guiPlacementy,Screen.width*0.25f,Screen.height*0.1f),"Start Game",emptyButtonTexture))
			{
				showLoadGames=true;
			}
			
            //Draws the "Quit Game" button. If clicked, quits the game
			if(GUI.Button (new Rect(Screen.width*0.37f,Screen.height*guiPlacementy+80f,Screen.width*0.25f,Screen.height*0.1f),"Quit Game",emptyButtonTexture))
			{
				Application.Quit();
			}
		} 

		else if((showLoadGames)&&(!areYouSure))
		{
			for(int counter=0;counter<maxSaveSlots;counter++)
			{
				string buttonText;

				//Checks if there exists a save file for each save slot, and changes the button text accordingly
				if(PlayerPrefs.GetInt("levelNumber"+counter.ToString())==0)
					buttonText="Empty";
				else if(PlayerPrefs.GetInt("levelNumber"+counter.ToString())==5)
					buttonText="Town";
				else
					buttonText="Level: "+PlayerPrefs.GetInt("levelNumber"+counter.ToString())+"\r\n"+"CheckPoint: "+PlayerPrefs.GetInt("checkPointNumber"+counter.ToString());

                //Creates the buttons for each save slot. 
                //If a save slot button is clicked, the gameManager loads that save file
				if(GUI.Button (new Rect(Screen.width*0.37f,Screen.height*0.1f*counter,Screen.width*0.25f,Screen.height*0.1f),buttonText,emptyButtonTexture))
				{
					gameManagerScript.gameManager.savefile=counter;
                    gameManagerScript.gameManager.Load();
				}
				//Creates the "Delete Save" buttons. If pressed they delete the save file
				if(GUI.Button (new Rect(Screen.width*0.65f,Screen.height*0.1f*counter+10,Screen.width*0.05f,Screen.height*0.05f),"",deleteButtonTexture))
				{
					areYouSure=true;
					saveslot=counter;
				}
			}

			//Creates the "Back" button. If pressed it goes back to the main menu
			if(GUI.Button (new Rect(backBtnX1, backBtnY1, backBtnX2, backBtnY2),"Back",backButtonTexture))
				showLoadGames=false;


		}
		else
		{
			GUI.Label ((new Rect(Screen.width*0.40f,Screen.height*0.5f,Screen.width*0.25f,Screen.height*0.1f)),"Are you sure you want to delete this savefile?",areYouSureGuiStyle);

            //Creates the "Yes Delete Save File" button.
            if (GUI.Button (new Rect(Screen.width*0.25f,Screen.height*0.6f,Screen.width*0.25f,Screen.height*0.1f),"Yes",emptyButtonTexture))
			{
				for(int i=1;i<=gameManagerScript.numberOfLevels;i++)
					PlayerPrefs.DeleteKey("level"+i+"IsUnlocked"+saveslot.ToString());
				PlayerPrefs.DeleteKey("levelNumber"+saveslot.ToString());
				PlayerPrefs.DeleteKey("checkPointNumber"+saveslot.ToString());
				areYouSure=false;
			}

            //Creates the "No Don't Delete Save File" button.
            if (GUI.Button (new Rect(Screen.width*0.55f,Screen.height*0.6f,Screen.width*0.25f,Screen.height*0.1f),"No",emptyButtonTexture))
			{
				areYouSure=false;
			}

		}
	}
}
