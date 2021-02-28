using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class TimerController : MonoBehaviour {
	
	[SerializeField] private Text runTimer;
	[SerializeField] private Text winMenuRunTime;

	[HideInInspector] public bool timerOn { get; private set; }
	private DateTime startTime;
	private TimeSpan thisRun;
	public bool showTimerAtStart = true;

	void Awake() {
	}
	
	// Use this for initialization
	void Start () {
		timerOn = false;
		startTime = DateTime.MinValue;
		thisRun = TimeSpan.Zero;
		if (showTimerAtStart) {
			this.ShowTimer ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(timerOn) 
			thisRun = DateTime.Now - startTime;
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
	}
	
	public void StopTimer() {
		timerOn = false;
		SetText (winMenuRunTime);
	}

	public void ToggleTimer() {
		timerOn = !timerOn;
	}

	private void SetText(Text textComp) {
		if(thisRun.Minutes > 0) {
			textComp.text = String.Format ("{0:D}:{1:D2}.{2:D3}", thisRun.Minutes, thisRun.Seconds, thisRun.Milliseconds);
		} else {
			textComp.text = String.Format ("{0:D}.{1:D3}", thisRun.Seconds, thisRun.Milliseconds);
		}
	}
}
