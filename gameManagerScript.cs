using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/*The "Game Manager" game object is a game object that contains static variables and functions that other
* game objects can call, like the current level number, or the save function. 
* It is attached to a single gameObject that never gets destroyed.*/
public class gameManagerScript : MonoBehaviour {

    //The gameManager singleton. Can only be set from gameManagerScript
    public static gameManagerScript gameManager { get; private set; }

    public const  int numberOfLevels=4;
    //Shows the save slot we are using (1-10)
    //Gets a value when the player selects a savefile from the main menu
    public int savefile=-1;
	static AudioSource audioManager;
	public  AudioClip level1Music;
	public  AudioClip level2Music;
	public  AudioClip level3Music;
	public  AudioClip level4Music;
	public  AudioClip levelTownMusic;
	public  AudioClip creditsTheme;
	public  AudioClip mainMenuTheme;
    public KeyCode pauseKey;
    //The offset on the Y-Axis for the pause scren GUI
    public float pauseGuiOffsetY;
    public GUIStyle pauseMenuButtonTexture;
    bool isPaused = false;

    /* initialises the static gameManager
     */
    private void Awake()
    {
        //Sets "fastest" quality setting, and resolution at: 1280x720
        QualitySettings.SetQualityLevel(0);
        Screen.SetResolution(1280, 720,false,60);

        if (gameManager == null)
        {
            gameManager = this;
        }
    }

    void Start ()
    {
        DontDestroyOnLoad(gameObject);
        audioManager =GetComponent<AudioSource>();
        //Loads the Main Menu scene
        SceneManager.LoadScene(1);
        ChangeMusic(1);
    }

    
    void Update()
    {
        CheckForPauseButton();
    }

    /* If the player hits the pause key, the gameManager sends a message to every 
     * gameobject in the level that the game is paused
     */
    void CheckForPauseButton()
    {
        //The game doesn't pause if the button is pressed in the "Main Menu" scene
        if ((Input.GetKeyDown(pauseKey))&&(SceneManager.GetActiveScene().buildIndex!=1))
        {
            Object[] objectList = FindObjectsOfType(typeof(GameObject));
            if (!isPaused)
            {
                Time.timeScale = 0;
                isPaused = true;
                foreach (GameObject gObject in objectList)
                    gObject.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                Time.timeScale = 1;
                isPaused = false;
                foreach (GameObject gObject in objectList)
                    gObject.SendMessage("OnUnPauseGame", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    /* Creates the pause screen buttons
     */
    void OnGUI()
    {
        if (isPaused)
        {
            //Checks if the loaded scene is not the "Level Select" scene
            if (SceneManager.GetActiveScene().buildIndex != 6)
            {
                if (GUI.Button(new Rect(Screen.width * 0.37f, Screen.height * pauseGuiOffsetY, Screen.width * 0.25f, Screen.height * 0.1f), "Return to Town", pauseMenuButtonTexture))
                {
                    Time.timeScale = 1;
                    isPaused = false;
                    ChangeMusic(6);
                    SceneManager.LoadScene(6);
                }
            }
            if (GUI.Button(new Rect(Screen.width * 0.37f, Screen.height * (pauseGuiOffsetY + 0.2f), Screen.width * 0.25f, Screen.height * 0.1f), "Quit Game", pauseMenuButtonTexture))
            {
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    isPaused = false;
                    ChangeMusic(1);
                    SceneManager.LoadScene(1);
                }
                else
                    Application.Quit();
            }
        }


    }

    /* Loads a save file. 
     * If a save file exists in the selected save slot, it reads the level number on the save file, and loads the scene of that level
     * If a save file doesn't exist in the selected save slot, it loads the "Town" level and creates a save file
     */
    public void Load()
	{
        //The number of the level we want to load
        //1: main menu , 2-5 : levels 1-4 , 6 : Level Select , 7 : Credits
        int levelNumber = 1;
        /*Checks if a "levelNumber file" exists for this save slot (e.g. levelNumber3 for the slot 3)
         *If it doesn't exist, it creates a new save file for this save slot
         */
        if (PlayerPrefs.HasKey ("levelNumber" + savefile))
		{
            //Scene 1 is the main menu, so we have to add 1 to the level number to get the correct one
			levelNumber = PlayerPrefs.GetInt ("levelNumber" + savefile) + 1;
		}
		else
		{
			levelNumber = 6;
			/* Creates flags for this save file, for every level depicting if they are unlocked or not
             * Makes it so only the first level is unlocked on the Level Select
             */
			PlayerPrefs.SetInt("level1IsUnlocked"+savefile,1);
			for(int i=2;i<=numberOfLevels;i++)
				PlayerPrefs.SetInt("level"+i+"IsUnlocked"+savefile,0);
		}

        SceneManager.LoadScene(levelNumber);
		ChangeMusic (levelNumber);

	}

    //Changes the music clip to the level's theme
	public void ChangeMusic(int levelNo)
	{
		if (levelNo == 1)
			audioManager.clip = mainMenuTheme;
		else if (levelNo == 2)
			audioManager.clip = level1Music;
		else if (levelNo == 3)
			audioManager.clip = level2Music;
		else if (levelNo == 4)
			audioManager.clip = level3Music;
		else if (levelNo == 5)
			audioManager.clip = level4Music;
		else if (levelNo == 6)
			audioManager.clip = levelTownMusic;
		else if (levelNo == 7)
			audioManager.clip = creditsTheme;
		audioManager.Play ();

	}
	//Saves the level number and checkPointNumber that the player is on, on a "levelNumber#" file
	public void Save(int levelNumber,int checkPointNumber)
	{
		PlayerPrefs.SetInt ("levelNumber"+savefile,levelNumber);
        PlayerPrefs.SetInt("checkPointNumber"+savefile, checkPointNumber);
        PlayerPrefs.Save ();
	}

    //Unlocks the next level on the level select
	public void Unlock(int levelToUnlock)
	{

		PlayerPrefs.SetInt ("level"+levelToUnlock.ToString()+"IsUnlocked"+savefile,1);
		PlayerPrefs.Save ();
	}

    /* Reduces the cooldown of the variable by the second
     */
    public float Cooldown(float cooldownValue)
    {
        if (cooldownValue > 0)
            cooldownValue -= Time.deltaTime;
        else
            cooldownValue = 0;
        return cooldownValue;
    }


    /* Flips the requested transform on the X-Axis
     */
    public void FlipTransform(Transform targetTransform)
    {
        Vector3 theScale = targetTransform.localScale;
        theScale.x *= -1;
        targetTransform.localScale = theScale;
    }
    

}
