using UnityEngine;
using System.Collections;

/* Contains the behaviour of the breakable Ice Block platforms
 * If the player touches the Ice Block for 0.4 seconds, the ice block cracks. After a while it breaks
 */
public class iceBlockScript : MonoBehaviour {
	public float timeUntilIceCracks;
	SpriteRenderer spriteRend;
	public Sprite cracked;
	AudioSource audiManager;
    
	void Start () 
	{
		audiManager = GetComponent<AudioSource> ();
		spriteRend=GetComponent<SpriteRenderer> ();
	}

    private void Update()
    {
    }

    /* As long as the player is touching the Ice Block, the timeUntilIceCracks counts down
    */
    void OnCollisionStay2D(Collision2D collision)
	{
        
        if (collision.gameObject.name=="Player")
		{
			timeUntilIceCracks -= Time.deltaTime;
		}
        if ((timeUntilIceCracks < 0.4) && (spriteRend.sprite != cracked))
        {
            audiManager.Play();
            spriteRend.sprite = cracked;
        }
        if (timeUntilIceCracks <= 0)
            Destroy(this.gameObject);
    }
}
