using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class RaycastReactor : MonoBehaviour {
	public UnityEvent thingsToDo;
	public UnityEvent thingstoUndo;

	public void DoTheThing() {
		thingsToDo.Invoke ();
	}

	public void UndoTheThing() {
		thingstoUndo.Invoke ();
	}
}
