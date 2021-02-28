using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshFollowerController : MonoBehaviour {
	public Transform thingToFollow;
	private List<NavMeshAgent> agents = new List<NavMeshAgent> ();

	void Awake() {
		agents = new List<NavMeshAgent> (GameObject.FindObjectsOfType<NavMeshAgent> ());
	}

	// Update is called once per frame
	void Update () {
		foreach (NavMeshAgent agent in agents) {
			agent.SetDestination (thingToFollow.position);
		}
	}
}
