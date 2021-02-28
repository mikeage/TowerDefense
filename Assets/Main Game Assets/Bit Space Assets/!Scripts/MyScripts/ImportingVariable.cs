using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportingVariable : MonoBehaviour {

	private int TakenVar;
	public int UpdatedVar;
	void Start () {
		TakenVar = SwitchObjectOnOff.number;
	}
	
	// Update is called once per frame
	void Update () {
		UpdatedVar = TakenVar;
	}
}
