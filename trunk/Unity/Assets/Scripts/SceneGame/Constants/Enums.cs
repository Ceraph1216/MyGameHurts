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
		J1,
		J2,
		JJFinish,
		JKFinish,
		K1,
		K2,
		KKFinish,
		KJFinish
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
}
