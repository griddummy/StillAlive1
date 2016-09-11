using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * 
 * */
public class SCEffects_LightningStorm : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject lineRendererPrefab;
	public GameObject lightRendererPrefab;
	public GameObject lightningImpactPrefab;
	public GameObject stormCloudPrefab;

	public Color startColour;
	public Color endColour;
	public int numberOfLightningStrikes = 10;
	public int lightningStrength = 4;
	public float timeBetweenLightning = 0.0f;

	[Header("Config")]
	public float startHeight = 7.0f;
	public float aoeRadius = 1.0f;

	private IList<SC_LightningBolt> lightningBolts { get; set; }
	private IList<Vector3> targets { get; set; }
	private float minRandomLightningHeight = 0.0f;
	private float maxRandomLightningHeight = 3.0f;

	void Awake () {
		// Initialise the targets list
		targets = new List<Vector3> ();
	}

	private void SpawnSpell (Vector3 target) {
		Vector3 spellPosition = new Vector3 (target.x, startHeight, target.z);

		StartCoroutine (StartSpell(spellPosition));
	}

	private IEnumerator StartSpell (Vector3 spellPosition) {

		// Number of lightning strikes
		for (int j = 0; j < numberOfLightningStrikes; j++) {
			Vector3 lightningPosition = spellPosition;

			lightningPosition += new Vector3 (Random.Range(-aoeRadius, aoeRadius), Random.Range(minRandomLightningHeight, maxRandomLightningHeight), Random.Range(-aoeRadius, aoeRadius));

			CreateStormCloud (lightningPosition);

			// Create each lightning bolt
			CreateBolt(lightningPosition);

			float randomDelayBetweenLightning = Random.Range (0.0f, timeBetweenLightning);
			yield return new WaitForSeconds (randomDelayBetweenLightning);
		}
	}

	private void CreateStormCloud (Vector3 stormPosition) {
		GameObject stormCloud = Instantiate(stormCloudPrefab) as GameObject;
		stormCloud.transform.position = stormPosition;
		Destroy (stormCloud, 5.0f);
	}

	private void CreateBolt (Vector3 lightningPosition) {
		Vector3 hitPosition = new Vector3 (lightningPosition.x, 0.0f, lightningPosition.z);

		GameObject storm = new GameObject ("SC_LightningStorm");
		SC_LightningStorm lightningStorm = storm.AddComponent<SC_LightningStorm> ();
		lightningStorm.SetOrigin (lightningPosition);
		lightningStorm.SetTarget (hitPosition);
		lightningStorm.SetParent (gameObject);
		lightningStorm.Init (lightningStrength, lineRendererPrefab, lightRendererPrefab, startColour, endColour);
		Destroy (storm, 2.0f);

		GameObject lightningImpact = Instantiate (lightningImpactPrefab) as GameObject;
		lightningImpact.transform.position = hitPosition;
		lightningImpact.transform.parent = storm.transform;
	}
}
