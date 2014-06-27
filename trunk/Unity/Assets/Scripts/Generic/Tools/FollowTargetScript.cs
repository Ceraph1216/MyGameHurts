using UnityEngine;
using System.Collections;

public class FollowTargetScript : MonoBehaviour 
{
	public Transform target;
	public bool followX;
	public bool followY;
	public bool followZ;

	private Vector3 previousVector;
	private Vector3 deltaVector;

	public Vector3 offset = Vector3.zero;
	
	private Transform myTransform;
	
	void Awake()
	{
		myTransform = transform;
	}
	
	// Use this for initialization
	void Start ()
	{
		Initialize();
	}
	
	void OnEnable ()
	{
		// Set up update event listeners
		SoftPauseScript.instance.SoftUpdate += SoftUpdate;
		SoftPauseScript.instance.SoftPause += SoftUpdate;
		Initialize();
	}
	
	void OnDisable()
	{
		// Make sure the update function is removed from the delegate when the script is not running
		SoftPauseScript.instance.SoftUpdate -= SoftUpdate;
		SoftPauseScript.instance.SoftPause -= SoftUpdate;
	}
	
	public void Initialize() 
	{
		if(target != null)
		{
			previousVector = ( target.position + offset );
		}
	}
	
	// Update is called once per frame
	void SoftUpdate (GameObject dispatcher)
	{
		deltaVector = new Vector3(0,0,0);
		if (followX)
		{
			deltaVector.x = ( target.position.x + offset.x ) - previousVector.x;	
		}
		if (followY)
		{
			deltaVector.y = ( target.position.y + offset.y ) - previousVector.y;	
		}
		if (followZ)
		{
			deltaVector.z = ( target.position.z + offset.z ) - previousVector.z;	
		}
		
		myTransform.Translate(deltaVector);
		
		previousVector = ( target.position + offset );
	}
}