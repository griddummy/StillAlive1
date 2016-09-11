using UnityEngine;
using System.Collections;

/**
 * Spawn debuff effect prefab over time at set rate.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_DebuffEffectOverTime : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject debuffPrefab;

	[Header("Config")]
	public float rateInSeconds = 1.0f;
	
	private float rateCount = 0.0f;

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Enemy") {
			SpawnDebuff (other);
			rateCount = 0.0f;
		}
	}
	
	void OnTriggerStay (Collider other) {
		if (other.tag == "Enemy") {

			rateCount += Time.deltaTime;
			if (rateCount >= rateInSeconds) {
				SpawnDebuff (other);
				rateCount = 0.0f;
			}
		}
	}

	void SpawnDebuff (Collider other) {
		GameObject minion = other.gameObject;
		GameObject debuff = GameObject.Instantiate(debuffPrefab);
		
		minion.GetComponent<SC_RandomMovement>().AddToMinionEffects(debuff);
	}
}
