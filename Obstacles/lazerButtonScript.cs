using UnityEngine;
using System.Collections;

/* Contains the behaviour for the button that stops the laser wall
 * When the button is damaged, it flips and destroyes the laser wall object
 */
public class lazerButtonScript : MonoBehaviour {
	public GameObject mainLazerBeam;

	AudioSource audioMan;
	bool buttonIsOff=false;
    
	void Start () {
		audioMan = GetComponent<AudioSource> ();
	}
	

	void GotHit(int damage)
	{
		if (!buttonIsOff)
		{
			audioMan.Play();
			buttonIsOff=true;
			transform.localScale=new Vector3(-1,1,1);
			Destroy (mainLazerBeam);
		}
	}
}
