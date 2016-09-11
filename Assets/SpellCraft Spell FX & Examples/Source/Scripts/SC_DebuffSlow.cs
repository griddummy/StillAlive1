using UnityEngine;
using System.Collections;

/**
 * Slows the current enemy game object's speed for a duration.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_DebuffSlow : MonoBehaviour {

	[Header("Config")]
	public float slowMoveSpeed;
	public float slowMoveDuration;

	void Start () {
		GameObject minion = SC_Helper.FindParentWithTag (gameObject, "Enemy");
		minion.GetComponent<SC_RandomMovement> ().ChangeMoveSpeedWithDuration (slowMoveSpeed, slowMoveDuration);
	}
}
