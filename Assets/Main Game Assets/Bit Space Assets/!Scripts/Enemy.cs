using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private GameController GC;
	private EnemySpawner sponer;
	public int goldValue = 1;

	// Use this for initialization
	void Start () {
		GC = GameObject.FindObjectOfType<GameController> ();
		sponer = GameObject.FindObjectOfType<EnemySpawner> ();
	}
	
	public void iDiedFrownyFace(){
		GC.AddGold (goldValue);
		sponer.EnemyDied ();
	}
}
