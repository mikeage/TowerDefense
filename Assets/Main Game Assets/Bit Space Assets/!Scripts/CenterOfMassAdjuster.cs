using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfMassAdjuster : MonoBehaviour {
	public Vector3 centerOfMassOffset = Vector3.zero;
	// Use this for initialization
	void Start () {
		this.GetComponent<Rigidbody> ().centerOfMass = this.GetComponent<Rigidbody> ().centerOfMass + centerOfMassOffset;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.green;
		Gizmos.DrawSphere (this.transform.TransformPoint(this.GetComponent<Rigidbody> ().centerOfMass + centerOfMassOffset), 0.1f);
	}
}
