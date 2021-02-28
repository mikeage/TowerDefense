using UnityEngine;
using System.Collections;

public class GenericGun : MonoBehaviour {
	public KeyCode fireKey = KeyCode.Mouse0;

	public GameObject projectilePrefab;
	public Transform launchPoint;
	public bool autoFire = false;
	[Tooltip("Projectile always fires along local Z+ direction of Launch Point")]
	public float launchVelocity;

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

			// ignore removing ammo if it's infinite
			if(!infiniteAmmo)
				currentAmmo -= 1;

			GameObject newGO;
			if (launchPoint != null)
			{
				newGO = GameObject.Instantiate (projectilePrefab, launchPoint.position, launchPoint.rotation) as GameObject;
			}
			else
			{
				newGO = GameObject.Instantiate (projectilePrefab, this.transform.position, this.transform.rotation) as GameObject;
			}

			Rigidbody newRB = newGO.GetComponent<Rigidbody> ();

			if (newRB != null)
			{
				newRB.AddRelativeForce (Vector3.forward * launchVelocity, ForceMode.VelocityChange);
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
