using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class LifePickup : MonoBehaviour {
	public float life = 1f;
	public string tagToCheck = "Player";

	void Awake() {
		bool triggerFound = false;
		foreach (Collider c in this.GetComponents<Collider>()) {
			triggerFound = triggerFound || c.isTrigger;
		}
		if (!triggerFound) {
			Debug.LogWarning ("None of the Colliders on " + this.gameObject.name + " are Triggers! Pickup probably won't work.");
		}
	}

	void OnTriggerEnter(Collider coll) {
		Damager d = coll.GetComponentInParent<Damager>();

		if(d != null && (coll.CompareTag(tagToCheck) || d.CompareTag(tagToCheck))) {
			d.RecoverHealth(life);
		}
	}
}
