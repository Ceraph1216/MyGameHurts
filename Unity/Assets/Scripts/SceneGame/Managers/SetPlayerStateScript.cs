using UnityEngine;
using System.Collections;

public class SetPlayerStateScript : MonoBehaviour 
{
	public tk2dSpriteAnimator myAnimator;

	private InputType _currentInput;
	public InputType currentInput
	{
		get 
		{
			return _currentInput;
		}
		private set 
		{
			_currentInput = value;
		}
	}

	private AttackInfo _currentAttack;
	public AttackInfo currentAttack
	{
		get 
		{
			return _currentAttack;
		}
		private set 
		{
			_currentAttack = value;
		}
	}

	private InputType _storedInput;

	private Transform myTransform;
	private AttackList _attackList;
	private BasicPlayerMovementScript _playerMovement;

	void Awake()
	{
		myTransform = transform;
		_attackList = GetComponent<AttackList> ();
		_playerMovement = GetComponent<BasicPlayerMovementScript> ();
	}

	void OnEnable()
	{
		Initialize();
		SoftPauseScript.instance.AddToHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);
	}

	public void Initialize() 
	{
		// Set up event listeners for state changes
		PlayerStateManager.instance.ChangeGroundState += ChangeGroundState;
		PlayerStateManager.instance.ChangeAttackState += ChangeAttackState;
		PlayerStateManager.instance.ChangeStunnedState += ChangeStunnedState;
		PlayerStateManager.instance.ChangeMoving += ChangeMoving;

		myAnimator.AnimationCompleted = OnAnimationComplete;
	}

	void OnDisable()
	{
		SoftPauseScript.instance.RemoveFromHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);

		PlayerStateManager.instance.ChangeGroundState -= ChangeGroundState;
		PlayerStateManager.instance.ChangeAttackState -= ChangeAttackState;
		PlayerStateManager.instance.ChangeStunnedState -= ChangeStunnedState;
		PlayerStateManager.instance.ChangeMoving -= ChangeMoving;
	}

	void SoftUpdate(GameObject dispatcher)
	{
		DetermineInput ();
		ExecuteInput (currentInput);
		UpdateInputWindow ();
        ProcessAttack();
	}

	private void ChangeGroundState(GameObject dispatcher)
	{
		switch (PlayerStateManager.instance.currentGroundState)
		{
			case Enums.PlayerGroundState.OnGround:
			{
				if (PlayerStateManager.instance.isMoving)
				{
					myAnimator.Play("run");
				}
				else if (PlayerStateManager.instance.currentAttackState == Enums.PlayerAttackState.None)
				{
					myAnimator.Play("idle");
				}
				break;
			}
			case Enums.PlayerGroundState.Rising:
			{
				myAnimator.Play("jump");
				break;
			}
			case Enums.PlayerGroundState.Falling:
			{
				myAnimator.Play("fall");
				break;
			}
			case Enums.PlayerGroundState.Landing:
			{
				if (PlayerStateManager.instance.currentAttackState == Enums.PlayerAttackState.None)
				{
					myAnimator.Play("land");
				}
				break;
			}
		}
	}

	private void ChangeAttackState(GameObject dispatcher)
	{
		if (PlayerStateManager.instance.currentGroundState == Enums.PlayerGroundState.Rising || 
		    PlayerStateManager.instance.currentGroundState == Enums.PlayerGroundState.Falling)
		{
			switch (PlayerStateManager.instance.currentAttackState)
			{
				// Do air attacks
			}
		}
		else
		{
			switch (PlayerStateManager.instance.currentAttackState)
			{
				// Do ground attacks
			}
		}
	}

	private void ChangeStunnedState(GameObject dispatcher)
	{
		switch (PlayerStateManager.instance.currentStunnedState)
		{
			case Enums.PlayerStunnedState.Hit:
			{
				myAnimator.Play("hit");
				break;
			}
		}
	}

	private void ChangeMoving(GameObject dispatcher)
	{
		if (PlayerStateManager.instance.currentAttackState != Enums.PlayerAttackState.None) 
		{
			return;
		}

		if (PlayerStateManager.instance.isMoving)
		{
			if (PlayerStateManager.instance.currentGroundState == Enums.PlayerGroundState.OnGround)
			{
				myAnimator.Play("run");
				currentAttack = null;
			}
		}
		else
		{
			if (PlayerStateManager.instance.currentGroundState == Enums.PlayerGroundState.OnGround)
			{
				myAnimator.Play("idle");
			}
		}
	}

	private void OnAnimationComplete(tk2dSpriteAnimator anim, tk2dSpriteAnimationClip clip)
	{
		// Take care of animation end

		// go to transition. Check if we have a stored input first
		if (clip.name.Contains ("Attack") && !clip.name.Contains ("_end")) 
		{
			if (_storedInput != InputType.None && ExecuteInput (_storedInput)) 
			{
				_storedInput = InputType.None;
			} 
			else 
			{
				string l_attackEndName = clip.name + "_end";
				myAnimator.Play (l_attackEndName);
				PlayerStateManager.instance.currentAttackState = Enums.PlayerAttackState.None;
			}

		}

		// transition over, go to idle
		if (clip.name.Contains ("Attack") && clip.name.Contains ("_end")) 
		{
			myAnimator.Play ("idle");
			_currentAttack = null;
		}
	}

	private void UpdateInputWindow ()
	{
		if (PlayerStateManager.instance.currentAttackState != Enums.PlayerAttackState.Attacking) 
		{
			return;
		}

		if (myAnimator.CurrentClip.name.Contains ("Attack") && !myAnimator.CurrentClip.name.Contains ("_end")) 
		{
			if (myAnimator.CurrentFrame >= currentAttack.inputStartFrame) 
			{
				PlayerStateManager.instance.currentAttackState = Enums.PlayerAttackState.InputOpen;
			}
		}
	}

	private void DetermineInput ()
	{
		currentInput = InputType.None;

		InputType l_receivedInput = InputType.None;

		if (InputManager.instance.GetLightAttack ()) 
		{
			l_receivedInput = InputType.LightAttack;
		} 
		else if (InputManager.instance.GetHeavyAttack ()) 
		{
			l_receivedInput = InputType.HeavyAttack;
		} 
		else if (InputManager.instance.GetMovement () < 0) // Moving left
		{ 
			l_receivedInput = InputType.MoveLeft;
		} 
		else if (InputManager.instance.GetMovement () > 0) // Moving Right
		{ 
			l_receivedInput = InputType.MoveRight;
		} 

		if (l_receivedInput == InputType.None) 
		{
			return;
		}

		if (PlayerStateManager.instance.currentAttackState == Enums.PlayerAttackState.None) 
		{
			currentInput = l_receivedInput;
		}

		if (PlayerStateManager.instance.currentAttackState == Enums.PlayerAttackState.InputOpen) 
		{
			// we only care about storing attacks. Storing movements causes the combo to be interrupted
			if (l_receivedInput != InputType.MoveLeft && l_receivedInput != InputType.MoveRight) 
			{
				_storedInput = l_receivedInput;
			}
		}
	}

	private bool ExecuteInput (InputType p_input)
	{
		if (p_input == InputType.LightAttack || p_input == InputType.HeavyAttack)
		{
			return Attack(p_input);
		}

		return false;
	}

	private bool Attack(InputType p_input)
	{
		AttackInfo l_newAttack = _attackList.GetAttackForInput (p_input, _currentAttack);

		if (l_newAttack == null) 
		{
			return false;
		}

		_playerMovement.Move (new Vector3 (0, 0, 0));
		_currentAttack = l_newAttack;
		PlayerStateManager.instance.currentAttackState = Enums.PlayerAttackState.Attacking;
		myAnimator.Play (_currentAttack.animationName);

		return true;
	}

	private void ProcessAttack ()
	{
        if (!isAttacking())
        {
            return;
        }

        float l_attackCompletePercentage = (float)myAnimator.CurrentFrame / (float)myAnimator.CurrentClip.frames.Length;
        Vector3 l_movementVector = Vector3.zero;
        l_movementVector.x = currentAttack.horizontalMovementCurve.Evaluate(l_attackCompletePercentage);
        l_movementVector.y = currentAttack.verticalMovementCurve.Evaluate(l_attackCompletePercentage);

        Debug.Log("attack movement vector: " + l_movementVector);

        _playerMovement.Move(l_movementVector);
    }

    private bool isAttacking()
    {
        return PlayerStateManager.instance.currentAttackState != Enums.PlayerAttackState.None;
    }
}