using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class ObstacleCourseThirdPersonController : MonoBehaviour {
	public BitSpaceUIController BUIC;
	public TimerController TC;

	private ObstacleCourseController gameController;
	private List<Vector3> impulses;
	[SerializeField] private float impulseThreshold = 50f;
	
	void Start () {
		BUIC.ShowStartMenu ();
		TC.ShowTimer ();
		BUIC.ShowStartMenu ();
		impulses = new List<Vector3> ();
	}
	
	void Update () {
		if (impulses.Count > 1) {
			Vector3[] narr = impulses.ToArray();
			for(int i = 0; i < impulses.Count-1; i++) {
				for(int j = i+1; j < impulses.Count; j++) {
					if(Vector3.Dot (narr[i], narr[j]) < -0.1) {
						Debug.Log (Vector3.Dot (narr[i], narr[j]));
					}
					if(Vector3.Dot (narr[i], narr[j]) < -impulseThreshold) {
						TC.StopTimer();
						BUIC.FadeOutLose();
					}
				}
			}
			
		}
		impulses.Clear ();
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

	public void RestartGame() {
		SceneManager.LoadSceneAsync (SceneManager.GetActiveScene ().buildIndex);
	}

	void OnTriggerExit(Collider other) {
		if (other.CompareTag ("Start Trigger")) {
			TC.StartTimer();
		}
	}

	void OnCollisionStay(Collision coll) {
		foreach (ContactPoint contact in coll.contacts) {
			if(!impulses.Contains(contact.normal*coll.impulse.magnitude)) {
				impulses.Add(contact.normal*coll.impulse.magnitude);
			}
//			if(coll.gameObject.CompareTag("Walls")) {
//				if(!impulses.Contains(contact.normal)) {
//					Debug.Log ("Wall pt: "+contact.point+", normal:"+contact.normal+"impulse: "+ coll.impulse+", mag:"+coll.impulse.magnitude+",nor*mag:"+contact.normal*coll.impulse.magnitude);
//					impulses.Add(contact.normal);
//				}
//			} else {
//				if(!impulses.Contains(coll.impulse)) {
//					Debug.Log ("OBSTACLE pt: "+contact.point+", normal:"+contact.normal+"impulse: "+ coll.impulse+", mag:"+coll.impulse.magnitude+",nor*mag:"+contact.normal*coll.impulse.magnitude);
//					impulses.Add(coll.impulse);
//				}
//			}

		}
	}
}
