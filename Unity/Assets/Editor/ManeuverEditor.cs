using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ManeuverInfo))]
public class ManeuverEditor : Editor
{
	public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Open Maneuver Editor")) 
			ManeuverEditorWindow.Init();
	}
}