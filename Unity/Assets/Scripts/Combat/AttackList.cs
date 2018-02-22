using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackList : MonoBehaviour 
{
	public AttackInfo[] availableAttacks;

	public AttackInfo GetAttackForInput (InputType p_input, AttackInfo p_previousAttack)
	{
		for (int i = 0; i < availableAttacks.Length; i++) 
		{
			AttackInfo l_currentAttack = availableAttacks [i];

			if (l_currentAttack.inputType == p_input && l_currentAttack.previousAttack == p_previousAttack) 
			{
				return l_currentAttack;
			}
		}

		return null;
	}
}
