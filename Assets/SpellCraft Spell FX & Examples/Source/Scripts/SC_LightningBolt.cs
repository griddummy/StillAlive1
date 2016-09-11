using UnityEngine;
using System.Collections;

/**
 * Creates the lightning bolt drawing.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_LightningBolt {

	public LineRenderer[] lineRenderer { get; set; }
	public LineRenderer lightRenderer { get; set; }
	
	public float segmentLength { get; set; }
	public int index { get; set; }
	public bool isActive { get; private set; }
	
	private Color startColour;
	private Color endColour;
	
	private float alphaValue = 1f;
	private float fadeRate = 0.02f;
	
	private GameObject parentObject;
	
	private GameObject[] lineRendererPrefabs { get; set; }
	private GameObject lightRendererPrefab { get; set; }
	
	public SC_LightningBolt(float segmentLength, int index, Color startColour, Color endColour) {
		this.segmentLength = segmentLength;
		this.index = index;
		this.startColour = new Color(startColour.r, startColour.g, startColour.b, alphaValue);
		this.endColour = new Color(endColour.r, endColour.g, endColour.b, alphaValue);
	}
	
	public void Init (int lineRendererCount, GameObject lineRendererPrefab, GameObject lightRendererPrefab) {
		lineRenderer = new LineRenderer[lineRendererCount];
		lineRendererPrefabs = new GameObject[lineRendererCount];
		
		for (int i = 0; i < lineRendererCount; i++) {
			lineRendererPrefabs[i] = GameObject.Instantiate(lineRendererPrefab) as GameObject;
			lineRendererPrefabs[i].transform.parent = this.parentObject.transform;
			
			lineRenderer[i] = lineRendererPrefabs[i].GetComponent<LineRenderer>();
			lineRenderer[i].enabled = false;
		}
		
		this.lightRendererPrefab = GameObject.Instantiate (lightRendererPrefab) as GameObject;
		this.lightRendererPrefab.transform.parent = this.parentObject.transform;
		
		lightRenderer = this.lightRendererPrefab.GetComponent<LineRenderer>();
		this.isActive = false;
	}
	
	public void Activate () {
		for (int i = 0; i < lineRenderer.Length; i++) {
			lineRenderer[i].enabled = true;
		}
		
		lightRenderer.enabled = true;
		this.isActive = true;
	}
	
	public void DrawLightning(Vector3 source, Vector3 target) {
		float distance = Vector3.Distance (source, target);
		int segments = 5;
		
		if (distance > segmentLength) {
			segments = Mathf.FloorToInt (distance / segmentLength) + 2;
		} else {
			segments = 4;
		}
		
		for (int i = 0; i < lineRenderer.Length; i++) {
			// Set the amount of points to the calculated value
			lineRenderer[i].SetVertexCount(segments);
			lineRenderer[i].SetPosition(0, source);
			Vector3 lastPosition = source;
			
			for (int j = 1; j < segments - 1; j++) {
				// Go linear from source to target
				Vector3 tmp = Vector3.Lerp (source, target, (float) j / (float) segments);
				
				// Add randomness
				lastPosition = new Vector3(tmp.x + Random.Range(-0.1f, 0.1f), tmp.y + Random.Range(-0.1f, 0.1f), tmp.z + Random.Range(-0.1f, 0.1f));
				
				// Set the calculated position
				lineRenderer[i].SetPosition(j, lastPosition);
			}
			
			lineRenderer[i].SetPosition(segments - 1, target);
		}
		
		// Set the points for the light
		lightRenderer.SetVertexCount (2);
		lightRenderer.SetPosition (0, source);
		lightRenderer.SetPosition (1, target);
		
		// Set the color of the light
		Color lightColor = new Color (startColour.r, startColour.g, startColour.b, Random.Range(0.2f, 1f));
		lightRenderer.SetColors (lightColor, lightColor);
	}
	
	public void FadeLightning() {
		
		// Fade the alpha value at the set rate
		this.alphaValue -= fadeRate;
		
		// Create a new color of the line renderer with the new alpha value
		Color newColor = new Color (startColour.r, startColour.g, startColour.b, alphaValue);
		
		// Set the color for the lightning and light line renderers
		for (int i = 0; i < lineRenderer.Length; i++) {
			lineRenderer[i].SetColors(newColor, newColor);
			lightRenderer.SetColors(newColor, newColor);
		}
	}
	
	public void SetParent (GameObject parent) {
		this.parentObject = parent;
	}
	
	public void SetStartColor (Color color) {
		startColour = color;
	}
	
	public void SetEndColor (Color color) {
		endColour = color;
	}
}
