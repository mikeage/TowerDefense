using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KillSwitch : MonoBehaviour {
	public string tagToCheck = "";
	public List<MonoBehaviour> objects = new List<MonoBehaviour>();

	void OnTriggerEnter(Collider coll) {
		if (coll.CompareTag (tagToCheck)) {
			foreach (MonoBehaviour ob in objects) {
				ob.StopAllCoroutines();
			}
		}
	}
}
