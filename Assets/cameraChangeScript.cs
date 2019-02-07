using UnityEngine;
using System.Collections;

public class cameraChangeScript : MonoBehaviour {
	public float newcameraSize;

	public float cameraPosX;
	public float cameraPosY;

	public Camera cam;
	public GameObject player; 
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.name == "Player") 
		{
			if (cam.orthographicSize == 10)
			{
				cam.orthographicSize = newcameraSize;
				cam.transform.parent = null;
				cam.transform.localPosition = new Vector3 (cameraPosX, cameraPosY, -1.77f);
			}
			else if (cam.orthographicSize == newcameraSize)
			{
				cam.orthographicSize = 10;
				cam.transform.parent=player.transform;
				cam.transform.localPosition= new Vector3(0.13f,3.2f,-1.77f);
			}
		}
	}
}
