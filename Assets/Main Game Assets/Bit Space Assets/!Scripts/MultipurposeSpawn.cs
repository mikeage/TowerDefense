using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipurposeSpawn : MonoBehaviour {
	[Header("List of Prefabs to spawn")]
	public List<GameObject> prefabList;
	[Tooltip("If false, objects are chosen in order.")]
	public bool chooseRandomPrefab = false;
	public int nextSpawnIndex = 0;
	public float timeBetweenSpawns = 1f;
	private float lastSpawnTime = float.MinValue;
	[Tooltip("Values of Infinity or <= 0 will be taken as forever")]
	public float spawnLifetime = float.PositiveInfinity;

	[Header("List of locations to spawn at")]
	public List<Transform> spawnLocations;
	public bool chooseRandomLocation = false;
	public int nextLocationIndex = 0;
	public bool useSpawnRotations = true;
//	public bool useSpawnScales = false;

	[Header("Auto Spawn Settings")]
	public bool autoSpawn = true;
	private bool _autoSpawning = false;
	private Coroutine _autoSpawnRoutine = null;
	public bool autoStart = true;
	public float autoStartDelay = 0f;
	public float timeBetweenAutoSpawn = 5f;
	private float lastAutoSpawnTime = float.MinValue;

	[Header("General Spawn Settings")]
	public KeyCode spawnKey = KeyCode.None;
	[Tooltip("What Transform the spawned objects should belong to")]
	public Transform spawnParent;
	[Tooltip("If a NavMeshFollower is attached to a prefab, set its target to this Transform")]
	public Transform navMeshFollowerTarget;
	// Use this for initialization
	void Start () {
		if (autoStart) {
			_autoSpawning = true;
			_autoSpawnRoutine = StartCoroutine (AutoSpawn (autoStartDelay));
		}
	}

	void Update() {
		if (Input.GetKeyDown (spawnKey)) {
			this.Spawn ();
		}
	}

	private IEnumerator AutoSpawn(float startDelay) {
		// let things settle for a frame before spawning
		yield return new WaitForFixedUpdate ();


		yield return new WaitForSeconds (startDelay);

		while (_autoSpawning) {
			if (Time.time >= lastAutoSpawnTime + timeBetweenAutoSpawn && Time.time >= lastSpawnTime + timeBetweenSpawns) {
				if (this.Spawn ()) {
					lastAutoSpawnTime = Time.time;
				}
			}
			yield return new WaitForSeconds (timeBetweenAutoSpawn);
		}
			
		_autoSpawnRoutine = null;
	}

	public void StartAutoSpawn() {
		if (_autoSpawnRoutine == null) {
			_autoSpawning = true;
			_autoSpawnRoutine = StartCoroutine (AutoSpawn (0f));
		}
	}

	public void StopAutoSpawn() {
		_autoSpawning = false;
		StopCoroutine (_autoSpawnRoutine);
		_autoSpawnRoutine = null;
	}

	public bool Spawn() {
		if (Time.time >= lastSpawnTime + timeBetweenSpawns) {
			int nextSpawn;
			if (chooseRandomPrefab) {
				nextSpawn = Random.Range (0, prefabList.Count);
			} else {
				nextSpawn = nextSpawnIndex;
				nextSpawnIndex = (nextSpawnIndex + 1) % prefabList.Count;
			}

			int nextLoc;
			if (chooseRandomLocation) {
				nextLoc = Random.Range (0, spawnLocations.Count);
			} else {
				nextLoc = nextLocationIndex;
				nextLocationIndex = (nextLocationIndex + 1) % spawnLocations.Count;
			}


			return this._Spawn (prefabList [nextSpawn], spawnLocations [nextLoc], spawnParent);
		}

		return false;
	}

	private bool _Spawn(GameObject obj, Transform location, Transform parent = null) {
		GameObject newGO = GameObject.Instantiate (obj, location.position, (useSpawnRotations ? location.rotation : obj.transform.rotation), parent);

		if (spawnLifetime > 0f && !float.IsInfinity (spawnLifetime)) {
			Destroy (newGO, spawnLifetime);
		}

		if (newGO.GetComponentInChildren<NavMeshFollower> () != null) {
			newGO.GetComponentInChildren<NavMeshFollower> ().thingToFollow = navMeshFollowerTarget;
		}

		lastSpawnTime = Time.time;

		return true;
	}

	void OnValidate() {
		nextSpawnIndex = Mathf.Clamp (nextSpawnIndex, 0, prefabList.Count-1);
		nextLocationIndex = Mathf.Clamp (nextLocationIndex, 0, spawnLocations.Count - 1);

		timeBetweenSpawns = Mathf.Max (Time.fixedDeltaTime, timeBetweenSpawns);
		timeBetweenAutoSpawn = Mathf.Max (Time.fixedDeltaTime, timeBetweenAutoSpawn);
	}
}
