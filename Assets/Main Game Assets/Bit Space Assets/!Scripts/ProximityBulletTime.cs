using UnityEngine;
using System.Collections;

public class ProximityBulletTime : MonoBehaviour
{
	public float minTimeScale = 0.1f;
	public float maxTimeScale = 1.0f;

	public LayerMask objectLayer;
	public float checkRange = 1f;

	private Collider[] nearbyObjects;
	private float minDistance, tempDistance;

	// Use this for initialization
	void Start ()
	{
		nearbyObjects = new Collider[50];
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		int nearbyCount = Physics.OverlapSphereNonAlloc (this.transform.position, checkRange, nearbyObjects, objectLayer);
		minDistance = checkRange;
		for (int i = 0; i < nearbyCount; i++) {
			tempDistance = Vector3.SqrMagnitude (nearbyObjects [i].transform.position - this.transform.position);
			if (tempDistance < minDistance) {
				minDistance = tempDistance;
			}
		}

		Time.timeScale = minTimeScale + (maxTimeScale - minTimeScale) * (minDistance / checkRange);
//		Debug.Log (Time.timeScale);
	}
}
