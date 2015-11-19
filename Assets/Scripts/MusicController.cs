//Made by Alexandre Jannuzzi & Alberto Menezes for Berklee Game Jam 2015

using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {

	public AudioSource [] sounds;
	public AudioSource layer1;
	public AudioSource layer2;
	public AudioSource layer3;

	void Start () {
		sounds = GetComponents<AudioSource> ();
		layer1 = sounds [0];
		layer2 = sounds [1];
		layer3 = sounds [2];
		layer1.volume = 1;
		layer2.volume = 0;
		layer3.volume = 0;
	}
	
	void FixedUpdate () {

		// This gets the scripts from player 1 and player 2 and then the current count
		GameObject player1 = GameObject.Find ("Player1");
		GameObject player2 = GameObject.Find ("Player2");
		Player01Movement player1movement = player1.GetComponent<Player01Movement> ();
		Player02Movement player2movement = player2.GetComponent<Player02Movement> ();
		if ((player1movement.count >= 5 && player1movement.count < 8) || (player2movement.count >= 5 && player2movement.count < 8)) {
			layer2.volume = 1;
		}
		if (player1movement.count >= 8 | player2movement.count >= 8) {
			layer3.volume = 1;
		} 
		else {
			return;
		}
	}
}
