using UnityEngine;
using System.Collections;

[AddComponentMenu("Movement Scripts/Smasher")]
public class Smasher : MotionBehaviour {
	protected override Offset localOffset {
		get {
			return new Offset(Vector3.zero, Quaternion.Euler(axis * degreesOfMovement * percent));
		}
	}

	public Axis axisOfRotation = Axis.zAxis;
	private Vector3 axis;
	public float degreesOfMovement = 90f;
	public float smashDuration = 1f;
	public float pauseDuration = 1f;
	public float returnDuration = 4f;
	public float restDuration = 1f;

	private float percent;
	private int cycle;

	protected override void Start ()
	{
		base.Start ();
		percent = 0.0f;
		cycle = 0;

		axis = this.AxisToVector3 (axisOfRotation);
	}

	void OnValidate() {
		smashDuration = Mathf.Max (Time.fixedDeltaTime, smashDuration);
		pauseDuration = Mathf.Max (0f, pauseDuration);
		returnDuration = Mathf.Max (Time.fixedDeltaTime, returnDuration);
		restDuration = Mathf.Max (0f, restDuration);
	}

	protected override IEnumerator Move() {
		readyToTrigger = false;
		float tEnd = internalTime;

		// loop for cyclesPerTrigger times, or forever if loopMotion is true
		for (cycle = 0; this.triggerSettings.loopMotion || (cycle < triggerSettings.cyclesPerTrigger); cycle++) {

			percent = 0.0f;

			tEnd += smashDuration;
			while (internalTime < tEnd) {
				// do forward motion
				percent = 1 - (tEnd - internalTime) / smashDuration;
				this.ReconcileOffsets();

				yield return new WaitForFixedUpdate ();
			}

			percent = 1.0f;
			this.ReconcileOffsets();

			tEnd += pauseDuration;
			while (internalTime < tEnd) {
				// do nothing
				yield return new WaitForFixedUpdate ();
			}

			percent = 1.0f;

			tEnd += returnDuration;
			while (internalTime < tEnd) {
				percent = (tEnd - internalTime) / returnDuration;
				yield return new WaitForFixedUpdate ();
				this.ReconcileOffsets();
			}

			percent = 0.0f;
			this.ReconcileOffsets();

			tEnd += restDuration;
			while (internalTime < tEnd) {
				// do nothing
				yield return new WaitForFixedUpdate ();
			}

		}
		readyToTrigger = true;
	}
}
