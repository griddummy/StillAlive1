using UnityEngine;
using System.Collections;

/**
 * Random movement script.
 * Moves within a set radius;
 * Doesn't work on the player as it uses a NavMeshAgent.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_RandomMovement : MonoBehaviour {

	[Header("Config")]
	public bool isMoving = true;
	public float moveSpeed = 1.0f;

	private float originalMoveSpeed;
	private float time = 0.0f;
	private float maxDistanceFromScene = 9.0f;

	void Awake () {
		// Set the original move speed
		originalMoveSpeed = moveSpeed;
	}

	void Start () {
		// Start with a random rotation
		Vector3 randomRotation = GetRandomRotation ();
		transform.Rotate(randomRotation);
	}

	void FixedUpdate () {
		if (isMoving) {
			time += Time.deltaTime;

			// Move forwards
			transform.position += transform.forward * Time.deltaTime * moveSpeed;

			// Check the distance from the center of the scene
			float distance = Vector3.Distance (Vector3.zero, transform.position);
			// If they are too far from the max distance radius
			if (distance >= maxDistanceFromScene) {
				// Rotate them to face the center of the scene to bring them back within range
				transform.LookAt (new Vector3 (0.0f, 0.5f, 0.0f)); // y is 0.5f to keep the unit at the correct height
				time = 0.0f;
			// Continue as is
			} else {
				// Rotate the unit randomly after a random amount of time
				float randomTimeLimit = Random.Range (0.5f, 5.0f);
				if (time > randomTimeLimit) {
					Vector3 randomRotation = GetRandomRotation();
					transform.Rotate (randomRotation);
					time = 0.0f;
				}
			}
		}
	}

	private Vector3 GetRandomRotation () {
		return new Vector3 (0.0f, Random.Range(0, 360), 0.0f);
	}

	private IEnumerator DoChangeMoveSpeedWithDuration (float moveSpeed, float duration) {
		this.moveSpeed = moveSpeed;
		yield return new WaitForSeconds (duration);
		this.moveSpeed = originalMoveSpeed;
	}

	private IEnumerator DoPauseMovementWithDuration (float duration) {
		isMoving = false;
		yield return new WaitForSeconds (duration);
		isMoving = true;
	}

	public void ChangeMoveSpeedWithDuration (float moveSpeed, float duration) {
		StartCoroutine (DoChangeMoveSpeedWithDuration(moveSpeed, duration));
	}

	public void PauseMovementWithDuration (float duration) {
		StartCoroutine (DoPauseMovementWithDuration(duration));
	}

	public void ChangeMoveSpeed (float moveSpeed) {
		this.moveSpeed = moveSpeed;
	}

	public void AddToMinionEffects (GameObject effect) {
		foreach (Transform child in transform) {
			if (child.name == "Minion Effects") {
				Vector3 effectLocation = new Vector3 (child.transform.position.x, effect.transform.position.y, child.transform.position.z);
				
				effect.transform.position = effectLocation;
				effect.transform.parent = child.transform;
			}
		}
	}

	public void ResetMinionAndEffects () {
		// Reset movement speed
		moveSpeed = originalMoveSpeed;

		// Make sure the minion is moving again
		isMoving = true;

		// Clear effects
		foreach (Transform child in transform) {
			if (child.name == "Minion Effects") {
				foreach (Transform effectTransform in child.transform) {
					Destroy (effectTransform.gameObject);
				}
			}
		}
	}
}
