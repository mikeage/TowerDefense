using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ResetLevelAndDisable : MonoBehaviour {

	public void ResetAndDisable() {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}
}
