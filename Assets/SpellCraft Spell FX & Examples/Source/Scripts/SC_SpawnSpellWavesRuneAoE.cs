using UnityEngine;
using System.Collections;

/**
 * Spawns an Area Of Effect (AOE) spell over a set number of waves.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_SpawnSpellWavesRuneAoE : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject spellShardPrefab;
	
	[Header("Config")]
	public float delayBetweenShards = 0.0f;
	public float delayBetweenWaves = 1.0f;
	public int numberOfWaves = 1;
	public int numberOfShards = 1;
	public float aoeRadius = 1.0f;
	public float startHeight = 1.0f;
	public bool randomiseShardPosition = false;
	public bool randomiseShardHeight = false;
	public bool randomiseDelayBetweenShards = false;
	public float waveDuration = 1.0f;

	[Header("Position & Rotation Modifiers")]
	public Vector3 positionModifier;
	public Vector3 rotationModifier;

	private float minRandomShardHeight = 0.0f;
	private float maxRandomShardHeight = 3.0f;
	
	public void SpawnSpell (Vector3 runePosition) {
		Vector3 spellPosition = new Vector3 (runePosition.x, startHeight, runePosition.z);

		StartCoroutine (StartWaves (spellPosition));
	}

	private IEnumerator StartWaves (Vector3 spellPosition) {
		// Number of waves
		for (int i = 0; i < numberOfWaves; i++) {
			GameObject spellWave = new GameObject ("SC_SpellWave");
			spellWave.transform.position = spellPosition;

			// Number of shards
			for (int j = 0; j < numberOfShards; j++) {

				Vector3 shardPosition = new Vector3 (spellPosition.x, spellPosition.y, spellPosition.z);
				shardPosition += positionModifier;

				if (randomiseShardPosition) {
					shardPosition += new Vector3 (Random.Range(-aoeRadius, aoeRadius), 0.0f, Random.Range(-aoeRadius, aoeRadius));
				}

				if (randomiseShardHeight) {
					shardPosition += new Vector3 (0.0f, Random.Range(minRandomShardHeight, maxRandomShardHeight), 0.0f);
				}

				GameObject spellShard = GameObject.Instantiate (spellShardPrefab);
				spellShard.transform.position = shardPosition;
				spellShard.transform.parent = spellWave.transform;

				if (randomiseDelayBetweenShards) {
					float randomDelayBetweenShards = Random.Range(0.0f, delayBetweenShards);
					yield return new WaitForSeconds (randomDelayBetweenShards);
				}
				yield return new WaitForSeconds (delayBetweenShards);
			}

			spellWave.transform.Rotate(rotationModifier);
			Destroy (spellWave, waveDuration);

			yield return new WaitForSeconds (delayBetweenWaves);
		}
	}
}
