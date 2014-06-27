using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopupManager : MonoBehaviour 
{
	// Set up delegate for popup events
	public delegate void EventHandler(GameObject e);
	public event EventHandler ChangePopup;
	public event EventHandler ChangeSecondaryPopup;

	static PopupManager mInstance;
	
	// Whether there is an instance of the PopupManager class present.
	
	static public bool isActive { get { return mInstance != null; } }
	
	// The instance of the PopupManager class. Will create it if one isn't already around.
	
	static public PopupManager instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = Object.FindObjectOfType(typeof(PopupManager)) as PopupManager;
				
				if (mInstance == null)
				{
					GameObject go = new GameObject("_PopupManager");
					DontDestroyOnLoad(go);
					mInstance = go.AddComponent<PopupManager>();
				}
			}
			return mInstance;
		}
	}
	
	private Enums.PopupType _currentPopup = Enums.PopupType.None;
	public Enums.PopupType currentPopup
	{
		get
		{
			return _currentPopup;
		}
		
		set
		{
			// Make sure we flag the popup as changing when this is set so
			// ShowPopupsBehavior knows when to show the new popup
			_currentPopup = value;
			if (ChangePopup != null)
			{
				ChangePopup(this.gameObject);
			}
		}
	}
	
	private Enums.SecondaryPopupType _currentSecondaryPopup = Enums.SecondaryPopupType.None;
	public Enums.SecondaryPopupType currentSecondaryPopup
	{
		get
		{
			return _currentSecondaryPopup;
		}
		
		set
		{
			// Make sure we flag the popup as changing when this is set so
			// ShowPopupsBehavior knows when to show the new popup
			_currentSecondaryPopup = value;
			if (ChangeSecondaryPopup != null)
			{
				ChangeSecondaryPopup(this.gameObject);
			}
		}
	}
	
	public void ResetEvents()
	{
		ChangePopup = null;
		ChangeSecondaryPopup = null;
	}
}