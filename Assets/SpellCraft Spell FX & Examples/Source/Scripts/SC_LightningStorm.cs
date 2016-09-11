using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * 
 */
public class SC_LightningStorm : MonoBehaviour {

	public Vector3 origin { get; set; }
	public Vector3 target { get; set; }

	private Color startColour;
	private Color endColour;
	private GameObject lineRendererPrefab;
	private GameObject lightRendererPrefab;
	private float segmentLength = 0.4f;
	private int lightningStrength;

	private SC_LightningBolt lightningBolt;
	private GameObject parent;

	public void Init (int lightningStrength, GameObject lineRendererPrefab, GameObject lightRendererPrefab, Color startColour, Color endColour) {
		this.lightningStrength = lightningStrength;
		this.lineRendererPrefab = lineRendererPrefab;
		this.lightRendererPrefab = lightRendererPrefab;
		this.startColour = startColour;
		this.endColour = endColour;
	}

	public void SetOrigin (Vector3 origin) {
		this.origin = origin;
	}

	public void SetTarget (Vector3 target) {
		this.target = target;
	}

	public void SetParent (GameObject parent) {
		this.parent = parent;
	}

	void Start () {
		lightningBolt = new SC_LightningBolt (segmentLength, 0, startColour, endColour);
		lightningBolt.SetParent (gameObject);
		lightningBolt.Init (lightningStrength, lineRendererPrefab, lightRendererPrefab);
		lightningBolt.Activate ();
	}

	void Update () {
		StartCoroutine (DrawLightning());
	}

	private IEnumerator DrawLightning () {
		lightningBolt.DrawLightning (origin, target);
		lightningBolt.FadeLightning ();

		yield return new WaitForSeconds (0.0f);
	}

}
