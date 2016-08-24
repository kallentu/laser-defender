using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Lives : MonoBehaviour {
	private Text text;
	private float amount;
	
	void Start () {
	amount = 10;
	
	text = GetComponent<Text>();
	text.text = PlayerController.health.ToString();
	}

	public void LoseLives (float health) {
		amount = health;
		text.text = amount.ToString();
	}
	
	public void FinishedWave () {
		amount += 1;
		text.text = amount.ToString();
	}
}
