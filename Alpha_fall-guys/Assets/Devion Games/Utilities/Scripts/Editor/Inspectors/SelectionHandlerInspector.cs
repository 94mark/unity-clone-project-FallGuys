using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace DevionGames
{
    [CustomEditor(typeof(SelectionHandler),true)]
    public class SelectionHandlerInspector : CallbackHandlerInspector
    {

        private SerializedProperty m_SelectionType;
        private SerializedProperty m_SelectionDistance;
        private SerializedProperty m_SelectionKey;
        private SerializedProperty m_RaycastOffset;
        private SerializedProperty m_LayerMask;
        private AnimBool m_RaycastOffsetOptions;
        private AnimBool m_SelectionKeyOptions;

        private SerializedProperty m_DeselectionType;
        private SerializedProperty m_DeselectionKey;
        private SerializedProperty m_DeselectionDistance;
        private AnimBool m_DeselectionKeyOptions;
        private AnimBool m_DeselectionDistanceOptions;

        protected override void OnEnable()
        {
            base.OnEnable();
         
            this.m_SelectionType = serializedObject.FindProperty("selectionType");
            this.m_SelectionDistance = serializedObject.FindProperty("m_SelectionDistance");
            this.m_SelectionKey = serializedObject.FindProperty("m_SelectionKey");
            this.m_RaycastOffset = serializedObject.FindProperty("m_RaycastOffset");
            this.m_LayerMask = serializedObject.FindProperty("m_LayerMask");
            if (this.m_RaycastOffsetOptions == null)
            {
                this.m_RaycastOffsetOptions = new AnimBool((target as SelectionHandler).selectionType.HasFlag<SelectionHandler.SelectionInputType>(SelectionHandler.SelectionInputType.Raycast));
                this.m_RaycastOffsetOptions.valueChanged.AddListener(Repaint);
            }

            if (this.m_SelectionKeyOptions == null)
            {
                this.m_SelectionKeyOptions = new AnimBool((target as SelectionHandler).selectionType.HasFlag<SelectionHandler.SelectionInputType>(SelectionHandler.SelectionInputType.Key));
                this.m_SelectionKeyOptions.valueChanged.AddListener(Repaint);
            }

            this.m_DeselectionType = serializedObject.FindProperty("deselectionType");
            this.m_DeselectionKey = serializedObject.FindProperty("m_DeselectionKey");
            this.m_DeselectionDistance = serializedObject.FindProperty("m_DeselectionDistance");
            if (this.m_DeselectionKeyOptions == null)
            {
                this.m_DeselectionKeyOptions = new AnimBool((target as SelectionHandler).deselectionType.HasFlag<SelectionHandler.DeselectionInputType>(SelectionHandler.DeselectionInputType.Key));
                this.m_DeselectionKeyOptions.valueChanged.AddListener(Repaint);
            }
            if (this.m_DeselectionDistanceOptions == null)
            {
                this.m_DeselectionDistanceOptions = new AnimBool((target as SelectionHandler).deselectionType.HasFlag<SelectionHandler.DeselectionInputType>(SelectionHandler.DeselectionInputType.Distance));
                this.m_DeselectionDistanceOptions.valueChanged.AddListener(Repaint);
            }
        }

        private void DrawInspector()
        {
            EditorGUILayout.PropertyField(this.m_SelectionType);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(this.m_LayerMask);
            EditorGUILayout.PropertyField(this.m_SelectionDistance);

            this.m_SelectionKeyOptions.target = (target as SelectionHandler).selectionType.HasFlag<SelectionHandler.SelectionInputType>(SelectionHandler.SelectionInputType.Key);
            if (EditorGUILayout.BeginFadeGroup(this.m_SelectionKeyOptions.faded))
            {
                EditorGUILayout.PropertyField(this.m_SelectionKey);
            }
            EditorGUILayout.EndFadeGroup();

            this.m_RaycastOffsetOptions.target = (target as SelectionHandler).selectionType.HasFlag<SelectionHandler.SelectionInputType>(SelectionHandler.SelectionInputType.Raycast);
            if (EditorGUILayout.BeginFadeGroup(this.m_RaycastOffsetOptions.faded))
            {
                EditorGUILayout.PropertyField(this.m_RaycastOffset);
            }
            EditorGUILayout.EndFadeGroup();
            EditorGUI.indentLevel -= 1;


            EditorGUILayout.PropertyField(this.m_DeselectionType);
            EditorGUI.indentLevel += 1;
            this.m_DeselectionKeyOptions.target = (target as SelectionHandler).deselectionType.HasFlag<SelectionHandler.DeselectionInputType>(SelectionHandler.DeselectionInputType.Key);
            if (EditorGUILayout.BeginFadeGroup(this.m_DeselectionKeyOptions.faded))
            {
                EditorGUILayout.PropertyField(this.m_DeselectionKey);
            }
            EditorGUILayout.EndFadeGroup();

            this.m_DeselectionDistanceOptions.target = (target as SelectionHandler).deselectionType.HasFlag<SelectionHandler.DeselectionInputType>(SelectionHandler.DeselectionInputType.Distance);
            if (EditorGUILayout.BeginFadeGroup(this.m_DeselectionDistanceOptions.faded))
            {
                EditorGUILayout.PropertyField(this.m_DeselectionDistance);
            }
            EditorGUILayout.EndFadeGroup();
            EditorGUI.indentLevel -= 1;
        }
    }
}