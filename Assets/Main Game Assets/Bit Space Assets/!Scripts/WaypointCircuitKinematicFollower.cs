using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WaypointCircuitKinematicFollower : MonoBehaviour {
	public Transform followPoint;
	public float speed;
	public float turnSpeed;
	private Rigidbody _rb;

	void Awake() {
		_rb = this.GetComponent<Rigidbody> ();
	}


	// Update is called once per frame
	void FixedUpdate () {
		_rb.MoveRotation (Quaternion.RotateTowards (_rb.rotation, Quaternion.LookRotation (followPoint.position - _rb.position, Vector3.up), turnSpeed * Time.fixedDeltaTime));
		_rb.MovePosition (transform.position + transform.forward * speed * Time.fixedDeltaTime);
	}

	public void SetSpeed(float newSpeed) {
		speed = newSpeed;
	}

	public void SetTurnSpeed(float newTurnSpeed) {
		turnSpeed = newTurnSpeed;
	}
}
