using UnityEngine;
using System.Collections;

/**
 * Spawns a collision effect.
 * Collisions occur on enemy, ground or both.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_CollisionEffect : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject collisionPrefab;

	[Header("Config")]
	public bool enemyCollision = true;
	public bool groundCollision = true;
	public float collisionDuration = 1.0f;

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Enemy" && enemyCollision) {
			SpawnCollisionEffect ();
		}

		if (other.tag == "Ground" && groundCollision) {
			SpawnCollisionEffect ();
		}
	}

	private void SpawnCollisionEffect () {
		if (collisionPrefab != null) {
			Quaternion rotate = collisionPrefab.transform.rotation;
			GameObject spell = (GameObject) GameObject.Instantiate(collisionPrefab, transform.position, rotate);
			
			if (collisionDuration > 0.0f) {
				GameObject.Destroy(spell.gameObject, collisionDuration);
			}
		}
	}
}
