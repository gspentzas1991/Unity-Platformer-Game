using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gameManagerScript : MonoBehaviour {
	public static int numberOfLevels=4;
	//public metavliti pou dixnei se pio savefile eimaste. To main menu tin orizei otan patithei kapio koubi
	public static int savefile=-1;
	//se pio level eine o pextis
	public int levelNumber;
	//to ranged weapon pou dialekse
	string rangedWeapon;
	//to melle weapon pou dialekse
	string meleeWeapon;

	AudioSource audioManager;
	public AudioClip level1Music;
	public AudioClip level2Music;
	public AudioClip level3Music;
	public AudioClip level4Music;
	public AudioClip levelTownMusic;
	public AudioClip creditsTheme;
	public AudioClip mainMenuTheme;


	// Otan dimiourgite pernei oti dedomeno thelei ke proxoraei sto main menu
	void Start () 
	{
		//Screen.SetResolution (960, 430, false);

		audioManager=GetComponent<AudioSource>();
		Object.DontDestroyOnLoad (this);
		changeMusic (1);
		Application.LoadLevel(1);
	}
	
	// Update is called once per frame
	void Update () 
	{


	}

	//an iparxoun apothikevmena dedomena, ta prosthetei etsi oste na parei to sosto level
	//pou tha ksekinisei me to continue. An den iparxei levelNumber, tote kanei newgame, ke ksekinaei
	//sto level 2

	//to load kalite apo to main menu
	void load()
	{
		if (Application.loadedLevel==1)
			levelNumber = 1;
		if (PlayerPrefs.HasKey ("levelNumber" + savefile))
		{
			//an to levelNumber eine 10, tote tha paei sto town
			if (PlayerPrefs.GetInt ("levelNumber" + savefile) == 10)
				levelNumber = 6;
			else
				levelNumber += PlayerPrefs.GetInt ("levelNumber" + savefile);
		}
		else
		{
			levelNumber = 6;
			//afou eine newgame, simenei oti prepei na exei ola ta level locked ektos to proto
			PlayerPrefs.SetInt("level1IsUnlocked"+savefile,1);
			for(int i=2;i<=numberOfLevels;i++)
				PlayerPrefs.SetInt("level"+i+"IsUnlocked"+savefile,0);
		}

		Application.LoadLevel (levelNumber);
		changeMusic (levelNumber);

	}

	void changeMusic(int levelNo)
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
	//apothikevei ton arithmo tou level pou eimaste, otan o pextis akoubaei ena checkPoint
	void save(int data)
	{
		PlayerPrefs.SetInt ("levelNumber"+savefile,data);
		PlayerPrefs.Save ();
	}

	void unlock(int data)
	{

		PlayerPrefs.SetInt ("level"+data.ToString()+"IsUnlocked"+savefile,1);
		PlayerPrefs.Save ();
	}


}
