using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEditorInternal;

namespace DevionGames{
	[CustomEditor(typeof(CallbackHandler), true)]
	public class CallbackHandlerInspector : Editor {
		protected GUIContent addButtonContent;
		protected GUIContent[] eventCallbackTypes;
		protected SerializedProperty m_DelegatesProperty;
		protected GUIContent eventCallbackName;
		protected GUIContent iconToolbarMinus;

        protected SerializedProperty m_Script;

        protected Dictionary<Type, string[]> m_ClassProperties;
        protected string[] m_PropertiesToExcludeForChildClasses;
        protected List<System.Action> m_DrawInspectors;

        protected virtual void OnEnable(){
            this.m_DrawInspectors = new List<System.Action>();
            List<string> propertiesToExclude = new List<string>();
            m_ClassProperties = new Dictionary<Type, string[]>();
            propertiesToExclude.Add("m_Script");
            Type[] subInspectors = Utility.BaseTypesAndSelf(GetType()).Where(x=>x.IsSubclassOf(typeof(CallbackHandlerInspector))).ToArray();
            Array.Reverse(subInspectors);


            for (int i = 0; i < subInspectors.Length; i++){

                MethodInfo method = subInspectors[i].GetMethod("DrawInspector", BindingFlags.NonPublic | BindingFlags.Instance);
                Type inspectedType = typeof(CustomEditor).GetField("m_InspectedType", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(subInspectors[i].GetCustomAttribute<CustomEditor>()) as Type;
                FieldInfo[] fields = inspectedType.GetAllSerializedFields().Where(x => !x.HasAttribute(typeof(HideInInspector))).ToArray();

                string[] classProperties = fields.Where(x => x.DeclaringType == inspectedType).Select(x => x.Name).ToArray();
               

                if (!this.m_ClassProperties.ContainsKey(inspectedType)) {
                    this.m_ClassProperties.Add(inspectedType, classProperties);
                }
                propertiesToExclude.AddRange(classProperties);


                if (method != null) {
                    m_DrawInspectors.Add(delegate { method.Invoke(this, null); });
                }else {
                    m_DrawInspectors.Add(delegate() {
                        for (int j = 0; j < classProperties.Length; j++) {
                            SerializedProperty property = serializedObject.FindProperty(classProperties[j]);
                            EditorGUILayout.PropertyField(property);
                        }

                    });
                }
            }

            this.m_PropertiesToExcludeForChildClasses = propertiesToExclude.ToArray();
           
            this.m_Script = base.serializedObject.FindProperty("m_Script");
            this.m_DelegatesProperty = base.serializedObject.FindProperty("delegates");
			this.addButtonContent = new GUIContent ("Add New Callback");
			this.eventCallbackName = new GUIContent(string.Empty);
			string[] names = (target as CallbackHandler).Callbacks;
			this.eventCallbackTypes = new GUIContent[(int)names.Length];
			for (int i = 0; i < (int)names.Length; i++)
			{
				this.eventCallbackTypes[i] = new GUIContent(names[i]);
			}
			this.iconToolbarMinus = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"))
			{
				tooltip = "Remove all events in this list."
			};
        }

        protected virtual void OnDestroy() {
        }

        public override void OnInspectorGUI ()
		{
            ScriptGUI();
            serializedObject.Update();
            for (int i = 0; i < m_DrawInspectors.Count; i++) {
                this.m_DrawInspectors[i].Invoke();
            }
           
            DrawPropertiesExcluding(serializedObject,this.m_PropertiesToExcludeForChildClasses);
            TriggerGUI();
			serializedObject.ApplyModifiedProperties();
		}

        protected void DrawTypePropertiesExcluding(Type type, params string[] propertyToExclude)
        {
            string[] propertiesToDraw;
            if (this.m_ClassProperties.TryGetValue(type, out propertiesToDraw))
            {

                for (int i = 0; i < propertiesToDraw.Length; i++)
                {

                    if (!propertyToExclude.Contains(propertiesToDraw[i]))
                    {

                        SerializedProperty property = serializedObject.FindProperty(propertiesToDraw[i]);
                        EditorGUILayout.PropertyField(property);
                    }

                }
            }
        }

        protected void DrawClassPropertiesExcluding(params string[] propertyToExclude) {
            string[] propertiesToDraw = new string[0];
            if (this.m_ClassProperties.TryGetValue(target.GetType(), out propertiesToDraw)){
            
                for (int i = 0; i < propertiesToDraw.Length; i++) {

                    if (!propertyToExclude.Contains(propertiesToDraw[i]))
                    {

                        SerializedProperty property = serializedObject.FindProperty(propertiesToDraw[i]);
                        EditorGUILayout.PropertyField(property);
                    }
                    
                }
            }
        }

        protected void ScriptGUI() {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(this.m_Script);
            EditorGUI.EndDisabledGroup();    
        }

        protected void TriggerGUI() {
            int num = -1;
            EditorGUILayout.Space();
            Vector2 vector2 = GUIStyle.none.CalcSize(this.iconToolbarMinus);
            for (int i = 0; i < this.m_DelegatesProperty.arraySize; i++)
            {
                SerializedProperty arrayElementAtIndex = this.m_DelegatesProperty.GetArrayElementAtIndex(i);
                SerializedProperty serializedProperty = arrayElementAtIndex.FindPropertyRelative("eventID");
                SerializedProperty serializedProperty1 = arrayElementAtIndex.FindPropertyRelative("callback");
                this.eventCallbackName.text = serializedProperty.stringValue;

                EditorGUILayout.PropertyField(serializedProperty1, this.eventCallbackName, new GUILayoutOption[0]);
                Rect lastRect = GUILayoutUtility.GetLastRect();
                Rect rect = new Rect(lastRect.xMax - vector2.x - 8f, lastRect.y + 1f, vector2.x, vector2.y);
                if (GUI.Button(rect, this.iconToolbarMinus, GUIStyle.none))
                {
                    num = i;
                }
                EditorGUILayout.Space();
            }
            if (num > -1)
            {
                this.RemoveEntry(num);
            }
            Rect rect1 = GUILayoutUtility.GetRect(this.addButtonContent, GUI.skin.button);
            rect1.x = rect1.x + (rect1.width - 200f) / 2f;
            rect1.width = 200f;
            if (GUI.Button(rect1, this.addButtonContent))
            {
                this.ShowAddEventmenu();
            }
        }
		
		private void ShowAddEventmenu(){
			GenericMenu genericMenu = new GenericMenu();
			for (int i = 0; i < (int)this.eventCallbackTypes.Length; i++)
			{
				bool flag = true;
				for (int j = 0; j < this.m_DelegatesProperty.arraySize; j++)
				{
					if (this.m_DelegatesProperty.GetArrayElementAtIndex(j).FindPropertyRelative("eventID").stringValue == eventCallbackTypes[i].text)
					{
						flag = false;
					}
				}
				if (!flag)
				{
					genericMenu.AddDisabledItem(this.eventCallbackTypes[i]);
				}
				else
				{
					genericMenu.AddItem(this.eventCallbackTypes[i], false, new GenericMenu.MenuFunction2(this.OnAddNewSelected), eventCallbackTypes[i].text);
				}
			}
			genericMenu.ShowAsContext();
			Event.current.Use();
		}
		
		private void OnAddNewSelected(object eventID ){
			string id = (string)eventID;
			SerializedProperty mDelegatesProperty = this.m_DelegatesProperty;
			mDelegatesProperty.arraySize = mDelegatesProperty.arraySize + 1;
			SerializedProperty arrayElementAtIndex = this.m_DelegatesProperty.GetArrayElementAtIndex(this.m_DelegatesProperty.arraySize - 1);
			arrayElementAtIndex.FindPropertyRelative("eventID").stringValue = id;
			base.serializedObject.ApplyModifiedProperties();
		}
		
		private void RemoveEntry(int index)
		{
			this.m_DelegatesProperty.DeleteArrayElementAtIndex(index);
		}
	}
}