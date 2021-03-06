using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

	public float basicSpeed = 20f;
	public int basicDamage = 40;
	public Rigidbody2D rb;
	public GameObject impactEffect;
	public int buff = 0;

	// Use this for initialization
	void Start()
	{
		float speed = basicSpeed + buff * 10f;
		rb.velocity = transform.right * speed;
	}

	void OnTriggerEnter2D(Collider2D hitInfo)
	{
		int damage = basicDamage + buff * 10;
		Boss boss = hitInfo.GetComponent<Boss>();
		if (boss != null)
		{
			boss.TakeDamage(damage);
		}
		Enemy enemy = hitInfo.GetComponent<Enemy>();
		if (enemy != null)
		{
			enemy.TakeDamage(damage);
		}
		Bird bird = hitInfo.GetComponent<Bird>();

		if (bird != null)
		{
			bird.TakeDamage(damage);
		}
		PlayerMovement player = hitInfo.GetComponent<PlayerMovement>();
		if (player != null)
		{
			player.TakeDamage(damage);
		}
		Instantiate(impactEffect, transform.position, transform.rotation);

		Destroy(gameObject);
	}

	public void Buff(int n)
	{
		buff = n;
	}

}
