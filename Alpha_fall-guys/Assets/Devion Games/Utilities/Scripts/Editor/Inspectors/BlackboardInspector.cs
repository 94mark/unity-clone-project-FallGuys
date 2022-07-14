using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DevionGames
{
    [CustomEditor(typeof(Blackboard),true)]
    public class BlackboardInspector : Editor
    {
        protected SerializedProperty m_Variables;
        private ReorderableList m_VariableList;
        private string m_VariableName = string.Empty;

        protected virtual void OnEnable() {
            if (target == null) return;
            this.m_Variables = serializedObject.FindProperty("m_Variables");
            CreateVariableList();
        }

        public override void OnInspectorGUI(){
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            EditorGUILayout.BeginVertical();
            serializedObject.Update();
            this.m_VariableList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.EndVertical();
            GUILayout.Space(-4.5f);

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUIStyle textStyle = new GUIStyle(EditorStyles.textField);
            textStyle.margin = new RectOffset(0,0,1,1);
            textStyle.alignment = TextAnchor.MiddleRight;
            this.m_VariableName = EditorGUILayout.TextField(this.m_VariableName,textStyle);
            if (string.IsNullOrEmpty(this.m_VariableName))
            {
                Rect variableNameRect = GUILayoutUtility.GetLastRect();
                GUIStyle variableNameOverlayStyle = new GUIStyle(EditorStyles.label);
                variableNameOverlayStyle.alignment = TextAnchor.MiddleRight;
                variableNameOverlayStyle.normal.textColor = Color.grey;
                GUI.Label(variableNameRect, "(New Variable Name)",variableNameOverlayStyle);
            }
            GUIStyle createAddNewDropDown = new GUIStyle("ToolbarCreateAddNewDropDown");

            if (GUILayout.Button(EditorGUIUtility.IconContent("d_Toolbar Plus"), createAddNewDropDown, GUILayout.Width(35f)))
            {
                
                if (string.IsNullOrEmpty(this.m_VariableName))
                {
                    EditorUtility.DisplayDialog("New Variable", "Please enter a variable name.", "OK");
                }else if (VariableNameExists(this.m_VariableName)){
                    EditorUtility.DisplayDialog("New Variable", "A variable with the same name already exists.", "OK");
                }
                else
                {
                    Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(x => typeof(Variable).IsAssignableFrom(x) && !x.IsAbstract && !x.HasAttribute(typeof(ExcludeFromCreation))).ToArray();
                    types = types.OrderBy(x => x.BaseType.Name).ToArray();

                    GenericMenu menu = new GenericMenu();
                    for (int i = 0; i < types.Length; i++)
                    {
                        Type type = types[i];
                        menu.AddItem(new GUIContent(ObjectNames.NicifyVariableName( type.Name.Replace("Variable",""))), false, () => { AddVariable(type); });
                    }
                    menu.ShowAsContext();
                }
                EditorGUI.FocusTextInControl("");
            }

            EditorGUI.BeginDisabledGroup(this.m_VariableList.index == -1);
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_Toolbar Minus"), EditorStyles.toolbarButton, GUILayout.Width(25f)))
            {
                this.serializedObject.Update();
                this.m_Variables.DeleteArrayElementAtIndex(this.m_VariableList.index);
                this.serializedObject.ApplyModifiedProperties();
                this.m_VariableList.index = this.m_Variables.arraySize - 1;
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
        }

        private bool VariableNameExists(string name) {
            for (int i = 0; i < this.m_Variables.arraySize;i++) {
                SerializedProperty element = this.m_Variables.GetArrayElementAtIndex(i);
                if (name == element.FindPropertyRelative("m_Name").stringValue) {
                    return true;
                }
            }
            return false;
        }

        public override bool UseDefaultMargins()
        {
            return false;
        }

        protected void CreateVariableList() {
            this.m_VariableList = new ReorderableList(serializedObject, this.m_Variables, true, false, false, false);
            this.m_VariableList.headerHeight = 0f;
            this.m_VariableList.footerHeight = 0f;
            this.m_VariableList.showDefaultBackground = false;
            float defaultHeight = this.m_VariableList.elementHeight;
            float verticalOffset = (defaultHeight - EditorGUIUtility.singleLineHeight) * 0.5f;

            this.m_VariableList.elementHeight = (defaultHeight+verticalOffset)*2;
            this.m_VariableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.y = rect.y + verticalOffset;
                SerializedProperty element = this.m_Variables.GetArrayElementAtIndex(index);
                if (!EditorGUIUtility.wideMode)
                {
                    EditorGUIUtility.wideMode = true;
                    EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 212;
                }
                EditorGUI.PropertyField(rect, element.FindPropertyRelative("m_Name"));
                rect.y = rect.y +verticalOffset+defaultHeight;

                SerializedProperty elementValue = element.FindPropertyRelative("m_Value");
                if (elementValue != null)
                {
                    EditorGUI.PropertyField(rect, elementValue, true);
                }
                else {
                    EditorGUI.LabelField(rect,"Runtime Value");
                }
    
            };
            this.m_VariableList.drawElementBackgroundCallback = (Rect rect, int index, bool isActive, bool isFocused) => {

                if (Event.current.type == EventType.Repaint)
                {
                    GUIStyle style = new GUIStyle("AnimItemBackground");
                    style.Draw(rect, false, isActive, isActive, isFocused);

                    GUIStyle style2 = new GUIStyle("RL Element");
                    style2.Draw(rect, false, isActive, isActive, isFocused);
                }
            };

            this.m_VariableList.onAddCallback = (ReorderableList list) => {

                Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(x => typeof(Variable).IsAssignableFrom(x) && !x.IsAbstract && !x.HasAttribute(typeof(ExcludeFromCreation))).ToArray();
                types = types.OrderBy(x => x.BaseType.Name).ToArray();

                GenericMenu menu = new GenericMenu();
                for (int i = 0; i < types.Length; i++)
                {
                    Type type = types[i];
                    menu.AddItem(new GUIContent(type.Name), false, () => { AddVariable(type); });
                }
                menu.ShowAsContext();
            };

        }

        private void AddVariable(Type type)
        {
            Variable value = Activator.CreateInstance(type) as Variable;
            value.name = this.m_VariableName;
            serializedObject.Update();
            this.m_Variables.arraySize++;
            this.m_Variables.GetArrayElementAtIndex(this.m_Variables.arraySize - 1).managedReferenceValue = value;
            serializedObject.ApplyModifiedProperties();
            this.m_VariableName = string.Empty;
            this.m_VariableList.index = this.m_Variables.arraySize-1;
     


        }

 
    }
}