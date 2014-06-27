using UnityEngine;
using System.Collections;

public class PrefabManager : MonoBehaviour 
{
	// Prefabs
	public GameObject testEnemy;

	// Object Pools
	public GameObjectPool testEnemyPool;

	static PrefabManager mInstance;
	
	// Whether there is an instance of the PrefabManager class present.
	
	static public bool isActive { get { return mInstance != null; } }
	
	// The instance of the PrefabManager class. Will create it if one isn't already around.
	
	static public PrefabManager instance
	{
		get
		{
			return mInstance;
		}
	}
	
	void Awake()
	{
		mInstance = this;

		testEnemyPool = new GameObjectPool(testEnemy, 1);
	}
	
	public GameObjectPool FindPoolForObject(GameObject obj)
	{	
		if(obj != null)
		{
			// Player
			if(obj.name.Contains("testEnemy"))
			{
				return testEnemyPool;
			}
		}
		return null;
	}
}