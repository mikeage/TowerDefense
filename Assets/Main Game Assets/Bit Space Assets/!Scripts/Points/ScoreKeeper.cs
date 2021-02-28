using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreKeeper : MonoBehaviour
{
	private int _currentPoints;
	public int currentPoints { get { return _currentPoints; } }

	static private int HighScore = 0;
	public int startingPoints = 0;

	[Header("If checked, points can go below zero")]
	public bool canGoNegative = false;

	[Header("Text to put before and after point value")]
	public string pointsPrefix = "";
	public string pointsSuffix = "";

	[Header("UI Text components to show current and high score (optional)")]
	public Text ScoreText;
	public Text HighScoreText;

	// Use this for initialization
	void Start ()
	{
		_currentPoints = startingPoints;
		this.UpdatePointText();
	}

	public void AddPoints(int pts) {
		_currentPoints += pts;
		UpdatePointText ();
	}

	public void SubtractPoints(int pts) {
		if (canGoNegative) {
			_currentPoints -= pts;
		} else {
			_currentPoints = Mathf.Max(0, _currentPoints - pts);
		}
		UpdatePointText ();
	}

	public void SetPoints(int pts) {
		if (canGoNegative) {
			_currentPoints = pts;
		} else {
			_currentPoints = Mathf.Max(0, pts);
		}
		UpdatePointText ();
	}

	private void UpdatePointText() {
		if (ScoreText != null)
			ScoreText.text = pointsPrefix + _currentPoints.ToString () + pointsSuffix;
		if (_currentPoints > HighScore) {
			HighScore = _currentPoints;
			if (HighScoreText != null) {
				HighScoreText.text = pointsPrefix + _currentPoints.ToString () + pointsSuffix;
			}
		}
	}
}
