using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

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
	public Transform groundCheck;

	public HealthBar healthBar;
	AudioSource audioSource;


	public float moveSpeed = 0f;
	float currentMove;

	public float jumpSpeed = 10f;
	float jumpMove;

	public int maxHealth = 100;
	public int currentHealth;
	bool isMoving = false;
	bool isJumping;
	bool jump;
	bool isGrounded;
	private Vector3 velocity = Vector3.zero;
	public UnityEvent landingEvent;
	bool isShooting = false;


	void Start()
	{
		currentMove = 0;
		jumpMove = 0;
		currentHealth = maxHealth;

		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();

		rigid = GetComponent<Rigidbody2D>();
		boxCollider2D = transform.GetComponent<BoxCollider2D>();

		// Health Bar
		healthBar.SetMaxHealth(maxHealth);

		// Get audio
		audioSource = GetComponent<AudioSource>();
		if (landingEvent == null)
			landingEvent = new UnityEvent();
		landingEvent.AddListener(OnLanding);
		StartCoroutine("Shoot");
	}
	// Update is called once per frame
	void Update()
	{
		if (IsWater())
		{
			LoadGameOverScreen();
		}

		currentMove = Input.GetAxisRaw("Horizontal") * 40f;
		if (Mathf.Abs(currentMove) > .01f)
		{
			animator.SetBool("IsRunning", true);
			isMoving = true;
		}
		else
        {
			animator.SetBool("IsRunning", false);
			isMoving = false;
        }

		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			animator.SetBool("IsJumping", true);
			jump = true;
		}

		if (Input.GetKey(KeyCode.Space))
		{
			if (!isShooting)
            {
				isShooting = true;
            }
		}
		else
        {
			isShooting = false;
        }

		// Sound moving
		if (isMoving)
		{
			if (!audioSource.isPlaying)
				audioSource.Play();
		}
		else
		{
			audioSource.Stop();
		}


	}

	private IEnumerator Shoot()
    {
		while (true)
        { 
			if (isShooting)
            {
				Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
				FindObjectOfType<AudioManager>().Play("PlayerShooting");
				yield return new WaitForSeconds(.5f);
			}
		}
	}

	private void FixedUpdate()
	{
		/*rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

		rigid.velocity = new Vector2(currentMove, rigid.velocity.y + jumpMove);*/
		// rigid.AddForce(new Vector2(currentMove, jumpMove * 50f));
		Move(currentMove * Time.fixedDeltaTime, jump);
		if (jump)
        {
			jump = false;
			return;
        }
		jump = false;
		bool wasGrounded = isGrounded;
		isGrounded = false;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, .2f, platformLayerMask);
		for (int i = 0; i < colliders.Length; i++)
        {
			if (colliders[i].gameObject != gameObject)
            {
				isGrounded = true;
				if (!wasGrounded)
					landingEvent.Invoke();
            }
        }
		//jumpMove = 0;
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
		currentHealth -= damage;
		healthBar.SetHealth(currentHealth);
		if (currentHealth <= 0)
		{
			Die();
		}
	}

	public void MoveDependOnSlide(Vector3 slideTransform)
	{
		Debug.Log("transform" + slideTransform);

		// transform.position = slideTransform;
	}

	void Die()
	{
		Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
		LoadGameOverScreen();
	}


	void LoadGameOverScreen()
	{
		SceneManager.LoadScene(1);

	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
        /*if (IsGrounded())
        {
            isJumping = false;
        }*/

    }

	private void OnCollisionExit2D(Collision2D collision)
	{
		// isJumping = true;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
	}

	public void Move(float move, bool jump)
    {
		Vector3 targetVelocity = new Vector2(move * 10f, rigid.velocity.y);
		rigid.velocity = Vector3.SmoothDamp(rigid.velocity, targetVelocity, ref velocity, .05f);
		if (move > 0 && spriteRenderer.flipX)
		{
			firePoint.Rotate(0f, 180f, 0f);
			firePoint.position = transform.position + new Vector3(1f, 0.5f, 0f);
			spriteRenderer.flipX = !spriteRenderer.flipX;
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (move < 0 && !spriteRenderer.flipX)
		{
			firePoint.Rotate(0f, 180f, 0f);
			firePoint.position = transform.position + new Vector3(-1f, 0.5f, 0f);
			spriteRenderer.flipX = !spriteRenderer.flipX;
		}
		if (isGrounded && jump)
		{
			isGrounded = false;
			rigid.AddForce(new Vector2(0f, 1100f));
			FindObjectOfType<AudioManager>().Play("PlayerJumping");
		}
	}

	public void OnLanding()
	{
		animator.SetBool("IsJumping", false);
	}
}
