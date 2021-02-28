using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public int currentGold = 0;
	public Damager myHealth;
	private EnemySpawner spawner;
	public Text goldText;
	public Text healthText;
	private StartWinLoseRestart swlr;
	public bool didDie = false;
	private bool didWin = false;
	public bool shouldStart = false;

	// Use this for initialization
	void Start () {
		spawner = GetComponent<EnemySpawner> ();
		swlr = GetComponent<StartWinLoseRestart> ();
		currentGold = 15;
	}
	
	// Update is called once per frame
	void Update () {
		goldText.text = currentGold.ToString();
		healthText.text = myHealth.currentHealth.ToString ();
		if (myHealth.currentHealth <= 0 && !didDie && !didWin) {
			swlr.TriggerLose ();
			didDie = true;
		} else {
			if (CheckWin () && !didWin) {
				swlr.TriggerWin ();
				didWin = true;
			}
		}
	}

	public void AddGold(int goldAmt){
		currentGold += goldAmt;
	}

	public int CheckGold(){
		return currentGold;
	}
	public bool CheckWin()
	{
		Enemy[] liveEnemies = GameObject.FindObjectsOfType<Enemy> ();
		if (spawner.currentWave >= 5 && liveEnemies.Length<=1){//spawner.livingEnemies <= 1) {
			return true;
		} else {
			return false;
		}
	}

	public void LetUsBegin()
	{
		if (!shouldStart) {
			shouldStart = true;
		}
	}
}
