using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.IO;
using System.Reflection;

namespace DevionGames
{
    public class AssetWindow : EditorWindow
    {
        protected UnityEngine.Object[] m_Targets;
        protected Vector2 m_ScrollPosition;
        protected List<Editor> m_Editors;
        protected string m_ElementPropertyPath;
        protected string m_ElementTypeName;

        protected Type m_ElementType;
        protected Type elementType {
            get {
                if (this.m_ElementType == null) {
                    this.m_ElementType = Utility.GetType(this.m_ElementTypeName);
                }
                return this.m_ElementType;
            }
            set {
                this.m_ElementType = value;
                this.m_ElementTypeName = this.m_ElementType.Name;
            }
        }
        protected UnityEngine.Object m_Target;
        protected static Component m_CopyComponent;
        protected bool m_ApplyToPrefab=false;
        protected bool m_HasPrefab;
        protected static Component[] m_CopyComponents;


        public static void ShowWindow(string title, SerializedProperty elements)
        {
            AssetWindow[] objArray = Resources.FindObjectsOfTypeAll<AssetWindow>();
            AssetWindow window = (objArray.Length <= 0 ? ScriptableObject.CreateInstance<AssetWindow>() : objArray[0]);
            window.hideFlags = HideFlags.HideAndDontSave;
            window.minSize = new Vector2(260f, 200f);
            window.titleContent = new GUIContent(title);
            window.m_ElementPropertyPath = elements.propertyPath;
            window.m_Target = elements.serializedObject.targetObject;
            window.m_Targets = new UnityEngine.Object[elements.arraySize];
            for (int i = 0; i < elements.arraySize; i++){
                window.m_Targets[i] = elements.GetArrayElementAtIndex(i).objectReferenceValue;
            }
            window.m_HasPrefab = PrefabUtility.GetNearestPrefabInstanceRoot(window.m_Target) != null;
            window.m_Editors = new List<Editor>();
            window.elementType = Utility.GetType(elements.arrayElementType.Replace("PPtr<$", "").Replace(">", ""));
            for (int i = 0; i < window.m_Targets.Length; i++)
            {
                Editor editor = Editor.CreateEditor(window.m_Targets[i]);
                window.m_Editors.Add(editor);
            }
            window.FixMissingAssets();
          //  EditorApplication.playModeStateChanged += OnPlaymodeStateChange;
            window.ShowUtility();
        }

        private void OnEnable()
        {
            AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnDisable()
        {
            AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }


        protected virtual void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            Close();
        }

        protected virtual void OnAfterAssemblyReload()
        {
            Close();
        }


        /*protected static void OnPlaymodeStateChange(PlayModeStateChange state) {
            AssetWindow[] objArray = Resources.FindObjectsOfTypeAll<AssetWindow>();
            for (int i = 0; i < objArray.Length; i++) {
                objArray[i].Close();
            }
        }*/

        protected virtual void OnGUI()
        {
            DoApplyToPrefab();
            this.m_ScrollPosition = EditorGUILayout.BeginScrollView(this.m_ScrollPosition);
            GUILayout.Space(1f);
            for (int i = 0; i < this.m_Targets.Length; i++)
            {
                UnityEngine.Object target = this.m_Targets[i];
                Editor editor = this.m_Editors[i];

                if (EditorTools.Titlebar(target, GetContextMenu(target)))
                {
                    EditorGUI.indentLevel += 1;
                    editor.OnInspectorGUI();
                    EditorGUI.indentLevel -= 1;
                }
            }
            EditorGUILayout.EndScrollView();
            GUILayout.FlexibleSpace();
            DoAddButton();
            GUILayout.Space(10f);
            DoCopyPaste();
        }



        protected virtual void DoApplyToPrefab() {
            if (this.m_HasPrefab && typeof(Component).IsAssignableFrom(this.m_Target.GetType()))
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label("Prefab Overrides: " + PrefabUtility.GetObjectOverrides((this.m_Target as Component).gameObject).Count);

                GUILayout.FlexibleSpace();

                if (!this.m_ApplyToPrefab)
                {
                    if (GUILayout.Button("Apply to prefab"))
                    {
                        PrefabUtility.ApplyPrefabInstance((this.m_Target as Component).gameObject, InteractionMode.AutomatedAction);
                    }
                }
                GUILayout.EndHorizontal();
            }
        }

