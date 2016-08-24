using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {
	public GameObject projectile;
	public float enemyHealth = 7f;
	public float projectileSpeed = -10f;
	public float shotsPerSeconds = 1f;
	public int scoreValue = 250;
	public AudioClip fireFromEnemy;
	public AudioClip deathEnemy;
	public AudioClip hitEnemy;
	
	private ScoreKeeper scoreKeeper;
	
	void Start () {
		scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
	}

	void Update () {
		float probability = Time.deltaTime * shotsPerSeconds;
		if (Random.value < probability) {
			Fire ();
		}
	}
	
	void Fire () {
		Vector3 startPosition = transform.position + new Vector3 (0f, -0.7f, 0f); 
		GameObject missile = Instantiate (projectile, startPosition, Quaternion.identity) as GameObject;
		missile.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed, 0);
		
		AudioSource.PlayClipAtPoint(fireFromEnemy, transform.position, 0.1f);
	}

	IEnumerator OnTriggerEnter2D (Collider2D collider) {
		//If the collider object is a projectile, it will turn into missile
		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		if (missile) {
			//Subtracts damage from health
			enemyHealth -= missile.GetDamage();
			
			//Destroys projectile bc it hit
			missile.Hit();
			if (enemyHealth <= 0) {
				Die ();
			}
			else {
				AudioSource.PlayClipAtPoint (hitEnemy, transform.position, 0.1f);
			}
		}
		
		this.gameObject.GetComponent<SpriteRenderer>().color = new Color (1f, 0.5f, 0.5f, 1f);
		yield return new WaitForSeconds(0.3f);
		this.gameObject.GetComponent<SpriteRenderer>().color = new Color (1f, 1f, 1f, 1f);
		yield return new WaitForSeconds(0.3f);
	}
	
	void Die () {
		Destroy (gameObject);
		//Sends the enemy value to the scorekeeper script
		scoreKeeper.Score(scoreValue);
		AudioSource.PlayClipAtPoint(deathEnemy, transform.position, 0.5f);
	}

}
