using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {
	public GameObject prefab;
	public float lockoutTime = 1f;
	public float nextSpawnTime = 0f;
	public Transform spawnLocation;
	public Transform spawnParent;
	public Vector3 initialVelocity = Vector3.zero;
	public KeyCode spawnKey = KeyCode.S;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(spawnKey)) {
			this.Spawn();
		}
	}

	public void Spawn() {
		if (Time.time > nextSpawnTime) {
			GameObject newGO = GameObject.Instantiate(prefab, spawnLocation.position, spawnLocation.rotation) as GameObject;
			newGO.transform.SetParent (spawnParent);
			Rigidbody rb = newGO.GetComponent<Rigidbody>();
			if (rb != null) {
				rb.AddForce(initialVelocity, ForceMode.VelocityChange);
			}
			nextSpawnTime = Time.time + lockoutTime;
		}
	}
}
