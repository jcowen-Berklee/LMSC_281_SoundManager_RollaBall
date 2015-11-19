/*Alberto Menezes - Berklee College of Music
 * Logic and Programming - LMSC-281
 * Jeanine Cowen
 * Fall 2015 - October 10th
 * Updated for Midterm Project - October 31st*/

using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

		// The transform is public but it actually wouldn't need to be, since we access the component from inside of the script
		public Transform target;
		// These are public so that you can change the speed of the enemy from Unity
		public int moveSpeed;
		public int rotationSpeed;

		PlayerMovement newInstance;
		
		void Start() {
			// This gets the position of the Player
			target = GameObject.Find("Player").transform;
		}
		
		void Update() 
		{    
			if (target != null) 
			{
				// The enemy's position will change accordingly to the target's position
				Vector3 dir = target.position - transform.position;
				// Only needed if objects don't share 'z' value.
				dir.z = 0.0f;
				if (dir != Vector3.zero) 
					transform.rotation = Quaternion.Slerp ( transform.rotation, 
					                                       Quaternion.FromToRotation (Vector3.right, dir), 
					                                       rotationSpeed * Time.deltaTime);
				
				// Move Towards Target
				transform.position += (target.position - transform.position).normalized 
					* moveSpeed * Time.deltaTime;

			}
		}
	}