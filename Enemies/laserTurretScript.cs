using UnityEngine;
using System.Collections;

/* Contains the behaviour for the laser turet obstacle
 * The turret fires a laser projectile when it's ranged attack cooldown is over
 * The projectile gets its velocity depending on the name its given (left or right)
 */
public class laserTurretScript : MonoBehaviour
{

    public float maxRangedCooldown;
    float currentRangedCooldown;
    AudioSource audioManager;
    public GameObject laserProjectile;
	Animator characterAnimator;
    
	void Start () {
		audioManager = GetComponent<AudioSource> ();
		characterAnimator = GetComponent<Animator> ();
		currentRangedCooldown = maxRangedCooldown;
        //The speed of the firing animation is inversely proportional to the ranged attack cooldown
		characterAnimator.speed = 2/maxRangedCooldown;
	}
	
	void Update () {
		currentRangedCooldown=gameManagerScript.gameManager.Cooldown(currentRangedCooldown);
		if (currentRangedCooldown == 0)
			RangedAttack ();
	}

    /* Creates a "laser" gameObject, and names it accordingly with where the turret is facing
     */
	void RangedAttack()
	{
		audioManager.Play ();
        string projectileName="bullet(right)";
        if (transform.localScale.x > 0)
        {
            projectileName = "bullet(left)";
        }
        Object bulletInstance = Instantiate(laserProjectile, new Vector2(transform.position.x + 2f, transform.position.y), transform.rotation);
        bulletInstance.name = projectileName;
        currentRangedCooldown = maxRangedCooldown;
	}
}
