using UnityEngine;
using System.Collections;

/**
 * Mouse Click on ground sends callback.
 * Has a rune prefab to display area.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_OnClickGroundRune : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject runePrefab;
	public GameObject callbackReceiver;
	
	[Header("Config")]
	public string callbackName;
	public float runeRadius = 1.0f;
	
	private MeshCollider groundCollider;
	private GameObject rune;
	
	void Awake () {
		groundCollider = GameObject.FindWithTag ("Ground").GetComponent<MeshCollider> ();

		rune = GameObject.Instantiate (runePrefab);
		rune.transform.localScale = new Vector3 (runeRadius, runeRadius, runeRadius);
		rune.transform.parent = transform;
		DisableRune ();
	}
	
	void Update () {
		RaycastHit hit = new RaycastHit ();
		if (groundCollider.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 9999.0f)) {
			EnableRune();

			Vector3 runePosition = new Vector3 (hit.point.x, runePrefab.transform.position.y, hit.point.z);
			rune.transform.position = runePosition;
			
			if (Input.GetMouseButtonDown(0)) {
				SendMessage (runePosition);
			}
		} else {
			DisableRune();
		}
	}
	
	private void SendMessage (Vector3 runePosition) {
		callbackReceiver.SendMessage (callbackName, runePosition);
	}

	private void EnableRune () {
		rune.SetActive (true);
		Cursor.visible = false;
	}

	private void DisableRune () {
		rune.SetActive (false);
		Cursor.visible = true;
	}
}
