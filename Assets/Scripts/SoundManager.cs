//LMSC-281 Logic and Programming
//Fall 2015 Jeanine Cowen

using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	 
	 //Garett Schmidt - Variabes for pitch and Amplutide automation
    public float amp = 1f;
    public float pch = 1f;
	
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
	//Garett Schmidt - Function to randomize pitch and volume

    void PlayRandomPitchAndVolume()
    {
        //Pitch and Amplitude Randomization.
        amp = Random.Range(0.9f, 1.1f);
        pch = Random.Range(0.8f, 1.2f);
        GetComponent<AudioSource>().volume = amp;
        GetComponent<AudioSource>().pitch = pch;
        GetComponent<AudioSource>().Play();
    }
}
