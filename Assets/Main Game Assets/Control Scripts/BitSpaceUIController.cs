using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

public class BitSpaceUIController : MonoBehaviour {

	[SerializeField] private MonoBehaviour playerControlScript;

	// need to turn on the interface object at the start of the game
	[SerializeField] private GameObject interfaceObject;
	[SerializeField] private GameObject startMenu;
	[SerializeField] private GameObject retryMenu;
	[SerializeField] private GameObject winMenu;
	[SerializeField] private GameObject pauseMenu;

	[SerializeField] public KeyCode mouseLockButton = KeyCode.L;

	[SerializeField] private Image solidColor;

	public bool showStartMenuAtStart = true;

	public bool freezesPlayer = true;
	public bool stopsTime = true;
	public bool locksCursor = true;

	private delegate void DelegateMethod();

	// Use this for initialization
	void Start () {
		if (playerControlScript == null) {
			Debug.LogWarning (this.gameObject.name + "'s BitSpaceUIController has no Player Control Script assigned!");
		}

		interfaceObject.SetActive (true);

		solidColor.enabled = false;

		winMenu.SetActive (false);
		retryMenu.SetActive (false);

		//startMenu.SetActive (showStartMenuAtStart);

		startMenu.SetActive(false);
		if (showStartMenuAtStart)
			ShowStartMenu ();
		
		if (locksCursor) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (locksCursor && Input.GetKeyDown (mouseLockButton)) {
			if(Cursor.lockState == CursorLockMode.Locked) {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			} else {
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}
		if(CrossPlatformInputManager.GetButtonDown ("Cancel")) {
			TogglePauseMenu ();
		}
	}

	public void ShowStartMenu() {
		startMenu.SetActive (true);
		FreezeStopUnlock ();
	}

	public void ShowRestartMenu() {
		FreezeStopUnlock ();
		retryMenu.SetActive (true);
	}

	public void ShowWinMenu() {
		FreezeStopUnlock ();
		winMenu.SetActive (true);
	}

	public void TogglePauseMenu() {
		if (!startMenu.activeSelf && !retryMenu.activeSelf && !winMenu.activeSelf) {
			if(!pauseMenu.activeSelf) {
				FreezeStopUnlock ();
				pauseMenu.SetActive (true);
			} else {
				ClearMenus();
			}
		}
	}

	public void FadeOutLose() {
		if(freezesPlayer && playerControlScript != null)
			playerControlScript.enabled = false;

		StartCoroutine (fadeToColorAndCallMethod (Color.black, ShowRestartMenu));
	}

	public void FadeOutWin() {
		if(freezesPlayer && playerControlScript != null)
			playerControlScript.enabled = false;
		
		StartCoroutine (fadeToColorAndCallMethod (Color.white, ShowWinMenu));
	}

	public void ClearMenus() {
		startMenu.SetActive (false);
		retryMenu.SetActive (false);
		winMenu.SetActive (false);
		pauseMenu.SetActive (false);

		if (freezesPlayer && playerControlScript != null)
			playerControlScript.enabled = true;
		if (stopsTime)
			Time.timeScale = 1.0f;
		if (locksCursor) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	private void FreezeStopUnlock() {
		if (freezesPlayer && playerControlScript != null)
			playerControlScript.enabled = true;
		if (stopsTime)
			Time.timeScale = 0f;
		if (locksCursor) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
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
		method ();
	}
	public void RestartGame() {
		SceneManager.LoadScene (SceneManager.GetSceneAt(0).buildIndex);
	}
}
