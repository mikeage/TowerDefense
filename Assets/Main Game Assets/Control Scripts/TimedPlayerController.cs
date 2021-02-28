using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TimedPlayerController : MonoBehaviour {
	private BitSpaceUIController BUIC;
	private TimerController TC;

	// Use this for initialization
	void Start () {
		BitSpaceUIController[] buics = GameObject.FindObjectsOfType<BitSpaceUIController> ();
		if (buics.Length > 1) {
			Debug.LogWarning ("More than one BitSpaceUIController found!");
			BUIC = buics [0];
			Debug.Log (this.gameObject.name + "'s TimedPlayerController is setting its BitSpaceUIController to " + buics [0].gameObject.name);
		} else if (buics.Length < 1) {
			Debug.LogWarning ("No BitSpaceUIController found!");
		} else {
			BUIC = buics [0];
		}

		TimerController[] tcs = GameObject.FindObjectsOfType<TimerController> ();
		if (tcs.Length > 1) {
			Debug.LogWarning ("More than one TimerController found!");
			TC = tcs [0];
			Debug.Log (this.gameObject.name + "'s TimedPlayerController is setting its TimerController to " + tcs [0].gameObject.name);
		} else if (tcs.Length < 1) {
			Debug.LogWarning ("No TimerController found!");
		} else {
			TC = tcs [0];
		}

		BUIC.ShowStartMenu ();
		TC.ShowTimer ();
	}

	void OnTriggerEnter(Collider other) {
		if(other.CompareTag ("Goal")) {
			TC.StopTimer();
			BUIC.FadeOutWin();
		}
		if(other.CompareTag ("Death Trigger")) {
			TC.StopTimer();
			BUIC.FadeOutLose ();
		}

	}
	void OnTriggerExit(Collider other) {
		if (other.CompareTag ("Start Trigger")) {
			TC.ShowTimer ();
			TC.StartTimer();
		}
	}
	public void RestartGame() {
		SceneManager.LoadSceneAsync (SceneManager.GetActiveScene ().buildIndex);

	}
}
