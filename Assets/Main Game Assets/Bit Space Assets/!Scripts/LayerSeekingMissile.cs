using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LayerSeekingMissile : MonoBehaviour {
	public float checkRadius = 10f;
	public LayerMask layersToCheck;

	[Range(0f, 1f)]
	public float frontPreference = 1f;
	public Transform target;

	public Damager.DamageRole rolesToFind;
	private Rigidbody _myRB;


	private float distanceFactor = 1f;
	private float minDistanceValue = 0.2f;
	private float angleFactor = 1f;

	public float turnDegreesPerSecond = 45f;
	public float speed = 1f;

	void Awake() {
		_myRB = this.GetComponent<Rigidbody> ();
		_myRB.isKinematic = true;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (target != null) {
			_myRB.MoveRotation (Quaternion.RotateTowards (_myRB.rotation, Quaternion.LookRotation (target.position - _myRB.worldCenterOfMass, Vector3.up), turnDegreesPerSecond * Time.fixedDeltaTime));
			_myRB.MovePosition (transform.position + transform.forward * speed * Time.fixedDeltaTime);
		} else {
			Collider[] hits = Physics.OverlapSphere (this.transform.position, checkRadius, layersToCheck);

			float highestScore = float.NegativeInfinity;
			Transform bestTarget = null;
			//List<Damager> damagersFound = new List<Damager> ();
			foreach (Collider coll in hits) {
				Damager d = coll.attachedRigidbody.GetComponent<Damager> ();
				if (d != null && d.myRole == rolesToFind) {
					// find the vector from the collider's closest point to my RB's center of mass
					Vector3 directionVector = coll.ClosestPointOnBounds (_myRB.worldCenterOfMass) - _myRB.worldCenterOfMass;
					float score = Mathf.Clamp01 (1 / Mathf.Max (directionVector.magnitude, minDistanceValue)) * Vector3.Dot (this.transform.forward, directionVector.normalized);
					Debug.Log (coll.name + "'s score: " + score);
					if (score > highestScore) {
						highestScore = score;
						bestTarget = coll.transform;
					}
				}
			}

			this.target = bestTarget;
		}
	}
}
