using UnityEngine;
using System.Collections;

/**
 * Stuns the current enemy game object for a duration.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_DebuffStun : MonoBehaviour {

	[Header("Config")]
	public float stunDuration;

	void Start () {
		GameObject minion = SC_Helper.FindParentWithTag (gameObject, "Enemy");
		minion.GetComponent<SC_RandomMovement> ().PauseMovementWithDuration (stunDuration);
	}
}
