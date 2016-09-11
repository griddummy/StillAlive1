using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Demo script for the Examples Mode.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_ExamplesModeDemo : MonoBehaviour {

	[Header("Prefabs")]
	public GUIText spellLabel;
	public GUIText spellIndexLabel;
	public Renderer groundRenderer;
	public Collider groundCollider;
	public GameObject cameraContainerPrefab;
	public GameObject uiObject;

	protected List<GameObject> spellExamples;
	protected int exampleIndex;
	protected GameObject currentSpellObject;
	protected Vector3 defaultCamPosition;
	protected Quaternion defaultCamRotation;
	protected bool slowMotion;
	protected bool uiActive = true;

	[Header("Example Mode Specific")]
	public GameObject heroObject;
	public GameObject allies;
	public GameObject enemies;
	public NavMeshAgent navAgent;
	public GUIText tooltipLabel;

	private Vector3 movePosition;
	private float originalMinionMoveSpeed;

	// OVERRIDE --------------------------------------------------

	void Awake () {
		spellExamples = new List<GameObject> ();
		int children = transform.childCount;

		for (int i = 0; i < children; i++) {
			GameObject child = transform.GetChild(i).gameObject;
			spellExamples.Add(child);
		}

		spellExamples.Sort (delegate(GameObject spell1, GameObject spell2) {
			return spell1.name.CompareTo(spell2.name);
		});

		defaultCamPosition = Camera.main.transform.position;
		defaultCamRotation = Camera.main.transform.rotation;

		UpdateUI ();

		// Settings for minions
		foreach (Transform minion in allies.transform) {
			originalMinionMoveSpeed = minion.GetComponent<SC_RandomMovement>().moveSpeed;
		}
	}

	void Start () {
		navAgent = heroObject.GetComponent<NavMeshAgent> ();

		// Spawn the first spell
		SpawnSpell ();
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			PreviousSpell();
			SpawnSpell ();
		} else if (Input.GetKeyDown(KeyCode.RightArrow)) {
			NextSpell();
			SpawnSpell ();
		}

		// Mouse scroll / Zoom in & out
		float scroll = Input.GetAxis ("Mouse ScrollWheel");
		if (scroll != 0.0f) {
			Camera.main.transform.Translate(Vector3.forward * (scroll < 0.0f ? -1.0f : 1.0f), Space.Self);
		}

		// Right click / Move hero
		if (Input.GetMouseButtonDown(1)) {
			RaycastHit hit = new RaycastHit();
			if (groundCollider.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 9999.0f)) {
				navAgent.SetDestination(hit.point);
				navAgent.Resume();
			}
		}

		// Toggle UI
		if (Input.GetKeyDown (KeyCode.H)) {
			uiActive = !uiActive;
			uiObject.SetActive(uiActive);
		}
	}

	// MESSAGES --------------------------------------------------

	void OnPreviousSpell () {
		PreviousSpell ();
		SpawnSpell ();
	}

	void OnNextSpell () {
		NextSpell ();
		SpawnSpell ();
	}

	void OnToggleGround () {
		groundRenderer.enabled = !groundRenderer.enabled;
	}

	void OnToggleRotation () {
		cameraContainerPrefab.GetComponent<SC_Rotation> ().isRotating = !cameraContainerPrefab.GetComponent<SC_Rotation> ().isRotating;
	}

	void OnToggleSlow () {
		slowMotion = !slowMotion;

		if (slowMotion) {
			Time.timeScale = 0.3f;
		} else {
			Time.timeScale = 1.0f;
		}
	}

	void OnEffectsMode () {
		Time.timeScale = 1.0f;
		Application.LoadLevel ("SpellCraft Effects Demo");
	}

	void OnExamplesMode () {
		Time.timeScale = 1.0f;
		Application.LoadLevel ("SpellCraft Examples Demo");
	}

	// UI --------------------------------------------------

	void UpdateUI () {
		spellLabel.text = spellExamples [exampleIndex].name;
		spellIndexLabel.text = string.Format ("{0}/{1}", (exampleIndex+1).ToString("00"), spellExamples.Count.ToString("00"));
	}

	// OTHER --------------------------------------------------

	void SpawnSpell () {
		Cursor.visible = true;
		Destroy(currentSpellObject);
		ResetMinionSpeed ();
		ResetMinionTargetable ();
		DestroyMinionEffects ();
		tooltipLabel.text = "";

		currentSpellObject = GameObject.Instantiate (spellExamples[exampleIndex]);
		currentSpellObject.SetActive (true);
	}

	void ResetMinionSpeed () {
		// Allies
		foreach (Transform minion in allies.transform) {
			minion.GetComponent<SC_RandomMovement>().moveSpeed = originalMinionMoveSpeed;
		}

		// Enemies
		foreach (Transform minion in enemies.transform) {
			minion.GetComponent<SC_RandomMovement>().moveSpeed = originalMinionMoveSpeed;
		}
	}

	void DestroyMinionEffects () {
		foreach (Transform minion in allies.transform) {
			foreach (Transform child in minion.transform) {
				if (child.name == "Minion Effects") {
					foreach (Transform effectChild in child.transform) {
						Destroy (effectChild.gameObject);
					}
				}
			}
		}

		foreach (Transform minion in enemies.transform) {
			foreach (Transform child in minion.transform) {
				if (child.name == "Minion Effects") {
					foreach (Transform effectChild in child.transform) {
						Destroy(effectChild.gameObject);
					}
				}
			}
		}
	}

	void ResetMinionTargetable () {
		foreach (Transform minion in allies.transform) {
			minion.GetComponent<SC_Target>().isTargetable = false;
		}

		foreach (Transform minion in enemies.transform) {
			minion.GetComponent<SC_Target>().isTargetable = false;
		}
	}

	void PreviousSpell () {
		exampleIndex--;

		if (exampleIndex < 0)
			exampleIndex = spellExamples.Count - 1;

		UpdateUI ();
	}

	void NextSpell () {
		exampleIndex++;

		if (exampleIndex >= spellExamples.Count)
			exampleIndex = 0;

		UpdateUI ();
	}
}
