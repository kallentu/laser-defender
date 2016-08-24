using UnityEngine;
using System.Collections;

public class Position : MonoBehaviour {

	void OnDrawGizmos () {
		//Shows the position of every enemy
		Gizmos.DrawWireSphere(transform.position, 1);
	}
}
