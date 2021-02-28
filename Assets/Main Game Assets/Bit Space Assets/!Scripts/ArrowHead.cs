using UnityEngine;
using System.Collections;

public class ArrowHead : MonoBehaviour {
	private Rigidbody rb;
	public GameObject explosionPrefab;
	private Transform hitTarget;
	void Start() {
	}

	void OnCollisionEnter(Collision coll) {
		rb = this.GetComponent<Rigidbody> ();
		//Debug.Break ();
//		if (coll.collider.attachedRigidbody == null ||
//				coll.collider.attachedRigidbody != rb) {
		if(coll.gameObject.CompareTag("Target")) {
			hitTarget = coll.gameObject.transform;
			StartCoroutine (StickArrow ());
		}
	}

	IEnumerator StickArrow() {
		yield return new WaitForFixedUpdate ();

		//this.transform.parent = hitTarget;

		if (hitTarget.name == "2nd ring") {
			hitTarget.GetChild (0).parent = null;
			if(hitTarget.parent)
				Destroy (hitTarget.parent.gameObject);
			Destroy (hitTarget.gameObject);
		} else if (hitTarget.name == "Bullseye") {
			if (hitTarget.parent) {
				if (hitTarget.parent.parent)
					Destroy (hitTarget.parent.parent.gameObject);
				Destroy (hitTarget.parent.gameObject);
			}
			Destroy (hitTarget.gameObject);
		} else {
			Debug.Log (hitTarget.name + ", children: " + hitTarget.childCount);
			hitTarget.GetChild (0).parent = null;
			Destroy (hitTarget.gameObject);
		}
		Instantiate (explosionPrefab, this.transform.position, this.transform.rotation);
		Destroy (rb);
	}
}
