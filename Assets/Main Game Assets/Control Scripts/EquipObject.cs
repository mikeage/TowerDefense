using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

// Handles the equipping and unequipping of an item
public class EquipObject : MonoBehaviour {

	// item to equip
	public Transform item;
	// enabled and disabled as needed
	private ItemRotator itemSpinner;

	// where the item will be equipped to
	// must be a child of a Camera
	public Transform equipPoint;
	// reference to the activating script
	public PlayerActivator pA;
	// reference to our instruction Text component
	public Text instructions;
	// key to drop item
	public KeyCode dropItemKey = KeyCode.X;

	// status and timing flags
	private bool itemEquipping;
	private bool itemReady;

	// adjustable constants
	public float equipTime;
	public float unequipTime;
	public float floatHeight;
	public float spinSpeed;

	// hierarchy information
	private Transform itemPreviousParent;
	private Transform camParent;
	
	void Awake() {
		if (!item) {
			Debug.LogError("EquipObject needs an object to work with! Disabling script...");
			this.enabled = false;
		}
	}

	// Use this for initialization
	void Start () {
		itemEquipping = false;
		itemReady = false;
		instructions.enabled = false;
		itemPreviousParent = item.transform.parent;
		if (!equipPoint.GetComponentInParent<Camera> ()) {
			Debug.LogError ("EquipObject: equipPoint must be child of a Camera component");
		}
		camParent = equipPoint.GetComponentInParent<Camera> ().transform;

		// set up the idle rotator
		if (!item.GetComponent<ItemRotator>()) {
			item.gameObject.AddComponent<ItemRotator>();
		}
		itemSpinner = item.GetComponent<ItemRotator> ();
		itemSpinner.yDegreesPerSecond = spinSpeed;
		itemSpinner.xDegreesPerSecond = itemSpinner.zDegreesPerSecond = 0;
		itemSpinner.enabled = true;
	}

	// Update is called once per frame
	void Update () {

		if (itemReady) {
			// 
			if (Input.GetKeyDown (dropItemKey)) {
				itemReady = false;
				StartCoroutine(DropItem());
				// don't allow anything to fire if we're dropping the item
				return;
			}
			if (CrossPlatformInputManager.GetButtonDown ("Fire1")) {
				// primary fire must be implemented
				try {
					item.SendMessage("Fire", SendMessageOptions.RequireReceiver);
				} catch {
					Debug.LogError("EquipObject: item needs to have a 'Fire()' method");
				}
			} 
			if (CrossPlatformInputManager.GetButtonDown ("Fire2")) {
				// alt fire not required
				item.SendMessage("AltFire", SendMessageOptions.DontRequireReceiver);
			}
		}

	}

	public void ActivateObject() {
		if (itemEquipping == false && itemReady == false) {
			StartCoroutine(MoveAndPrepItem());
		}
	}

	IEnumerator MoveAndPrepItem() {
		itemSpinner.enabled = false;
		instructions.enabled = true;
		pA.setBusy ();
		pA.reticle.enabled = true;
		item.transform.parent = camParent;
		float endTime = Time.time + equipTime;
		float percent;
		while(Time.time <= endTime) {
			percent = 1 - (endTime - Time.time) / equipTime;
			item.position = Vector3.Lerp (item.position, equipPoint.position, percent);
			item.rotation = Quaternion.Slerp (item.rotation, equipPoint.rotation, percent);
			yield return new WaitForFixedUpdate();
		}
		itemReady = true;
		try {
			item.SendMessage ("ActivateItem", SendMessageOptions.RequireReceiver);
		} catch {
			Debug.LogError ("EquipObject: item needs to implement ActivateItem()");
		}
	}

	IEnumerator DropItem() {
		instructions.enabled = false;
		itemReady = false;
		item.transform.parent = itemPreviousParent;
		float endTime = Time.time + unequipTime;
		float percent;
		// set up the destination point
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (new Vector3 (Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2));
		Vector3 destPos;
		// if we try to put it into the ground ...
		if (Physics.Raycast (ray, out hit, pA.interactDistance + 0.5f)) {
			// float it above ground
			destPos = hit.point + Vector3.up * floatHeight;
		} else {
			// if we try to put it in the air, move it away from us, then drop it to floatHeight
			destPos = item.position + equipPoint.forward * pA.interactDistance * 0.8f;
			if (Physics.Raycast (destPos, Vector3.down, out hit)) {
				destPos = hit.point + Vector3.up * floatHeight;
			}
		}
		Quaternion destRot = Quaternion.AngleAxis (equipPoint.eulerAngles.y, Vector3.up);

		while(Time.time <= endTime) {
			percent = 1 - (endTime - Time.time) / unequipTime;
			item.position = Vector3.Lerp (item.position, destPos, percent);
			item.rotation = Quaternion.Slerp (item.rotation, destRot, percent);
			yield return new WaitForFixedUpdate();
		}
		itemEquipping = false;
		pA.setFree ();
		itemSpinner.enabled = true;
		try {
			item.SendMessage ("DeactivateItem", SendMessageOptions.RequireReceiver);
		} catch {
			Debug.LogError ("EquipObject: item needs to implement DeactivateItem()");
		}
	}
}
