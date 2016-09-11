using UnityEngine;
using System.Collections;

/**
 * Sets the current ally game object's move speed for a duration.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_BuffMoveSpeedDuration : MonoBehaviour {

	[Header("Config")]
	public float buffMoveSpeed;
	public float buffMoveSpeedDuration;
	
	void Start () {
		GameObject minion = SC_Helper.FindParentWithTag (gameObject, "Ally");
		minion.GetComponent<SC_RandomMovement> ().ChangeMoveSpeedWithDuration (buffMoveSpeed, buffMoveSpeedDuration);
	}
}
