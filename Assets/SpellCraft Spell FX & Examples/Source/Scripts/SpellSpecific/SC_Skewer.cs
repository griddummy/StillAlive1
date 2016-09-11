using UnityEngine;
using System.Collections;

/**
 * UNIQUE: Fires a line of spikes and stuns enemies that come in contact.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_Skewer : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject skewerPrefab;
	
	[Header("Config")]
	public int numberOfSpikes = 10;

	private float spikeSeparation = 0.5f;
	private float maxSpikeScaleSize = 6.0f;
	private float timeBetweenSpikes = 0.05f;
	private float spikeArea = 0.05f;
	private GameObject player;
	private Collider groundCollider;
	
	void Awake () {
		player = GameObject.FindWithTag ("Player");
		groundCollider = GameObject.FindWithTag ("Ground").GetComponent<MeshCollider>();
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit = new RaycastHit();
			if (groundCollider.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 9999.0f)) {
				Vector3 targetPosition = new Vector3 (hit.point.x, skewerPrefab.transform.position.y, hit.point.z);
				StartCoroutine (CreateSpikes (targetPosition));
			}
		}
	}
	
	IEnumerator CreateSpikes (Vector3 targetPosition) {
		Vector3 startPosition = new Vector3 (player.transform.position.x, skewerPrefab.transform.position.y, player.transform.position.z);
		
		Quaternion rotate = skewerPrefab.transform.rotation;
		float spikeScaleIncrement = maxSpikeScaleSize / numberOfSpikes;
		
		for (int i = 1; i <= numberOfSpikes; i++) {
			GameObject skewerSpike = (GameObject) GameObject.Instantiate (skewerPrefab, startPosition, rotate);
			skewerSpike.transform.parent = gameObject.transform;
			skewerSpike.transform.LookAt(targetPosition);
			
			Vector3 adjustedForwardPosition = new Vector3 (skewerSpike.transform.forward.x + Random.Range(-spikeArea, spikeArea), skewerSpike.transform.forward.y, skewerSpike.transform.forward.z + Random.Range(-spikeArea, spikeArea));
			Vector3 spikePosition = startPosition + (adjustedForwardPosition * i * spikeSeparation);
			
			skewerSpike.transform.position = spikePosition;
			skewerSpike.transform.localScale = Vector3.Lerp (skewerSpike.transform.localScale, new Vector3(spikeScaleIncrement * i, spikeScaleIncrement * i, spikeScaleIncrement * i), spikeScaleIncrement * Time.deltaTime);
			
			yield return new WaitForSeconds(timeBetweenSpikes);
		}
	}
}
