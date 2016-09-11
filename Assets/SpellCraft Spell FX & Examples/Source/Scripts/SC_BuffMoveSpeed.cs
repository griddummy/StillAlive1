using UnityEngine;
using System.Collections;

/**
 * Speeds the current ally game object's move speed.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_BuffMoveSpeed : MonoBehaviour {

	[Header("Config")]
	public float buffMoveSpeed;
	
	void Start () {
		GameObject minion = SC_Helper.FindParentWithTag (gameObject, "Ally");
		minion.GetComponent<SC_RandomMovement> ().ChangeMoveSpeed (buffMoveSpeed);
	}
}
