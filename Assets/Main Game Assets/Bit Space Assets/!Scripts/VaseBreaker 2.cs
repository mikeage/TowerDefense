using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VaseBreaker : MonoBehaviour {
	public float breakThreshold = 3f;
	public string tagForSafeObjects = "Cat Paw";
	public float maxFlyApartSpeed = 5f;
	private bool broke = false;
	public UnityEvent breakEvents;

	void OnCollisionEnter(Collision hit) {
//		Debug.Log (hit.impulse.magnitude);
		if (!broke && hit.impulse.magnitude > breakThreshold && hit.collider.tag != tagForSafeObjects) {
		
			this.Demolish ();

			broke = true;
		}
	}

	public void Demolish() {
		// safety check in case someone decides to call this twice
		if (broke)
			return;
		
		foreach (Transform t in this.GetComponentsInChildren<Transform>()) {
			if (t == this.transform && t.GetComponent<Rigidbody>() != null) {
				Destroy (t.GetComponent<Rigidbody> ());
				continue;
			}

			if (t.GetComponent<Rigidbody> () == null) {
				t.gameObject.AddComponent<Rigidbody> ();
			}
				
			Rigidbody rb = t.GetComponent<Rigidbody> ();
			rb.maxDepenetrationVelocity = maxFlyApartSpeed;
			rb.isKinematic = false;
			rb.useGravity = true;
			t.parent = this.transform;
		}

		if (this.GetComponent<AudioSource> () != null) {
			this.GetComponent<AudioSource> ().Play ();
		}

		breakEvents.Invoke ();
	}


}
