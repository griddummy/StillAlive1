using UnityEngine;
using System.Collections;

/**
 * Spawn spell prefab on target location.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_SpawnSpellLocationTarget : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject spellPrefab;

	public void SpawnSpell (GameObject target) {
		Vector3 spellPosition = new Vector3 (target.transform.position.x, spellPrefab.transform.position.y, target.transform.position.z);

		GameObject minionEffects = SC_Helper.FindChildWithTag (target, "Minion Effects");
		
		// Set the position and parent of the spell
		GameObject spell = GameObject.Instantiate (spellPrefab);
		spell.transform.position = spellPosition;
		spell.transform.parent = minionEffects.transform;
	}
}
