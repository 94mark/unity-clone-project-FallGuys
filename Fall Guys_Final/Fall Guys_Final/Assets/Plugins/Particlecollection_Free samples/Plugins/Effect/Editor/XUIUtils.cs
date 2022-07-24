using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditorInternal;
using System.Reflection;
using System;

static class XUIUtils {
	/////////////////////////////////////////////////////////////////////////////
	/*
	* This section makes an EditorGUILayout.Foldout, except you can actually click anywhere on the word to open it
	* instead of having to click on the teeny-tiny triangle.
	* 
	* ... what the bananas, Unity
	*/ 
	private static GUIStyle openFoldoutStyle;
	private static GUIStyle closedFoldoutStyle;
	private static bool initted;

	private static void Init()
	{
		openFoldoutStyle = new GUIStyle(GUI.skin.FindStyle("Label"));
		openFoldoutStyle.fontStyle = (FontStyle)8;
		openFoldoutStyle.alignment = TextAnchor.MiddleLeft;
		openFoldoutStyle.stretchHeight = true;
		closedFoldoutStyle = new GUIStyle(openFoldoutStyle);
		openFoldoutStyle.normal = openFoldoutStyle.onNormal;
		openFoldoutStyle.active = openFoldoutStyle.onActive;
		initted = true;
	}
	public static bool Foldout(bool open, ref bool toggled, string text) { return Foldout(open, ref toggled, new GUIContent(text)); }
	public static bool Foldout(bool open, ref bool toggled, GUIContent text)
	{
		if (!initted) Init();
		if (open) {
			GUILayout.BeginHorizontal(EditorStyles.miniButton, GUILayout.Height(15));
			toggled = GUILayout.Toggle(toggled, "", GUILayout.Width(10));
			if (toggled) {
				openFoldoutStyle.normal.textColor = new Vector4 (0.8f, 0.8f, 0.8f, 1.0f);
			} else {
				openFoldoutStyle.normal.textColor = new Vector4 (0.8f, 0.8f, 0.8f, 0.5f);
			}
			if (GUILayout.Button(text, openFoldoutStyle, GUILayout.Height(15))) {
				GUI.FocusControl("");
				GUI.changed = false; // force change-checking group to take notice
				GUI.changed = true;
				return false;
			}
			GUILayout.EndHorizontal();
		}
		else {
			GUILayout.BeginHorizontal(EditorStyles.miniButton, GUILayout.Height(15));
			toggled = GUILayout.Toggle(toggled, "", GUILayout.Width(10));
			if (toggled) {
				closedFoldoutStyle.normal.textColor = new Vector4 (0.8f, 0.8f, 0.8f, 1.0f);
			} else {
				closedFoldoutStyle.normal.textColor = new Vector4 (0.8f, 0.8f, 0.8f, 0.5f);
			}
			if (GUILayout.Button(text, closedFoldoutStyle, GUILayout.Height(15))) {
				GUI.FocusControl("");
				GUI.changed = false; // force change-checking to take notice
				GUI.changed = true;
				return true;
			}
			GUILayout.EndHorizontal();
		}
		return open;
	}

	public static string[] GetSortingLayerNames()
	{
		Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
		return (string[])sortingLayersProperty.GetValue(null, new object[0]);
	}

	// Get the unique sorting layer IDs -- tossed this in for good measure
	public static int[] GetSortingLayerUniqueIDs()
	{
		Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
		return (int[])sortingLayerUniqueIDsProperty.GetValue(null, new object[0]);
	}
}

