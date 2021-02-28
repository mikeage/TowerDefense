using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class PlayerActivator : MonoBehaviour {

	// whether the player has successfully activated something
	// initially false, but other scripts change this value
	private bool occupied;

	public Image reticle;
	public Text instructions;

	public float interactDistance;

	// Use this for initialization
	void Start () {
	}

	public void setBusy() {
		occupied = true;
		reticle.enabled = false;
		instructions.enabled = false;
	}

	public void setFree() {
		occupied = false;
		reticle.enabled = true;
		instructions.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (occupied == false) {
			if (CrossPlatformInputManager.GetButtonDown ("Fire1")) {
				Ray ray = Camera.main.ScreenPointToRay (new Vector3 (Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2));
				RaycastHit hit;
				// 
				if (Physics.Raycast (ray, out hit, interactDistance)) {
					Debug.Log (hit.transform.name);
					hit.transform.SendMessageUpwards ("ActivateObject", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
		if(CrossPlatformInputManager.GetButtonDown ("Pause Menu")) {

		}
	}
}
