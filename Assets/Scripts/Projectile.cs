using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public float damage = 1f;
	
	public float GetDamage () {
		//Sends whatever calls the function, the damage
		return damage;
	}
	
	public void Hit () {
		//Destroys the projectile once it hits something
		Destroy (gameObject);
	}
}