        protected virtual void DoAddButton() {
            GUIStyle buttonStyle = new GUIStyle("AC Button");
            GUIContent buttonContent = new GUIContent("Add " + ObjectNames.NicifyVariableName(elementType.Name));
            Rect buttonRect = GUILayoutUtility.GetRect(buttonContent, buttonStyle, GUILayout.ExpandWidth(true));
            buttonRect.width = buttonStyle.fixedWidth;
            buttonRect.x = position.width * 0.5f - buttonStyle.fixedWidth * 0.5f;
            if (GUI.Button(buttonRect, buttonContent, buttonStyle))
            {
                AddObjectWindow.ShowWindow(buttonRect, elementType, AddAsset, CreateScript);
            }
        }

        protected virtual void RemoveTarget(int index) {
            DestroyImmediate(this.m_Editors[index]);
            this.m_Editors.RemoveAt(index);
            DestroyImmediate(this.m_Targets[index]);
            ArrayUtility.RemoveAt(ref this.m_Targets, index);

            SerializedObject serializedObject = new SerializedObject(this.m_Target);
            SerializedProperty elements = serializedObject.FindProperty(this.m_ElementPropertyPath);
            serializedObject.Update();
            elements.GetArrayElementAtIndex(index).objectReferenceValue = null;
            elements.DeleteArrayElementAtIndex(index);
            serializedObject.ApplyModifiedProperties();
        }


        protected virtual void DoCopyPaste() {

            Event currentEvent = Event.current;
            switch (currentEvent.rawType)
            {
                case EventType.KeyUp:
                    if (currentEvent.control)
                    {
                        if (currentEvent.keyCode == KeyCode.C)
                        {
                            AssetWindow.m_CopyComponents = this.m_Targets.Where(x => typeof(Component).IsAssignableFrom(x.GetType())).Select(y => y as Component).ToArray();
                        }
                        else if (currentEvent.keyCode == KeyCode.V && AssetWindow.m_CopyComponents != null && AssetWindow.m_CopyComponents.Length > 0)
                        {
                            for (int i = 0; i < this.m_Targets.Length; i++)
                            {
                                int index = i;
                                RemoveTarget(index);
                            }
                            for (int i = 0; i < AssetWindow.m_CopyComponents.Length; i++)
                            {
                                Component copy = AssetWindow.m_CopyComponents[i];
                                AddAsset(copy.GetType());
                                UnityEditorInternal.ComponentUtility.CopyComponent(copy);
                                UnityEditorInternal.ComponentUtility.PasteComponentValues((Component)this.m_Targets[i]);
                            }
                            if (this.m_HasPrefab && typeof(Component).IsAssignableFrom(this.m_Target.GetType()) && this.m_ApplyToPrefab)
                            {
                                PrefabUtility.ApplyPrefabInstance((this.m_Target as Component).gameObject, InteractionMode.AutomatedAction);
                            }
                        }
                    }
                    break;
            }

        }

        protected virtual void AddAsset(Type type) {
            SerializedObject serializedObject = new SerializedObject(this.m_Target);
            SerializedProperty elements = serializedObject.FindProperty(this.m_ElementPropertyPath);
           
            UnityEngine.Object element = null;
            if (this.m_Target is Component)
            {
                element = (this.m_Target as Component).gameObject.AddComponent(type);
            }

            element.hideFlags = HideFlags.HideInInspector;
            ArrayUtility.Add<UnityEngine.Object>(ref this.m_Targets, element);
            Editor editor = Editor.CreateEditor(element);
            this.m_Editors.Add(editor);
            serializedObject.Update();
            elements.arraySize++;
            elements.GetArrayElementAtIndex(elements.arraySize - 1).objectReferenceValue = element;
            serializedObject.ApplyModifiedProperties();

            this.m_ScrollPosition.y = float.MaxValue;
            if (this.m_HasPrefab && typeof(Component).IsAssignableFrom(this.m_Target.GetType()) && this.m_ApplyToPrefab)
            {
                PrefabUtility.ApplyPrefabInstance((this.m_Target as Component).gameObject, InteractionMode.AutomatedAction);
            }
            Focus();
        }

