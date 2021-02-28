using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour {
	
	[SerializeField] private Text runTimer;

	[HideInInspector] public bool timerOn { get; private set; }
	private DateTime startTime;
	private DateTime endTime;
	private TimeSpan thisRun;
	public bool startImmediately = true;
	public enum TimerType {CountDown, CountUp};
	public float maxTime = 180f;
	public TimerType timerType;
	public UnityEngine.Events.UnityEvent endTimeEvents;

	void Awake() {
	}
	
	// Use this for initialization
	void Start () {
		timerOn = false;
		startTime = DateTime.MinValue;
		thisRun = TimeSpan.Zero;
		if (startImmediately) {
			this.StartTimer ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (timerOn) {
			if (timerType == TimerType.CountDown) {
				thisRun = endTime - DateTime.Now;
				if (maxTime > 0f && thisRun < TimeSpan.Zero) {
					thisRun = TimeSpan.Zero;
					timerOn = false;
					endTimeEvents.Invoke ();
				}			
			} else {
				thisRun = DateTime.Now - startTime;
				if (maxTime > 0f && DateTime.Now > endTime) {
					thisRun = endTime - startTime;
					timerOn = false;
					endTimeEvents.Invoke ();
				}
			}
		}
		SetText (runTimer);
	}

	public void ShowTimer() {
		runTimer.enabled = true;
	}

	public void HideTimer() {
		runTimer.enabled = false;
	}
	
	public void StartTimer() {
		timerOn = true;
		startTime = DateTime.Now;
		endTime = DateTime.Now + new TimeSpan (0, 0, (int) maxTime);
	}
	
	public void StopTimer() {
		timerOn = false;
	}

	public void ToggleTimer() {
		timerOn = !timerOn;
	}

	public void AddTime(int sec) {
		endTime += new TimeSpan (0, 0, sec);
	}

	private void SetText(Text textComp) {
		if(thisRun.Minutes > 0) {
			textComp.text = String.Format ("{0:D}:{1:D2}.{2:D1}", thisRun.Minutes, thisRun.Seconds, (int) (thisRun.Milliseconds/100f));
		} else {
			textComp.text = String.Format ("{0:D}.{1:D1}", thisRun.Seconds, (int) (thisRun.Milliseconds/100f));
		}
	}
}
