using UnityEngine;
using System.Collections;

/**
 * Attach the debuff prefab to the minions that are effected.
 * Removes the debuff when out of range.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_DebuffEffect : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject debuffPrefab;
	
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Enemy") {
			GameObject minion = other.gameObject;
			GameObject debuff = GameObject.Instantiate(debuffPrefab);
			
			minion.GetComponent<SC_RandomMovement>().AddToMinionEffects(debuff);
		}
	}
	
	void OnTriggerExit (Collider other) {
		if (other.tag == "Enemy") {
			GameObject minion = other.gameObject;
			
			minion.GetComponent<SC_RandomMovement>().ResetMinionAndEffects();
		}
	}
}
