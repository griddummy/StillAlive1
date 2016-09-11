using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * UNIQUE: StarFall shards fall on enemy minions within
 * the player's spell radius.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_StarFall : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject spellPrefab;
	public GameObject shardPrefab;

	[Header("Config")]
	public int numberOfWaves = 10;
	public int damagePerStar = 30;
	public float effectRadius = 5.0f;

	private GameObject player;
	private bool isCasting = false;
	private IList<GameObject> targets;
	private SphereCollider damageAreaCollider;
	private float delayBetweenWaves = 1.0f;

	void Start () {
		player = GameObject.FindWithTag ("Player");
		targets = new List<GameObject>();

		transform.position = player.transform.position;
		transform.parent = player.transform;
	}

	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			if (!isCasting) {
				StartCoroutine (StarFall ());
			}
		}
	}

	private IEnumerator StarFall () {
		targets = new List<GameObject>();
		isCasting = true;

		// Collider checks for allys in range to be teleported
		damageAreaCollider = gameObject.AddComponent <SphereCollider>();
		damageAreaCollider.isTrigger = true;
		damageAreaCollider.radius = effectRadius;

		while (isCasting) {
			GameObject spell = GameObject.Instantiate(spellPrefab);
			Destroy(spell, 15.0f);
			spell.transform.position = new Vector3(transform.position.x, spell.transform.position.y, transform.position.z);
			spell.transform.parent = transform;

			for (int i = 0; i < numberOfWaves; i++) {
				for (int j = 0; j < targets.Count; j++) {
					GameObject target = targets[j];

					GameObject starShard = GameObject.Instantiate(shardPrefab);
					starShard.transform.position = new Vector3(target.transform.position.x, shardPrefab.transform.position.y, target.transform.position.z);
					starShard.transform.parent = target.transform;

					float randomShardSpawnWait = Random.Range(0.03f, 0.07f);
					yield return new WaitForSeconds(randomShardSpawnWait);
				}

				yield return new WaitForSeconds(delayBetweenWaves);
			}

			isCasting = false;
		}

		Destroy(gameObject.GetComponent <SphereCollider>());
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Enemy") {
			targets.Add(other.gameObject);
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "Enemy") {
			if (targets.Count > 0) {
				for (int i = 0; i < targets.Count; i++) {
					if (other.gameObject.name == targets[i].name) {
						targets.RemoveAt(i);
					}
				}
			}
		}
	}
}
