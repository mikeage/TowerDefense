using UnityEngine;
using System.Collections;

public class SelfDemolition : MonoBehaviour {
	
	public float maxFlyApartSpeed = 5f;

	public void Demolish() {
		foreach (Transform t in this.GetComponentsInChildren<Transform>()) {
			if (t.GetComponent<Rigidbody> () == null) {
				t.gameObject.AddComponent<Rigidbody> ();
			}
			Rigidbody rb = t.GetComponent<Rigidbody> ();
			rb.maxDepenetrationVelocity = maxFlyApartSpeed;
			rb.isKinematic = false;
			rb.useGravity = true;
			t.parent = this.transform;
		}
	}
}
