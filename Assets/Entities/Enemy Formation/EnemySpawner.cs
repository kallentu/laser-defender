using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	public GameObject enemyPrefab;
	public float width;
	public float height;
	public float speed;
	public float spawnDelay = 0.5f;
	public Sprite[] enemySprite;
	
	private bool movingRight = true;
	private float xmax;
	private float xmin;
	private int timesBeat;
	private Lives life;

	// Use this for initialization
	void Start () {
		life = GameObject.FindObjectOfType<Lives>();
	
		//Finds constant z distance
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		//Clamps the enemies to a certain area
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera));
		
		xmax = rightBoundary.x;
		xmin = leftBoundary.x;
		
		timesBeat = 0;
		SpawnUntilFull();
	}
	
	void SpawnUntilFull () {
		Transform freePosition = NextFreePosition();
		if (freePosition) {
			if (timesBeat == 0 || timesBeat == 1) {
				//If there is a free place, we can instantiate a prefab
				enemyPrefab.GetComponent<SpriteRenderer>().sprite = enemySprite[0];
				GameObject enemy = Instantiate (enemyPrefab, freePosition.transform.position, Quaternion.identity) as GameObject;
				enemy.transform.parent = freePosition;
			}
			else if (timesBeat == 2 || timesBeat == 3) {
				//If there is a free place, we can instantiate a prefab
				enemyPrefab.GetComponent<SpriteRenderer>().sprite = enemySprite[1];
				GameObject enemy = Instantiate (enemyPrefab, freePosition.transform.position, Quaternion.identity) as GameObject;
				enemy.transform.parent = freePosition;
			}
			else if (timesBeat == 4) {
				//If there is a free place, we can instantiate a prefab
				enemyPrefab.GetComponent<SpriteRenderer>().sprite = enemySprite[2];
				GameObject enemy = Instantiate (enemyPrefab, freePosition.transform.position, Quaternion.identity) as GameObject;
				enemy.transform.parent = freePosition;
			}
		}
		
		if (NextFreePosition()) {
			//Makes sure it spawns only when entire formation is down.
			Invoke ("SpawnUntilFull", spawnDelay);
		}
	}
	
	public void OnDrawGizmos () {
		Gizmos.DrawWireCube(transform.position, new Vector3 (width, height));
	
	}
	
	// Update is called once per frame
	void Update () {
		if(movingRight) {
			transform.position += Vector3.right * speed * Time.deltaTime;
		}
		else {
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
		
		float rightEdgeOfFormation = transform.position.x + (0.45f * width);
		float leftEdgeOfFormation = transform.position.x - (0.45f * width);

		if (leftEdgeOfFormation <= xmin) {
			movingRight = true;
		}
		else if (rightEdgeOfFormation >= xmax) {
			movingRight = false;
		}
		
		if (AllMembersDead()) {
			if (enemyPrefab.GetComponent<SpriteRenderer>().sprite != enemySprite[2]) {
				timesBeat += 1;
			}
			else {
				timesBeat = 0;
			}
			life.FinishedWave();
			SpawnUntilFull ();
			
		}
		
	}
	
	Transform NextFreePosition () {
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount == 0) {
			// if a child count comes back as 0 or as empty, then it returns the tranforms back to the caller
			return childPositionGameObject;
			}
		}
		return null;
	
	}
	
	bool AllMembersDead() {
		//For every transform (Position), we get the count of the child. For each transform so it will add up.
		foreach(Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount > 0) {
				//If childs are still up, then the bool method is false and will not activate the if statement in Update()
				return false;
			}
		}
		return true;
	}
}
