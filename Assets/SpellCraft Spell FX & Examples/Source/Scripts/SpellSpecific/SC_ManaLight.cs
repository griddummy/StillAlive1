using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * UNIQUE: ManaLight spell deals damage and drains the same amount in mana.
 * Shared functionality with LightningChain.
 * Still needs a clean up - J.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_ManaLight : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject lineRendererPrefab;
	public GameObject lightRendererPrefab;
	public GameObject glowParticlePrefab;
	
	[Header("Config")]
	public int lightningStrength = 4;
	public Color startColour;
	public Color endColour;

	private GameObject player;
	private GameObject target;
	
	void Awake () {
		player = GameObject.FindWithTag ("Player");
	}

	private void SpawnSpell (GameObject target) {
		CreateBeam (player, target);
	}
	
	public void CreateBeam (GameObject origin, GameObject target) {
		GameObject beam = new GameObject("SCSpell_LightningBounce");
		SC_LightningBeam lightningBeam = beam.AddComponent<SC_LightningBeam>();
		lightningBeam.SetOrigin(origin);
		lightningBeam.SetTarget(target);
		lightningBeam.SetBounceRadius(0.0f);
		lightningBeam.SetTimeBetweenBounces(0.0f);
		lightningBeam.SetParent (gameObject);
		lightningBeam.Init(lightningStrength, lineRendererPrefab, lightRendererPrefab, glowParticlePrefab, startColour, endColour);
		Destroy(beam, 2.0f);
	}
	
	public void EndChainLightning () {
		Destroy(target.GetComponent<SC_TargetHit>());
	}
}