        protected virtual void CreateScript(string scriptName) {
            scriptName = scriptName.Replace(" ", "_");
            scriptName = scriptName.Replace("-", "_");
            string path = "Assets/" + scriptName + ".cs";

            if (File.Exists(path) == false)
            {
                using (StreamWriter outfile = new StreamWriter(path))
                {
                    MethodInfo[] methods = elementType.GetAllMethods();
                    methods = methods.Where(x => x.IsAbstract).ToArray();

                    outfile.WriteLine("using UnityEngine;");
                    outfile.WriteLine("using System.Collections;");
                    outfile.WriteLine("using "+elementType.Namespace+";");
                    outfile.WriteLine("");
                    if (!typeof(Component).IsAssignableFrom(elementType))
                    {
                        outfile.WriteLine("[System.Serializable]");
                    }
                    outfile.WriteLine("public class " + scriptName + " : "+elementType.Name+ "{");
                    for (int i = 0; i < methods.Length; i++)
                    {
                        MethodInfo method = methods[i];
                        ParameterInfo[] parameters = method.GetParameters();
                        string parameterString = string.Empty;
                        for (int j = 0; j < parameters.Length; j++) {
                            string typeName = parameters[j].ParameterType.Name;
                            string parameterName = string.Empty;
                            if (Char.IsLower(typeName, 0)) {
                                parameterName = "_" + typeName;
                            }else {
                               parameterName =  char.ToLowerInvariant(typeName[0]) + typeName.Substring(1);
                            }
                            parameterString += ", " + typeName + " " + parameterName;
                        }

                        if (!string.IsNullOrEmpty(parameterString)) {
                            parameterString = parameterString.Substring(1);
                        }

                        outfile.WriteLine("\t"+(method.IsPublic?"public":"protected")+" override "+ EditorTools.CovertToAliasString(method.ReturnType) +" "+method.Name  +"("+parameterString+") {");

                        if (method.ReturnType == typeof(string))
                        {
                            outfile.WriteLine("\t\treturn string.Empty;");
                        }
                        else if (method.ReturnType == typeof(bool))
                        {
                            outfile.WriteLine("\t\treturn true;");
                        }
                        else if (method.ReturnType == typeof(Vector2))
                        {
                            outfile.WriteLine("\t\treturn Vector2.zero;");
                        }
                        else if (method.ReturnType == typeof(Vector3))
                        {
                            outfile.WriteLine("\t\treturn Vector3.zero;");
                        }
                        else if (!method.ReturnType.IsValueType || method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            outfile.WriteLine("\t\treturn null;");
                        }
                        else if (UnityTools.IsInteger(method.ReturnType))
                        {
                            outfile.WriteLine("\t\treturn 0;");
                        }
                        else if (UnityTools.IsFloat(method.ReturnType))
                        {
                            outfile.WriteLine("\t\treturn 0.0f;");
                        }else if (method.ReturnType.IsEnum) {
                            outfile.WriteLine("\t\treturn "+method.ReturnType.Name+"."+ Enum.GetNames(method.ReturnType)[0] + ";");
                        } 

                        outfile.WriteLine("\t}");
                        outfile.WriteLine("");
                    }
                    outfile.WriteLine("}");
                }
            }
            AssetDatabase.Refresh();
            EditorPrefs.SetString("NewScriptToCreate", scriptName);
            EditorPrefs.SetInt("AssetWindowID", GetInstanceID());
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        protected static void OnScriptsReloaded()
        {
             string scriptName = EditorPrefs.GetString("NewScriptToCreate");
             int windowID = EditorPrefs.GetInt("AssetWindowID");
            if (string.IsNullOrEmpty(scriptName))
            {
                EditorPrefs.DeleteKey("NewScriptToCreate");
                EditorPrefs.DeleteKey("AssetWindowID");
                return;
            }

            Type type = Utility.GetType(scriptName);
             if (!EditorApplication.isPlayingOrWillChangePlaymode && !string.IsNullOrEmpty(scriptName) && type != null)
             {
                AssetWindow[] windows = Resources.FindObjectsOfTypeAll<AssetWindow>();
                AssetWindow window = windows.Where(x => x.GetInstanceID() == windowID).FirstOrDefault();
                if (window != null) {
                    window.AddAsset(type);
                }

             }
             EditorPrefs.DeleteKey("NewScriptToCreate");
             EditorPrefs.DeleteKey("AssetWindowID");
        }


        protected virtual GenericMenu GetContextMenu(UnityEngine.Object target) {
            GenericMenu menu = new GenericMenu();
            int index = Array.IndexOf(this.m_Targets,target);
            menu.AddItem(new GUIContent("Reset"), false, delegate {
                Type type = target.GetType();
                DestroyImmediate(target);
                this.m_Targets[index] = (this.m_Target as Component).gameObject.AddComponent(type);
                DestroyImmediate(this.m_Editors[index]);
                this.m_Editors[index] = Editor.CreateEditor(this.m_Targets[index]);
                SerializedObject serializedObject = new SerializedObject(this.m_Target);
                SerializedProperty elements = serializedObject.FindProperty(this.m_ElementPropertyPath);
                serializedObject.Update();
                elements.GetArrayElementAtIndex(index).objectReferenceValue = this.m_Targets[index];
                serializedObject.ApplyModifiedProperties();
                if (this.m_HasPrefab && typeof(Component).IsAssignableFrom(this.m_Target.GetType()) && this.m_ApplyToPrefab)
                {
                    PrefabUtility.ApplyPrefabInstance((this.m_Target as Component).gameObject, InteractionMode.AutomatedAction);
                }

            });
            menu.AddSeparator(string.Empty);
            menu.AddItem(new GUIContent("Remove"), false, delegate {
                DestroyImmediate(this.m_Editors[index]);
                this.m_Editors.RemoveAt(index);
                DestroyImmediate(target);
                ArrayUtility.RemoveAt(ref this.m_Targets, index);

                SerializedObject serializedObject = new SerializedObject(this.m_Target);
                SerializedProperty elements = serializedObject.FindProperty(this.m_ElementPropertyPath);
                serializedObject.Update();
                elements.GetArrayElementAtIndex(index).objectReferenceValue = null;
                elements.DeleteArrayElementAtIndex(index); 
                serializedObject.ApplyModifiedProperties();
                if (this.m_HasPrefab && typeof(Component).IsAssignableFrom(this.m_Target.GetType()) && this.m_ApplyToPrefab)
                {
                    PrefabUtility.ApplyPrefabInstance((this.m_Target as Component).gameObject, InteractionMode.AutomatedAction);
                }
            });

            menu.AddItem(new GUIContent("Copy"), false, delegate {
                m_CopyComponent = target as Component;
            });

            if (m_CopyComponent != null && m_CopyComponent.GetType() == target.GetType())
            {
                menu.AddItem(new GUIContent("Paste"), false, delegate {
                    UnityEditorInternal.ComponentUtility.CopyComponent(m_CopyComponent);
                    UnityEditorInternal.ComponentUtility.PasteComponentValues((Component)target);
                    if (this.m_HasPrefab && typeof(Component).IsAssignableFrom(this.m_Target.GetType()) && this.m_ApplyToPrefab)
                    {
                        PrefabUtility.ApplyPrefabInstance((this.m_Target as Component).gameObject, InteractionMode.AutomatedAction);
                    }
                });
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Paste"));
            }

            if (index > 0)
            {
                menu.AddItem(new GUIContent("Move Up"), false, delegate
                {
                    ArrayUtility.RemoveAt(ref this.m_Targets, index);
                    ArrayUtility.Insert(ref this.m_Targets, index - 1, target);
                    Editor editor = this.m_Editors[index];
                    this.m_Editors.RemoveAt(index);
                    this.m_Editors.Insert(index-1,editor);

                    SerializedObject serializedObject = new SerializedObject(this.m_Target);
                    SerializedProperty elements = serializedObject.FindProperty(this.m_ElementPropertyPath);
                    serializedObject.Update();
                    elements.MoveArrayElement(index,index-1);
                    serializedObject.ApplyModifiedProperties();
                    if (this.m_HasPrefab && typeof(Component).IsAssignableFrom(this.m_Target.GetType()) && this.m_ApplyToPrefab)
                    {
                        PrefabUtility.ApplyPrefabInstance((this.m_Target as Component).gameObject, InteractionMode.AutomatedAction);
                    }
                });
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Move Up"));
            }
            if (index < this.m_Targets.Length - 1)
            {
                menu.AddItem(new GUIContent("Move Down"), false, delegate
                {
                    ArrayUtility.RemoveAt(ref this.m_Targets, index);
                    ArrayUtility.Insert(ref this.m_Targets, index + 1, target);
                    Editor editor = this.m_Editors[index];
                    this.m_Editors.RemoveAt(index);
                    this.m_Editors.Insert(index + 1, editor);

                    SerializedObject serializedObject = new SerializedObject(this.m_Target);
                    SerializedProperty elements = serializedObject.FindProperty(this.m_ElementPropertyPath);
                    serializedObject.Update();
                    elements.MoveArrayElement(index, index + 1);
                    serializedObject.ApplyModifiedProperties();
                    if (this.m_HasPrefab && typeof(Component).IsAssignableFrom(this.m_Target.GetType()) && this.m_ApplyToPrefab)
                    {
                        PrefabUtility.ApplyPrefabInstance((this.m_Target as Component).gameObject, InteractionMode.AutomatedAction);
                    }
                });
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Move Down"));
            }
            return menu;
        }

        protected void FixMissingAssets() {

            //Component added manually
            if (typeof(Component).IsAssignableFrom(this.m_Target.GetType()))
            {
                Component[] components = (this.m_Target as Component).GetComponents(elementType);
                components = components.Where(x => !this.m_Targets.Contains(x)).ToArray();
                for (int i = 0; i < components.Length; i++)
                {
                    ArrayUtility.Add<UnityEngine.Object>(ref this.m_Targets, components[i]);
                    Editor editor = Editor.CreateEditor(components[i]);
                    this.m_Editors.Add(editor);
                    SerializedObject serializedObject = new SerializedObject(this.m_Target);
                    SerializedProperty elements = serializedObject.FindProperty(this.m_ElementPropertyPath);
                    serializedObject.Update();
                    elements.arraySize++;
                    elements.GetArrayElementAtIndex(elements.arraySize - 1).objectReferenceValue = components[i];
                    serializedObject.ApplyModifiedProperties();
                    if (this.m_HasPrefab && typeof(Component).IsAssignableFrom(this.m_Target.GetType()) && this.m_ApplyToPrefab)
                    {
                        PrefabUtility.ApplyPrefabInstance((this.m_Target as Component).gameObject, InteractionMode.AutomatedAction);
                    }
                }
            }
            //Component removed manually
            for (int i = 0; i < this.m_Targets.Length; i++) {
                if (this.m_Targets[i] == null) {
                    DestroyImmediate(this.m_Editors[i]);
                    this.m_Editors.RemoveAt(i);
                    ArrayUtility.RemoveAt(ref this.m_Targets, i);

                    SerializedObject serializedObject = new SerializedObject(this.m_Target);
                    SerializedProperty elements = serializedObject.FindProperty(this.m_ElementPropertyPath);
                    serializedObject.Update();
                    elements.GetArrayElementAtIndex(i).objectReferenceValue = null;
                    elements.DeleteArrayElementAtIndex(i);
                    serializedObject.ApplyModifiedProperties();
                    if (this.m_HasPrefab && typeof(Component).IsAssignableFrom(this.m_Target.GetType()) && this.m_ApplyToPrefab)
                    {
                        PrefabUtility.ApplyPrefabInstance((this.m_Target as Component).gameObject, InteractionMode.AutomatedAction);
                    }
                }
            }
        }


        protected virtual void Update()
        {
            Repaint();
        }

        protected virtual void OnDestroy()
        {
            for (int i = this.m_Editors.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(this.m_Editors[i]);
            }
        }

    }
}