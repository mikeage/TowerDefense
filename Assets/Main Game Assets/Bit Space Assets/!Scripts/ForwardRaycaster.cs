using UnityEngine;
using UnityEngine.Events;
using System.Collections;


public class ForwardRaycaster : MonoBehaviour {
	public Transform forwardTransform;
	public LayerMask layersToHit;
	public float maxDistance = 3f;
	public float raycastWidth = 0.01f;
	public GameObject activeTarget;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hitInfo;
		if (Physics.SphereCast (forwardTransform.position, raycastWidth, forwardTransform.forward, out hitInfo, maxDistance, layersToHit)) {
			if (hitInfo.collider.gameObject != activeTarget) {
				// if the thing we hit wasn't the thing we previously hit
				//Debug.Log ("New target found! " + hitInfo.collider.gameObject.name);

				// undo anything we did to the previous target, if we moved directly from target to target
				if (activeTarget != null) {
					RaycastReactor oldThing = activeTarget.GetComponentInParent<RaycastReactor> ();
					if(oldThing != null)
						oldThing.UndoTheThing ();
				}

				activeTarget = hitInfo.collider.gameObject;
				RaycastReactor newThing = activeTarget.GetComponentInParent<RaycastReactor> ();
				if (newThing != null) {
					newThing.DoTheThing ();
				}
			} else {
				// nothing new to do
			}
		} else {
			// if we were facing a thing before, we want to undo any work to it
			if (activeTarget != null) {
				RaycastReactor oldThing = activeTarget.GetComponentInParent<RaycastReactor> ();
				if (oldThing != null) {
					oldThing.UndoTheThing ();
				}
			}
			// clear the active target if we've hit nothing
			activeTarget = null;
		}
	}
}
