using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

	static MusicPlayer initialLoad = null;
	
	public AudioClip startClip;
	public AudioClip instructionClip;
	public AudioClip gameClip;
	public AudioClip endClip;
	
	private AudioSource music;

	void Awake () {

		if (initialLoad != null)
		{
			Destroy (gameObject);
		}
		else
		{
			initialLoad = this;
			GameObject.DontDestroyOnLoad(gameObject);
			//Grabs music from own audiosource
			music = GetComponent<AudioSource>();
			music.clip = startClip;
			music.loop = true;
			music.Play();
		}
	}
	
	void OnLevelWasLoaded (int level) {
		Debug.Log("MusicPlayer: Loaded level" + level);
		music.Stop();
		
		if (level == 0) {
			music.clip = startClip;
		}
		else if (level == 1) {
			music.clip = instructionClip;
		}
		else if (level == 2) {
			music.clip = gameClip;
		}
		else if (level == 3) {
			music.clip = endClip;
		}
		
		music.Play ();
	}
}
