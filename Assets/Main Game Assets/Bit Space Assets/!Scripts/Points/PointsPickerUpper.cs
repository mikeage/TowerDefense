using UnityEngine;
using System.Collections;

public class PointsPickerUpper : MonoBehaviour {
	private ScoreKeeper _pointsHUD;
	void Awake() {
		_pointsHUD = GameObject.FindObjectOfType<ScoreKeeper> ();
		if (_pointsHUD == null) {
			Debug.LogWarning ("You have a PointsPickerUpper and no ScoreKeeper!");
		}
	}

	void OnTriggerEnter(Collider coll) {
		PointsPickup pu = coll.GetComponent<PointsPickup> ();
		if (pu != null) {
			GameObject.Destroy (pu.gameObject);
			_pointsHUD.AddPoints (pu.points);
		}
	}
}
