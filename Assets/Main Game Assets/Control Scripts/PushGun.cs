using UnityEngine;
using System.Collections;

public class PushGun : MonoBehaviour {
	public float pushForce;

	public float pullForce;

	private Camera cam;

	private bool readyToFire;

	// Use this for initialization
	void Start () {
		cam = null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ActivateItem() {
		cam = this.transform.GetComponentInParent<Camera> ();
		readyToFire = true;
	}

	public void DeactivateItem() {
		readyToFire = false;
	}

	public void Fire() {
		if (readyToFire) {
			Ray ray = cam.ScreenPointToRay (new Vector3 (cam.pixelWidth / 2, cam.pixelHeight / 2));
			RaycastHit hit;
			// 
			if (Physics.Raycast (ray, out hit) && hit.transform.GetComponent<Rigidbody> () != null) {
				hit.transform.GetComponent<Rigidbody> ().AddForceAtPosition (this.transform.forward * pushForce, hit.point);
			}
		}
	}

	public void AltFire() {
		if (readyToFire) {
			Ray ray = cam.ScreenPointToRay (new Vector3 (cam.pixelWidth / 2, cam.pixelHeight / 2));
			RaycastHit hit;
			// 
			if (Physics.Raycast (ray, out hit) && hit.transform.GetComponent<Rigidbody> () != null) {
				hit.transform.GetComponent<Rigidbody> ().AddForceAtPosition (-this.transform.forward * pullForce, hit.point);
			}
		}
	}
}
