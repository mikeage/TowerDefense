using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class TriggerEvent : MonoBehaviour {
	public bool destroyOnEnter = true;
	public bool oneTimeTrigger = false;
	public bool destroyOnExit = false;
	private bool enterTriggered = false;
	private bool exitTriggered = false;

	public List<string> tagsToCheck = new List<string> (new string[] { "Player" });

	public UnityEvent enterEvents;
	public UnityEvent exitEvents;

	void OnTriggerEnter(Collider other) {
		foreach (string aTag in tagsToCheck) {
			if (other.CompareTag (aTag)) {
				HandleEnter ();
			}
		}
	}

	void OnTriggerExit(Collider other) {
		foreach (string aTag in tagsToCheck) {
			if (other.CompareTag (aTag)) {
				HandleExit ();
			}
		}
	}

	private void HandleEnter() {
		if (!oneTimeTrigger || !enterTriggered) {
			enterEvents.Invoke ();
			this.enterTriggered = true;

			if (destroyOnEnter) {
				Destroy (this.gameObject);
			}
		}
	}

	private void HandleExit() {
		if (!oneTimeTrigger || !exitTriggered) {
			exitEvents.Invoke ();
			this.exitTriggered = true;

			if (destroyOnExit) {
				Destroy (this.gameObject);
			}
		}
	}
}
