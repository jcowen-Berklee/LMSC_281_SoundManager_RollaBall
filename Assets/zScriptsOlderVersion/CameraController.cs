/*Alberto Menezes - Berklee College of Music
 * Logic and Programming - LMSC-281
 * Jeanine Cowen
 * Fall 2015 - October 10th
 * Updated for Midterm Project - October 31st*/

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// You can attach the object in Unity because of this
	public GameObject player;

	// We don't need to access this variable outside of the script, so it's private
	private Vector3 offset;

	// Just attaching the camera to the player doesn't work since it is a sphere, so we need to use an offset
	void Start () {
		offset = transform.position - player.transform.position;
	}
	
	/* So this makes the camera follow only the global position of the player. It is in LateUpdate so that it's guaranteed
		that the object has moved so that the camera can follow it*/
	void LateUpdate () {
		transform.position = player.transform.position + offset;
	}
}
