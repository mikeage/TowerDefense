using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject[] wave1;
	public GameObject[] wave2;
	public GameObject[] wave3;
	public GameObject[] wave4;
	public GameObject[] wave5;
	public int startWait = 20;
	public int waveWait = 60;
	public int currentWave = 0;
	public int livingEnemies = 0;
	public Transform spawnPoint;
	private GameController GC;
	private bool didWeStart = false;

	// Use this for initialization
	void Start () {
		GC = GetComponent<GameController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (GC.shouldStart && !didWeStart) {
			didWeStart = true;
			StartCoroutine (WaveMaker ());
		}
	}

	public IEnumerator WaveMaker()
	{
		
		yield return new WaitForSeconds (startWait);
		currentWave = 1;
		for (int i = 0; i < wave1.Length; i++) {
			GameObject enemy = Instantiate (wave1 [i], spawnPoint.position, Quaternion.identity) as GameObject;
			livingEnemies++;
		}
		yield return new WaitForSeconds (waveWait);
		currentWave = 2;
		for (int i = 0; i < wave2.Length; i++) {
			GameObject enemy = Instantiate (wave2 [i], spawnPoint.position, Quaternion.identity) as GameObject;
			livingEnemies++;
		}
		yield return new WaitForSeconds (waveWait);
		currentWave = 3;
		for (int i = 0; i < wave3.Length; i++) {
			GameObject enemy = Instantiate (wave3 [i], spawnPoint.position, Quaternion.identity) as GameObject;
			livingEnemies++;
		}
		yield return new WaitForSeconds (waveWait);
		currentWave = 4;
		for (int i = 0; i < wave4.Length; i++) {
			GameObject enemy = Instantiate (wave4 [i], spawnPoint.position, Quaternion.identity) as GameObject;
			livingEnemies++;
		}
		yield return new WaitForSeconds (waveWait);
		currentWave = 5;
		for (int i = 0; i < wave5.Length; i++) {
			GameObject enemy = Instantiate (wave5 [i], spawnPoint.position, Quaternion.identity) as GameObject;
			livingEnemies++;
		}
			
	}

	public void EnemyDied()
	{
		livingEnemies--;
	}
}
