using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevionGames
{
    [System.Serializable]
    public abstract class ObjectCollectionEditor<T> : CollectionEditor<T> where T : INameable
    {
        protected string m_ToolbarName;

        protected int m_TargetInstanceID;
        protected string m_SerializedPropertyPath;
        protected SerializedObject m_SerializedObject;
        protected SerializedProperty m_SerializedProperty;

        public override string ToolbarName => this.m_ToolbarName;

        protected override List<T> Items => this.m_SerializedProperty.GetValue() as List<T>;


        public ObjectCollectionEditor(SerializedObject serializedObject, SerializedProperty serializedProperty) : this(ObjectNames.NicifyVariableName(typeof(T).Name+"s"), serializedObject, serializedProperty)
        {
        }

        public ObjectCollectionEditor(string toolbar, SerializedObject serializedObject, SerializedProperty serializedProperty) {
            this.m_SerializedObject = serializedObject;
            this.m_SerializedProperty = serializedProperty;
            this.m_TargetInstanceID = serializedObject.targetObject.GetInstanceID();
            this.m_SerializedPropertyPath = serializedProperty.propertyPath;
            this.m_ToolbarName = toolbar;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        protected override void DrawItem(T item)
        {
            int index = Items.IndexOf(item);
            this.m_SerializedObject.Update();

            SerializedProperty element = this.m_SerializedProperty.GetArrayElementAtIndex(index);
            object value = element.GetValue();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", value != null ? EditorTools.FindMonoScript(value.GetType()) : null, typeof(MonoScript), true);
            EditorGUI.EndDisabledGroup();
         
            foreach (var child in element.EnumerateChildProperties())
            {
                EditorGUILayout.PropertyField(
                    child,
                    includeChildren: true
                );
                EditorGUI.EndDisabledGroup();
            }

            this.m_SerializedObject.ApplyModifiedProperties();
        }

        protected override void Create()
        {
            T value = (T)System.Activator.CreateInstance(typeof(T));
            this.m_SerializedObject.Update();
            this.m_SerializedProperty.arraySize++;
            this.m_SerializedProperty.GetArrayElementAtIndex(this.m_SerializedProperty.arraySize - 1).managedReferenceValue = value;
            this.m_SerializedObject.ApplyModifiedProperties();
        }

        protected override void Remove(T item)
        {
            this.m_SerializedObject.Update();
            int index = Items.IndexOf(item);
            this.m_SerializedProperty.DeleteArrayElementAtIndex(index);
            this.m_SerializedObject.ApplyModifiedProperties();
        }

        protected override void Duplicate(T item)
        {
            T duplicate = (T)EditorTools.Duplicate(item);
            this.m_SerializedObject.Update();
            this.m_SerializedProperty.arraySize++;
            this.m_SerializedProperty.GetArrayElementAtIndex(this.m_SerializedProperty.arraySize - 1).managedReferenceValue = duplicate;
            this.m_SerializedObject.ApplyModifiedProperties();
        }

        protected override string GetSidebarLabel(T item)
        {
            return item.Name;
        }

        protected override bool MatchesSearch(T item, string search)
        {
            return (item.Name.ToLower().Contains(search.ToLower()) || search.ToLower() == item.GetType().Name.ToLower());
        }

        private void OnPlayModeStateChanged(PlayModeStateChange playModeStateChange)
        {
            this.m_SerializedObject = new SerializedObject(EditorUtility.InstanceIDToObject(this.m_TargetInstanceID));
            this.m_SerializedProperty = this.m_SerializedObject.FindProperty(this.m_SerializedPropertyPath);
        }

        private void OnAfterAssemblyReload()
        {
            this.m_SerializedObject = new SerializedObject(EditorUtility.InstanceIDToObject(this.m_TargetInstanceID));
            this.m_SerializedProperty = this.m_SerializedObject.FindProperty(this.m_SerializedPropertyPath);
        }

    }
}