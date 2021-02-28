using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookLock : MonoBehaviour {
	public Camera cam;

	void LateUpdate () {
		this.transform.rotation = Quaternion.LookRotation (Vector3.ProjectOnPlane (cam.transform.forward, Vector3.up), Vector3.up);
	}
}
