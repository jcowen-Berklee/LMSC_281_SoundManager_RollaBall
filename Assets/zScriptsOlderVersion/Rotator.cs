/*Alberto Menezes - Berklee College of Music
 * Logic and Programming - LMSC-281
 * Jeanine Cowen
 * Fall 2015 - October 10th
 * Updated for Midterm Project - October 31st*/

using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	// This is a basic rotation function for the yellow pickup cubes
	void Update () 
	{
		transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
	}
}
