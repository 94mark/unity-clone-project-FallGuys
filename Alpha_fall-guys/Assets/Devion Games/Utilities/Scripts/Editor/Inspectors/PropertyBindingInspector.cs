using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DevionGames{
	[CustomEditor(typeof(PropertyBinding))]
	public class PropertyBindingInspector : Editor {
		private SerializedProperty execution;
		private SerializedProperty interval;
		
		private void OnEnable(){
			execution = base.serializedObject.FindProperty ("m_Execution");
			interval = base.serializedObject.FindProperty ("m_Interval");
		}
		
		public override void OnInspectorGUI ()
		{
			base.serializedObject.Update ();
			EditorGUILayout.PropertyField (execution);
			if (execution.enumValueIndex == 4) {
				EditorGUI.indentLevel+=1;
				EditorGUILayout.PropertyField(interval);
				EditorGUI.indentLevel-=1;
			}
			Type sourceType = PropertyRefHint ("m_Source",typeof(void),false);
			Type targetType = PropertyRefHint ("m_Target",sourceType,true);
			if (sourceType !=null && targetType != null && !sourceType.IsAssignableFrom (targetType)) {
				EditorGUILayout.HelpBox("Unable to convert "+sourceType.Name+" to "+targetType.Name,MessageType.Error);
			}
			base.serializedObject.ApplyModifiedProperties();
		}
		
		private Type PropertyRefHint(string property, Type filter, bool requiresWrite){
			SerializedProperty baseProperty = base.serializedObject.FindProperty (property);
			SerializedProperty componentProperty = baseProperty.FindPropertyRelative ("m_Component");
			SerializedProperty propertyPath = baseProperty.FindPropertyRelative ("m_PropertyPath");
			
			EditorGUILayout.PropertyField (componentProperty, new GUIContent(property.Replace("m_","")));
			Component component = componentProperty.objectReferenceValue as Component;
			
			if (component != null) {
				Type propertyType=GetPropertyType(component.GetType(),propertyPath.stringValue);
				
				string current=string.IsNullOrEmpty(propertyPath.stringValue)?"<Missing>":component.GetType().Name+"."+propertyPath.stringValue;
				if(GUILayout.Button(current,"MiniPopup")){
					GenericMenu menu=new GenericMenu();
					List<Entry> list = GetProperties(component.gameObject,filter,requiresWrite);
					for(int i=0;i<list.Count;i++){
						Entry entry=list[i];
						string content=entry.ToString();
						menu.AddItem(new GUIContent(content),content==current,delegate {
							serializedObject.Update();
							componentProperty.objectReferenceValue=entry.target;
							propertyPath.stringValue=entry.name;
							serializedObject.ApplyModifiedProperties();
						});
					}
					menu.ShowAsContext();
				}
				return propertyType;
			}
			return typeof(void);
		}
		
		private Type GetPropertyType(Type type,string field){
			FieldInfo fieldInfo = type.GetField (field);
			if (fieldInfo != null) {
				return fieldInfo.FieldType;
			}
			PropertyInfo propertyInfo = type.GetProperty (field);
			if (propertyInfo != null) {
				return propertyInfo.PropertyType;
			}
			return typeof(void);
		} 
		
		private List<Entry> GetProperties (GameObject target, Type filter, bool requiresWrite)
		{
			Component[] comps = target.GetComponents<Component>();
			
			List<Entry> list = new List<Entry>();
			for (int i = 0, imax = comps.Length; i < imax; ++i)
			{
				Component comp = comps[i];
				
				Type type = comp.GetType();
				BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
				FieldInfo[] fields = type.GetFields(flags);
				PropertyInfo[] props = type.GetProperties(flags);
				
				for (int b = 0; b < fields.Length; ++b)
				{
					FieldInfo field = fields[b];
					if (filter != typeof(void) && !filter.IsAssignableFrom(field.FieldType)) continue;
					Entry ent = new Entry();
					ent.target = comp;
					ent.name = field.Name;
					list.Add(ent);
				}
				
				for (int b = 0; b < props.Length; ++b)
				{
					PropertyInfo prop = props[b];
					if (filter != typeof(void) && !filter.IsAssignableFrom(prop.PropertyType)) continue;
					if (!prop.CanRead) continue;
					if (requiresWrite && !prop.CanWrite) continue;
					
					Entry ent = new Entry();
					ent.target = comp;
					ent.name = prop.Name;
					list.Add(ent);
				}
			}
			return list;
		}
		
		public class Entry
		{
			public Component target;
			public string name;
			
			public override string ToString(){
				return target.GetType().Name + "/" + name;
			}
		}
	}
}
