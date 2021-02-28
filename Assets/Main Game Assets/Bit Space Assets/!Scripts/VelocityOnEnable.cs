using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VelocityOnEnable : MonoBehaviour {

	public Transform target;
	public bool pointTowardsTarget = false;
	public float velocityTowardsObject = 0f;

	public float velocityInRandomDirection = 0f;
	public float randomAngularVelocity = 0f;

	public Vector3 worldVelocity = Vector3.zero;
	public Vector3 localVelocity = Vector3.zero;
	public Vector3 worldAngularVelocity = Vector3.zero;
	public Vector3 localAngularVelocity = Vector3.zero;

	void OnEnable() {
		if (pointTowardsTarget && target != null) {
			this.transform.LookAt (target);
		}


		foreach (Rigidbody rb in this.GetComponentsInChildren<Rigidbody>()) {
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
