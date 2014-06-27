using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour 
{
	private static float SPAWN_DELAY = 3f;
	private static int MAX_SPAWNS;
	private static float CAMERA_LEFT;
	private static float CAMERA_RIGHT;

	public float spawnXMin;
	public float spawnXMax;
	public float spawnY;

	[HideInInspector]
	public List<GameObject> spawnedEnemies;

	[HideInInspector]
	public List<Transform> spawnedEnemyTransforms;

	private float currentSpawnTime;
	private Transform _cameraTransform;
	
	void Awake() 
	{
		// Divide by 10 to account for 10/1 pixel ratio in scene
		CAMERA_LEFT = (-Constants.SCREEN_WIDTH / 10) - 1.5f;
		CAMERA_RIGHT = (Constants.SCREEN_WIDTH / 10) + 1.5f;
		_cameraTransform = Camera.main.transform;
	}
	
	void OnEnable()
	{
		currentSpawnTime = SPAWN_DELAY;

		SoftPauseScript.instance.SoftUpdate += SoftUpdate;
	}

	void OnDisable()
	{
		SoftPauseScript.instance.SoftUpdate -= SoftUpdate;
	}

	void SoftUpdate(GameObject dispatcher) 
	{
		if (currentSpawnTime > 0)
		{
			currentSpawnTime -= Time.deltaTime;
		}
		else
		{
			Vector3 spawnPos = new Vector3(0, spawnY, 0);
			spawnPos.x = Random.Range(spawnXMin, spawnXMax);
			CreateEnemy(spawnPos);

			currentSpawnTime = SPAWN_DELAY;
		}
	}

	public void CreateEnemy(Vector3 p_spawnPosition)
	{
		GameObject l_newEnemy = PrefabManager.instance.testEnemyPool.Spawn(p_spawnPosition, Quaternion.identity) as GameObject;
		spawnedEnemies.Add(l_newEnemy);
		spawnedEnemyTransforms.Add(l_newEnemy.transform);
	}

	public void DestroyEnemy(GameObject p_enemy)
	{
		PrefabManager.instance.FindPoolForObject(p_enemy).Unspawn(p_enemy);
		p_enemy.GetComponent<TrackEnemyVisibilityScript>().SubtractFromVisible();
		spawnedEnemies.Remove(p_enemy);
		spawnedEnemyTransforms.Remove(p_enemy.transform);
	}

	public float GetRightPosition()
	{
		float furthestRight = _cameraTransform.position.x;

		for (int i = 0; i < spawnedEnemyTransforms.Count; i++)
		{
			Transform l_transform = spawnedEnemyTransforms[i];

			// Transform is further to the right than previous furthest point but also within bounds
			if (l_transform.position.x > _cameraTransform.position.x && l_transform.position.x > furthestRight &&
			    l_transform.position.x < _cameraTransform.position.x + CAMERA_RIGHT)
			{
				furthestRight = l_transform.position.x;
			}
		}

		return furthestRight;
	}

	public float GetLeftPosition()
	{
		float furthestLeft = _cameraTransform.position.x;
		
		for (int i = 0; i < spawnedEnemyTransforms.Count; i++)
		{
			Transform l_transform = spawnedEnemyTransforms[i];
			
			// Transform is further to the left than previous furthest point but also within bounds
			if (l_transform.position.x < _cameraTransform.position.x && l_transform.position.x < furthestLeft &&
			    l_transform.position.x > _cameraTransform.position.x - CAMERA_LEFT)
			{
				furthestLeft = l_transform.position.x;
			}
		}
		
		return furthestLeft;
	}
}
