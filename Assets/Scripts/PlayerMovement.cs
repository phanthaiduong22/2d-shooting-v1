using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private LayerMask platformLayerMask;

	// public CharacterController2D controller;
	public Animator animator;
	Rigidbody2D rigid;
	SpriteRenderer spriteRenderer;
	private BoxCollider2D boxCollider2D;
	public GameObject bulletPrefab;
	public Transform firePoint;


	public float moveSpeed = 10f;
	float currentMove;

	public float jumpSpeed = 20f;
	float jumpMove;

	void Start()
	{
		currentMove = 0;
		jumpMove = 0;

		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();

		rigid = GetComponent<Rigidbody2D>();
		boxCollider2D = transform.GetComponent<BoxCollider2D>();
	}
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded())
		{
			Debug.Log("Jumping");
			jumpMove = jumpSpeed;
			animator.SetBool("IsJumping", true);
		}
		else if (Input.GetKey(KeyCode.LeftArrow))
		{
			Debug.Log("Moving Left");
			currentMove = -moveSpeed;

			// Rotate Fire Point
			if (spriteRenderer.flipX == false)
			{
				firePoint.Rotate(0f, 180f, 0f);
				firePoint.position = transform.position + new Vector3(-1f, 0.5f, 0f);
			}

			// Rotate left
			spriteRenderer.flipX = true;

			// Call animation running
			if (IsGrounded())
				animator.SetBool("isRunning", true);

		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			Debug.Log("Moving Right");
			currentMove = moveSpeed;

			// Rotate Fire Point
			if (spriteRenderer.flipX == true)
			{
				firePoint.Rotate(0f, 180f, 0f);
				firePoint.position = transform.position + new Vector3(1f, 0.5f, 0f);
			}

			// Rotate left
			spriteRenderer.flipX = false;

			// Call animation running
			if (IsGrounded())
				animator.SetBool("isRunning", true);


		}
		else if (Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log("Shooting");
			Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
			// animator.SetBool("IsCrouching", true);
		}
		else
		{

			currentMove = 0;
			animator.SetBool("IsJumping", false);
			animator.SetBool("isRunning", false);
			animator.SetBool("IsCrouching", false);
		}


	}

	private void FixedUpdate()
	{
		rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

		rigid.velocity = new Vector2(currentMove, rigid.velocity.y + jumpMove);
		// rigid.AddForce(new Vector2(currentMove, jumpMove * 50f));

		jumpMove = 0;
	}

	private bool IsGrounded()
	{
		// Debug.Log("Player is Grounded");

		float extraHeightText = .5f;
		RaycastHit2D raycastHit = Physics2D.Raycast(boxCollider2D.bounds.center, Vector2.down, boxCollider2D.bounds.extents.y + extraHeightText, platformLayerMask);
		Color rayColor;
		if (raycastHit.collider != null)
		{
			rayColor = Color.green;
		}
		else
		{
			rayColor = Color.red;
		}
		Debug.DrawRay(boxCollider2D.bounds.center, Vector2.down * (boxCollider2D.bounds.extents.y + extraHeightText), rayColor);

		return raycastHit.collider != null;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.tag.Equals("Ground"))
			Debug.Log("Enter collider");
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		Debug.Log("Exit collider");
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("Enter trigger");
	}

}
