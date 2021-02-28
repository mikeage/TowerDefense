using UnityEngine;
using System.Collections;

public class TriggerMotion : MonoBehaviour {
	public GameObject objectWithMotions;
	public bool triggerOnCollision = true;
	public bool triggerOnTriggers = true;
	public string triggerOnTag = "";
	

	void onValidate() {
		if (objectWithMotions == null) {
			if (this.GetComponents<MotionBehaviour> ().Length == 0) {
				Debug.LogWarning (this.name + "'s TriggerMotion is targetting itself but has no MotionBehaviours!");
			}
			objectWithMotions = this.gameObject;
		} else {
			if (objectWithMotions.GetComponents<MotionBehaviour> ().Length == 0) {
				Debug.LogWarning (this.name + "'s TriggerMotion has a target with no MotionBehaviours!");
			}
		}
	}

	void OnTriggerEnter(Collider coll) {
		if (triggerOnTriggers && (triggerOnTag == "" || coll.CompareTag(triggerOnTag))) {
			foreach (MotionBehaviour mb in objectWithMotions.GetComponents<MotionBehaviour> ()) {
				mb.Trigger ();
			}
		}
	}
	void OnCollisionEnter(Collision coll) {
		if (triggerOnTriggers && (triggerOnTag == "" || coll.gameObject.CompareTag(triggerOnTag))) {
			foreach (MotionBehaviour mb in objectWithMotions.GetComponents<MotionBehaviour> ()) {
				mb.Trigger ();
			}
		}
	}
}
