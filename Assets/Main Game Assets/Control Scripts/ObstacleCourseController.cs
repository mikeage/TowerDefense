using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Vehicles.Ball;


public class ObstacleCourseController : MonoBehaviour {

	[SerializeField] private BallUserControl player;

	[SerializeField] private GameObject startMenu;
	[SerializeField] private GameObject retryMenu;
	[SerializeField] private GameObject winMenu;
	[SerializeField] private Text winMenuRunTime;

	[SerializeField] private Image solidColor;
	private Text runTimer;

	private bool gameFinished;
	private DateTime startTime;
	private TimeSpan thisRun;

	private delegate void DelegateMethod();

	void Awake() {
	}

	// Use this for initialization
	void Start () {
		gameFinished = false;
		startTime = DateTime.MinValue;
		thisRun = TimeSpan.MaxValue;

		Time.timeScale = 0f;

		player.enabled = false;

		solidColor.enabled = false;

		runTimer = GameObject.Find ("Run Timer").GetComponent<Text> ();
		runTimer.enabled = false;
		winMenu.SetActive (false);
		retryMenu.SetActive (false);

		startMenu.SetActive (true);

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(!gameFinished) 
			thisRun = DateTime.Now - startTime;
		if(thisRun.Minutes > 0) {
			runTimer.text = String.Format ("{0:D}:{1:D2}.{2:D3}", thisRun.Minutes, thisRun.Seconds, thisRun.Milliseconds);
		} else {
			runTimer.text = String.Format ("{0:D}.{1:D3}", thisRun.Seconds, thisRun.Milliseconds);
		}

		if (CrossPlatformInputManager.GetButtonDown ("MouseLock")) {
			if(Cursor.lockState == CursorLockMode.Locked) {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			} else {
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}
	}

	public void StartGame() {
		startMenu.SetActive (false);
		player.enabled = true;
		runTimer.enabled = true;
		startTime = DateTime.Now;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		Time.timeScale = 1.0f;
	}

	public void BeginGameOver() {
		player.enabled = false;
		gameFinished = true;
//		Time.timeScale = 0f;
		StartCoroutine (fadeToColorAndCallMethod (Color.black, ShowRestartMenu));
	}

	private void ShowRestartMenu() {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		retryMenu.SetActive (true);
	}

	public void RestartGame() {
		SceneManager.LoadSceneAsync (SceneManager.GetActiveScene ().buildIndex);
	}

	public void PlayerWins() {
		player.enabled = false;
		gameFinished = true;
		StartCoroutine (fadeToColorAndCallMethod (Color.white, ShowWinMenu));
	}

	private void ShowWinMenu() {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		winMenu.SetActive (true);
		winMenuRunTime.text = runTimer.text;
	}

	private IEnumerator fadeToColorAndCallMethod(Color targetColor, DelegateMethod method) {
		solidColor.color = targetColor;
		solidColor.enabled = true;
		Color temp;

		float duration = 1.0f;
		float endTime = Time.time + duration;
		while (Time.time < endTime) {
			temp = solidColor.color;
			temp.a = 1 - (endTime-Time.time)/duration;
			solidColor.color = temp;
			yield return new WaitForFixedUpdate();
		}
		Time.timeScale = 0f;
		method ();
	}

}
