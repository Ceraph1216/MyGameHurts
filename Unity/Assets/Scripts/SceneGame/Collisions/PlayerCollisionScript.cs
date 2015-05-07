using UnityEngine;
using System.Collections;

public class PlayerCollisionScript : MonoBehaviour 
{
	private Rigidbody2D myRigidbody;
	
	void Awake() 
	{
		myRigidbody = GetComponent<Rigidbody2D>();
	}
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		// Check collision stuff
	}
}
