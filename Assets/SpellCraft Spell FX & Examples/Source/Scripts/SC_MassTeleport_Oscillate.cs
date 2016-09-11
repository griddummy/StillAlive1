using UnityEngine;
using System.Collections;

/**
 * Moves the current object up and down on the y-axis
 * at a set oscillation.
 * Used for MassTeleportation.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_MassTeleport_Oscillate : MonoBehaviour {
	
	public float maxSpeed;
	public float distanceScale;
	public bool startGoingUp = true;
	private Vector3 startPosition;

	void Start () {
		startPosition = transform.position;
	}

	void Update () {
		MoveVertical ();
	}

	void MoveVertical () {
		transform.position = new Vector3 (transform.position.x, (startPosition.y + Mathf.Sin (Time.time * maxSpeed))/distanceScale, transform.position.z);
	}
}
