using UnityEngine;
using System.Collections;

[AddComponentMenu("Movement Scripts/Linear Motion")]
public class LinearMotion : MotionBehaviour {

	protected override Offset localOffset {
		get {
			return new Offset(dir * moveDistance * movementCurve.Evaluate(percent), Quaternion.identity);
		}
	}

	[Header("Motion Durations")]
	public Axis directionOfMovement = Axis.zAxis;
	[Tooltip("Distance this object will move in the above Axis. Can be negative.")]
	public float moveDistance = 5.0f;
	[Tooltip("Duration in seconds from origin to full extension.")]
	public float forwardDuration = 1.0f;
	[Tooltip("Duration in seconds object will wait at full extension.")]
	public float pauseDuration = 1.0f;
	[Tooltip("Duration in seconds from full extension back to origin.")]
	public float returnDuration = 4.0f;
	[Tooltip("Duration in seconds object will wait before starting a new cycle.")]
	public float restDuration = 1.0f;
	[Tooltip("Movement curve. Click to edit.\nMust start at (0,0) and end at (1,1).")]
	public AnimationCurve movementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	[Tooltip("Along what axis to move")]
	private Vector3 dir;
	private float percent;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		dir = AxisToVector3 (directionOfMovement);
	}
	
	void OnValidate() {
		forwardDuration = Mathf.Max (Time.fixedDeltaTime, forwardDuration);
		pauseDuration = Mathf.Max (0f, pauseDuration);
		returnDuration = Mathf.Max (Time.fixedDeltaTime, returnDuration);
		restDuration = Mathf.Max (0f, restDuration);
	}
	
	protected override IEnumerator Move() {
		readyToTrigger = false;
		float tEnd = internalTime;
		
		// loop for cyclesPerTrigger times, or forever if loopMotion is true
		for (int cycle = 0; this.triggerSettings.loopMotion || (cycle < triggerSettings.cyclesPerTrigger); cycle++) {

			percent = 0.0f;
			this.ReconcileOffsets();

			tEnd += forwardDuration;
			while (internalTime < tEnd) {
				// do forward motion
				percent = 1 - (tEnd - internalTime) / forwardDuration;
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

			this.ReconcileOffsets();

			tEnd += returnDuration;
			while (internalTime < tEnd) {
				percent = (tEnd - internalTime) / returnDuration;
				this.ReconcileOffsets();
				yield return new WaitForFixedUpdate ();
			}

			percent = 0.0f;
			this.ReconcileOffsets();

			tEnd += restDuration;
			while (internalTime < tEnd) {
				// do nothing
				yield return new WaitForFixedUpdate ();
			}

			this.ReconcileOffsets();
		}
		readyToTrigger = true;
	}
}
