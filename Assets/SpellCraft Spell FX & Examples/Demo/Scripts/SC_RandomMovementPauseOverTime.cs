using UnityEngine;
using System.Collections;

/**
 * Pause movement of the game object with SC_RandomMovement over time.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_RandomMovementPauseOverTime : MonoBehaviour {

	[Header("Config")]
	public float effectDuration = 1.0f;
	
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Enemy") {
			PauseOverTime (other);
		}
	}
	
	private void PauseOverTime (Collider other) {
		other.GetComponent<SC_RandomMovement>().PauseMovementWithDuration(effectDuration);
	}
}
