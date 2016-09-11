using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * UNIQUE: MassTeleportation teleports all allies within the player radius
 * to a target ally location.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_MassTeleportation : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject spellPrefab;
	public GameObject targetSpellPrefab;
	public GameObject teleporterSpellPrefab;
	
	[Header("Config")]
	public float teleportRadius = 3.0f;
	
	private GameObject player;
	private GameObject target;
	private bool isTeleporting = false;
	private float teleportWaitDuration = 4.0f;
	private Rigidbody rigidBody;
	
	private IList<GameObject> teleporters;
	
	void Start () {
		player = GameObject.FindWithTag ("Player");
		teleporters = new List<GameObject>();

		transform.position = player.transform.position;
		transform.parent = player.transform;
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit = new RaycastHit ();
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 9999.0f)) {
				if (hit.transform.tag == "Ally" && !isTeleporting) {
					GameObject target = hit.transform.gameObject;
					StartCoroutine (MassTeleportation (target));
				}
			}
		}
	}
	
	private IEnumerator MassTeleportation (GameObject target) {
		isTeleporting = true;
		teleporters = new List<GameObject>();

		// Set the target
		this.target = target;

		// Add player to the teleporters list
		teleporters.Add (player);
		
		// Stop player movement
		player.GetComponent<NavMeshAgent> ().Stop();
		
		/* 1st Teleport Sequence
		 **********************/
		GameObject spell = GameObject.Instantiate (spellPrefab);
		Vector3 spellPosition = new Vector3 (player.transform.position.x, spellPrefab.transform.position.y, player.transform.position.z);

		spell.transform.position = spellPosition;
		spell.transform.parent = player.transform;

		// Collider checks for allys in range to be teleported
		SphereCollider teleportCollider = gameObject.AddComponent <SphereCollider>();
		teleportCollider.isTrigger = true;
		teleportCollider.radius = teleportRadius;
		
		/* 2nd Teleport Sequence
		 **********************/
		
		// Stop target movement
		target.GetComponent<SC_RandomMovement> ().isMoving = false;
		
		GameObject targetSpell = GameObject.Instantiate (targetSpellPrefab);
		Vector3 targetSpellPosition = new Vector3 (target.transform.position.x, targetSpellPrefab.transform.position.y, target.transform.position.z);
		targetSpell.transform.position = targetSpellPosition;
		targetSpell.transform.parent = SC_Helper.FindChildWithTag(target, "Minion Effects").transform;
		
		yield return new WaitForSeconds (teleportWaitDuration);
		
		/* 3rd Teleport Sequence
		 **********************/
		
		for (int i = 0; i < teleporters.Count; i++) {
			GameObject teleporter = teleporters[i];
			
			Vector3 teleporterFromLocation = new Vector3(teleporter.transform.position.x, teleporterSpellPrefab.transform.position.y, teleporter.transform.position.z);
			GameObject teleporterFrom = GameObject.Instantiate(teleporterSpellPrefab);
			teleporterFrom.transform.position = teleporterFromLocation;
		}

		// Mini-wait before position change
		yield return new WaitForSeconds (0.5f);
		
		/* 4th Teleport Sequence
		 **********************/
		for (int i = 0; i < teleporters.Count; i++) {
			GameObject teleporter = teleporters[i];
			
			// Random teleport position within target range
			Vector3 teleporterInPosition = new Vector3(targetSpellPosition.x + Random.Range(-1.5f, 1.5f), teleporter.transform.position.y, targetSpellPosition.z + Random.Range(-1.0f, 1.0f));
			teleporter.transform.position = teleporterInPosition;
			
			GameObject teleporterIn = GameObject.Instantiate(teleporterSpellPrefab);
			teleporterIn.transform.position = new Vector3(teleporterInPosition.x, teleporterSpellPrefab.transform.position.y, teleporterInPosition.z);
			teleporterIn.transform.parent = SC_Helper.FindChildWithTag(teleporter, "Minion Effects").transform;
		}

		// Finished teleporting
		player.GetComponent<NavMeshAgent> ().Stop();
		isTeleporting = false;
		teleporters = new List<GameObject>();
		Destroy(gameObject.GetComponent <SphereCollider>());
		
		// Resume target movement
		target.GetComponent<SC_RandomMovement> ().isMoving = true;
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Ally" && isTeleporting) {
			if (other.gameObject.name != target.name) {
				// Add allies to the teleport list
				teleporters.Add(other.gameObject);
			}
		}
	}
	
	void OnTriggerExit (Collider other) {
		if (other.tag == "Ally") {
			// Remove the ally from the teleport list
			if (teleporters.Count > 0) {
				for (int i = 0; i < teleporters.Count; i++) {
					if (other.gameObject.name == teleporters[i].name) {
						teleporters.RemoveAt(i);
					}
				}
			}
		}
	}

	private void DoNothing (GameObject target) {
		// Mass Teleport handles everything.
	}
}
