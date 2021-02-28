using UnityEngine;
using System.Collections;

[AddComponentMenu("Movement Scripts/Spinner")]
public class Spinner : MotionBehaviour {

	protected override Offset localOffset {
		get {
			return new Offset(Vector3.zero, Quaternion.Euler(axis * degreesOfMovement * percent));
		}
	}

	public Axis axisOfRotation = Axis.zAxis;
	private Vector3 axis;
	public float degreesOfMovement = 60.0f;
	public float durationOfMovement = 1f;

	private float percent;
	private int cycle;

	protected override void Start ()
	{
		base.Start ();
		percent = 0.0f;
		cycle = 0;

		axis = this.AxisToVector3 (axisOfRotation);
	}

	void onValidate() {
		durationOfMovement = Mathf.Max (Time.fixedDeltaTime, durationOfMovement);

	}

	protected override IEnumerator Move() {
		readyToTrigger = false;
		int endCycle = cycle + triggerSettings.cyclesPerTrigger;
		this.ReconcileOffsets ();
		float tEnd = internalTime;
		// loop for cyclesPerTrigger times, or forever if loopMotion is true
		while(this.triggerSettings.loopMotion || (cycle < endCycle)) {

			tEnd += durationOfMovement;
			while (internalTime < tEnd) {
				percent = cycle + 1 - (tEnd - internalTime) / durationOfMovement;
				this.ReconcileOffsets();
				yield return new WaitForFixedUpdate ();
			}
			percent = cycle + 1.0f;
			this.ReconcileOffsets();
			cycle++;
		}
		readyToTrigger = true;
	}
}

