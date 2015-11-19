//Made by Alexandre Jannuzzi & Alberto Menezes for Berklee Game Jam 2015

using UnityEngine;
using System.Collections;

public class EnemyScript2nd : MonoBehaviour {

	public int moveSpeed;
	public int rotationSpeed;

	private Transform target;
	private int yo;
	private Vector3 player2position;

	PlayerMovement newInstance;

	void Start() {
		// This gets the position of the Player
		target = GameObject.Find ("Player2").transform;
		player2position = new Vector3 (-12.0F, 0.5F, 0F);
	}

	void Update() 
	{  
		// this "yo" is multiplied by the speed of the Move Towards Target line of code. So when the player is destroyed and moved to the spawning position, the enemy doesn't move
		if (target.position == player2position) {
			yo = 0;
		}
		else {
			yo = 1;
		}

		if (target != null) 
		{
			// The enemy's position will change accordingly to the target's position
			Vector3 dir = target.position - transform.position;
			// Only needed if objects don't share 'z' value.
			dir.z = 0.0f;
			if (dir != Vector3.zero) 
				transform.rotation = Quaternion.Slerp ( transform.rotation, Quaternion.FromToRotation (Vector3.right, dir), rotationSpeed * Time.deltaTime);

			// Move Towards Target
			transform.position += (target.position - transform.position).normalized 
				* (moveSpeed * yo) * Time.deltaTime;
		}
	}
}