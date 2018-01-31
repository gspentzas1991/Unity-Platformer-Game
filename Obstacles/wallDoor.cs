using UnityEngine;
using System.Collections;

public class wallDoor : MonoBehaviour 
{
	public bool isClosed;
    public GameObject boss;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isClosed = true;
        }
    }

    void Update () 
	{
        if (boss == null)
            isClosed = false;

		if (!isClosed)
		{
			GetComponent<Collider2D>().enabled = false;
			GetComponent<Renderer>().enabled=false;
		}
		else
		{
			GetComponent<Collider2D>().enabled = true;
			GetComponent<Renderer>().enabled=true;
		}
	
	}

}