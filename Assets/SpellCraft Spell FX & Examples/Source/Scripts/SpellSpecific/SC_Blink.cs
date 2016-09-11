using UnityEngine;
using System.Collections;

/**
 * UNIQUE: Blink teleports the player from
 * it's current location to the target location.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_Blink : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject blinkInPrefab;
	public GameObject blinkOutPrefab;

	private GameObject player;
	private MeshCollider groundCollider;

	private float blinkEffectDuration = 2.0f;

	// Scaling variables
	private Vector3 originalScale;
	private bool isScaling;
	private float targetScale = 0.05f;
	private float scaleDuration = 0.2f;


	void Awake () {
		player = GameObject.FindWithTag ("Player");
		groundCollider = GameObject.FindWithTag ("Ground").GetComponent<MeshCollider> ();
	}

	void Start () {
		originalScale = player.transform.localScale;
		isScaling = false;
	}

	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit = new RaycastHit();
			if (groundCollider.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 9999.0f)) {
				
				// Start blink spell
				StartCoroutine(BlinkToTarget(hit.point));
			}
		}
	}

	private IEnumerator BlinkToTarget (Vector3 blinkPosition) {
		// Stop player movement when blinking
		player.GetComponent<NavMeshAgent>().Stop();
		
		// Blink out animation
		GameObject blinkOut = GameObject.Instantiate(blinkOutPrefab);
		blinkOut.transform.position = player.transform.position;
		Destroy(blinkOut, blinkEffectDuration);
		
		// Start shrinking player
		Vector3 playerOriginalPosition = player.transform.position;
		isScaling = true;
		float startTime = Time.time;
		while (Time.time - startTime < scaleDuration) {
			float amount = (Time.time - startTime) / scaleDuration;
			player.transform.localScale = Vector3.Lerp(originalScale, Vector3.one * targetScale, amount);
			player.transform.position = new Vector3(player.transform.position.x, playerOriginalPosition.y, player.transform.position.z);
			yield return null;
		}
		
		// Wait to fit the spell animation
		player.transform.localScale = Vector3.one * targetScale;
		yield return new WaitForSeconds(0.05f);
		
		// Change player location
		player.transform.position = blinkPosition;
		
		// Blink in animation
		GameObject blinkInObject = GameObject.Instantiate(blinkInPrefab);
		blinkInObject.transform.position = player.transform.position;
		blinkInObject.transform.parent = player.transform;
		Destroy(blinkInObject, blinkEffectDuration);
		
		// Start growing player
		startTime = Time.time;
		while (Time.time - startTime < scaleDuration) {
			float amount = (Time.time - startTime) / scaleDuration;
			player.transform.localScale = Vector3.Lerp(Vector3.one * targetScale, originalScale, amount);
			yield return null;
		}
		player.transform.localScale = originalScale;
		
		// Finished
		isScaling = false;
	}
}
