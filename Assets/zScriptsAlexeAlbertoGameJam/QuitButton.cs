/*Alberto Menezes - Berklee College of Music
 * Logic and Programming - LMSC-281
 * Jeanine Cowen
 * Fall 2015 - October 17th*/

using UnityEngine;
using System.Collections;

public class QuitButton : MonoBehaviour {

	// This is so that you can leave the application when you hit the quit button
	// I added this because I have been sending these games to my dad, and he mentioned that it is kind of inconvenient to have to hit alt+F4 to leave a program
	// So I guess due to "client demand" hahah
	public void OnQuitButton () {
		Application.Quit() ;
	}
}
	
