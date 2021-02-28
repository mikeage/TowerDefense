using UnityEngine;
using System.Collections;

public class ItemRotator : MonoBehaviour {

	public float xDegreesPerSecond;
	public float yDegreesPerSecond;
	public float zDegreesPerSecond;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		this.transform.Rotate (xDegreesPerSecond * Time.deltaTime, yDegreesPerSecond * Time.deltaTime, zDegreesPerSecond * Time.deltaTime);
	}
}
