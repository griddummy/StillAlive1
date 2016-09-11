using UnityEngine;
using System.Collections;

/**
 * Spawn spell prefab on player facing ground click location.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_SpawnSpellLocationSelfFacing : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject spellPrefab;

	[Header("Config")]
	public string callbackName;
	
	private GameObject player;
	
	void Awake () {
		player = GameObject.FindWithTag ("Player");
	}
	
	public void SpawnSpell (RaycastHit hit) {
		Vector3 spellPosition = new Vector3 (player.transform.position.x, spellPrefab.transform.position.y, player.transform.position.z);
		Vector3 lookPosition = new Vector3 (hit.point.x, spellPrefab.transform.position.y, hit.point.z);

		GameObject spell = GameObject.Instantiate (spellPrefab);
		spell.transform.position = spellPosition;
		spell.transform.LookAt (lookPosition);

		spell.SendMessage (callbackName, hit.transform);
	}
}
