using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
	float moveSpeed = 0.03f;
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
		if (transform.position.x <= -2 || transform.position.x >= 18f)
		{
			moveSpeed = -moveSpeed;
		}
		transform.position = transform.position + new Vector3(moveSpeed, 0f, 0f);
	}
}
