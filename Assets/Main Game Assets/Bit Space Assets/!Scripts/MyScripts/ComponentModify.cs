using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentModify : MonoBehaviour {
	public GameObject TheObject;

	// Use this for initialization
	void Start () {
		TheObject.GetComponent<MeshRenderer> ().enabled = false;
	}
	

}
