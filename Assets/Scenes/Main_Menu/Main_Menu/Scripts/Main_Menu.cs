/* Summary
 * Main Menu
 * Akolouthei tin main camera
 */
using UnityEngine;
using System.Collections;

public class Main_Menu : MonoBehaviour 
{
	public Texture backgroundTexture;
	public GUIStyle emptyTexture;
	public GUIStyle backTexture;
	public GUIStyle deleteTexture;

	public GUIStyle areYouSureText;

	public int maxSavedGames;
	//leei se ti pososto tis othonis tha emfanizete to koubi
	public float guiPlacementy;

	bool showLoadGames;
	bool areYouSure;

	int temp;

	public float x1,x2,y1,y2;

	void Start()
	{
		temp=0;
		showLoadGames=false;
		areYouSure=false;
	}
	void OnGUI()
	{
		/*Tha emfanizei to backgroundTexture
		i eikona tha ksekinaei apo to 0 top ke 0 left, mexri to ipsos ke fardos tis othonis*/
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),backgroundTexture);

		if((!showLoadGames)&&(!areYouSure))
		{
			//start game button
			//otan patithei emfanizei tin lista me ta saves
			if(GUI.Button (new Rect(Screen.width*0.37f,Screen.height*guiPlacementy,Screen.width*0.25f,Screen.height*0.1f),"Start Game",emptyTexture))
			{
				showLoadGames=true;
			}
			
			if(GUI.Button (new Rect(Screen.width*0.37f,Screen.height*guiPlacementy+80f,Screen.width*0.25f,Screen.height*0.1f),"Quit Game",emptyTexture))
			{
				Application.Quit();
			}
		} 
		else if((showLoadGames)&&(!areYouSure))
		{
			for(int i=0;i<=maxSavedGames;i++)
			{
				float b=0.1f;
				string buttonString="";

				//kathorizei an to button tha grafei Empty i oxi
				//an to levelNumber eine 0, tote den exei xrisimopiithei pote to save
				if(PlayerPrefs.GetInt("levelNumber"+i.ToString())==0)
					buttonString="Empty";
				else if(PlayerPrefs.GetInt("levelNumber"+i.ToString())==10)
					buttonString="Town";
				else
					buttonString="Level: "+PlayerPrefs.GetInt("levelNumber"+i.ToString())+"\r\n"+"CheckPoint: "+PlayerPrefs.GetInt("checkPointNumber"+i.ToString());

				//dimiourgei ta buttons gia to kathe savefile, ke otan to pataei 
				//o pextis leei ston gameManager pio save na kanei load
				if(GUI.Button (new Rect(Screen.width*0.37f,Screen.height*b*i,Screen.width*0.25f,Screen.height*0.1f),buttonString,emptyTexture))
				{
					gameManagerScript.savefile=i;
					GameObject.Find ("gameManager").SendMessage("load");
				}
				//delete save button
				//otan patithei diagrafei to adistixo save
				if(GUI.Button (new Rect(Screen.width*0.65f,Screen.height*b*i+10,Screen.width*0.05f,Screen.height*0.05f),"",deleteTexture))
				{
					areYouSure=true;
					temp=i;
				}
			}

			//back button
			//an patithei, ksanadixnei to proto menu
			if(GUI.Button (new Rect(x1,y1,x2,y2),"Back",backTexture))
				showLoadGames=false;


		}
		else if(areYouSure)
		{
			GUI.Label ((new Rect(Screen.width*0.40f,Screen.height*0.5f,Screen.width*0.25f,Screen.height*0.1f)),"Are you sure you want to delete this savefile?",areYouSureText);
			if(GUI.Button (new Rect(Screen.width*0.25f,Screen.height*0.6f,Screen.width*0.25f,Screen.height*0.1f),"Yes",emptyTexture))
			{
				for(int i=1;i<=gameManagerScript.numberOfLevels;i++)
					PlayerPrefs.DeleteKey("level"+i+"IsUnlocked"+temp.ToString());
				PlayerPrefs.DeleteKey("levelNumber"+temp.ToString());
				PlayerPrefs.DeleteKey("checkPointNumber"+temp.ToString());
				//PlayerPrefs.DeleteKey("Score"+temp.ToString());
				areYouSure=false;
			}
			if(GUI.Button (new Rect(Screen.width*0.55f,Screen.height*0.6f,Screen.width*0.25f,Screen.height*0.1f),"No",emptyTexture))
			{
				areYouSure=false;
			}

		}
	}
}
