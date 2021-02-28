using UnityEngine;
using System.Collections;

public class DisplayActivator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.LogError (Display.displays.Length+" displays active");
		foreach (Display d in Display.displays) {
			d.Activate ();
		}
	}

	void Update() {
		if (Input.GetKeyUp(KeyCode.Escape)) {
			Application.Quit();
		}
	}
}
