using UnityEngine;
using System.Collections;

public class AttackCollisionScript : MonoBehaviour 
{
	public LayerMask enemyLayerMask;

	private bool hasChecked = false;

	void OnEnable()
	{
		hasChecked = false;
		SoftPauseScript.instance.AddToHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);
	}

	void OnDisable()
	{
		hasChecked = false;
		SoftPauseScript.instance.RemoveFromHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);
	}

	void SoftUpdate(GameObject dispatcher)
	{
		if (hasChecked)
		{
			gameObject.SetActive(false);
		}
		
		if (!hasChecked)
		{
			hasChecked = true;
		}
	}

	void OnTriggerEnter2D(Collider2D trig)
	{
		if(((1<<trig.gameObject.layer) & enemyLayerMask) != 0)
		{
			PrefabManager.instance.FindPoolForObject(trig.gameObject).Unspawn(trig.gameObject);
		}
	}
}
