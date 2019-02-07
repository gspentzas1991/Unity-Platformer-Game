using UnityEngine;
using System.Collections;

public class feetColliderScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void gotHit(int damage)
	{
		transform.parent.SendMessage ("gotHit", damage);
	}
}
