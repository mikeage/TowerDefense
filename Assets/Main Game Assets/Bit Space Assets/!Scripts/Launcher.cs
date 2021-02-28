using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour {
	public bool autoFire = false;
	public KeyCode fireKey = KeyCode.None;

	public GameObject projectile;
	public Transform launchPoint;
	public bool relativeLaunchVelocity;
	public Vector3 launchVelocity;
	public float projectilesPerSecond = 1f;
	public float projectileLifetime = 3f;
	private float _lastFireTime = 0f;

	[Header("If checked, ignore all ammo settings below")]
	public bool infiniteAmmo = true;
	public int startingAmmo = 10;
	public int maxAmmo = 10;
	public int currentAmmo;
	public float ammoRechargeTime = 1f;
	private float _lastAmmoRechargeTime = 0f;


	void Start() {
		currentAmmo = startingAmmo;
	}

	void Update() {
		if (autoFire || Input.GetKey (fireKey))
		{
			Launch();
		}

		if (ammoRechargeTime > 0f && Time.time > _lastAmmoRechargeTime + ammoRechargeTime) {
			this.AddAmmo(1);
		}
	}

	public void Launch() {
		if (currentAmmo > 0f  && Time.time - _lastFireTime >= 1f / projectilesPerSecond)
		{
			_lastFireTime = Time.time;
			currentAmmo -= 1;

			GameObject newGO;
			if (launchPoint != null)
			{
				newGO = GameObject.Instantiate (projectile, launchPoint.position, launchPoint.rotation) as GameObject;
			}
			else
			{
				newGO = GameObject.Instantiate (projectile, this.transform.position, this.transform.rotation) as GameObject;
			}
				
			Rigidbody newRB = newGO.GetComponent<Rigidbody> ();

			if (newRB != null)
			{
				if (relativeLaunchVelocity)
				{
					newRB.AddRelativeForce (launchVelocity, ForceMode.VelocityChange);
				}
				else
				{
					newRB.AddForce (launchVelocity, ForceMode.VelocityChange);
				}
			}
			if (projectileLifetime > 0f) {
				GameObject.Destroy(newGO, projectileLifetime);
			}
		}
	}

	public void AddAmmo(int amount) {
		currentAmmo = Mathf.Min(maxAmmo, currentAmmo + 1);
	}
}
