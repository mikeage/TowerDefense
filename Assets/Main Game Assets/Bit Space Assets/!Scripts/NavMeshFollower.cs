using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshFollower : MonoBehaviour {
	public Transform thingToFollow;
	private GameController GC;

	// Use this for initialization
	void Start () {
		thingToFollow = GameObject.FindGameObjectWithTag ("Player").transform;
		GC = GameObject.FindObjectOfType<GameController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!GC.didDie) {
			this.GetComponent<NavMeshAgent> ().SetDestination (thingToFollow.position);
		}
	}
}
