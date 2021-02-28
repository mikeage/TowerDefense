using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class PointsPickup : MonoBehaviour {
	public int points = 1;

	void Awake() {
		bool triggerFound = false;
		foreach (Collider c in this.GetComponents<Collider>()) {
			triggerFound = triggerFound || c.isTrigger;
		}
		if (!triggerFound) {
			Debug.LogWarning ("None of the Colliders on " + this.gameObject.name + " are Triggers! Pickup probably won't work.");
		}
	}
}
