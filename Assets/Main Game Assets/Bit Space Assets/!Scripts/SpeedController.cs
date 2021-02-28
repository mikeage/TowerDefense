using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpeedController : MonoBehaviour {

	private Rigidbody _rb;

	public float desiredSpeed = 5f;

	// Use this for initialization
	void Start () {
		this._rb = this.GetComponent<Rigidbody> ();	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		_rb.velocity = _rb.velocity.normalized * desiredSpeed;
	}
}
