using UnityEngine;
using System.Collections;

/**
 * Mouse Click on targets send callback.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_OnClickTarget : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject callbackReceiver;
	
	[Header("Config")]
	public string callbackName;
	public bool targetSelf = true;
	public bool targetAllies = true;
	public bool targetEnemies = true;

	void Start () {
		InitTargetables ();
	}

	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit = new RaycastHit ();
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 9999.0f)) {
				GameObject target = hit.transform.gameObject;
				CheckTarget (target);
			}
		}
	}

	private void CheckTarget (GameObject target) {
		// Player
		if (target.tag == "Player" && targetSelf) {
			SendMessage (target);
		}

		// Ally
		if (target.tag == "Ally" && targetAllies) {
			SendMessage (target);
		}

		// Enemy
		if (target.tag == "Enemy" && targetEnemies) {
			SendMessage (target);
		}
	}

	private void SendMessage (GameObject target) {
		callbackReceiver.SendMessage (callbackName, target);
	}

	private void InitTargetables () {
		// Allies
		if (targetAllies) {
			GameObject[] allies = GameObject.FindGameObjectsWithTag ("Ally");
			for (int i = 0; i < allies.Length; i++) {
				allies [i].GetComponent<SC_Target> ().isTargetable = true;
			}
		} else {
			GameObject[] allies = GameObject.FindGameObjectsWithTag ("Ally");
			for (int i = 0; i < allies.Length; i++) {
				allies [i].GetComponent<SC_Target> ().isTargetable = false;
			}
		}

		// Enemies
		if (targetEnemies) {
			GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
			for (int i = 0; i < enemies.Length; i++) {
				enemies [i].GetComponent<SC_Target> ().isTargetable = true;
			}
		} else {
			GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
			for (int i = 0; i < enemies.Length; i++) {
				enemies [i].GetComponent<SC_Target> ().isTargetable = false;
			}
		}
	}
}
