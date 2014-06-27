using UnityEngine;
using System.Collections;

public class DebugPlayerAnimationScript : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			PlayerStateManager.instance.currentGroundState = Enums.PlayerGroundState.OnGround;
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			PlayerStateManager.instance.currentGroundState = Enums.PlayerGroundState.Rising;
		}

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			PlayerStateManager.instance.currentGroundState = Enums.PlayerGroundState.Landing;
		}

		if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
		{
			PlayerStateManager.instance.isMoving = true;
		}
		else
		{
			PlayerStateManager.instance.isMoving = false;
		}
	}
}
