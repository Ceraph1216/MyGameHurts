using UnityEngine;
using System.Collections;

public class TrackEnemyVisibilityScript : MonoBehaviour 
{
	private bool _isVisible;

	void OnBecameVisible()
	{
		if (!_isVisible)
		{
			CombatCamera.numEnemiesOnScreen ++;
			_isVisible = true;
			Debug.Log("visible: " + CombatCamera.numEnemiesOnScreen);
		}
	}

	void OnBecameInvisible()
	{
		SubtractFromVisible();
	}

	void OnDisable()
	{
//		SubtractFromVisible();
	}

	public void SubtractFromVisible()
	{
		if (_isVisible)
		{
			CombatCamera.numEnemiesOnScreen --;
			Debug.Log("invisible: " + CombatCamera.numEnemiesOnScreen);
			_isVisible = false;
		}
	}
}
