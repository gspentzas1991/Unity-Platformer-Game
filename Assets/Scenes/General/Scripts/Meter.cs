using UnityEngine;
using System.Collections;

public class Meter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//otan gemisei i bara eksafanizete (1 eine to 100% tou scale)
		if (transform.localScale.x == 0)
			transform.GetComponent<Renderer>().enabled = false;
		else
			transform.GetComponent<Renderer>().enabled = true;
		
	}

	//o kivos mikronei analoga me to poso cooldown exei perasei
	//data.x=currentValue , data.y=maxValue
	void getRangedCooldown(Vector2 data)
	{
		//finalScale eine to pososto % pou prepei na fenete (to 1 eine to 100% tou scale)
		float finalScale = 1 * data.x / data.y;
		//i bara gemizei mazi me to cooldown ton ranged epitheseon
		transform.localScale = new Vector2 (finalScale, transform.localScale.y);
	}
}
