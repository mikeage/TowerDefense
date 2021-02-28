using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class TriggerOnImpulse : MonoBehaviour {
	
	private List<Vector3> impulses;
	public float crushThreshold = 50f;
	public float smashThreshold = 500f;
	public bool logCrushesToConsole = false;
	public bool logSmashesToConsole = false;
	public UnityEvent functionsToTrigger;
	
	void Start () {
		impulses = new List<Vector3> ();
	}
	
	void Update () {
		if (impulses.Count > 1) {
			for(int i = 0; i < impulses.Count-1; i++) {
				if (impulses [i].sqrMagnitude > smashThreshold * smashThreshold) {
					functionsToTrigger.Invoke ();
					if (logSmashesToConsole)
						Debug.Log ("SMASHED! " + impulses [i].ToString () + " had magnitude " + impulses[i].magnitude);
				}
				for (int j = i + 1; j < impulses.Count; j++) {
					float impulseDot = Vector3.Dot (impulses [i], impulses [j]);
					if (impulseDot < -crushThreshold) {
						functionsToTrigger.Invoke ();
						if (logCrushesToConsole)
							Debug.Log ("CRUSHED! " + impulses [i].ToString () + " dot " + impulses [j] + " = " + impulseDot.ToString ("F2"));
					}
				}
			}
			
		}
		impulses.Clear ();
	}




	void OnCollisionStay(Collision coll) {
		foreach (ContactPoint contact in coll.contacts) {
			if(!impulses.Contains(contact.normal*coll.impulse.magnitude)) {
				impulses.Add(contact.normal*coll.impulse.magnitude);
			}
		}
	}
}
