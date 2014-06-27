using UnityEngine;
using System.Collections;

public class PlayerCollisionScript : MonoBehaviour 
{
	private Rigidbody2D myRigidbody;
	
	void Awake() 
	{
		myRigidbody = rigidbody2D;
	}
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		// Check collision stuff
	}
}
