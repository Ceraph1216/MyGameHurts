using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(AttackInfo))]
public class AttackEditor : Editor
{
	public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Open Attack Editor")) 
			AttackEditorWindow.Init();
	}
}