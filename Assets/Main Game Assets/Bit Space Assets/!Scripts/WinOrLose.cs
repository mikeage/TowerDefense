using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class WinOrLose : MonoBehaviour {
	public string winTag;
	public UnityEvent winEvents;

	public string loseTag;
	public UnityEvent loseEvents;

	public bool checkCollisions = true;
	public bool checkTriggers = true;

	void OnTriggerEnter(Collider coll) {
		if (checkTriggers) {
			CheckTag (coll.tag);
		}
	}

	void OnCollisionEnter(Collision coll) {
		if (checkCollisions) {
			if (coll.collider.attachedRigidbody) {
				CheckTag (coll.collider.attachedRigidbody.tag);
			}
			CheckTag (coll.transform.tag);
		}
	}

	private void CheckTag(string tag) {
		if (tag == winTag) {
			winEvents.Invoke ();
		} else if (tag == loseTag) {
			loseEvents.Invoke ();
		}
	}
}
