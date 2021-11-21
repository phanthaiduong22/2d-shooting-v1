using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Bird : MonoBehaviour
{
	public AIPath aIPath;
	public GameObject deathEffect;
	public GameObject impactEffect;
	public int health = 300;
	public int damage = 40;

	void Start()
	{
		switch (GameValues.Difficulty)
		{
			case GameValues.Difficulties.Easy:
				// moveSpeed = 0.03f;
				health = 300;
				break;
			case GameValues.Difficulties.Medium:
				// moveSpeed = 0.05f;
				health = 400;
				break;
			case GameValues.Difficulties.Hard:
				// moveSpeed = 0.07f;
				health = 500;
				break;
		}
	}
	// Update is called once per frame
	void Update()
	{
		if (aIPath.desiredVelocity.x >= 0.01f)
		{
			transform.localScale = new Vector3(6f, 6f, 6f);
		}
		else if (aIPath.desiredVelocity.x <= -0.01f)
		{
			transform.localScale = new Vector3(-6f, 6f, 6f);

		}
	}
	void Die()
	{
		Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D hitInfo)
	{
		Debug.Log("Bird touch player");
		Debug.Log(hitInfo.name);
		PlayerMovement playerMovement = hitInfo.GetComponent<PlayerMovement>();
		if (playerMovement != null)
		{
			playerMovement.TakeDamage(damage);
		}

		Instantiate(impactEffect, transform.position, transform.rotation);

		// Destroy(gameObject);
	}
	public void TakeDamage(int damage)
	{
		FindObjectOfType<AudioManager>().Play("BulletTouching");
		health -= damage;
		if (health <= 0)
		{
			Die();
		}
	}
}
