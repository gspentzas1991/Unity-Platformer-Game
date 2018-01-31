using UnityEngine;
using System.Collections;

public class Meter : MonoBehaviour {
    
	/* The meter's transform scale shrinks according to the data.x/data.y percentage
	*  data.x=currentValue , data.y=maxValue
    *  When the transform dissapears, the ranged attack cooldown is over and the player can attack again
    */
	public void GetRangedCooldown(Vector2 data)
	{
		float transformPercentage = data.x / data.y;
		transform.localScale = new Vector2 (transformPercentage, transform.localScale.y);
	}
}
