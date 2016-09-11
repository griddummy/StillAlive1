using UnityEngine;
using System.Collections;

/**
 * Locks onto target and moves towards it with a set speed.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_ProjectileHoming : MonoBehaviour {

	[Header("Config")]
	public bool isMoving = false;
	public float moveSpeed = 5.0f;
	public float spellDurationAfterCollision = 1.0f;
	[HideInInspector]
	public Transform target;

	void Update () {
		if (isMoving) {
			Vector3 targetDirection = target.position - transform.position;
			float step = moveSpeed * Time.deltaTime;
			Vector3 newDirection = Vector3.RotateTowards (transform.forward, targetDirection, step, 0.0f);
			transform.rotation = Quaternion.LookRotation (newDirection);
			transform.position = Vector3.MoveTowards (transform.position, target.position, step);
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Enemy") {
			// If it's the correct target
			if (other.name == target.name) {
				// Destroy spell
				Destroy(gameObject, spellDurationAfterCollision);
			}
		}
	}

	public void FireProjectile (GameObject target) {
		this.target = target.transform;
		isMoving = true;
	}
}
