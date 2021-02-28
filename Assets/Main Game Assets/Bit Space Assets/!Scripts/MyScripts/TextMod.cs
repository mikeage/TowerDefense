using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextMod : MonoBehaviour {

	public GameObject TheTextBox;

	IEnumerator Start () {
		TheTextBox.GetComponent<Text>().text = "W3lc0m3";
		yield return new WaitForSeconds (3);
		TheTextBox.GetComponent<Text>().text = " ";
		yield return new WaitForSeconds (1);
		TheTextBox.GetComponent<Text>().text = "thanks for arriving";
		yield return new WaitForSeconds (3);
		TheTextBox.GetComponent<Text>().text = " ";
		yield return new WaitForSeconds (1);
		TheTextBox.GetComponent<Text>().text = "dont ever go";


	}
	

}
