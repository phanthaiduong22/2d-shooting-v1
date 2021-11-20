using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private LayerMask platformLayerMask;
	[SerializeField] private LayerMask waterLayerMask;

	public Animator animator;
	public GameObject bulletPrefab;
	public Transform firePoint;
	public GameObject deathEffect;
	public Transform groundCheck;
	public HealthBar healthBar;
	public float moveSpeed = 10f;
	public int maxHealth = 100;
	public int currentHealth;

	private AudioSource audioSource;
	private Rigidbody2D rigid;
	private SpriteRenderer spriteRenderer;
	private BoxCollider2D boxCollider2D;
	private float currentMove;
	private bool isMoving = false;
	private bool jump;
	private bool isGrounded;
	private Vector3 velocity = Vector3.zero;
	private bool isShooting = false;


	void Start()
	{
		currentMove = 0;
		currentHealth = maxHealth;

		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();

		rigid = GetComponent<Rigidbody2D>();
		boxCollider2D = transform.GetComponent<BoxCollider2D>();

		// Health Bar
		healthBar.SetMaxHealth(maxHealth);

		// Get audio
		audioSource = GetComponent<AudioSource>();

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
				animator.SetBool("IsShooting", true);
				isShooting = true;
            }
		}
		else
        {
			isShooting = false;
			animator.SetBool("IsShooting", false);
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
				yield return new WaitForSeconds(.4f);
			}
			else
				yield return new WaitForEndOfFrame();
		}
	}

	private void FixedUpdate()
	{
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
					animator.SetBool("IsJumping", false);
            }
        }
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

	private void Move(float move, bool jump)
    {
		Vector3 targetVelocity = new Vector2(move * moveSpeed, rigid.velocity.y);
		rigid.velocity = Vector3.SmoothDamp(rigid.velocity, targetVelocity, ref velocity, .05f);
		if (move > 0 && spriteRenderer.flipX)
		{
			firePoint.Rotate(0f, 180f, 0f);
			firePoint.position = transform.position + new Vector3(1f, 0.5f, 0f);
			spriteRenderer.flipX = !spriteRenderer.flipX;
		}
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

}
