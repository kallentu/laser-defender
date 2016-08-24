using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 15.0f;
	public float padding = 1f;
	public GameObject projectile;
	public float projectileSpeed;
	public float firingRate;
	public static float health;
	public AudioClip fireSound;
	public AudioClip deathSound;
	public AudioClip hitSound;
	
	private float xmin;
	private float xmax;
	private Lives lives;
	
	void Awake () {
		health = 10;
	}
	
	void Start () {
		lives = GameObject.FindObjectOfType<Lives>();
	
		//finds the z distance to keep it constant
		float distance = transform.position.z - Camera.main.transform.position.z;
		//clamps the ship to screen size in vector3
		Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
		xmin = leftmost.x + padding;
		xmax = rightmost.x - padding;
	}
	
	void FireProjectile () {
		Vector3 startPosition = transform.position + new Vector3 (0f, 0.7f, 0f);
		//Creates the PlayerLaser at the player
		GameObject beam = Instantiate(projectile, startPosition, Quaternion.identity) as GameObject;
		beam.GetComponent<Rigidbody2D>().velocity = new Vector3 (0, projectileSpeed, 0);
		
		AudioSource.PlayClipAtPoint(fireSound, transform.position, 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			//Invokes the method in 0.0001 seconds and then repeatedly every "firingRate"
			InvokeRepeating ("FireProjectile", 0.0000001f, firingRate);
		}
		
		if (Input.GetKeyUp(KeyCode.Space)) {
			CancelInvoke("FireProjectile");
		}
	
		if ((Input.GetKey (KeyCode.LeftArrow)) || (Input.GetKey (KeyCode.A)))
		{
			//transform.position += new Vector3 (-speed * Time.deltaTime, 0f, 0f);
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
		else if ((Input.GetKey (KeyCode.RightArrow)) || (Input.GetKey (KeyCode.D)))
		{
			//transform.position += new Vector3 (speed * Time.deltaTime, 0f, 0f);
			transform.position += Vector3.right * speed * Time.deltaTime;
		}
		
		//restricts player to game space
		float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
		transform.position = new Vector3(newX, transform.position.y, transform.position.z);
	}
	
	IEnumerator OnTriggerEnter2D (Collider2D collider) {
		//If the collider object is a projectile, it will turn into missile
		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		if (missile) {
			//Subtracts damage from health
			health -= missile.GetDamage();
			
			//Destroys projectile bc it hit
			missile.Hit();
			if (health <= 0) {
				Destroy(gameObject);
				Dead ();
				AudioSource.PlayClipAtPoint(deathSound, transform.position, 0.5f);
			}
			else {
				AudioSource.PlayClipAtPoint(hitSound, transform.position, 0.1f);
				lives.LoseLives(health);
			}
		}
		this.gameObject.GetComponent<SpriteRenderer>().color = new Color (1f, 0.2f, 0.2f, 1f);
		yield return new WaitForSeconds(0.3f);
		this.gameObject.GetComponent<SpriteRenderer>().color = new Color (1f, 1f, 1f, 1f);
		yield return new WaitForSeconds(0.3f);
		
	}
	
	void Dead () {
		LevelManager man = GameObject.Find("Level Manager").GetComponent<LevelManager>();
		man.LoadLevel("Win");
	}
	
}
