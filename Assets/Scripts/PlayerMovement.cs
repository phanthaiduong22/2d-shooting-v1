using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private LayerMask platformLayerMask;
	[SerializeField] private LayerMask waterLayerMask;

	// public CharacterController2D controller;
	public Animator animator;
	Rigidbody2D rigid;
	SpriteRenderer spriteRenderer;
	private BoxCollider2D boxCollider2D;
	public GameObject bulletPrefab;
	public Transform firePoint;
	public GameObject deathEffect;


	public float moveSpeed = 0f;
	float currentMove;

	public float jumpSpeed = 10f;
	float jumpMove;

	public int health = 100;

	bool isJumping;


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
		if (IsWater())
		{
			Debug.Log("Player is in Water");
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
		}
		if (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded())
		{
			// Debug.Log("Jumping");
			jumpMove = jumpSpeed;
			isJumping = true;
			animator.SetBool("IsJumping", true);
			// rigid.AddForce(new Vector2(currentMove, jumpMove * 50f));

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
			{
				animator.SetBool("IsRunning", true);
			}

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
			{
				animator.SetBool("IsRunning", true);
			}

		}
		else if (Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log("Shooting");
			Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			// animator.SetBool("IsCrouching", true);
		}
		else
		{
			currentMove = 0;
			if (!isJumping)
			{
				animator.SetBool("IsJumping", false);
			}
			animator.SetBool("IsRunning", false);
			animator.SetBool("IsCrouching", false);
		}


	}

	private void FixedUpdate()
	{
		rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

		Debug.Log(currentMove);
		rigid.velocity = new Vector2(currentMove, rigid.velocity.y + jumpMove);
		// rigid.AddForce(new Vector2(currentMove, jumpMove * 50f));
		jumpMove = 0;
	}

	private bool IsGrounded()
	{
		float extraHeightText = .5f;
		RaycastHit2D raycastHit = Physics2D.Raycast(boxCollider2D.bounds.center, Vector2.down, boxCollider2D.bounds.extents.y + extraHeightText, platformLayerMask);

		return raycastHit.collider != null;
	}
	private bool IsWater()
	{

		float extraHeightText = .5f;
		RaycastHit2D raycastHit = Physics2D.Raycast(boxCollider2D.bounds.center, Vector2.down, boxCollider2D.bounds.extents.y + extraHeightText, waterLayerMask);

		return raycastHit.collider != null;
	}
	public void TakeDamage(int damage)
	{
		health -= damage;
		if (health <= 0)
		{
			Die();
		}
	}

	void Die()
	{
		Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log("Enter collider");
		if (IsGrounded())
			isJumping = false;
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		Debug.Log("Exit collider");
		// isJumping = true;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("Enter trigger");
	}

}
