using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretStore : MonoBehaviour {

	public Transform turretLocation;
	private GameController GC;
	public GameObject basicTurret;
	public Button basicButton;
	public int basicCost = 10;
	public GameObject bombTurret;
	public Button bombButton;
	public int bombCost = 80;
	public GameObject magicTurret;
	public Button magicButton;
	public int magicCost = 70;
	private bool hasTurret = false;

	// Use this for initialization
	void Start () {
		GC = GameObject.FindObjectOfType<GameController> ();
		basicButton.interactable = false;
		bombButton.interactable = false;
		magicButton.interactable = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!hasTurret) {
			if (GC.CheckGold () >= basicCost) {
				basicButton.interactable = true;
			} else {
				basicButton.interactable = false;
			}
			if (GC.CheckGold () >= bombCost) {
				bombButton.interactable = true;
			}else {
				bombButton.interactable = false;
			}
			if (GC.CheckGold () >= magicCost) {
				magicButton.interactable = true;
			}else {
				magicButton.interactable = false;
			}
		} else {
			this.gameObject.SetActive (false);
		}
	}

	public void MakeBasicTurret()
	{
		if (!hasTurret){
			Instantiate (basicTurret, turretLocation.position, Quaternion.identity);
			hasTurret = true;
			GC.AddGold (basicCost * -1);
		}
	}
	public void MakeBombTurret()
	{
		if (!hasTurret){
			Instantiate (bombTurret, turretLocation.position, Quaternion.identity);
			hasTurret = true;
			GC.AddGold (bombCost * -1);
		}
	}
	public void MakeMagicTurret()
	{
		if (!hasTurret){
			Instantiate (magicTurret, turretLocation.position, Quaternion.identity);
			hasTurret = true;
			GC.AddGold (magicCost * -1);
		}
	}
}
