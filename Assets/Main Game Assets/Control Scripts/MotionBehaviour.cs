using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public abstract class MotionBehaviour : MonoBehaviour {
	/*
	 *  MotionMehaviour.cs
	 *  
	 *  A base class for all sorts of Rigidbody-based physics scripting
	 * 
	 *  When you create a child class:
	 *    1. Create whatever variables you need, and expose them to the inspector
	 *    2. You must implement the getter for localOffset
	 *       This must return the current total Offset (positon + rotation) at the current Time.timeSinceLevelLoad
	 *    3. You must implement Move()
	 *       This controls the timing of trigger events - this will change 
	 *    3. If you override Start, make sure to call base.Start()
	 * 
	 * 
	 * 
	 */

	// an internal variable 
	[HideInInspector] public bool readyToTrigger { get; protected set; }


	[System.Serializable]
	public class TriggerSettings {
		[SerializeField, Tooltip("Does this motion begin at game start?")]
		public bool triggerAtStart = true;
		[SerializeField, Tooltip("What key will trigger this motion?")]
		public KeyCode triggerKey = KeyCode.None;
		[SerializeField, Tooltip("Does this motion continue indefinitely?")]
		public bool loopMotion = true;
		// how long to delay the start of motion for after a trigger
		// used to offset scripts that move on the same period
		[SerializeField, Tooltip("How long to delay the start of motion for after a trigger")]
		public float startDelay = 0f;
		[SerializeField, Tooltip("How many times to do the motion when this script is triggered. Ignored if Loop Motion is true.")]
		public int cyclesPerTrigger=1;

	}

	[SerializeField] public TriggerSettings triggerSettings;

	[System.Serializable]
	public class TimeSettings {
		public bool autoAdvanceTime = true;
		public float currentTimeScale = 1.0f;
		[HideInInspector] public float originalTimeScale = 1.0f;
	}

	public TimeSettings timeSettings;

//	[SerializeField, Tooltip("What key will trigger this motion?")] protected KeyCode triggerKey = KeyCode.None;
//	[SerializeField, Tooltip("How many times to do the motion when this script is triggered. Ignored if Loop Motion is true.")] protected int cyclesPerTrigger=1;
//	
//	// how long to delay the start of motion for after a trigger
//	// used to offset scripts that move on the same period
//	[SerializeField, Tooltip("How long to delay the start of motion for after a trigger")] protected float startDelay = 0f;


	// this coroutine is called by Trigger()
	// must be implemented by all child classes
	abstract protected IEnumerator Move();

	// this getter should return the current Offset at this Time.timeSinceLevelLoad
	// must be implemented by child classes
	abstract protected Offset localOffset { get; }  

	// in order to properly calculate movement, these classes have to be aware of all
	// other MotionBehaviours on the same GameObject, and be able to dyanamically keep track of
	// our total desired position and rotation. 
	protected List<MotionBehaviour> otherMotionBehaviours; // array of all MotionBehaviours on this GameObject

	private int cacheHits = 0;
	private int cacheMisses = 0;

	virtual protected Offset totalOffset {  // *may* be implemented by child classes
		get {
			if (internalTime > this.mostRecentTime) {
				cachedOffset = this.localOffset;
				foreach (MotionBehaviour mb in otherMotionBehaviours) {
					cachedOffset += mb.localOffset;
				}
				mostRecentTime = internalTime;
				cacheMisses++;
			} else {
				cacheHits++;
			}
			return cachedOffset;
		}
	}

	// we use internalTime instead of Time.fixedTime to allow for pausing, slowing down, and resuming.
	protected float internalTime = 0f;

	private float mostRecentTime = -1f;

	private Offset cachedOffset;

	// our utility "not-a-Transform" class
	protected struct Offset {
		public Vector3 position;
		public Quaternion rotation;

		public static Offset identity = new Offset (Vector3.zero, Quaternion.identity);
		
		public Offset(Vector3 position, Quaternion rotation) {
			this.position = position;
			this.rotation = rotation;
		}
		
		public override string ToString() {
			return "pos: " + position.ToString () + ", rot: " + rotation.ToString ();
		}
		
		public static Offset operator +(Offset o1, Offset o2) {
			return new Offset(o1.position + o2.position, o1.rotation * o2.rotation);
		}
		public static Offset operator -(Offset o1, Offset o2) {
			return new Offset(o1.position - o2.position, o1.rotation * Quaternion.Inverse(o2.rotation));
		}
	}


	// provides a sum of all Offsets in other SimpleMovements on this GameObject
	// shouldn't need to change this in child classes, so it's implemented here
	protected Offset SumAllOffsets() {
		Offset o = new Offset(Vector3.zero, Quaternion.identity);
		if (otherMotionBehaviours.Count > 0) {
			foreach(MotionBehaviour sm in otherMotionBehaviours) {
				o = o + sm.localOffset;
			}
		}
		return o;
	}

	protected void ReconcileOffsets() {
		this.rb.MovePosition (this.origin.position + this.totalOffset.position);
		this.rb.MoveRotation (this.origin.rotation * this.totalOffset.rotation);
	//	Debug.Log ("cache Hits: " + cacheHits + ", Misses: " + cacheMisses + ", percent Hits: " + ((float) cacheHits / (float)(cacheHits + cacheMisses)));
	}
	

	// utility enumeration used by most inherited classes
	public enum Axis { xAxis, yAxis, zAxis }

	// converts an Axis to a Vector3
	public Vector3 AxisToVector3(Axis axis) {
		switch (axis) {
		case Axis.xAxis:
			return Vector3.right;
		case Axis.yAxis:
			return Vector3.up;
		default:
		case Axis.zAxis:
			return Vector3.forward;
		}
	}

	// only used internally
	protected Offset origin;
	protected Rigidbody rb;

	protected virtual void Awake() {
		// get the Rigidbody attached to this GameObject, and ensure it's isKinematic
		this.rb = this.GetComponent<Rigidbody> ();
		if (!rb.isKinematic) {
			Debug.LogWarning ("GameObject \"" + this.gameObject.name + "\" has a RigidBody Component that was not set to Kinematic. Script \"" + this.ToString () + "\" is setting isKinematic to true");
			rb.isKinematic = true;
		}

		// we want to save the origin, for reasons
		origin = new Offset (this.rb.position, this.rb.rotation);

		mostRecentTime = -1f;
		timeSettings.originalTimeScale = timeSettings.currentTimeScale;
	}

	// This can be overridden, but contains some useful stuff for every kind of movement
	// if you need to add things to a child Start() function, make sure to call base.Start()!
	protected virtual void Start () {
		// we're ready!
		readyToTrigger = true;
		if (triggerSettings.loopMotion && triggerSettings.triggerAtStart) {
			StartCoroutine (ProcessStartDelay ());
		}

		StartCoroutine (TimeAdvancer ());
	}

	// we put the keyboard detection code here, mostly to stay out of the way of regular Update() calls.
	protected virtual void LateUpdate() {
		if (Input.GetKeyDown (triggerSettings.triggerKey)) {
			this.Trigger ();
		}
	}

	// this is the all-important function that actually starts movement.
	public virtual void Trigger() {
		if(this.readyToTrigger) {
			StartCoroutine(this.ProcessStartDelay());
		}
	}

	virtual protected IEnumerator ProcessStartDelay() {
		readyToTrigger = false;

		// process startDelay
		float tEnd = Time.fixedTime + triggerSettings.startDelay;
		while(Time.fixedTime < tEnd) {
			yield return new WaitForFixedUpdate();
		}
		this.ReconcileOffsets ();
		StartCoroutine(this.Move());
	}

	void OnDisable() {
		this.otherMotionBehaviours.Clear ();
		foreach (MotionBehaviour sm in this.GetComponents<MotionBehaviour>()) {
			if (sm != this) {
				sm.ConnectToPeers ();
			}
		}
		this.PauseMotion ();
	}

	void OnEnable() {
		foreach (MotionBehaviour sm in this.GetComponents<MotionBehaviour>()) {
			sm.ConnectToPeers ();
		}
		this.ResumeMotion ();
	}

	public void ConnectToPeers() {
		if (this.otherMotionBehaviours == null) {
			// build our list of other SimpleMovements
			this.otherMotionBehaviours = new List<MotionBehaviour> ();
		}
		this.otherMotionBehaviours.Clear ();
		foreach(MotionBehaviour sm in this.GetComponents<MotionBehaviour>()) {
			if (sm.enabled && sm != this) {
				this.otherMotionBehaviours.Add (sm);
			}
		}
//		Debug.Log ("ConnectToPeers():\notherMotionBehaviours.Count = " + otherMotionBehaviours.Count);
	}

	private IEnumerator TimeAdvancer() {
		while (true) {
			if (timeSettings.autoAdvanceTime) {
				internalTime += Time.fixedDeltaTime * timeSettings.currentTimeScale;
			}
			yield return new WaitForFixedUpdate ();
		}
	}
	
	public void PauseMotion() {
		timeSettings.currentTimeScale = 0f;
	}

	public void ResumeMotion() {
		timeSettings.currentTimeScale = timeSettings.originalTimeScale;
	}

	public void advanceTime(int ticks) {
		internalTime += Time.fixedDeltaTime * timeSettings.currentTimeScale * ticks;
	}
}