using UnityEngine;
using System.Collections;

/**
 * Spawn debuff effect prefab. Triggers only on enter.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_DebuffEffectTriggerEnter : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject debuffPrefab;

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Enemy") {
			GameObject minion = other.gameObject;

			GameObject debuff = GameObject.Instantiate(debuffPrefab);
			
			minion.GetComponent<SC_RandomMovement>().AddToMinionEffects(debuff);
		}
	}
}
