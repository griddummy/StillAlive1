using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * UNIQUE: LightningChain spell bounces between nearby targets.
 * This is for the effects demo.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SCEffects_LightningChain : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject lineRendererPrefab;
	public GameObject lightRendererPrefab;
	
	[Header("Config")]
	public int numberOfBounces = 7;
	public float timeBetweenBounces = 0.2f;
	public int lightningStrength = 4;
	public Color startColour;
	public Color endColour;
	
	private IList<SC_LightningBolt> lightningBolts { get; set; }
	private IList<Vector3> targets { get; set; }
	private float segmentLength = 0.4f;
	
	void Awake () {
		lightningBolts = new List<SC_LightningBolt> ();
		targets = new List<Vector3>();

		SC_LightningBolt lightningBolt;
		for (int i = 0; i < numberOfBounces; i++) {
			lightningBolt = new SC_LightningBolt(segmentLength, i, startColour, endColour);
			lightningBolt.SetParent(gameObject);
			lightningBolt.Init(lightningStrength, lineRendererPrefab, lightRendererPrefab);
			
			lightningBolts.Add(lightningBolt);
		}

		BuildRandomTargets ();
	}
	
	void Update () {
		StartCoroutine (DrawLightning ());
	}

	private IEnumerator DrawLightning () {
		for (int i = 0; i < targets.Count; i++) {
			if (i == 0) {
				lightningBolts[i].DrawLightning (Vector3.zero, targets[i]);
			} else {
				lightningBolts[i].DrawLightning (targets[i - 1], targets[i]);
			}

			// Fade the lightning
			lightningBolts[i].FadeLightning();

			yield return new WaitForSeconds (timeBetweenBounces);
		}
	}
	
	private void BuildRandomTargets () {
		for (int i = 0; i < numberOfBounces; i++) {
			targets.Add(new Vector3 (Random.Range(-5.0f, 5.0f), Random.Range(1.0f, 3.0f), Random.Range(-5.0f, 5.0f)));
			lightningBolts[i].Activate();
		}
	}
}
