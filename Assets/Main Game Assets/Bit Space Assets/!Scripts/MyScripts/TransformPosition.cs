using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformPosition : MonoBehaviour {

	public GameObject pointB; 

	void Start (){
		Debug.Log (pointB.transform.position);
		this.transform.position = pointB.transform.position;
	} 
	void Update () {
		transform.Translate (Vector3.forward * 3 * Time.deltaTime, Space.World);
	}

}
