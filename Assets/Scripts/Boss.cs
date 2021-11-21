using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
	public int health = 500;
	public int damage = 40;
	public GameObject bulletPrefab;
	public Transform firePoint;
	public Transform firePoint1;
	public GameObject deathEffect;
	public GameObject impactEffect;
	SpriteRenderer spriteRenderer;
	public float fireRate = 10f;
	private float nextFire = 0.0f;

	private int rotatetimes = 6;
	private int reverse = 1;
	public float maxLeft;
	public float maxRight;
	float moveSpeed;


	// Start is called before the first frame update
	void Start()
	{
		firePoint.Rotate(0f, 0f, -10f);
		firePoint1.Rotate(0f, 180f, -10f);
		switch (GameValues.Difficulty)
		{
			case GameValues.Difficulties.Easy:
				moveSpeed = 0.05f;
				health = 500;
				break;
			case GameValues.Difficulties.Medium:
				moveSpeed = 0.08f;
				health = 700;
				break;
			case GameValues.Difficulties.Hard:
				moveSpeed = 0.10f;
				health = 1000;
				break;
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		SlideMove();
		if (Time.time > nextFire)
		{
			if (!bulletPrefab)
				return;
			nextFire = Time.time + fireRate;
			Shooting();
		}
	}

	void SlideMove()
	{
		if (transform.position.x <= maxLeft * 1f || transform.position.x >= maxRight * 1f)
		{
			moveSpeed = -moveSpeed;
		}
		transform.position = transform.position + new Vector3(moveSpeed, 0f, 0f);
	}
	void Shooting()
	{
		if (rotatetimes <= 0)
		{
			reverse = -reverse;
			rotatetimes = 6;
		}
		firePoint.Rotate(0f, 0f, reverse * 10f);
		Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		firePoint1.Rotate(0f, 0f, reverse * 10f);
		Instantiate(bulletPrefab, firePoint1.position, firePoint1.rotation);
		rotatetimes -= 1;
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
