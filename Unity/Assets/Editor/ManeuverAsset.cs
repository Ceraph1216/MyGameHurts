using UnityEngine;
using UnityEditor;
using System.Collections;

public class ManeuverAsset
{
	[MenuItem("Assets/Create/Combat/Maneuver Info")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<ManeuverInfo> ();
	}
}