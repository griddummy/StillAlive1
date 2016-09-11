using UnityEngine;
using System.Collections;

/**
 * Attach the buff prefab to the minions that are effected.
 * Removes the buff when out of range.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_BuffEffect: MonoBehaviour {
	
	[Header("Prefabs")]
	public GameObject buffPrefab;

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Ally") {
			GameObject minion = other.gameObject;
			GameObject buff = GameObject.Instantiate(buffPrefab);

			minion.GetComponent<SC_RandomMovement>().AddToMinionEffects(buff);
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "Ally") {
			GameObject minion = other.gameObject;

			minion.GetComponent<SC_RandomMovement>().ResetMinionAndEffects();
		}
	}
}
