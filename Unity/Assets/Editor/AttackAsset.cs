using UnityEngine;
using UnityEditor;
using System.Collections;

public class AttackAsset
{
	[MenuItem("Assets/Create/Combat/Attack Info")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<AttackInfo> ();
	}
}