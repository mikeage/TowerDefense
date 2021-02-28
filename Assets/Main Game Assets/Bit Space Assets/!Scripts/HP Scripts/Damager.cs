using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Damager : MonoBehaviour
{
	public enum DamageRole
	{
		Player,
		Enemy,
		Other

	}

	public DamageRole myRole = DamageRole.Other;

	[Header("Displays life hearts (optional)")]
	public DamagerHUD livesDisplay;

	[Header("Limit what kinds of Colliders I can hit (Optional)")]
	public bool ignoreTriggers = false;
	public bool ignoreCollisions = false;
	public bool ignoreCharacterControllers = false;

	[Header("How much damage I deal to others")]
	public float damagePlayer = 0f;
	public float damageEnemy = 0f;
	public float damageOther = 0f;

	[Header("How much damage I can take")]
	public float maxHealth = 1f;
	private float _curHealth;
	public float startingHealth = 1f;

	public float currentHealth { get { return _curHealth; } }

	[Header("What to call when I take damage")]
	public UnityEvent damageEvents;

	[Header("Invincibility time after taking a hit")]
	public float invincTime = 0f;
	private bool _invincible = false;
	private float blinkTime = 0.1f;
	public Material invincibleMaterial;

	[Header("Destroy this GameObject when I die")]
	public bool destroyUponDeath = true;

	[Header("Destroy when I hit *anything*")]
	public bool destroyUponHit = false;

	[Header("Create this prefab when I die")]
	public GameObject spawnWhenDestroyed;

	[Header("What to call when I die")]
	public UnityEvent deathEvents;

	//	private WaveGroup _owner;

	void Awake()
	{
		_curHealth = startingHealth;

		if (livesDisplay == null && this.GetComponent<DamagerHUD>() != null) {
			this.livesDisplay = this.GetComponent<DamagerHUD>();
		}
		if (this.livesDisplay != null) {
			this.livesDisplay.UpdateLives(_curHealth);
		}

	}

	public virtual void Die()
	{
		if (spawnWhenDestroyed != null) {
			GameObject.Instantiate(spawnWhenDestroyed, this.transform.position, this.transform.rotation);
		}

		deathEvents.Invoke();

		if (this.destroyUponDeath) {
			Destroy(this.gameObject);
		}
	}

	public virtual void OnCollisionEnter(Collision cdata)
	{
		if (ignoreCollisions) {
			return;
		}

		Damager i = cdata.collider.GetComponentInParent<Damager>();
		if (i != null) {
			HandleInteraction(i);
		}
		if (destroyUponHit) {
			this.Die();
		}
	}

	public virtual void OnTriggerEnter(Collider coll)
	{
		if (ignoreTriggers) {
			return;
		}

		Damager i = coll.GetComponentInParent<Damager>();
		if (i != null) {
			HandleInteraction(i);
		}
	}

	public virtual void OnControllerColliderHit(ControllerColliderHit hit) {
		if (ignoreCharacterControllers) {
			return;
		}

		Damager i = hit.collider.GetComponentInParent<Damager>();

		if(i != null) {
			HandleInteraction(i);
		}

		if (destroyUponHit) {
			this.Die();
		}
	}


	protected virtual void HandleInteraction(Damager other)
	{
		switch (other.myRole) {
			case DamageRole.Player:
				if (damagePlayer > 0f) {
					other.TakeDamage(damagePlayer);
				}
				break;

			case DamageRole.Enemy:
				if (damageEnemy > 0f) {
					other.TakeDamage(damageEnemy);
				}
				break;

			case DamageRole.Other:
				if (damageOther > 0f) {
					other.TakeDamage(damageOther);
				}
				break;

			default:
				break;
		}
	}

	public virtual void TakeDamage(float damage)
	{

		if (!_invincible) {

			this._curHealth -= damage;

			if (livesDisplay != null) {
				livesDisplay.UpdateLives(currentHealth);
			}

			damageEvents.Invoke();

			if (this._curHealth <= 0f) {
				this.Die();
			}

			if (this.invincTime > 0f) {
				StartCoroutine(InvincibilityCoroutine(Time.fixedTime + invincTime));
			}
		}
	}

	public virtual void RecoverHealth(float health)
	{
		this._curHealth = Mathf.Min(this._curHealth + health, this.maxHealth);

		if (livesDisplay != null) {
			livesDisplay.UpdateLives(currentHealth);
		}
	}

	private IEnumerator InvincibilityCoroutine(float endTime)
	{
		bool meshOn = false;
		MeshRenderer[] renderers = this.GetComponentsInChildren<MeshRenderer>();
		_invincible = true;

		if (invincibleMaterial != null) {
			// we have a cool color-blinking thing going on
			Dictionary<MeshRenderer, Material> prevMaterial = new Dictionary<MeshRenderer, Material>();
			foreach (MeshRenderer mr in renderers) {
				prevMaterial[mr] = mr.material;
			}
			while (Time.fixedTime < endTime) {
				foreach (MeshRenderer mr in renderers) {
					mr.material = meshOn ? prevMaterial[mr] : invincibleMaterial;
				}
				meshOn = !meshOn;
				yield return new WaitForSeconds(blinkTime);
			}
			foreach (MeshRenderer mr in renderers) {
				mr.material = prevMaterial[mr];
			}

		} else {
			// just blink!
			while (Time.fixedTime < endTime) {
				foreach (MeshRenderer mr in renderers) {
					mr.enabled = meshOn;
				}
				meshOn = !meshOn;
				yield return new WaitForSeconds(blinkTime);
			}
			foreach (MeshRenderer mr in renderers) {
				mr.enabled = true;
			}
		}
		_invincible = false;
	}
}
