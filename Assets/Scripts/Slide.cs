using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Slide : MonoBehaviour
{
	float moveSpeed = 0.03f;
	public float maxLeft;
	public float maxRight;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

		SlideMove();

	}
	void SlideMove()
	{
		if (transform.position.x <= maxLeft * 1f || transform.position.x >= maxRight * 1f)
		{
			moveSpeed = -moveSpeed;
		}
		transform.position = transform.position + new Vector3(moveSpeed, 0f, 0f);
	}
	// void OnTriggerEnter2D(Collider2D hitInfo)
	// {
	// 	Debug.Log("Slide Enter collider");
	// PlayerMovement player = hitInfo.GetComponent<PlayerMovement>();
	// 	// if (player != null)
	// 	// {
	// 	// 	player.MoveDependOnSlide(transform.position);
	// 	// }
	// }
	void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log("Slide Enter collider");
		PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
		if (player != null)
		{
			player.MoveDependOnSlide(transform.position);
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		// PlayerMovement playerMovement = hitInfo.GetComponent<PlayerMovement>();

		Debug.Log("Slide Exit collider");
		// isJumping = true;
	}
}
