using UnityEngine;
using System.Collections;

/**
 * Spawn projectile spell prefab where it has an origin (player) and target.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_SpawnSpellProjectileTarget : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject spellPrefab;

	[Header("Config")]
	public string callbackName;
	
	private GameObject player;
	
	void Awake () {
		player = GameObject.FindWithTag ("Player");
	}
	
	public void SpawnSpell (GameObject target) {
		Vector3 spellPosition = new Vector3 (player.transform.position.x, spellPrefab.transform.position.y, player.transform.position.z);
		
		// Set the position and parent of the spell
		GameObject spell = GameObject.Instantiate (spellPrefab);
		spell.transform.position = spellPosition;

		Vector3 lookPosition = new Vector3 (target.transform.position.x, 0.5f, target.transform.position.z);
		spell.transform.LookAt (lookPosition);

		spell.SendMessage (callbackName, target);
	}
}
