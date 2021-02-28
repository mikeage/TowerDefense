using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Movement Scripts/Waypoint Follower")]
public class WaypointFollower : MotionBehaviour {
	[System.Serializable]
	public class Waypoint {
		public Transform point;
		public float travelDuration = 1f;
		public float pauseDuration = 1f;
		public AnimationCurve movementFunction = AnimationCurve.EaseInOut (0f, 0f, 1f, 1f);
	}

	public Waypoint[] waypointArray = new Waypoint[2];

	private Transform previousTransform;
	private Transform nextTransform;
	private float percent = 0f;


	protected override void Awake () {
		base.Awake ();
		if(waypointArray.Length < 2) {
			Debug.LogError ("Less than 2 Waypoints on " + this.name + "! Aborting script.");
			this.enabled = false;
			return;
		}
		previousTransform = waypointArray [0].point;
		nextTransform = waypointArray [1].point;
		//origin = new Offset (waypointArray [0].point.position, waypointArray [0].point.rotation);
		//this.ReconcileOffsets ();
	}


	protected override Offset localOffset {
		get {
			return new Offset(Vector3.LerpUnclamped(previousTransform.position, nextTransform.position, percent), 
				Quaternion.SlerpUnclamped(previousTransform.rotation, nextTransform.rotation, percent)) - this.origin;
		}
	}

	void OnValidate() {
//		for (int i = 0; i < waypointArray.Length; i++) {
//			if (waypointArray [i].point == null) {
//				if (i == 0) {
//					waypointArray [0].point = this.transform;
//				} else {
//					Debug.LogWarning ("No Transform set for Waypoint " + (i + 1) + " on object " + this.name);
//				}
//			}
//		}

		if (waypointArray.Length < 2) {
			Debug.LogWarning ("Less than 2 Waypoints on " + this.name + "! The script won't run!");
		}
	}
	
	protected override IEnumerator Move() {
		readyToTrigger = false;
		float tEnd = internalTime;
		// loop for cyclesPerTrigger times, or forever if loopMotion is true
		for (int cycle = 0; this.triggerSettings.loopMotion || (cycle < triggerSettings.cyclesPerTrigger); cycle++) {
//			Debug.Log ("waypointArray.Length = " + waypointArray.Length);
			for(int i = 0; i < waypointArray.Length; i++) {
				previousTransform = waypointArray [i].point;;
				Waypoint nextWP = waypointArray [(i + 1) % waypointArray.Length];
				nextTransform = nextWP.point;
//				Debug.Log (this.name + " travelling from " + previousTransform.name + " to " + nextTransform.name);
				percent = 0f;
				tEnd += nextWP.travelDuration;
				while (internalTime < tEnd) {
					percent = nextWP.movementFunction.Evaluate (1 - (tEnd - internalTime) / nextWP.travelDuration);
					this.ReconcileOffsets ();
					yield return new WaitForFixedUpdate ();
				}
				percent = 1.0f;
				this.ReconcileOffsets ();
				tEnd += nextWP.pauseDuration;
				while (internalTime < tEnd) {
					yield return new WaitForFixedUpdate ();
				}

			}
			this.ReconcileOffsets();
		}
		readyToTrigger = true;
	}
}
