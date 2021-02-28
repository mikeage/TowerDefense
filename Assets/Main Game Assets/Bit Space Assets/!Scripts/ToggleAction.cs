using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ToggleAction : MonoBehaviour {
	public KeyCode toggleKey;
	public UnityEvent onEvents;
	public UnityEvent offEvents;

	private bool running = false;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (toggleKey)) {
			Toggle ();
		}
	}

	public void Activate() {
		onEvents.Invoke ();
		running = true;
	}

	public void Deactivate() {
		offEvents.Invoke ();
		running = false;
	}

	public void Toggle() {
		if (running) {
			Deactivate ();
		} else {
			Activate ();
		}
	}
}
