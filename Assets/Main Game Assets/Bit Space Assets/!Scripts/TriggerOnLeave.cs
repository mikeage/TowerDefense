using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerOnLeave : MonoBehaviour {
	public string tagToCompare = "Player";
	public bool destroyOnComplete = true;
	public UnityEvent events;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerExit(Collider coll) {
		if (coll.CompareTag (tagToCompare)) {
			events.Invoke ();
			if (destroyOnComplete) {
				Destroy (this.gameObject);
			}
		}
	}
}
