using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ManeuverEditorWindow : EditorWindow
{
	public static ManeuverEditorWindow maneuverEditorWindow;
	public static ManeuverInfo sentManeuverInfo;
	private ManeuverInfo _maneuverInfo;

	private Vector2 scrollPos;

	// The various styles for our GUI
	private string titleStyle;
	private string removeButtonStyle;
	private string addButtonStyle;
	private string rootGroupStyle;
	private string subGroupStyle;
	private string arrayElementStyle;
	private string subArrayElementStyle;
	private string toggleStyle;
	private string foldStyle;
	private string enumStyle;
	private GUIStyle labelStyle;

	private List<Hit> _hits;

	[MenuItem("Window/Data Type Editors/Maneuver Editor")]
	public static void Init()
	{
		maneuverEditorWindow = EditorWindow.GetWindow<ManeuverEditorWindow>(false, "Maneuver", true);
		maneuverEditorWindow.Show();
		maneuverEditorWindow.Populate();
	}

	void OnSelectionChange()
	{
		Populate();
		Repaint();
	}
	
	void OnEnable()
	{
		Populate();
	}
	
	void OnFocus()
	{
		Populate();
	}

	void Populate()
	{
		// Style Definitions
		titleStyle = "MeTransOffRight";
		removeButtonStyle = "TL SelectionBarCloseButton";
		addButtonStyle = "CN CountBadge";
		rootGroupStyle = "GroupBox";
		subGroupStyle = "ObjectFieldThumb";
		arrayElementStyle = "flow overlay box";
		subArrayElementStyle = "HelpBox";
		foldStyle = "Foldout";
		enumStyle = "MiniPopup";
		toggleStyle = "BoldToggle";
		
		labelStyle = new GUIStyle();
		labelStyle.alignment = TextAnchor.MiddleCenter;
		labelStyle.fontStyle = FontStyle.Bold;
		labelStyle.normal.textColor = Color.white;
		
		
		if (sentManeuverInfo != null){
			EditorGUIUtility.PingObject( sentManeuverInfo );
			Selection.activeObject = sentManeuverInfo;
			sentManeuverInfo = null;
		}
		
		Object[] selection = Selection.GetFiltered(typeof(ManeuverInfo), SelectionMode.Assets);
		if (selection.Length > 0){
			if (selection[0] == null) return;
			_maneuverInfo = (ManeuverInfo) selection[0];
		}

		if (_maneuverInfo.hits != null)
		{
			_hits = _maneuverInfo.hits.ToList();
		}
		else
		{
			_hits = new List<Hit>();
		}

	}

	public void OnGUI()
	{
		if (_maneuverInfo == null)
		{
			GUILayout.BeginHorizontal("GroupBox");
			GUILayout.Label("Select a maneuver file or create a new maneuver.","CN EntryInfo");
			GUILayout.EndHorizontal();
			EditorGUILayout.Space();
			if (GUILayout.Button("Create new maneuver"))
				ScriptableObjectUtility.CreateAsset<ManeuverInfo> ();
			return;
		}

		GUIStyle fontStyle = new GUIStyle();
		fontStyle.font = (Font) EditorGUIUtility.Load("EditorFont.TTF");
		fontStyle.fontSize = 30;
		fontStyle.alignment = TextAnchor.UpperCenter;
		fontStyle.normal.textColor = Color.white;
		fontStyle.hover.textColor = Color.white;

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		{
			SerializedObject l_serializedManeuver = new SerializedObject(_maneuverInfo);

			// Draw the message data

			EditorGUILayout.BeginVertical();
			{
				if (_maneuverInfo.movementCurve == null) 
				{
					_maneuverInfo.movementCurve = new AnimationCurve ();
				}

				if (_maneuverInfo.rotationCurve == null) 
				{
					_maneuverInfo.rotationCurve = new AnimationCurve ();
				}

				EditorGUIUtility.labelWidth = 90;
				_maneuverInfo.maneuverName = EditorGUILayout.TextField("Name:", _maneuverInfo.maneuverName);
				_maneuverInfo.animationName = EditorGUILayout.TextField("Animation Name:", _maneuverInfo.animationName);
				_maneuverInfo.movementCurve = EditorGUILayout.CurveField ("Movement Curve:", _maneuverInfo.movementCurve);
				_maneuverInfo.rotationCurve = EditorGUILayout.CurveField ("Rotation Curve:", _maneuverInfo.rotationCurve);
				_maneuverInfo.inputType = (InputType) EditorGUILayout.EnumPopup ("Input Type:", _maneuverInfo.inputType);
				_maneuverInfo.previousManeuver = (ManeuverInfo) EditorGUILayout.ObjectField("Previous Maneuver:", _maneuverInfo.previousManeuver, typeof(ManeuverInfo), false);
				ShowHits();
			}EditorGUILayout.EndVertical();

		EditorGUIUtility.labelWidth = 150;
		EditorGUILayout.Space();

		}EditorGUILayout.EndScrollView();

		if (GUI.changed) 
		{
			Undo.RecordObject(_maneuverInfo, "Maneuver Editor Modify");
			EditorUtility.SetDirty(_maneuverInfo);
		}
	}

	/// <summary>
	/// Atlas selection callback.
	/// </summary>
	
	/*void OnSelectAtlas (Object obj)
	{
		SerializedObject l_serializedMessage = new SerializedObject(_maneuverInfo);
		l_serializedMessage.Update();
		SerializedProperty sp = l_serializedMessage.FindProperty("atlas");
		sp.objectReferenceValue = obj;
		l_serializedMessage.ApplyModifiedProperties();
		NGUITools.SetDirty(l_serializedMessage.targetObject);
		NGUISettings.atlas = obj as UIAtlas;

		Undo.RecordObject(_maneuverInfo, "Message Editor Modify");
		EditorUtility.SetDirty(_maneuverInfo);
		Repaint();
	}*/
	
	/// <summary>
	/// Sprite selection callback function.
	/// </summary>
	
	/*void SelectSprite (string spriteName)
	{
		SerializedObject l_serializedMessage = new SerializedObject(_maneuverInfo);
		l_serializedMessage.Update();
		_maneuverInfo.spriteName = spriteName;
		l_serializedMessage.ApplyModifiedProperties();
//		NGUITools.SetDirty(l_serializedMessage.targetObject);
		NGUISettings.selectedSprite = spriteName;

		Undo.RecordObject(_maneuverInfo, "Message Editor Modify");
		EditorUtility.SetDirty(_maneuverInfo);
		Repaint();
	}*/

	/// <summary>
	/// Draw the sprite preview.
	/// </summary>
	
	/*public void PreviewSprite (Rect p_rect)
	{
		if (_maneuverInfo.atlas == null) return;

		UISpriteData l_spriteData = _maneuverInfo.atlas.GetSprite(_maneuverInfo.spriteName);

		if (l_spriteData == null) return;

		Material l_atlasMaterial = _maneuverInfo.atlas.spriteMaterial;
		Texture2D l_tex = l_atlasMaterial.mainTexture as Texture2D;

		if (l_tex == null) return;
		
		NGUIEditorTools.DrawSprite(l_tex, p_rect, l_spriteData, Color.white);
	}*/

	private void ShowHits ()
	{
		GUIStyle l_darkStyle = new GUIStyle();
		l_darkStyle.normal.background = MakeTex(600, 1, new Color(0.5f, 0.5f, 0.5f));
		l_darkStyle.normal.textColor = new Color(0.7f, 0.7f, 0.7f);

		for (int i = 0; i < _hits.Count; i++)
		{
			Hit l_currentHit = _hits[i];

			if(i%2 == 0)
			{
				EditorGUILayout.BeginVertical(l_darkStyle);
			}
			else
			{
				EditorGUILayout.BeginVertical();
			}
				
				EditorGUILayout.BeginVertical();
			{

				EditorGUILayout.BeginHorizontal ();
				{
//					Rect rect = GUILayoutUtility.GetRect(50, 70);
//					EditorStyles.textField.wordWrap = true;
					l_currentHit.startTime = EditorGUILayout.FloatField ("Start Time:", l_currentHit.startTime);
					l_currentHit.endTime = EditorGUILayout.FloatField ("End Time:", l_currentHit.endTime);
					l_currentHit.damage = EditorGUILayout.FloatField ("Damage:", l_currentHit.damage);

					if (GUILayout.Button ("-", GUILayout.Width (20f))) {
						l_currentHit.dirty = true;
					}
				}
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.BeginHorizontal ();
				{
//					Rect rect = GUILayoutUtility.GetRect(50, 70);
//					EditorStyles.textField.wordWrap = true;
					l_currentHit.offset = EditorGUILayout.Vector2Field ("Offset:", l_currentHit.offset);
					l_currentHit.size = EditorGUILayout.Vector2Field ("Size:", l_currentHit.size);
					l_currentHit.knockback = EditorGUILayout.Vector2Field ("Knockback:", l_currentHit.knockback);
				}
				EditorGUILayout.EndHorizontal ();

			} EditorGUILayout.EndVertical();
			EditorGUILayout.EndVertical();
		}

		for (int i = 0; i < _hits.Count; i++)
		{
			Hit l_currentHit = _hits[i];

			if (l_currentHit.dirty)
			{
				_hits.Remove(l_currentHit);
			}
		}

		if (GUILayout.Button("Add Hit", GUILayout.Width(100f)))
		{
			Hit l_newHit = new Hit();
			_hits.Add(l_newHit);
		}

		_maneuverInfo.hits = _hits.ToArray();
		EditorUtility.SetDirty(_maneuverInfo);
		Repaint();
	}

	private Texture2D MakeTex(int width, int height, Color col)
	{
		Color[] pix = new Color[width*height];
		
		for(int i = 0; i < pix.Length; i++)
			pix[i] = col;
		
		Texture2D result = new Texture2D(width, height);
		result.SetPixels(pix);
		result.Apply();
		
		return result;
	}
}