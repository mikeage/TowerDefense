using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DamagerHUD : MonoBehaviour {
	public GameObject lifeHeartPrefab;
	public LayoutGroup livesContainer;

	public void UpdateLives(float lives) {
		int livesInt = Mathf.FloorToInt(lives);

		List<RectTransform> lifeIcons = new List<RectTransform> ();
		livesContainer.GetComponentsInChildren<RectTransform>(lifeIcons);

		if (livesContainer.GetComponent<RectTransform> () != null) {
			lifeIcons.Remove (livesContainer.GetComponent<RectTransform>());
		}

		int lifeIconsCount = lifeIcons.Count;

		while (lifeIconsCount > livesInt) {
			Destroy(lifeIcons[lifeIconsCount-1].gameObject);

			lifeIconsCount--;
		}

		while (lifeIconsCount < livesInt) {
			GameObject newHeart = Instantiate(lifeHeartPrefab) as GameObject;
			newHeart.transform.SetParent(livesContainer.transform, false);
			lifeIconsCount++;
		}
	}
}
