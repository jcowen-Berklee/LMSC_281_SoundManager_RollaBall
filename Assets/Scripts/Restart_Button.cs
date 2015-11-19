//Made by Alexandre Jannuzzi & Alberto Menezes for Berklee Game Jam 2015

using UnityEngine;
using System.Collections;

public class Restart_Button : MonoBehaviour {

	//This is my custom function, which I made public so that it is accessible by the UI Button
	//When you press Restart you see it in the console, the application loads itself again, and the time scale is restored
	public void OnRestartGame() {

		GetComponent<AudioSource>().Play () ;
		Debug.Log ("You pressed restart!");
		Application.LoadLevel ("AlexeAlberto_GameJam");
		Time.timeScale = 1.0F;
	}
}
