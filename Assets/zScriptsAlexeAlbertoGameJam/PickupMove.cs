using UnityEngine;
using System.Collections;

public class PickupMove : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter (Collider other) {

		Vector3 position = new Vector3 (Random.Range (-9.0F, 9.0F), 0.5F, Random.Range (-9.0F, 9.0F));

		// This just moves the pick-up cube in case it spawns inside of one of the moving walls
		if (other.gameObject.CompareTag ("Wall")) {
			gameObject.transform.position = position;
		}
	}
}