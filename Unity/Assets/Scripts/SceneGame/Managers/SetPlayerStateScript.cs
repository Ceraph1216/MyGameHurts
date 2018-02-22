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

	private Transform myTransform;
	private AttackList _attackList;

	void Awake()
	{
		myTransform = transform;
		_attackList = GetComponent<AttackList> ();
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
		ExecuteInput ();
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

		// go to transition
		if (clip.name.Contains ("Attack") && !clip.name.Contains ("_end")) 
		{
			string l_attackEndName = clip.name + "_end";
			myAnimator.Play (l_attackEndName);
			PlayerStateManager.instance.currentAttackState = Enums.PlayerAttackState.None;
		}

		// transition over, go to idle
		if (clip.name.Contains ("Attack") && clip.name.Contains ("_end")) 
		{
			myAnimator.Play ("idle");
			_currentAttack = null;
		}
	}

	private void DetermineInput ()
	{
		_currentInput = InputType.None;

		if (PlayerStateManager.instance.currentAttackState == Enums.PlayerAttackState.None) 
		{
			if (InputManager.instance.GetLightAttack ()) 
			{
				_currentInput = InputType.LightAttack;
			} 
			else if (InputManager.instance.GetHeavyAttack ()) 
			{
				_currentInput = InputType.HeavyAttack;
			} 
			else if (InputManager.instance.GetMovement () < 0) // Moving left
			{ 
				_currentInput = InputType.MoveLeft;
			} 
			else if (InputManager.instance.GetMovement () > 0) // Moving Right
			{ 
				_currentInput = InputType.MoveRight;
			} 
		}
	}

	private void ExecuteInput ()
	{
		if (_currentInput == InputType.LightAttack || _currentInput == InputType.HeavyAttack)
		{
			Attack();
		}
	}

	private void Attack()
	{
		AttackInfo l_newAttack = _attackList.GetAttackForInput (_currentInput, _currentAttack);

		if (l_newAttack == null) 
		{
			return;
		}

		_currentAttack = l_newAttack;
		PlayerStateManager.instance.currentAttackState = Enums.PlayerAttackState.Attacking;
		myAnimator.Play (_currentAttack.animationName);
	}
}