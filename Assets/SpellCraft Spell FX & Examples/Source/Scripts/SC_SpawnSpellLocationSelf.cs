using UnityEngine;
using System.Collections;

/**
 * Spawn spell prefab on location self.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_SpawnSpellLocationSelf : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject spellPrefab;

	[Header("Config")]
	public bool followPlayer = true;

	private GameObject player;

	void Awake () {
		player = GameObject.FindWithTag ("Player");
	}
	
	public void SpawnSpell () {
		Vector3 spellPosition = new Vector3 (player.transform.position.x, spellPrefab.transform.position.y, player.transform.position.z);

		if (followPlayer) {
			// Set the position and parent of the spell example
			transform.position = spellPosition;
			transform.parent = player.transform;

			// Set the position and parent of the spell
			GameObject spell = GameObject.Instantiate (spellPrefab);
			spell.transform.position = transform.position;
			spell.transform.parent = transform;
		} else {
			GameObject spell = GameObject.Instantiate (spellPrefab);
			spell.transform.position = spellPosition;
		}
	}
}
