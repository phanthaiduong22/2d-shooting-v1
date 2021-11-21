using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
	[SerializeField] private LayerMask platformLayerMask;

	float speed = 10f;
	int damage = 20;
	public Rigidbody2D rb;
	public GameObject impactEffect;
	public Transform groundCheck;

	// Use this for initialization
	void Start()
	{
		rb.velocity = transform.right * speed;
	}

	void OnTriggerEnter2D(Collider2D hitInfo)
	{
		PlayerMovement player = hitInfo.GetComponent<PlayerMovement>();
		if (player != null)
		{
			player.TakeDamage(damage);

			Instantiate(impactEffect, transform.position, transform.rotation);

			Destroy(gameObject);
		}

	}
	void FixedUpdate()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, .2f, platformLayerMask);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				Instantiate(impactEffect, transform.position, transform.rotation);

				Destroy(gameObject);
			}
		}
	}
}
