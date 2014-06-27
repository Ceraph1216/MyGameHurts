using UnityEngine;
using System.Collections;

public class StayAtScreenPoint : MonoBehaviour 
{
	Vector3 myScreenPoint;
	Transform myTransform;
	Camera camera;

	void Start () 
	{
		myTransform = transform;
		camera = GameObject.FindGameObjectWithTag("MainCamera").camera;
		myScreenPoint = camera.WorldToScreenPoint(myTransform.position);
	}

	void OnEnable()
	{
		SoftPauseScript.instance.SoftUpdate += SoftUpdate;
	}
	
	void OnDisable()
	{
		SoftPauseScript.instance.SoftUpdate -= SoftUpdate;
	}
	
	// Update is called once per frame
	void SoftUpdate(GameObject dispatcher) 
	{
		Vector3 correctWorldPoint = camera.ScreenToWorldPoint(myScreenPoint);

		myTransform.position = correctWorldPoint;
	}
}