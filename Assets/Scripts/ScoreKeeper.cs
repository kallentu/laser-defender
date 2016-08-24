using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {
	public static int score = 0;
	
	private Text showText;

	void Start () {
		 showText = GetComponent<Text>();
		 showText.text = score.ToString();

	}

	public void Score (int points) {
		score += points;
		
		//Get the text of showText and assigns the string of int score
		showText.text = score.ToString();
	}
	
	public static void Reset () {
		score = 0;
		PlayerController.health = 10;
	}
	
}
