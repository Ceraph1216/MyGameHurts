using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// A general pool object for reusable game objects.
//
// It supports spawning and unspawning game objects that are
// instantiated from a common prefab. Can be used preallocate
// objects to avoid calls to Instantiate during gameplay. Can
// also create objects on demand (which it does if no objects
// are available in the pool).
public class GameObjectPool 
{
	// Delegates
   	public delegate void Initialize(GameObject obj);
   	public delegate void GenericFunction(GameObject obj);
	
	// The prefab that the game objects will be instantiated from.
	private GameObject prefab;
	
	// The list of available game objects (initially empty by default).
	private Stack available;
	
	// The list of all game objects created thus far (used for efficiently
	// unspawning all of them at once, see UnspawnAll).
	private List<GameObject> all;

	// An optional function that will be called whenever a new object is instantiated.
	// The newly instantiated object is passed to it, which allows users of the pool
	// to do custom initialization.
	//private Initialize initializationFunction; // Function
	
	// Indicates whether the pool's game objects should be activated/deactivated
	// recursively (i.e. the game object and all its children) or non-recursively (just the
	// game object).
	//private bool setActiveRecursively;
	
	// Creates a pool.
	// The initialCapacity is used to initialize the .NET collections, and determines
	// how much space they pre-allocate behind the scenes. It does not pre-populate the
	// collection with game objects. For that, see the PrePopulate function.
	// If an initialCapacity that is <= to zero is provided, the pool uses the default
	// initial capacities of its internal .NET collections.
	public GameObjectPool(GameObject prefab, int initialCapacity/*, Initialize initializationFunction, bool setActiveRecursively*/)
	{
		this.prefab = prefab;
		if(initialCapacity > 0)
		{
			this.available = new Stack(initialCapacity);
			this.all = new List<GameObject>(initialCapacity);
			
			// Try initializing everything at the very beginning! Could be dangerous :X
			/*List<GameObject> objects = new List<GameObject>();
			
			for(int i = 0; i < initialCapacity; i++)
			{
				objects.Add(Spawn(new Vector3(0, 100, 0), Quaternion.identity));
			}
			
			for(int i = 0; i < initialCapacity; i++)
			{
				Unspawn(objects[i]);
			}*/
		} 
		else 
		{
			// Use the .NET defaults
			this.available = new Stack();
			this.all = new List<GameObject>();
		}
		
		//this.initializationFunction = initializationFunction;
		//this.setActiveRecursively = setActiveRecursively;
	}

	// Spawn a game object with the specified position/rotation.
	public GameObject Spawn(Vector3 position, Quaternion rotation)
	{
		GameObject result;

		if(available.Count == 0)
		{
			// Create an object and initialize it.
			result = GameObject.Instantiate(prefab, position, rotation) as GameObject;
			/*if(initializationFunction != null)
			{
				initializationFunction(result);
			}*/
			
			// Keep track of it.
			all.Add(result);
		} 
		else 
		{
			result = available.Pop() as GameObject;
			
			// Get the result's transform and reuse for efficiency.
			// Calling gameObject.transform is expensive.
			var resultTrans = result.transform;
			resultTrans.position = position;
			resultTrans.rotation = rotation;

			this.SetActive(result, true);
		}
		
		result.SetActive(true);
		return result;
	}

	// Unspawn the provided game object.
	// The function is idempotent. Calling it more than once for the same game object is
	// safe, since it first checks to see if the provided object is already unspawned.
	// Returns true if the unspawn succeeded, false if the object was already unspawned.
	public bool Unspawn(GameObject obj)
	{
		if(!available.Contains(obj))
		{ 
			// Make sure we don't insert it twice.
			available.Push(obj);
			this.SetActive(obj, false);
			obj.SetActive(false);
			return true; // Object inserted back in stack.
		}
		return false; // Object already in stack.
	}

	// Pre-populates the pool with the provided number of game objects.
	public void PrePopulate(int count)
	{
		GameObject[] array  = new GameObject[count];
		for(var i = 0; i < count; i++)
		{
			array[i] = Spawn(Vector3.zero, Quaternion.identity);
			this.SetActive(array[i], false);
		}
		
		for(var j = 0; j < count; j++)
		{
			Unspawn(array[j]);
		}
	}

	// Unspawns all the game objects created by the pool.
	public void UnspawnAll()
	{
		for(var i = 0; i < all.Count; i++)
		{
			GameObject obj  = all[i] as GameObject;
			
			if(obj.activeInHierarchy)
			{
				Unspawn(obj);
			}
		}
	}

	// Unspawns all the game objects and clears the pool.
	public void Clear()
	{
		UnspawnAll();
		available.Clear();
		all.Clear();
	}

	// Returns the number of active objects.
	public int GetActiveCount()
	{
		return all.Count - available.Count;
	}

	// Returns the number of available objects.
	public int GetAvailableCount()
	{
		return available.Count;
	}

	// Returns the prefab being used by this pool.
	public GameObject GetPrefab() 
	{
		return prefab;
	}

	// Applies the provided function to some or all of the pool's game objects.
	public void ForEach(GenericFunction func, bool activeOnly)
	{
		for(var i = 0; i < all.Count; i++)
		{
			GameObject obj = all[i] as GameObject;
			if(!activeOnly || obj.activeInHierarchy)
			{
				func(obj);
			}
		}
	}

	// Activates or deactivates the provided game object using the method
	// specified by the setActiveRecursively flag.
	public void SetActive(GameObject obj, bool val)
	{
		/* if(setActiveRecursively)
			obj.SetActiveRecursively(val);
		else*/
		obj.SetActive(val);
	}
}