using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DevionGames
{
    [CustomEditor(typeof(AudioEventListener))]
    public class AudioEventListenerInspector : Editor
    {
        private SerializedProperty m_Script;
        private SerializedProperty m_AudioGroups;
        private string m_AudioGroupName = string.Empty;
        private ReorderableList m_AudioGroupList;

        private void OnEnable()
        {
            this.m_Script = serializedObject.FindProperty("m_Script");
            this.m_AudioGroups = serializedObject.FindProperty("m_AudioGroups");
            CreateAudioGroupList();
            
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            EditorGUILayout.BeginVertical();
            serializedObject.Update();
            this.m_AudioGroupList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.EndVertical();
            GUILayout.Space(-4.5f);

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUIStyle textStyle = new GUIStyle(EditorStyles.textField);
            textStyle.margin = new RectOffset(0,1, 1, 1);
            textStyle.alignment = TextAnchor.MiddleRight;
            this.m_AudioGroupName = EditorGUILayout.TextField(this.m_AudioGroupName, textStyle);
            if (string.IsNullOrEmpty(this.m_AudioGroupName))
            {
                Rect variableNameRect = GUILayoutUtility.GetLastRect();
                GUIStyle variableNameOverlayStyle = new GUIStyle(EditorStyles.label);
                variableNameOverlayStyle.alignment = TextAnchor.MiddleRight;
                variableNameOverlayStyle.normal.textColor = Color.grey;
                GUI.Label(variableNameRect, "(New Group Name)", variableNameOverlayStyle);
            }

            if (GUILayout.Button(EditorGUIUtility.IconContent("d_Toolbar Plus"), (GUIStyle)"toolbarbuttonLeft", GUILayout.Width(28f)))
            {

                if (string.IsNullOrEmpty(this.m_AudioGroupName))
                {
                    EditorUtility.DisplayDialog("New Audio Group", "Please enter a group name.", "OK");
                }
                else if (AudioGroupNameExists(this.m_AudioGroupName))
                {
                    EditorUtility.DisplayDialog("New Audio Group", "A group with the same name already exists.", "OK");
                }
                else
                {
                    AddGroup();
                }
                EditorGUI.FocusTextInControl("");
            }

            EditorGUI.BeginDisabledGroup(this.m_AudioGroupList.index == -1);
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_Toolbar Minus"), EditorStyles.toolbarButton, GUILayout.Width(25f)))
            {
                this.serializedObject.Update();
                this.m_AudioGroups.DeleteArrayElementAtIndex(this.m_AudioGroupList.index);
                this.serializedObject.ApplyModifiedProperties();
                this.m_AudioGroupList.index = this.m_AudioGroups.arraySize - 1;
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
        }

        private bool AudioGroupNameExists(string name)
        {
            for (int i = 0; i < this.m_AudioGroups.arraySize; i++)
            {
                SerializedProperty element = this.m_AudioGroups.GetArrayElementAtIndex(i);
                if (name == element.FindPropertyRelative("name").stringValue)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool UseDefaultMargins()
        {
            return false;
        }

        protected void CreateAudioGroupList()
        {
            this.m_AudioGroupList = new ReorderableList(serializedObject, this.m_AudioGroups, true, false, false, false);
            this.m_AudioGroupList.headerHeight = 0f;
            this.m_AudioGroupList.footerHeight = 0f;
            this.m_AudioGroupList.showDefaultBackground = false;
            float defaultHeight = this.m_AudioGroupList.elementHeight;
            float verticalOffset = (defaultHeight - EditorGUIUtility.singleLineHeight) * 0.5f;

            this.m_AudioGroupList.elementHeight = (defaultHeight + verticalOffset) * 2;
            this.m_AudioGroupList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.y = rect.y + verticalOffset;
                SerializedProperty element = this.m_AudioGroups.GetArrayElementAtIndex(index);
                if (!EditorGUIUtility.wideMode)
                {
                    EditorGUIUtility.wideMode = true;
                    EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 212;
                }
                EditorGUI.PropertyField(rect, element.FindPropertyRelative("name"));
                rect.y = rect.y + verticalOffset + defaultHeight;
                EditorGUI.PropertyField(rect, element.FindPropertyRelative("m_AudioMixerGroup"), true);

            };
            this.m_AudioGroupList.drawElementBackgroundCallback = (Rect rect, int index, bool isActive, bool isFocused) => {

                if (Event.current.type == EventType.Repaint)
                {
                    GUIStyle style = new GUIStyle("AnimItemBackground");
                    style.Draw(rect, false, isActive, isActive, isFocused);

                    GUIStyle style2 = new GUIStyle("RL Element");
                    style2.Draw(rect, false, isActive, isActive, isFocused);
                }
            };
        }

        private void AddGroup()
        {
           
            serializedObject.Update();
            this.m_AudioGroups.arraySize++;
            SerializedProperty property = this.m_AudioGroups.GetArrayElementAtIndex(this.m_AudioGroups.arraySize - 1);
            property.FindPropertyRelative("name").stringValue=this.m_AudioGroupName;
            serializedObject.ApplyModifiedProperties();
            this.m_AudioGroupName = string.Empty;
            this.m_AudioGroupList.index = this.m_AudioGroups.arraySize - 1;



        }
    }
}