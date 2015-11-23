//Made by Alexandre Jannuzzi & Alberto Menezes for Berklee Game Jam 2015
//Modified by Raz Ezra

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player02Movement : MonoBehaviour {

	// Pick-up cube
	public GameObject cubePreFabInstance;
	// Enemy that spawns halfway through the game
	public GameObject Enemy2PreFab;
	// Explosion animation
	public GameObject explosion;
	// Moving walls
	public GameObject Obstacle3;
	public GameObject Obstacle4;
	// UI Texts
	public Text count2Text;
	public Text loseText;
	public float speed;
	public float speedMultiplier;
	public Button restart;
	public Button quit;
	public int powerUpDuration;
	public int powerUpSpawn;
	public int deathTime;
	public int pointsToVictory;
	public AudioClip [] sounds; //RE - store all sounds related to this object
	AudioSource playSound; //RE - for relationship with AudioSource component
	public float count;

	private Rigidbody rb;
	private Light pul;
	private MeshRenderer mesh;
	
	bool canSpawn = true;
	bool speedUp = false;
	bool canKill = false;
	
	void Start () {

		rb = GetComponent <Rigidbody> ();
		pul = GetComponent <Light> ();
		mesh = GetComponent <MeshRenderer> ();
		count = 0;
		count2Text.text = "P2 Points: " + count.ToString ();
	}
	
	void FixedUpdate () {

		// Movement (the input keys are defined in Unity's input window)
		float moveHorizontal = Input.GetAxis ("Horizontal2");
		float moveVertical = Input.GetAxis ("Vertical2");
		
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
		Vector3 player2position = new Vector3 (-12.0F, 0.5F, 0F);
		Vector3 enemyposition = new Vector3 (-12.0F, 0.5F, -12.0F);
		
		// On collision with the trigger, the previous cube becomes inactive, you add to the count text, the count text is changed,
		// and another cube is instantiated on the previously declared random position
		if (other.gameObject.CompareTag ("Pick Up")) {
			other.gameObject.transform.position = position;
			SoundManager.PlayObjectSoundOnce(other.gameObject); //RE - Call Sound Manager to play sound of other object
			SoundManager.RaisePitch(other.gameObject); //RE - Call Sound Manager to raise the pitch of object's AudioSource
			SoundManager.RandomVolume(other.gameObject); //RE - Call SoundManager to randomize the volume of an object's AudioSource
			count = count + 1;
			count2Text.text = "P2 Points: " + count.ToString ();
			Obstacle3.transform.position = position2;
			Obstacle4.transform.position = position3;
			// Halfway through the game, the enemy spawns
			if (count == pointsToVictory / 2 && canSpawn == true) {
				Instantiate (Enemy2PreFab, enemyposition, Quaternion.identity);
				canSpawn = false;
			}
			// On victory, the buttons pop-up and time freezes
			if (count == pointsToVictory) {
				Debug.Log ("Game Over! Player 2 Wins");
				loseText.text = "Game Over! Player 2 Wins";
				restart.enabled = true;
				restart.GetComponentInChildren<CanvasRenderer> ().SetAlpha(1);
				restart.GetComponentInChildren<Text> ().color = Color.black;
				quit.enabled = true;
				quit.GetComponentInChildren<CanvasRenderer> ().SetAlpha(1);
				quit.GetComponentInChildren<Text> ().color = Color.black;
				Time.timeScale = 0;
			}
		}

		// Speed power-up pickup
		if (other.gameObject.CompareTag ("Speed")) {
			speedUp = true;
			PlaySoundOnce (0); //RE - Play Speed On sound
			other.gameObject.SetActive (false);
			yield return new WaitForSeconds (powerUpDuration);
			speedUp = false;
			PlaySoundOnce (1); //RE - Play Speed Off sound
			yield return new WaitForSeconds (powerUpSpawn);
			other.gameObject.SetActive (true);
			SoundManager.PlayObjectSoundOnce(other.gameObject);
		}

		// Destroyer power-up pickup
		if (other.gameObject.CompareTag ("Kill")) {
			pul.enabled = !pul.enabled;
			PlaySoundOnce (2); //RE - Play Kill On sound
			other.gameObject.SetActive (false);
			canKill = true;
			yield return new WaitForSeconds (powerUpDuration);
			pul.enabled = false;
			PlaySoundOnce (3); //RE - Play Kill Off sound
			canKill = false;
			yield return new WaitForSeconds (powerUpSpawn);
			other.gameObject.SetActive (true);
			SoundManager.PlayObjectSoundOnce(other.gameObject);
		}

		// When you hit the enemy that spawns halfway through the game
		if (other.gameObject.CompareTag ("Enemy")) {
			Instantiate(explosion, transform.position, Quaternion.identity);
			mesh.enabled = false;
			count = count - 1;
			count2Text.text = "P2 Points: " + count.ToString ();
			Destroy (other.gameObject);
			rb.constraints = RigidbodyConstraints.FreezeAll;
			yield return new WaitForSeconds (deathTime);
			gameObject.transform.position = player2position;
			rb.constraints = RigidbodyConstraints.None;
			mesh.enabled = !mesh.enabled;
			yield return new WaitForSeconds (powerUpSpawn);
			Instantiate (Enemy2PreFab, enemyposition, Quaternion.identity);	
		}
	}

	IEnumerator OnCollisionEnter (Collision col) {
		
		Vector3 player1position = new Vector3 (12.0F, 0.5F, 0F);

		// When you have the powerup, you can kill the other player
		if (canKill) {
			if (col.gameObject.name == "Player1") {
				Player1Movement player1 = col.gameObject.GetComponent <Player1Movement> ();
				player1.count -= 1;
				player1.count1Text.text = "P1 Points: " + player1.count.ToString ();
				col.gameObject.SetActive (false);
				Instantiate (explosion, col.transform.position, Quaternion.identity);
				col.gameObject.transform.position = player1position;
				yield return new WaitForSeconds (deathTime);
				col.gameObject.SetActive (true);
			}
		}
	}

	//Raz Ezra - function to play sound
	void PlaySoundOnce(int soundID){
		playSound = GetComponent<AudioSource>(); //get AudioSource Component
		playSound.PlayOneShot (sounds [soundID]);//play correct sound via the AudioSource
	}
}