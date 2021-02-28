using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TimedTrigger : MonoBehaviour {
	public bool triggerAtStart = true;

	[System.Serializable]
	public class TimerEvent {
		public float timeToWait = 1f;
		public UnityEvent uEvent;
	}
	public TimerEvent[] timedEvents;

	void Start() {
		if (triggerAtStart) {
			this.Trigger();
		}
	}

	public void Trigger () {
		foreach(TimerEvent te in timedEvents) {
			StartCoroutine(processTrigger(te));
		}
	}

	public void CancelTriggers() {
		this.StopAllCoroutines();
	}

	private IEnumerator processTrigger(TimerEvent te) {
		yield return new WaitForSeconds(te.timeToWait);
		te.uEvent.Invoke();
	}
}
