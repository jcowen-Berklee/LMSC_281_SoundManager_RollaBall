//LMSC-281 Logic and Programming
//Fall 2015 Jeanine Cowen
//Modified by Raz Ezra

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
	public static void PlayObjectSoundOnce (GameObject thisObject) {
		if (!thisObject.GetComponent<AudioSource> ().isPlaying) {
			thisObject.GetComponent<AudioSource> ().Play ();
		}
	}

	//RE - function to raise the pitch of the AudioSource
	public static void RaisePitch(GameObject thisObject){
		thisObject.gameObject.GetComponent<AudioSource> ().pitch += 0.05f; //RE - Raise the pitch
	}

	//RE - function to random the volume of sound
	public static void RandomVolume(GameObject thisObject){
		thisObject.gameObject.GetComponent<AudioSource> ().volume = Random.Range(0.75f, 1.25f); //RE - randomize volume
	}

	//to use this function in another script use this code:
	//SoundManager.instance.PlayObjectSoundOnce (this.gameObject);
}
