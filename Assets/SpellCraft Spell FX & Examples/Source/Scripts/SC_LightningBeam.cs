using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Creates the lightning beam drawing.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_LightningBeam : MonoBehaviour {

	public GameObject origin { get; set; }
	public GameObject target { get; set; }
	public Color startColour;
	public Color endColour;
	
	private float segmentLength = 0.4f;
	private int lightningStrength;
	private GameObject lineRendererPrefab;
	private GameObject lightRendererPrefab;
	private GameObject glowParticlesPrefab;
	
	private SC_LightningBolt lightningBolt;
	private float timeBetweenBounces;
	private float bounceRadius;
	private SphereCollider bounceCollider;
	private List<GameObject> bounceTargets { get; set; }
	
	private GameObject parent;
	
	public void Init (int lightningStrength, GameObject lineRendererPrefab, GameObject lightRendererPrefab, GameObject glowParticlesPrefab, Color startColour, Color endColour) {
		this.lightningStrength = lightningStrength;
		this.lineRendererPrefab = lineRendererPrefab;
		this.lightRendererPrefab = lightRendererPrefab;
		this.glowParticlesPrefab = glowParticlesPrefab;
		this.startColour = startColour;
		this.endColour = endColour;
	}
	
	void Awake () {
		bounceTargets = new List<GameObject>();
	}
	
	void Start () {
		lightningBolt = new SC_LightningBolt (segmentLength, 0, startColour, endColour);
		lightningBolt.SetParent (gameObject);
		lightningBolt.Init (lightningStrength, lineRendererPrefab, lightRendererPrefab);
		lightningBolt.Activate ();
		
		// Add collider for bounce detection
		bounceCollider = gameObject.AddComponent<SphereCollider> ();
		bounceCollider.transform.position = target.transform.position;
		bounceCollider.radius = bounceRadius;
		bounceCollider.isTrigger = true;
		
		// Add a rigidbody
		Rigidbody rigidBody = gameObject.AddComponent<Rigidbody> ();
		rigidBody.isKinematic = true;
		rigidBody.useGravity = false;
	}
	
	void Update () {
		StartCoroutine (DrawLightning());
	}
	
	public void SetOrigin (GameObject origin) {
		this.origin = origin;
	}
	
	public void SetTarget (GameObject target) {
		this.target = target;
	}
	
	public void SetBounceRadius (float bounceRadius) {
		this.bounceRadius = bounceRadius;
	}
	
	public void SetTimeBetweenBounces (float timeBetweenBounces) {
		this.timeBetweenBounces = timeBetweenBounces;
	}
	
	public void SetParent (GameObject parent) {
		this.parent = parent;
	}
	
	private IEnumerator DrawLightning () {
		glowParticlesPrefab = GameObject.Instantiate (glowParticlesPrefab);
		glowParticlesPrefab.transform.position = target.transform.position;
		glowParticlesPrefab.transform.parent = transform;
		Destroy (glowParticlesPrefab, 2.0f);
		lightningBolt.DrawLightning (origin.transform.position, target.transform.position);
		lightningBolt.FadeLightning ();
		
		yield return new WaitForSeconds(timeBetweenBounces);
	}
	
	public void DoJump () {
		target.AddComponent<SC_TargetHit> ();
		parent.GetComponent<SC_LightningChain> ().AddBounceTarget (target);
		
		bounceTargets.Sort (delegate(GameObject bounceTarget1, GameObject bounceTarget2) {
			return Vector3.Distance(target.transform.position, bounceTarget1.transform.position).CompareTo(Vector3.Distance(target.transform.position, bounceTarget2.transform.position));
		});
		
		for (int i = 0; i < bounceTargets.Count; i++) {
			if (Vector3.Distance(target.transform.position, bounceTargets[i].transform.position) == 0.0f) {
				bounceTargets.RemoveAt(i);
			}
		}
		
		// We only want to jump to the closest target - not all that are in range
		if (bounceTargets.Count > 0) {
			parent.GetComponent<SC_LightningChain> ().CreateBounce (target, bounceTargets [0]);
		} else {
			// There are no more bounces so reset the targets hit state
			parent.GetComponent<SC_LightningChain>().EndLightningChain();
		}
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Enemy") {
			GameObject bounceTarget = other.gameObject;
			SC_TargetHit targetHit = bounceTarget.GetComponent<SC_TargetHit>();
			if (!targetHit) {
				bounceTargets.Add(bounceTarget);
			}
		}
	}
}
