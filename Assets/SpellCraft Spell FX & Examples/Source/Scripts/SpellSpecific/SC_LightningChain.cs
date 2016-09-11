using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * UNIQUE: LightningChain spell bounces between nearby targets.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_LightningChain : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject lineRendererPrefab;
	public GameObject lightRendererPrefab;
	
	[Header("Config")]
	public float timeBetweenBounces = 0.2f;
	public float bounceRadius = 2f;
	public int lightningStrength = 4;
	public Color startColour;
	public Color endColour;
	
	private IList<GameObject> bounceTargets { get; set; }
	private GameObject player;
	
	void Awake () {
		player = GameObject.FindWithTag ("Player");
		bounceTargets = new List<GameObject>();
	}

	private void SpawnSpell (GameObject target) {
		CreateBounce (player, target);
	}
	
	public void CreateBounce (GameObject origin, GameObject target) {
		GameObject bounce = new GameObject("SCSpell_LightningBounce");
		SC_LightningBounce lightningBounce = bounce.AddComponent<SC_LightningBounce>();
		lightningBounce.SetOrigin(origin);
		lightningBounce.SetTarget(target);
		lightningBounce.SetBounceRadius(bounceRadius);
		lightningBounce.SetTimeBetweenBounces(timeBetweenBounces);
		lightningBounce.SetParent (gameObject);
		lightningBounce.Init(lightningStrength, lineRendererPrefab, lightRendererPrefab, startColour, endColour);
		Destroy(bounce, 2.0f);
		
		StartCoroutine (CheckForNextBounce (lightningBounce));
	}
	
	private IEnumerator CheckForNextBounce (SC_LightningBounce lightningBounce) {
		yield return new WaitForSeconds (timeBetweenBounces);
		
		lightningBounce.DoJump ();
	}
	
	public void AddBounceTarget (GameObject bounceTarget) {
		bounceTargets.Add (bounceTarget);
	}
	
	public void EndLightningChain () {
		for (int i = 0; i < bounceTargets.Count; i++) {
			Destroy(bounceTargets[i].GetComponent<SC_TargetHit>());
		}
	}
}
