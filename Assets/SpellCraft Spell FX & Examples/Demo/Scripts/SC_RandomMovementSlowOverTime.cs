using UnityEngine;
using System.Collections;

/**
 * Slows the game object with SC_RandomMovement over time.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_RandomMovementSlowOverTime : MonoBehaviour {

	[Header("Config")]
	public float slowedMoveSpeed = 0.5f;
	public float effectDuration = 1.0f;
	
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Enemy") {
			SlowOverTime (other);
		}
	}

	private void SlowOverTime (Collider other) {
		other.GetComponent<SC_RandomMovement>().ChangeMoveSpeedWithDuration(slowedMoveSpeed, effectDuration);
	}
}
