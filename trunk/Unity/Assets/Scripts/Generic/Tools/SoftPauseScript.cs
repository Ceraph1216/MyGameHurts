using UnityEngine;
using System.Collections;

public class SoftPauseScript : MonoBehaviour
{
	private static bool _softPaused = false;
	public static bool softPaused
	{
		get
		{
			return _softPaused;
		}
		
		set
		{
			//bool oldVal = _softPaused;
			_softPaused = value;
			
			/*if(oldVal != _softPaused)
			{
				if(_softPaused)
				{
					AnimationManager.pause();
				}
				else
				{
					AnimationManager.resume();
				}
			}*/
		}
	}
	
	public delegate void EventHandler(GameObject e);
	public event EventHandler EarlySoftUpdate;
	public event EventHandler SoftUpdate;
	public event EventHandler SoftFixedUpdate;
	public event EventHandler SoftPause;
	public event EventHandler SoftLateUpdate;
	
	static SoftPauseScript mInstance;

	// Whether there is an instance of the SoftPauseScript class present.

	static public bool isActive { get { return mInstance != null; } }

	// The instance of the SoftPauseScript class. Will create it if one isn't already around.

	static public SoftPauseScript instance
	{
		get
		{
			return mInstance;
		}
	}
	
	void Awake()
	{
		mInstance = this;
	}
	
	void Update () 
	{
		if(this.enabled)
		{
			if (!softPaused)
			{
				if (EarlySoftUpdate != null)
				{
					EarlySoftUpdate(this.gameObject);
				}
				if (SoftUpdate != null)
				{
					SoftUpdate(this.gameObject);
				}
			}
			else
			{
				if (SoftPause != null)
				{
					SoftPause(this.gameObject);
				}
			}
		}
	}

	void FixedUpdate()
	{
		if (this.enabled)
		{
			if (!softPaused)
			{
				if (SoftFixedUpdate != null)
				{
					SoftFixedUpdate(this.gameObject);
				}
			}
		}
	}

	void LateUpdate()
	{
		if (this.enabled)
		{
			if (!softPaused)
			{
				if (SoftLateUpdate != null)
				{
					SoftLateUpdate(this.gameObject);
				}
			}
		}
	}
}