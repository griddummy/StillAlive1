using UnityEngine;
using System.Collections;

/**
 * Attach the buff prefab to the minions that are effected.
 * Apply the buff effect over time at a set rate.
 * Removes the buff when out of range.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_BuffEffectOverTime : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject buffPrefab;
	
	[Header("Config")]
	public float rateInSeconds = 1.0f;
	
	private float rateCount = 0.0f;
	
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Ally") {
			SpawnBuff (other);
			rateCount = 0.0f;
		}
	}
	
	void OnTriggerStay (Collider other) {
		if (other.tag == "Ally") {
			
			rateCount += Time.deltaTime;
			if (rateCount >= rateInSeconds) {
				SpawnBuff (other);
				rateCount = 0.0f;
			}
		}
	}
	
	void SpawnBuff (Collider other) {
		GameObject minion = other.gameObject;
		GameObject buff = GameObject.Instantiate(buffPrefab);
		
		minion.GetComponent<SC_RandomMovement>().AddToMinionEffects(buff);
	}
}
