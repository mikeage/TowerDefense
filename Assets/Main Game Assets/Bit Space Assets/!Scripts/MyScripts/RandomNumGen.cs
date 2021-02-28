using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNumGen : MonoBehaviour {
	public int CheckOne;
	public int CheckTwo;
	public AudioSource PopSound;

	void Start () {
		CheckOne = Random.Range (1, 4);
		CheckTwo = Random.Range (1, 2);

		if (CheckOne == 2) {
			if (CheckTwo == 1) {
				StartCoroutine (TwoPop ());
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator TwoPop(){
		PopSound.Play ();
		yield return new WaitForSeconds (3);
		PopSound.Play ();
	}
}
