using UnityEngine;
using System.Collections;

/**
 * All Spells Demo Mode Script.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_AllSpellsModeDemo : MonoBehaviour {

	[Header("Prefabs")]
	public Renderer groundRenderer;
	public GameObject cameraContainerPrefab;
	public GameObject uiObject;

	protected Vector3 defaultCamPosition;
	protected Quaternion defaultCamRotation;
	protected bool slowMotion;
	protected bool uiActive = true;

	// OVERRIDE --------------------------------------------------

	void Awake () {
		defaultCamPosition = Camera.main.transform.position;
		defaultCamRotation = Camera.main.transform.rotation;
	}

	void Update () {
		// Mouse scroll / Zoom In & Out
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if (scroll != 0.0f) {
			Camera.main.transform.Translate(Vector3.forward * (scroll < 0.0f ? -1.0f : 1.0f), Space.Self);
		}

		// Right click / Reset camera
		if (Input.GetMouseButtonDown(1)) {
			Camera.main.transform.position = defaultCamPosition;
			Camera.main.transform.rotation = defaultCamRotation;
		}

		// Toggle UI
		if (Input.GetKeyDown (KeyCode.H)) {
			uiActive = !uiActive;
			uiObject.SetActive(uiActive);
		}
	}

	// MESSAGES --------------------------------------------------

	protected virtual void OnToggleGround () {
		groundRenderer.enabled = !groundRenderer.enabled;
	}

	protected virtual void OnToggleRotation () {
		cameraContainerPrefab.GetComponent<SC_Rotation> ().isRotating = !cameraContainerPrefab.GetComponent<SC_Rotation> ().isRotating;
	}

	protected virtual void OnToggleSlow () {
		slowMotion = !slowMotion;

		if (slowMotion) {
			Time.timeScale = 0.3f;
		} else {
			Time.timeScale = 1.0f;
		}
	}
}
