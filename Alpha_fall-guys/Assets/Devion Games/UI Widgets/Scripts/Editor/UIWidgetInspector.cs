using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEditor.AnimatedValues;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

namespace DevionGames.UIWidgets
{
	[CustomEditor (typeof(UIWidget), true)]
	public class UIWidgetInspector: CallbackHandlerInspector
	{

        protected CanvasGroup canvasGroup;

        private string[] m_WidgetPropertiesToExcludeForDefaultInspector;
        private AnimBool m_ShowAndHideOptions;
        private SerializedProperty m_ShowAndHideCursor;
        private SerializedProperty m_CameraPreset;
        private SerializedProperty m_CloseOnMove;
       // private SerializedProperty m_Deactivate;
        private SerializedProperty m_FocusPlayer;

        protected override void OnEnable ()
		{
            base.OnEnable();
            this.canvasGroup = (target as UIWidget).GetComponent<CanvasGroup>();

            this.m_ShowAndHideCursor = serializedObject.FindProperty("m_ShowAndHideCursor");
            this.m_CameraPreset = serializedObject.FindProperty("m_CameraPreset");
            this.m_CloseOnMove = serializedObject.FindProperty("m_CloseOnMove");
           // this.m_Deactivate = serializedObject.FindProperty("m_Deactivate");
            this.m_FocusPlayer = serializedObject.FindProperty("m_FocusPlayer");

            this.m_ShowAndHideOptions = new AnimBool(this.m_ShowAndHideCursor.boolValue);
            this.m_ShowAndHideOptions.valueChanged.AddListener(new UnityAction(this.Repaint));

            this.m_WidgetPropertiesToExcludeForDefaultInspector = new[] {
                this.m_ShowAndHideCursor.propertyPath,
                this.m_CameraPreset.propertyPath,
                this.m_CloseOnMove.propertyPath,
                //this.m_Deactivate.propertyPath,
                this.m_FocusPlayer.propertyPath
            };
        }

        private void DrawInspector()
        {
            DrawTypePropertiesExcluding(typeof(UIWidget), this.m_WidgetPropertiesToExcludeForDefaultInspector);
            EditorGUILayout.PropertyField(this.m_ShowAndHideCursor);
            this.m_ShowAndHideOptions.target = this.m_ShowAndHideCursor.boolValue;
            if (EditorGUILayout.BeginFadeGroup(this.m_ShowAndHideOptions.faded))
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                EditorGUILayout.PropertyField(this.m_CameraPreset);
               // EditorGUILayout.PropertyField(this.m_Deactivate);
                EditorGUILayout.PropertyField(this.m_CloseOnMove);
                EditorGUILayout.PropertyField(this.m_FocusPlayer);
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
            EditorGUILayout.EndFadeGroup();
        }

        protected virtual void OnSceneGUI()
        {
            if (canvasGroup == null)
            {
                return;
            }
            Handles.BeginGUI();
            Rect rect = Camera.current.pixelRect;
           
            if (GUI.Button(new Rect(rect.width - 110f, rect.height - 30f, 100f, 20f), canvasGroup.alpha > 0.1f ? "Hide" : "Show"))
            {

                if (canvasGroup.alpha > 0.1f)
                {
                    canvasGroup.alpha = 0f;
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = false;
                }
                else
                {
                    canvasGroup.alpha = 1f;
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                }
                EditorUtility.SetDirty(canvasGroup);
            }

            Handles.EndGUI();
        }
    }
}