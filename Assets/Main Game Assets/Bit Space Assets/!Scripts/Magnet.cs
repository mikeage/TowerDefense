using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class Magnet : MonoBehaviour {
	public enum MagnetType {
		Attract,
		Repel
	};
	public MagnetType magnetType = MagnetType.Attract;

	public float effectRange = 5f;

	public float effectStrength = 10f;

	public LayerMask layersToHit;
	private Rigidbody myRB;
	public float minDistance = 0.01f;

	void Awake() {
		myRB = this.GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		Collider[] hits = Physics.OverlapSphere (this.transform.position, effectRange, layersToHit, QueryTriggerInteraction.Ignore);
		if (hits.Length > 0) {
			List<Rigidbody> RBs = new List<Rigidbody> ();
			foreach (Collider c in hits) {
				if (c.attachedRigidbody != null && !RBs.Contains(c.attachedRigidbody)) {
					Magnet m = c.attachedRigidbody.GetComponent<Magnet> ();
					if (m != null) {
						RBs.Add (c.attachedRigidbody);
						Vector3 closestPoint = c.ClosestPointOnBounds (myRB.worldCenterOfMass);
						Vector3 forceVector = (myRB.worldCenterOfMass - closestPoint).normalized * effectStrength / Mathf.Max((myRB.worldCenterOfMass - closestPoint).magnitude, minDistance) * (this.magnetType == MagnetType.Repel ? -1 : 1);

						c.attachedRigidbody.AddForceAtPosition (forceVector, closestPoint, ForceMode.Force);

					}
				} else {

				}
			}
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = new Color (1f, 0f, 0f, 0.5f);
		Gizmos.DrawWireSphere (transform.position, effectRange);
	}

	void OnValidate() {
		effectRange = Mathf.Max (0.1f, effectRange);
		minDistance = Mathf.Max (0.001f, minDistance);
	}
}
