using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

	int health = 300;
	int damage = 40;
	float moveSpeed;
	public float minLeft = -12;
	public float minRight = 12;
	public GameObject deathEffect;
	public GameObject impactEffect;
	SpriteRenderer spriteRenderer;


	float currentMove;
	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.flipX = true;
		switch (GameValues.Difficulty)
		{
			case GameValues.Difficulties.Easy:
				moveSpeed = 0.10f;
				health = 300;
				break;
			case GameValues.Difficulties.Medium:
				moveSpeed = 0.15f;
				health = 400;
				break;
			case GameValues.Difficulties.Hard:
				moveSpeed = 0.20f;
				health = 500;
				break;
		}
	}
	void FixedUpdate()
	{

		EnemyMove();

	}
	void EnemyMove()
	{
		if (transform.position.x <= minLeft * 1f || transform.position.x >= minRight * 1f)
		{
			moveSpeed = -moveSpeed;
			spriteRenderer.flipX = !spriteRenderer.flipX;
		}
		transform.position = transform.position + new Vector3(moveSpeed, 0f, 0f);
	}
	void Die()
	{
		Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D hitInfo)
	{
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

