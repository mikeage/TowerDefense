using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchObjectOnOff : MonoBehaviour {

	public GameObject ObjectToAffect;

	public static bool OnOrOff = true;
	public static int number = 3;


	void OnTriggerEnter(Collider col){
		OnOrOff = !OnOrOff;
		ObjectToAffect.SetActive (OnOrOff);
	}


}
