using UnityEngine;
using System.Collections;

/* Contains the code for the checkPoint system
 * When the player touches a checkpoint, the game saves his progress and refills his health
 * The saving of the game happens through the gameManager
 */
public class checkPoint : MonoBehaviour {
	public int levelNumber;
	public int checkPointNumber;
	bool playerTouchedCheckpoint=false;
	Animator checkPointAnimator;
	AudioSource flagAudio;
    
	void Start () 
	{
		checkPointAnimator = GetComponent<Animator>();
		flagAudio = GetComponent<AudioSource>();
        
	}
	

	void OnTriggerEnter2D(Collider2D targetCollider)
	{
        //Checks if the player has collided with the checkpoint
		if((targetCollider.name=="Player")&&(!playerTouchedCheckpoint))
		{
            playerTouchedCheckpoint = true;
            targetCollider.gameObject.SendMessage("GainHealth",100);
            //sends the "check" boolean to the animator, to change the flags animation
            checkPointAnimator.SetBool("checked", playerTouchedCheckpoint);
            flagAudio.Play();
            //saves the level and checkpoint numbers on the save file
            gameManagerScript.gameManager.Save(levelNumber,checkPointNumber);
			
		}
	}
}
