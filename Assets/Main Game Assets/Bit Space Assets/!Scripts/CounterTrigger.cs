using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CounterTrigger : MonoBehaviour {
	public int targetNumber = 3;
	private int _number;
	public Text optionalDisplay;
	public UnityEvent successEvents;

	public void Start() {
		_number = 0;
		CheckNumber();
	}

	public void AddOne() {
		_number++;
		CheckNumber ();
	}

	public void SubtractOne() {
		_number--;
		CheckNumber ();
	}

	public void AddNumber(int val) {
		_number += val;
		CheckNumber ();
	}

	private void CheckNumber() {
		if (optionalDisplay != null) {
			optionalDisplay.text = _number.ToString();
		}

		if (_number >= targetNumber) {
			successEvents.Invoke ();
		}
	}
}
