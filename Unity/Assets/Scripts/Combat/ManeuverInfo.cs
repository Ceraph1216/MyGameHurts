using UnityEngine;
using System.Collections;

[System.Serializable]
public class Hit
{
	/// <summary>
	/// Whether or not the message needs to be removed from the list
	/// </summary>
	public bool dirty;
	public float startTime;
	public float endTime;
	public float damage;
	public Vector2 offset;
	public Vector2 size;
	public Vector2 knockback;
}

public enum InputType
{
	Attack1,
	Attack2
}

[System.Serializable]
public class ManeuverInfo : ScriptableObject
{
	public string maneuverName;
	public string animationName;
	public AnimationCurve movementCurve;
	public AnimationCurve rotationCurve;
	public InputType inputType;
	public ManeuverInfo previousManeuver;
	public Hit[] hits;
}