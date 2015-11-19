/*Alberto Menezes - Berklee College of Music
 * Logic and Programming - LMSC-281
 * Jeanine Cowen
 * Fall 2015 - October 10th
 * Updated for Midterm Project - October 31st*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speed;
	public GameObject cubePreFabInstance;
	public Text countText;
	public Text loseText;
	public Text HighScore;
	public Text countDownText;
	public Button restart;

	private int count;
	private Rigidbody rb;
	
	//Declarations
	Rigidbody myRigidbody;
	string startMessage = "Game started! Catch the yellow cube, flee from the red cube!";
	string pointMessage = "Point!";
	bool isHidden = true;

	// This gets the Rigidbody component, sets the counter, the counter text, the lose text and starts the countdown coroutine
	void Start () {


		rb = GetComponent <Rigidbody> ();
		count = 0;
		countDownText.text = "";
		countText.text = "Points: " + count.ToString ();
		loseText.text = "";
		HighScore.text = "High Score: " + PlayerPrefs.GetInt ("Player Score");
		StartCoroutine(CountDown());

	//This hides the Restart button
		if (isHidden) {
			restart.enabled = false;
			restart.GetComponentInChildren<CanvasRenderer> ().SetAlpha (0);
			restart.GetComponentInChildren<Text> ().color = Color.clear;
		}	
	}

	// Y is 0 because we don't want the player to move up, the other variables of the vector define the movement
	void FixedUpdate () {

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);
	}

	//This is to pickup the yellow cubes, and also to engage the Restart button and defeat screen
	void OnTriggerEnter (Collider other) {

		// This is the vector for the random position in which the new yellow cube will appear once you pick up the previous one
		Vector3 position = new Vector3 (Random.Range (-9.0F, 9.0F), 0.5F, Random.Range (-9.0F, 9.0F));

		// On collision with the trigger, the previous cube becomes inactive, you add to the count text, the count text is changed,
		// and another cube is instantiated on the previously declared random position
		if (other.gameObject.CompareTag ("Pick Up")) {
			other.gameObject.SetActive (false);
			count = count + 1;
			countText.text = "Points: " + count.ToString ();
			Instantiate (cubePreFabInstance, position, Quaternion.identity);
			// This makes the game faster (therefore harder) as you get more and more points
			Debug.Log (pointMessage);
			if (count >= 10 && count < 20) {
				Time.timeScale = 1.2f;
				Debug.Log ("Speed up!");
			}
			if (count >= 20 && count <30) {
				Time.timeScale = 1.4f;
				Debug.Log ("Speed up!");
			}
			if (count >= 30 && count <40) {
				Time.timeScale = 1.6f;
				Debug.Log ("Speed up!");
			}
			if (count >= 40 && count <50) {
				Time.timeScale = 1.8f;
				Debug.Log ("Speed up!");
			}
			if (count >= 50 && count <60) {
				Time.timeScale = 2.0f;
				Debug.Log ("Speed up!");
			}

		}

		// In case you collide with the enemy, the Restart button shows up, the high score is set (if you beat the previous one),
		// lose text is displayed, lose text and score is outputted to the console, and time scale is set to 0 so that there is
		// no more movement
		if (other.gameObject.CompareTag ("Enemy")) {
			isHidden = false;
			restart.enabled = true;
			restart.GetComponentInChildren<CanvasRenderer> ().SetAlpha(1);
			restart.GetComponentInChildren<Text> ().color = Color.black;
			if (count > PlayerPrefs.GetInt ("Player Score")) {
				PlayerPrefs.SetInt("Player Score", count);
				HighScore.text = "High Score: " + count;
				loseText.text = "You lose! New high score: " + count;
				Debug.Log ("You lose! New high score: " + count);
			}
			else {
				Debug.Log ("You lose! Your score is " + count);
				loseText.text = "You lose! Your score is " + count;
			}
			Time.timeScale = 0;
		}
	}

	// This is the countdown before the game starts, a custom function I wrote
	IEnumerator CountDown () {

		// Since the time scale is altered, I had to go with a really small time, which is this delay float
		float delay = .005F;

		// This loop counts the seconds and displays it to screen
		for (int i = 3; i >= 0; i--) {
			Time.timeScale = 0.01F;
			yield return  new WaitForSeconds(delay);
			Debug.Log (i);
			countDownText.text = "" + i;
		}

		// After the loop is done, the time scale is back to normal and the game starts
		Time.timeScale = 1.0f;
		Debug.Log (startMessage);
		// This instantiates the first pickup cube
		Instantiate (cubePreFabInstance, new Vector3 (9.0F, 0.5F, 7.0F), Quaternion.identity);
		countDownText.text = "Go!";
		yield return new WaitForSeconds (1);
		countDownText.text = "";
	}
}