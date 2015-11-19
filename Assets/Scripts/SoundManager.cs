//LMSC-281 Logic and Programming
//Fall 2015 Jeanine Cowen

using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	
	//since we know we will only have one sound manager in the game we can create a static instance
	public static SoundManager instance;

	//if we want to limit the ability for outside classes to make changes we can use the following code
	//this sets our class to basic read-only status
	//	public static SoundManager instance { 
	//		get; 
	//		private set; 
	//	}
	
	// This will make the class available when the level loads
	void Awake () {
		instance = this;
	}
	
	//this function/method will check to make sure the audiosource on the passed in object is not already playing
	//if it is playing it will not play, otherwise it will
	public void PlayObjectSoundOnce (GameObject thisObject) {
		
		if (!thisObject.GetComponent<AudioSource>().isPlaying) {
			thisObject.GetComponent<AudioSource>().Play ();
		}
	}

	//to use this function in another script use this code:
	//SoundManager.instance.PlayObjectSoundOnce (this.gameObject);
}
