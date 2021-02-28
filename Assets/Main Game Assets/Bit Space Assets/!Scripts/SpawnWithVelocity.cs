using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWithVelocity : MonoBehaviour {
	public GameObject prefabToSpawn;
	public Transform spawnPoint;
	public Transform containerTransform;

	public bool inheritVelocity = true;
	public Rigidbody velocitySource;

	public Transform target;
	public bool pointTowardsTarget = false;
	public float velocityTowardsObject = 0f;

	public float velocityInRandomDirection = 0f;
	public float randomAngularVelocity = 0f;

	[Tooltip("Max speed overlapping objects should fly apart at")]
	public float maxDepenetrationVelocity = 3f;

	public Vector3 worldVelocity = Vector3.zero;
	public Vector3 localVelocity = Vector3.zero;
	public Vector3 worldAngularVelocity = Vector3.zero;
	public Vector3 localAngularVelocity = Vector3.zero;

	void Awake() {
		if (spawnPoint == null) {
			spawnPoint = this.transform;
		}
	}

	public void Spawn() {
		GameObject newGO = GameObject.Instantiate<GameObject> (prefabToSpawn, spawnPoint.position, spawnPoint.rotation, containerTransform);

		if (pointTowardsTarget && target != null) {
			newGO.transform.LookAt (target);
		}


		foreach (Rigidbody rb in newGO.GetComponentsInChildren<Rigidbody>()) {
			if (target != null) {
				rb.AddForce ((target.position - rb.position).normalized * velocityTowardsObject, ForceMode.VelocityChange);
			}

			if (velocityInRandomDirection > 0f) {
				rb.AddForce (Random.onUnitSphere * velocityInRandomDirection, ForceMode.VelocityChange);
			}

			if (randomAngularVelocity > 0f) {
				rb.AddRelativeTorque (Random.onUnitSphere * randomAngularVelocity, ForceMode.VelocityChange);
			}

			rb.AddForce (worldVelocity, ForceMode.VelocityChange);
			rb.AddRelativeForce (localVelocity, ForceMode.VelocityChange);
			rb.AddTorque (worldAngularVelocity, ForceMode.VelocityChange);
			rb.AddRelativeTorque (localAngularVelocity, ForceMode.VelocityChange);
		}
	}
}
