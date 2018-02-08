using UnityEngine;
using UnityEditor;
using System.IO;

public static class ScriptableObjectUtility
{
    public static void CreateAsset<T> () where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T> ();
        
        string path = AssetDatabase.GetAssetPath (Selection.activeObject);
        if (path == "") 
        {
            path = "Assets/Resources/Data";
        } 
        else if (Path.GetExtension (path) != "") 
        {
            path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
        }
		
        string fileName;
		if (asset is ManeuverInfo) 
		{
			path = "Assets/Resources/Data/Maneuvers";
			fileName = "New Maneuver";
		}
		else
		{
			fileName = typeof(T).ToString();
		}
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/" + fileName + ".asset");

        AssetDatabase.CreateAsset (asset, assetPathAndName);
        
        AssetDatabase.SaveAssets ();
        Selection.activeObject = asset;
        EditorUtility.FocusProjectWindow ();
		
		if (asset is ManeuverInfo) 
		{
			ManeuverEditorWindow.Init();
		}
    }
}
