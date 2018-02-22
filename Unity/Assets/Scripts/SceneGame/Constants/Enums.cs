using UnityEngine;
using System.Collections;

namespace Enums
{
	public enum PlayerGroundState
	{
		OnGround,
		Rising,
		Falling,
		Landing
	}

	public enum PlayerAttackState
	{
		None,
		Attacking
	}

	public enum PlayerStunnedState
	{
		None,
		Hit
	}

	public enum PopupType
	{
		None,
		Options,
		Tutorial,
		Pause
	}

	public enum SecondaryPopupType
	{
		None
	}

	public enum GestureType
	{
		None,
		Swipe,
		DoubleTap,
		Press,
		Release
	}

	public enum UpdateType
	{
		InitialSoftUpdate,
		InitialSoftPause,
		EarlySoftUpdate,
		EarlySoftPause,
		SoftUpdate,
		SoftPause,
		FixedSoftUpdate,
		FixedSoftPause,
		LateSoftUpdate,
		LateSoftPause,
		FinalSoftUpdate,
		FinalSoftPause
	}
}
