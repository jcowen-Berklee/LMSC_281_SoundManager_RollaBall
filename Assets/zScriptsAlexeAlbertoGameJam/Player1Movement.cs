//Made by Alexandre Jannuzzi & Alberto Menezes for Berklee Game Jam 2015

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player1Movement : MonoBehaviour {

	// Pick-up cube
	public GameObject cubePreFabInstance;
	// Enemy that spawns halfway through the game
	public GameObject Enemy1PreFab;
	// Explosion animation
	public GameObject explosion;
	// Walls that move around
	public GameObject Obstacle1;
	public GameObject Obstacle2;
	// UI Texts
	public Text count1Text;
	public Text loseText;
	public Text countDownText;
	public Button restart;
	public Button quit;
	public float speed;
	public float speedMultiplier;
	public int powerUpDuration;
	public int powerUpSpawn;
	public int deathTime;
	public int pointsToVictory;
	public AudioSource [] sounds;
	public AudioSource speedOn;
	public AudioSource speedOff;
	public AudioSource killOn;
	public AudioSource killOff;
	public AudioSource countSound;
	public AudioSource goSound;
	public float count;

	private Rigidbody rb;
	private Light pul;
	private MeshRenderer mesh;
	
	bool isHidden = true;
	bool canSpawn = true;
	bool speedUp = false;
	bool canKill = false;

	void Start () {

		rb = GetComponent <Rigidbody> ();
		pul = GetComponent <Light> ();
		mesh = GetComponent <MeshRenderer> ();
		count = 0;
		countDownText.text = "";
		count1Text.text = "P1 Points: " + count.ToString ();
		loseText.text = "";
		sounds = GetComponents<AudioSource>();
		speedOn = sounds[0];
		speedOff = sounds[1];
		killOn = sounds[2];
		killOff = sounds[3];
		countSound = sounds[4];
		goSound = sounds [5];
		StartCoroutine(CountDown());
		
		//This hides the Restart button
		if (isHidden) {
			restart.enabled = false;
			restart.GetComponentInChildren<CanvasRenderer> ().SetAlpha (0);
			restart.GetComponentInChildren<Text> ().color = Color.clear;
			quit.enabled = false;
			quit.GetComponentInChildren<CanvasRenderer> ().SetAlpha (0);
			quit.GetComponentInChildren<Text> ().color = Color.clear;
		}
	}
	
	void FixedUpdate () {

		// Movement (the input keys are defined in Unity's input window)
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		// This defines the speed, if you have the speedup or not
		if (speedUp) {
			rb.AddForce (movement * speed * speedMultiplier);
		}
			else {
			rb.AddForce (movement * speed);
		}
	}

	IEnumerator OnTriggerEnter (Collider other) {
		
		// This is the vector for the random position in which the new yellow cube will appear once you pick up the previous one
		Vector3 position = new Vector3 (Random.Range (-12.0F, 12.0F), 0.5F, Random.Range (-12.0F, 12.0F));
		// These are the vectors for the spawning of the obstacles
		Vector3 position2 = new Vector3 (Random.Range (-9.0F, 9.0F), 0, Random.Range (-9.0F, 9.0F));
		Vector3 position3 = new Vector3 (Random.Range (-8.0F, 8.0F), 0, Random.Range (-8.0F, 8.0F));
		// These are the vectors for the player position and the enemy position
		Vector3 player1position = new Vector3 (12.0F, 0.5F, 0F);
		Vector3 enemyposition = new Vector3 (12.0F, 0.5F, 12.0F);

		// On collision with the trigger, the previous cube becomes inactive, you add to the count text, the count text is changed,
		// and another cube is instantiated on the previously declared random position
		if (other.gameObject.CompareTag ("Pick Up")) {
			other.gameObject.transform.position = position;
			other.gameObject.GetComponent<AudioSource>().Play () ;
			count = count + 1;
			count1Text.text = "P1 Points: " + count.ToString ();
			Obstacle1.transform.position = position2;
			Obstacle2.transform.position = position3;
			// Halfway through the game, the enemy spawns
			if (count == pointsToVictory / 2 && canSpawn == true) {
				Instantiate (Enemy1PreFab, enemyposition, Quaternion.identity);
				canSpawn = false;
			}
		}
		// On victory, the buttons pop-up and time freezes
		if (count == pointsToVictory) {
			Debug.Log ("Game Over! Player 1 Wins");
			loseText.text = "Game Over! Player 1 Wins";
			isHidden = false;
			restart.enabled = true;
			restart.GetComponentInChildren<CanvasRenderer> ().SetAlpha(1);
			restart.GetComponentInChildren<Text> ().color = Color.black;
			quit.enabled = true;
			quit.GetComponentInChildren<CanvasRenderer> ().SetAlpha(1);
			quit.GetComponentInChildren<Text> ().color = Color.black;
			Time.timeScale = 0;
		}

		// Speed power-up pickup
		if (other.gameObject.CompareTag ("Speed")) {
			speedUp = true;
			speedOn.Play();
			other.gameObject.SetActive (false);
			yield return new WaitForSeconds (powerUpDuration);
			speedUp = false;
			speedOff.Play();
			yield return new WaitForSeconds (powerUpSpawn);
			other.gameObject.SetActive (true);
		}

		// Destroyer power-up pickup
		if (other.gameObject.CompareTag ("Kill")) {
			pul.enabled = !pul.enabled;
			killOn.Play ();
			other.gameObject.SetActive (false);
			canKill = true;
			yield return new WaitForSeconds (powerUpDuration);
			pul.enabled = false;
			killOff.Play ();
			canKill = false;
			yield return new WaitForSeconds (powerUpSpawn);
			other.gameObject.SetActive (true);
		}

		// When you hit the enemy that spawns halfway through the game
		if (other.gameObject.CompareTag ("Enemy")) {
			Instantiate(explosion, transform.position, Quaternion.identity);
			mesh.enabled = false;
			count = count - 1;
			count1Text.text = "P1 Points: " + count.ToString ();
			Destroy (other.gameObject);
			rb.constraints = RigidbodyConstraints.FreezeAll;
			yield return new WaitForSeconds (deathTime);
			gameObject.transform.position = player1position;
			rb.constraints = RigidbodyConstraints.None;
			mesh.enabled = !mesh.enabled;
			yield return new WaitForSeconds (powerUpSpawn);
			Instantiate (Enemy1PreFab, enemyposition, Quaternion.identity);
		}
	}

	IEnumerator OnCollisionEnter (Collision col) {

		Vector3 player2position = new Vector3 (-12.0F, 0.5F, 0F);

		// When you have the powerup, you can kill the other player
		if (canKill) {
			if (col.gameObject.name == "Player2") {
				Player2Movement player2 = col.gameObject.GetComponent <Player2Movement> ();
				player2.count -= 1;
				player2.count2Text.text = "P2 Points: " + player2.count.ToString ();
				col.gameObject.SetActive (false);
				Instantiate (explosion, col.transform.position, Quaternion.identity);
				col.gameObject.transform.position = player2position;
				yield return new WaitForSeconds (deathTime);
				col.gameObject.SetActive (true);
			}
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
			countSound.Play ();
			countDownText.text = " " + i;
		}
		
		// After the loop is done, the time scale is back to normal and the game starts
		Time.timeScale = 1.0f;
		// This instantiates the first pickup cube
		countDownText.text = "Go!";
		goSound.Play ();
		yield return new WaitForSeconds (1);
		countDownText.text = "";
	}
}