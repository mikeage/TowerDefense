using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(BitSpaceUIController))]
public class BitSpaceFPSController : MonoBehaviour {
	[SerializeField] private RigidbodyFirstPersonController playerControlScript;
	[SerializeField] private PlayerActivator player;
	
	private BitSpaceUIController BUIC;

	void Awake() {
		BUIC = this.GetComponent<BitSpaceUIController> ();
	}

	void Start () {
		BUIC.ShowStartMenu ();

	}
	
	void Update () {
	}

	public void StartGame() {
		BUIC.ClearMenus ();
		player.setFree ();
	}

	public void RestartGame() {
		SceneManager.LoadSceneAsync (SceneManager.GetActiveScene ().buildIndex);
	}
}
