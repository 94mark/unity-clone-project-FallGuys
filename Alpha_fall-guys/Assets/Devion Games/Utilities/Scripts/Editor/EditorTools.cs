using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Reflection;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DevionGames{
	/// <summary>
	/// Editor helper class.
	/// </summary>
	public static class EditorTools {
        private static Dictionary<Type, CustomDrawer> m_Drawers;
        private static Dictionary<Type, EditorTools.DrawerKeySet> m_DrawerTypeForType;
        private static Dictionary<Type, MonoScript> m_TypeMonoScriptLookup;
        private static Dictionary<Type, bool> m_CustomPropertyDrawerLookup;

        static EditorTools() {
            EditorTools.m_TypeMonoScriptLookup = new Dictionary<Type, MonoScript>();
            EditorTools.m_Drawers = new Dictionary<Type, CustomDrawer>();
            EditorTools.m_CustomPropertyDrawerLookup = new Dictionary<Type, bool>();
        }

        public static string SearchField(string search,bool focus = false, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal();
            string before = search;

            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, "ToolbarSeachTextField", options);
            rect.x += 2f;
            rect.width -= 2f;
            Rect buttonRect = rect;
            buttonRect.x = rect.width - 14;
            buttonRect.width = 14;

            if (!String.IsNullOrEmpty(before))
                EditorGUIUtility.AddCursorRect(buttonRect, MouseCursor.Arrow);

            if (Event.current.type == EventType.MouseUp && buttonRect.Contains(Event.current.mousePosition) || before == "Search..." && GUI.GetNameOfFocusedControl() == "SearchTextFieldFocus")
            {
                before = "";
                GUI.changed = true;
                GUI.FocusControl(null);

            }
            GUI.SetNextControlName("SearchTextFieldFocus");
            GUIStyle style = new GUIStyle("ToolbarSeachTextField");
            if (before == "Search...")
            {
                style.normal.textColor = Color.gray;
                style.hover.textColor = Color.gray;
            }
            string after = EditorGUI.TextField(rect, "", before, style);
            if(focus)
                EditorGUI.FocusTextInControl("SearchTextFieldFocus");

            GUI.Button(buttonRect, GUIContent.none, (after != "" && after != "Search...") ? "ToolbarSeachCancelButton" : "ToolbarSeachCancelButtonEmpty");
            EditorGUILayout.EndHorizontal();
            return after;
        }

        /// <summary>
        /// Search field gui.
        /// </summary>
        /// <returns>The field.</returns>
        /// <param name="search">Search.</param>
        /// <param name="options">Options.</param>
        public static string[] SearchField(string search,string filter,List<string> filters,params GUILayoutOption[] options){
			GUILayout.BeginHorizontal ();
			string[] result = new string[]{filter,search};
			string before = search;

			Rect rect = GUILayoutUtility.GetRect (GUIContent.none,(GUIStyle)"ToolbarSeachTextFieldPopup",options);
            rect.x += 2f;
            rect.width -= 2f;
            Rect buttonRect = rect;
            buttonRect.x = rect.width - 14;
            buttonRect.width = 14;
            if (!String.IsNullOrEmpty(before))
                EditorGUIUtility.AddCursorRect(buttonRect, MouseCursor.Arrow);


            if (Event.current.type == EventType.MouseUp && buttonRect.Contains(Event.current.mousePosition) || before == "Search..." && GUI.GetNameOfFocusedControl() == "SearchTextFieldFocus")
            {
                before = "";
                GUI.changed = true;
                GUI.FocusControl(null);
            }

            GUIStyle style = new GUIStyle("ToolbarSeachTextField");
            if (before == "Search...")
            {
                style.normal.textColor = Color.gray;
                style.hover.textColor = Color.gray;
            }
            //  string after = EditorGUI.TextField(rect, "", before, style);
            Rect rect1 = GUILayoutUtility.GetLastRect();
            rect1.width = 20;

            int filterIndex = filters.IndexOf(filter);
            filterIndex = EditorGUI.Popup(rect1, filterIndex, filters.ToArray(), "label");
            if (filterIndex != -1)
            {
                result[0] = filters[filterIndex];
                if (filters.Contains(search))
                {
                    before = result[0];
                }
            }
            string after = EditorGUI.TextField(rect, "", before, (GUIStyle)"ToolbarSeachTextFieldPopup");

            GUI.Button(buttonRect, GUIContent.none, (after != "" && after != "Search...") ? "ToolbarSeachCancelButton" : "ToolbarSeachCancelButtonEmpty");
            EditorGUILayout.EndHorizontal();
            result[1] = after;
            return result;
		}

        public static bool LeftButton(GUIContent content, params GUILayoutOption[] options)
        {
            if (GUILayout.Button(content, Styles.leftTextButton, options))
            {
                return true;
            }
            return false;
        }

        public static bool RightArrowButton(GUIContent content, params GUILayoutOption[] options)
        {
            bool result = false;
            if (GUILayout.Button(content, Styles.leftTextButton, options))
            {
                result = true;
            }
            Rect rect = GUILayoutUtility.GetLastRect();
            rect.x += rect.width - 20f;
            GUI.Label(rect, Styles.rightArrow);
            return result;
        }

        public static bool RightArrowToolbarButton(GUIContent content, params GUILayoutOption[] options)
        {
            bool result = false;
            if (GUILayout.Button(content, Styles.leftTextToolbarButton, options))
            {
                result = true;
            }
            Rect rect = GUILayoutUtility.GetLastRect();
            rect.x += rect.width - 20f;
            GUI.Label(rect, Styles.rightArrow);
            return result;
        }

        public static bool RightArrowButton(Rect position, GUIContent content)
        {
            bool result = false;
            if (GUI.Button(position,content, Styles.leftTextButton))
            {
                result = true;
            }
            position.x += position.width - 20f;
            GUI.Label(position, Styles.rightArrow);
            return result;
        }

        public static void Seperator() {
            GUILayout.Label(GUIContent.none,Styles.seperator);
        }

        public static void BeginIndent(int indent, bool fold = false)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(indent * 15f - (fold ? 2f : 0f));
            GUILayout.BeginVertical();
        }

        public static void EndIndent()
        {
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

        }

        public static LayerMask LayerMaskField(GUIContent label, LayerMask layerMask, params GUILayoutOption[] options)
        {
            List<string> layers = new List<string>();
            List<int> layerNumbers = new List<int>();

            for (int i = 0; i < 32; i++)
            {
                string layerName = LayerMask.LayerToName(i);
                if (layerName != "")
                {
                    layers.Add(layerName);
                    layerNumbers.Add(i);
                }
            }
            int maskWithoutEmpty = 0;
            for (int i = 0; i < layerNumbers.Count; i++)
            {
                if (((1 << layerNumbers[i]) & layerMask.value) > 0)
                    maskWithoutEmpty |= (1 << i);
            }
            maskWithoutEmpty = EditorGUILayout.MaskField(label, maskWithoutEmpty, layers.ToArray(), options);
            int mask = 0;
            for (int i = 0; i < layerNumbers.Count; i++)
            {
                if ((maskWithoutEmpty & (1 << i)) > 0)
                    mask |= (1 << layerNumbers[i]);
            }
            layerMask.value = mask;
            return layerMask;
        }

        public static LayerMask LayerMaskField(Rect rect,GUIContent label, LayerMask layerMask)
        {
            List<string> layers = new List<string>();
            List<int> layerNumbers = new List<int>();

            for (int i = 0; i < 32; i++)
            {
                string layerName = LayerMask.LayerToName(i);
                if (layerName != "")
                {
                    layers.Add(layerName);
                    layerNumbers.Add(i);
                }
            }
            int maskWithoutEmpty = 0;
            for (int i = 0; i < layerNumbers.Count; i++)
            {
                if (((1 << layerNumbers[i]) & layerMask.value) > 0)
                    maskWithoutEmpty |= (1 << i);
            }
            maskWithoutEmpty = EditorGUI.MaskField(rect,label, maskWithoutEmpty, layers.ToArray());
            int mask = 0;
            for (int i = 0; i < layerNumbers.Count; i++)
            {
                if ((maskWithoutEmpty & (1 << i)) > 0)
                    mask |= (1 << layerNumbers[i]);
            }
            layerMask.value = mask;
            return layerMask;
        }

        public static bool Foldout(string hash, GUIContent content)
        {
            return Foldout(hash, content,null, EditorStyles.foldout);
        }

        public static bool Foldout(string hash, GUIContent content, GenericMenu context)
        {
            return Foldout(hash, content, context, EditorStyles.foldout);
        }

        public static bool Foldout(string hash, GUIContent content, GenericMenu context, GUIStyle style)
        {
            bool foldout = EditorPrefs.GetBool("Fold" + hash, true);

            bool flag = EditorGUILayout.Foldout(foldout, content, style);
            if (context != null)
            {
                Rect rect = GUILayoutUtility.GetLastRect();
                if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && rect.Contains(Event.current.mousePosition))
                {
                    context.ShowAsContext();
                }
            }
            if (flag != foldout)
            {
                EditorPrefs.SetBool("Fold" + hash, flag);
            }

            return flag;
        }
       
        public static bool Titlebar(object target)
        {
            GUIContent title = new GUIContent("Missing Script");
            if (target != null) {
                title = new GUIContent(ObjectNames.NicifyVariableName(target.GetType().Name));
            }
            return Titlebar(target != null ? target.GetHashCode().ToString() : "",title, target, null);
        }

        public static bool Titlebar(object target, GenericMenu menu)
        {
            return Titlebar(target != null ? target.GetHashCode().ToString() : "", target, menu);
        }

        public static bool Titlebar(string hash, object target, GenericMenu menu)
        {
            GUIContent title = new GUIContent("Missing Script");
            if (target != null)
            {
                title = new GUIContent(ObjectNames.NicifyVariableName(target.GetType().Name));
            }
            return Titlebar(hash, title, target, menu);
        }

        public static bool Titlebar(string hash, GUIContent content, object target, GenericMenu menu)
        {

            int controlID = EditorGUIUtility.GetControlID(FocusType.Passive);

             Rect position = GUILayoutUtility.GetRect(GUIContent.none, Styles.inspectorTitle, GUILayout.ExpandWidth(true));
             if (Event.current.type == EventType.Repaint)
             {
                 Color color = GUI.color;
                 GUI.color = position.Contains(Event.current.mousePosition) ? color * 1.3f : color * 1.1f;
                 Styles.inspectorBigTitle.Draw(new Rect(position.x, position.y, position.width + 4f, position.height), GUIContent.none, controlID, false);
                 GUI.color = color;
             }

             Rect rect = new Rect(position.x + (float)Styles.inspectorTitle.padding.left, position.y + (float)Styles.inspectorTitle.padding.top, 16f, 16f);
             Rect rect1 = new Rect(position.xMax - (float)Styles.inspectorTitle.padding.right - 2f - 16f, rect.y, 16f, 16f);
             Rect rect4 = rect1;
             rect4.x = rect4.x - 18f;

             Rect rect2 = new Rect(position.x + 2f + 2f + 16f * 3, rect.y, 100f, rect.height)
             {
                 xMax = rect4.xMin - 2f
             };

             Rect rect3 = new Rect(position.x + 16f, rect.y, 20f, 20f);
             Texture2D icon = EditorGUIUtility.FindTexture("cs Script Icon");
             if (target != null)
             {
                 IconAttribute iconAttribute = target.GetType().GetCustomAttribute<IconAttribute>();
                 if (iconAttribute != null)
                 {
                     if (iconAttribute.type != null)
                     {
                         icon = AssetPreview.GetMiniTypeThumbnail(iconAttribute.type);
                     }
                     else
                     {
                         icon = Resources.Load<Texture2D>(iconAttribute.path);
                     }
                 }
             }
             GUI.Label(new Rect(position.x + 13f, rect.y, 18f, 18f), icon);
             Rect rect5 = rect3;
             rect5.x = rect5.x + 16f;
             if (target != null)
             {
                 if (typeof(MonoBehaviour).IsAssignableFrom(target.GetType()))
                 {
                     MonoBehaviour behaviour = target as MonoBehaviour;
                     behaviour.enabled = GUI.Toggle(rect5, behaviour.enabled, GUIContent.none);
                 }
                 else
                 {
                     FieldInfo enableField = target.GetType().GetSerializedField("m_Enabled");

                     if (enableField != null)
                     {
                         bool isEnabled = GUI.Toggle(rect5, (bool)enableField.GetValue(target), GUIContent.none);
                         enableField.SetValue(target, isEnabled);
                     }
                 }
             }
             if (menu != null && GUI.Button(rect1, EditorGUIUtility.FindTexture("d__Menu"), Styles.inspectorTitleText))
             {
                 menu.ShowAsContext();
             }

             EventType eventType = Event.current.type;
             if (menu != null && eventType == EventType.MouseDown && Event.current.button == 1 && position.Contains(Event.current.mousePosition))
             {
                 menu.ShowAsContext();
             }

             bool isFolded = EditorPrefs.GetBool("TitlebarFold" + hash, true);
             if (eventType != EventType.MouseDown)
             {
                 if (eventType == EventType.Repaint)
                 {
                     Styles.inspectorTitle.Draw(position, GUIContent.none, controlID, isFolded);
                     Styles.inspectorTitleText.Draw(rect2, content, controlID, isFolded);
                 }
             }

             bool flag = DoToggleForward(position, controlID, isFolded, GUIContent.none, GUIStyle.none);
             if (flag != isFolded)
             {
                 EditorPrefs.SetBool("TitlebarFold" +hash, flag);
             }
             return flag;
        }

        private static bool DoToggleForward(Rect position, int id, bool value, GUIContent content, GUIStyle style)
        {
            Event ev = Event.current;
            if (MainActionKeyForControl(ev, id))
            {
                value = !value;
                ev.Use();
                GUI.changed = true;
            }
            if (EditorGUI.showMixedValue)
            {
                style = "ToggleMixed";
            }
            EventType eventType = ev.type;
            bool flag = (ev.type != EventType.MouseDown ? false : ev.button != 0);
            if (flag)
            {
                ev.type = EventType.Ignore;
            }
            bool flag1 = GUI.Toggle(position, id, (!EditorGUI.showMixedValue ? value : false), content, style);
            if (flag)
            {
                ev.type = eventType;
            }
            else if (ev.type != eventType)
            {
                GUIUtility.keyboardControl = id;
            }
            return flag1;
        }

        private static bool MainActionKeyForControl(Event evt, int controlId)
        {
            if (GUIUtility.keyboardControl != controlId)
            {
                return false;
            }
            bool flag = (evt.alt || evt.shift || evt.command ? true : evt.control);
            if (evt.type == EventType.KeyDown && evt.character == ' ' && !flag)
            {
                evt.Use();
                return false;
            }
            return (evt.type != EventType.KeyDown || evt.keyCode != KeyCode.Space && evt.keyCode != KeyCode.Return && evt.keyCode != KeyCode.KeypadEnter ? false : !flag);
        }

        public static string CovertToAliasString(Type type) {
            if (type == typeof(System.Boolean)){
                return "bool";
            } else if (type== typeof(System.Byte)) {
                return "byte";
            }
            else if (type == typeof(System.SByte))
            {
                return "sbyte";
            }
            else if (type == typeof(System.Char))
            {
                return "char";
            }
            else if (type == typeof(System.Decimal))
            {
                return "decimal";
            }
            else if (type == typeof(System.Double))
            {
                return "double";
            }
            else if (type == typeof(System.Single))
            {
                return "float";
            }
            else if (type == typeof(System.Int32))
            {
                return "int";
            }
            else if (type == typeof(System.UInt32))
            {
                return "uint";
            }
            else if (type == typeof(System.Int64))
            {
                return "long";
            }
            else if (type == typeof(System.UInt64))
            {
                return "ulong";
            }
            else if (type == typeof(System.Object))
            {
                return "object";
            }
            else if (type == typeof(System.Int16))
            {
                return "short";
            }
            else if (type == typeof(System.UInt16))
            {
                return "ushort";
            }
            else if (type == typeof(System.String))
            {
                return "string";
            }
            else if (type == typeof(void))
            {
                return "void";
            }

            return type.Name;
        }

        /// <summary>
		/// Creates a custom asset.
		/// </summary>
		/// <returns>The asset.</returns>
		/// <param name="displayFilePanel">If set to <c>true</c> display file panel.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T CreateAsset<T>(bool displayFilePanel) where T : ScriptableObject
        {
            return (T)CreateAsset(typeof(T), displayFilePanel);
        }

        /// <summary>
        /// Creates a custom asset.
        /// </summary>
        /// <returns>The asset.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T CreateAsset<T>() where T : ScriptableObject
        {
            return (T)CreateAsset(typeof(T));
        }

        /// <summary>
        /// Creates a custom asset at path.
        /// </summary>
        /// <returns>The asset.</returns>
        /// <param name="path">Path.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T CreateAsset<T>(string path) where T : ScriptableObject
        {
            return (T)CreateAsset(typeof(T), path); ;
        }

        public static ScriptableObject CreateAsset(Type type, bool displayFilePanel)
        {

            if (displayFilePanel)
            {
                string mPath = EditorUtility.SaveFilePanelInProject(
                    "Create Asset of type " + type.Name,
                    "New " + type.Name + ".asset",
                    "asset", "");
                return CreateAsset(type, mPath);
            }
            return CreateAsset(type);
        }

        /// <summary>
        /// Creates a custom asset.
        /// </summary>
        /// <returns>The asset.</returns>
        /// <param name="type">Type.</param>
        public static ScriptableObject CreateAsset(Type type)
        {

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (System.IO.Path.GetExtension(path) != "")
            {
                path = path.Replace(System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + type.Name + ".asset");
            return CreateAsset(type, assetPathAndName);
        }

        /// <summary>
        /// Creates a custom asset at path.
        /// </summary>
        /// <returns>The asset.</returns>
        /// <param name="type">Type.</param>
        /// <param name="path">Path.</param>
        public static ScriptableObject CreateAsset(Type type, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            ScriptableObject data = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(data, path);
            AssetDatabase.SaveAssets();
            return data;
        }

        /// <summary>
        /// Finds components the in scene.
        /// </summary>
        /// <returns>The in scene.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> FindInScene<T>() where T : Component
        {
            T[] comps = Resources.FindObjectsOfTypeAll(typeof(T)) as T[];

            List<T> list = new List<T>();

            foreach (T comp in comps)
            {
                if (comp.gameObject.hideFlags == 0)
                {
                    string path = AssetDatabase.GetAssetPath(comp.gameObject);
                    if (string.IsNullOrEmpty(path)) list.Add(comp);
                }
            }
            return list;
        }

        public static T[] FindAssets<T>() where T : UnityEngine.Object
        {
            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets.ToArray();
        }

        public static MonoScript FindMonoScript(Type type)
        {
            MonoScript monoScript;
            if (!EditorTools.m_TypeMonoScriptLookup.TryGetValue(type, out monoScript))
            {
                string[] assetPaths = AssetDatabase.GetAllAssetPaths();
                foreach (string assetPath in assetPaths)
                {
                    if (assetPath.EndsWith(".cs"))
                    {
                        MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
                        if (script.GetClass() != null && script.GetClass() == type)
                        {
                            EditorTools.m_TypeMonoScriptLookup.Add(type, script);
                            return script;
                        }
                    }
                }
            }
            return monoScript;
        }

        struct PropertyPath
        {
            public string propertyName;
            public int elementIndex;
        }

        static Regex arrayElementRegex = new Regex(@"\GArray\.data\[(\d+)\]", RegexOptions.Compiled);

        public static object GetValue(this SerializedProperty property)
        {
            string propertyPath = property.propertyPath;
            object value = property.serializedObject.targetObject;
            int i = 0;
            while (NextPropertyPath(propertyPath, ref i, out var token))
                value = GetPropertyPathValue(value, token);
            return value;
        }

        public static void SetValue(this SerializedProperty property, object value)
        {
            string propertyPath = property.propertyPath;
            object container = property.serializedObject.targetObject;

            int i = 0;
            NextPropertyPath(propertyPath, ref i, out var deferredToken);
            while (NextPropertyPath(propertyPath, ref i, out var token))
            {
                container = GetPropertyPathValue(container, deferredToken);
                deferredToken = token;
            }
            SetPropertyPathValue(container, deferredToken, value);

            EditorUtility.SetDirty(property.serializedObject.targetObject);
            property.serializedObject.ApplyModifiedProperties();
            var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            }
        }

        public static object GetParent(this SerializedProperty property) {
            string propertyPath = property.propertyPath;
            object container = property.serializedObject.targetObject;

            int i = 0;
            NextPropertyPath(propertyPath, ref i, out var deferredToken);
            while (NextPropertyPath(propertyPath, ref i, out var token))
            {
                container = GetPropertyPathValue(container, deferredToken);
                deferredToken = token;
            }
            return container;
        }

        private static bool NextPropertyPath(string propertyPath, ref int index, out PropertyPath component)
        {
            component = new PropertyPath();

            if (index >= propertyPath.Length)
                return false;

            var arrayElementMatch = arrayElementRegex.Match(propertyPath, index);
            if (arrayElementMatch.Success)
            {
                index += arrayElementMatch.Length + 1; 
                component.elementIndex = int.Parse(arrayElementMatch.Groups[1].Value);
                return true;
            }

            int dot = propertyPath.IndexOf('.', index);
            if (dot == -1)
            {
                component.propertyName = propertyPath.Substring(index);
                index = propertyPath.Length;
            }
            else
            {
                component.propertyName = propertyPath.Substring(index, dot - index);
                index = dot + 1; 
            }

            return true;
        }

        private static object GetPropertyPathValue(object container, PropertyPath component)
        {
            if (component.propertyName == null )
            {

                IList list = (IList)container;
                if (list.Count - 1 < component.elementIndex)
                {
                    for (int i = list.Count - 1; i < component.elementIndex; i++)
                    {
                        list.Add(default);
                    }
                }
                return list[component.elementIndex];
            }
            else
            {
                return GetFieldValue(container, component.propertyName);
            }
        }

        private static void SetPropertyPathValue(object container, PropertyPath component, object value)
        {

            if (component.propertyName == null)
            {
                ((IList)container)[component.elementIndex] = value;
            }
            else
            {
                SetFieldValue(container, component.propertyName, value);
            }
        }

        private static object GetFieldValue(object container, string name)
        {
            if (container == null)
                return null;
            var type = container.GetType();
            FieldInfo field = type.GetSerializedField(name);
            return field.GetValue(container);
        }

        private static void SetFieldValue(object container, string name, object value)
        {
            var type = container.GetType();
            FieldInfo field = type.GetSerializedField(name);
            field.SetValue(container, value);
        }



        public static EventType ReserveEvent(params Rect[] areas)
        {
            EventType eventType = Event.current.type;
            foreach (Rect area in areas)
            {
                if ((area.Contains(Event.current.mousePosition) && (eventType == EventType.MouseDown || eventType == EventType.ScrollWheel)))
                {
                    Event.current.type = EventType.Ignore;
                }
            }
            return eventType;
        }


        public static float PropertyElementField(SerializedProperty arrayProperty, int index, bool drawScript = true) {

            float height = 0f;

            SerializedProperty element = arrayProperty.GetArrayElementAtIndex(index);
            
            string propertyPath = arrayProperty.propertyPath;
            Type type = element.GetValue().GetType();
            if (type != null && drawScript) {
                MonoScript monoScript = FindMonoScript(type);
                if (monoScript != null)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.ObjectField("Script", monoScript, typeof(MonoScript), true);
                    EditorGUI.EndDisabledGroup();
                    height += EditorGUIUtility.singleLineHeight;
                }
            }

            
            if (HasCustomPropertyDrawer(type))
            {
                height += EditorGUIUtility.standardVerticalSpacing;
                EditorGUILayout.PropertyField(element, true);
                height += EditorGUI.GetPropertyHeight(element, true);
            }
            else
            {
                while (element.NextVisible(true))
                {
                    if (element.propertyPath.Contains(propertyPath + ".Array.data[" + index + "]") && !element.propertyPath.Replace(propertyPath + ".Array.data[" + index + "].", "").Contains("."))
                    {
                        height += EditorGUIUtility.standardVerticalSpacing;
                        EditorGUILayout.PropertyField(element, true);
                        height += EditorGUI.GetPropertyHeight(element, true);
                    }
                }
            }
            return height;
        }

        public static float PropertyElementField(Rect rect, SerializedProperty arrayProperty, int index, bool drawScript = true)
        {

            float height = 0f;

            SerializedProperty element = arrayProperty.GetArrayElementAtIndex(index);
            
            string propertyPath = arrayProperty.propertyPath;
            Type type = element.GetValue().GetType();
            if (type != null && drawScript)
            {
                MonoScript monoScript = FindMonoScript(type);
                if (monoScript != null)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    rect.height = EditorGUIUtility.singleLineHeight;
                    height += rect.height;
                    EditorGUI.ObjectField(rect,"Script", monoScript, typeof(MonoScript), true);
                    EditorGUI.EndDisabledGroup();
                   
                }
            }


            if (HasCustomPropertyDrawer(type))
            {
                rect.y += EditorGUIUtility.standardVerticalSpacing+rect.height;
                rect.height = EditorGUI.GetPropertyHeight(element, true);

                height += EditorGUIUtility.standardVerticalSpacing+rect.height;
                EditorGUI.PropertyField(rect,element, true);
            }
            else
            {
                while (element.NextVisible(true))
                {
                    if (element.propertyPath.Contains(propertyPath + ".Array.data[" + index + "]") && !element.propertyPath.Replace(propertyPath + ".Array.data[" + index + "].", "").Contains("."))
                    {
                        rect.y += EditorGUIUtility.standardVerticalSpacing+rect.height;
                        rect.height = EditorGUI.GetPropertyHeight(element, true);
                        height += EditorGUIUtility.standardVerticalSpacing + rect.height;
                        EditorGUI.PropertyField(rect,element, true);
                    }
                }
            }
            return height;
        }


        public static float PropertyElementHeight(SerializedProperty arrayProperty, int index, bool drawScript = true)
        {

            float height = 0f;

            SerializedProperty element = arrayProperty.GetArrayElementAtIndex(index);

            string propertyPath = arrayProperty.propertyPath;
            Type type = element.GetValue().GetType();
            if (type != null && drawScript)
            {
                MonoScript monoScript = FindMonoScript(type);
                if (monoScript != null)
                {
                    height += EditorGUIUtility.singleLineHeight;
                }
            }


            if (HasCustomPropertyDrawer(type))
            {
                height += EditorGUIUtility.standardVerticalSpacing;
                height += EditorGUI.GetPropertyHeight(element, true);
            }
            else
            {
                while (element.NextVisible(true))
                {
                    if (element.propertyPath.Contains(propertyPath + ".Array.data[" + index + "]") && !element.propertyPath.Replace(propertyPath + ".Array.data[" + index + "].", "").Contains("."))
                    {
                        height += EditorGUIUtility.standardVerticalSpacing;
                        height += EditorGUI.GetPropertyHeight(element, true);
                    }
                }
            }
            return height;
        }

        public static bool HasCustomPropertyDrawer(Type type)
        {
            if (EditorTools.m_CustomPropertyDrawerLookup.ContainsKey(type)) {
                return EditorTools.m_CustomPropertyDrawerLookup[type];
            }

            foreach (Type typesDerivedFrom in TypeCache.GetTypesDerivedFrom<GUIDrawer>())
            {
                object[] customAttributes = typesDerivedFrom.GetCustomAttributes<CustomPropertyDrawer>();
                for (int i = 0; i < (int)customAttributes.Length; i++)
                {
                    CustomPropertyDrawer customPropertyDrawer = (CustomPropertyDrawer)customAttributes[i];

                    FieldInfo field = customPropertyDrawer.GetType().GetField("m_Type", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    Type type1 = (Type)field.GetValue(customPropertyDrawer);
                    if (type == type1)
                    {
                        EditorTools.m_CustomPropertyDrawerLookup.Add(type, true);
                        return true;
                    }
                }
            }
            EditorTools.m_CustomPropertyDrawerLookup.Add(type, false);
            return false;
        }

        public static object DrawFields(Rect rect, object obj)
        {
            if (obj == null) { return null; }

            Type type = obj.GetType();

            FieldInfo[] fields = type.GetSerializedFields().Where(x => !x.HasAttribute(typeof(HideInInspector))).GroupBy(x => x.Name).Select(x => x.First()).ToArray();

            for (int j = 0; j < fields.Length; j++)
            {
                FieldInfo field = fields[j];
                TooltipAttribute attribute = field.GetCustomAttribute<TooltipAttribute>();
                string tooltip = attribute != null ? attribute.tooltip : string.Empty;
                GUIContent label = new GUIContent(ObjectNames.NicifyVariableName(field.Name), tooltip);
                object value = field.GetValue(obj);

                if (value == null)
                {
                    if (Type.GetTypeCode(field.FieldType) == TypeCode.String)
                    {
                        value = string.Empty;
                    }
                    else if (typeof(IList).IsAssignableFrom(field.FieldType))
                    {
                        value = Activator.CreateInstance(field.FieldType, new object[] { 0 });
                    }
                    else if (typeof(UnityEngine.Object).IsAssignableFrom(field.FieldType))
                    {

                    }
                    else
                    {
                        value = System.Activator.CreateInstance(field.FieldType);
                    }
                    field.SetValue(obj, value);
                }

                EditorGUI.BeginChangeCheck();
                rect.height= CalcHeight(label, obj, value, field)-EditorGUIUtility.standardVerticalSpacing;

                object val = EditorTools.DrawField(rect,label, obj, value, field);
                rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
                if (EditorGUI.EndChangeCheck())
                {
                    field.SetValue(obj, val);
                }

            }
            return obj;
        }

        public static object DrawField(Rect rect, GUIContent label, object obj, object value, FieldInfo field, params GUILayoutOption[] options)
        {

            Type type = field.FieldType;
            if (value != null)
            {
                type = value.GetType();
            }
            CustomDrawer customDrawer = GetCustomDrawer(type);
            if (customDrawer != null)
            {
                customDrawer.declaringObject = obj;
                customDrawer.fieldInfo = field;
                customDrawer.value = value;
                customDrawer.OnGUI(label);
                return value;
            }

            if (type == typeof(int))
            {
                return EditorGUI.IntField(rect,label, (int)value);
            }
            else if (type == typeof(float))
            {
                return EditorGUI.FloatField(rect,label, (float)value);
            }
            else if (type == typeof(string))
            {
                if (field.HasAttribute(typeof(TextAreaAttribute)))
                {
                    TextAreaAttribute attribute = field.GetCustomAttribute<TextAreaAttribute>();
                    rect.height = EditorGUIUtility.singleLineHeight;
                    EditorGUI.LabelField(rect,label);
                    rect.y += rect.height;
                    rect.height = attribute.minLines * EditorGUIUtility.singleLineHeight;
                    return EditorGUI.TextArea(rect,(string)value);
                }
                else
                {
                    return EditorGUI.TextField(rect,label, (string)value);
                }
            }
            else if (typeof(Enum).IsAssignableFrom(type))
            {
                return EditorGUI.EnumPopup(rect,label, (Enum)value);
            }
            else if (type == typeof(bool))
            {
                return EditorGUI.Toggle(rect, label, (bool)value);
            }
            else if (type == typeof(Color))
            {
                return EditorGUI.ColorField(rect,label, (Color)value);
            }
            else if (type == typeof(Bounds))
            {
                return EditorGUI.BoundsField(rect,label, (Bounds)value);
            }
            else if (type == typeof(AnimationCurve))
            {
                return EditorGUI.CurveField(rect,label, (AnimationCurve)value);
            }
            else if (type == typeof(Rect))
            {
                return EditorGUI.RectField(rect,label, (Rect)value);
            }
            else if (type == typeof(Vector2))
            {
                return EditorGUI.Vector2Field(rect,label, (Vector2)value);
            }
            else if (type == typeof(Vector3))
            {
                return EditorGUI.Vector3Field(rect,label, (Vector3)value);
            }
            else if (type == typeof(Vector4))
            {
                return EditorGUI.Vector4Field(rect,label, (Vector4)value);
            }
            else if (type == typeof(LayerMask))
            {
                return LayerMaskField(rect,label, (LayerMask)value);
            }
            else if (typeof(UnityEngine.Object).IsAssignableFrom(type))
            {
                return EditorGUI.ObjectField(rect,label, (UnityEngine.Object)value, type, true);
            }
            else if (typeof(IList).IsAssignableFrom(type))
            {
                if (EditorTools.Foldout(type.Name + label.text, label))
                {
                    EditorTools.BeginIndent(1, true);
                    Type elementType = Utility.GetElementType(type);
                    IList list = (IList)value;
                    EditorGUI.BeginChangeCheck();
                    int size = EditorGUILayout.IntField("Size", list.Count);
                    size = Mathf.Clamp(size, 0, int.MaxValue);

                    if (size != list.Count)
                    {

                        Array array = Array.CreateInstance(elementType, size);
                        int index = 0;
                        while (index < size)
                        {
                            object item = null;
                            if (index < list.Count)
                            {
                                item = list[index];
                            }
                            else
                            {
                                if (Type.GetTypeCode(elementType) == TypeCode.String)
                                {
                                    item = string.Empty;
                                }
                                else
                                {
                                    item = Activator.CreateInstance(elementType, true);
                                }

                            }
                            array.SetValue(item, index);
                            index++;

                        }
                        if (type.IsArray)
                        {
                            list = array;
                        }
                        else
                        {
                            list.Clear();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array.GetValue(i));
                            }
                        }
                    }

                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i] = DrawField(new GUIContent("Element " + i), list, list[i], field);
                    }
                    EditorTools.EndIndent();
                    return list;
                }
                return value;
            }

            if (EditorTools.Foldout(type.Name + label.text, label))
            {
                EditorTools.BeginIndent(1, true);
                value = DrawFields(value);
                EditorTools.EndIndent();
            }
            return value;
        }

        public static object DrawFields(object obj, params GUILayoutOption[] options)
        {
            if (obj == null) { return null; }

            Type type = obj.GetType();

            FieldInfo[] fields = type.GetSerializedFields().Where(x => !x.HasAttribute(typeof(HideInInspector))).GroupBy(x => x.Name).Select(x => x.First()).ToArray();

            for (int j = 0; j < fields.Length; j++)
            {
                FieldInfo field = fields[j];
                TooltipAttribute attribute = field.GetCustomAttribute<TooltipAttribute>();
                string tooltip = attribute != null ? attribute.tooltip : string.Empty;
                GUIContent label = new GUIContent(ObjectNames.NicifyVariableName(field.Name), tooltip);
                object value = field.GetValue(obj);

                if (value == null)
                {
                    if (Type.GetTypeCode(field.FieldType) == TypeCode.String)
                    {
                        value = string.Empty;
                    }
                    else if (typeof(IList).IsAssignableFrom(field.FieldType))
                    {
                        value = Activator.CreateInstance(field.FieldType, new object[] { 0 });
                    }
                    else if (typeof(UnityEngine.Object).IsAssignableFrom(field.FieldType))
                    {

                    }
                    else
                    {
                        value = System.Activator.CreateInstance(field.FieldType);
                    }
                    field.SetValue(obj, value);
                }

                EditorGUI.BeginChangeCheck();
                object val = EditorTools.DrawField(label, obj, value, field,options);
                if (EditorGUI.EndChangeCheck())
                {
                    field.SetValue(obj, val);
                }

            }
            return obj;
        }

        public static object DrawField(GUIContent label, object obj, object value, FieldInfo field, params GUILayoutOption[] options)
        {

            Type type = field.FieldType;
            if (value != null)
            {
                type = value.GetType();
            }
            CustomDrawer customDrawer = GetCustomDrawer(type);
            if (customDrawer != null)
            {
                customDrawer.declaringObject = obj;
                customDrawer.fieldInfo = field;
                customDrawer.value = value;
                customDrawer.OnGUI(label);
                if (customDrawer.dirty) {
                    GUI.changed = true;
                    customDrawer.dirty = false;
                }
                return value;
            }

            if (type == typeof(int))
            {
                return EditorGUILayout.IntField(label, (int)value, options);
            }
            else if (type == typeof(float))
            {
                return EditorGUILayout.FloatField(label, (float)value, options);
            }
            else if (type == typeof(string))
            {
                if (field.HasAttribute(typeof(TextAreaAttribute)))
                {
                    TextAreaAttribute attribute = field.GetCustomAttribute<TextAreaAttribute>();
                    EditorGUILayout.LabelField(label);
                    List<GUILayoutOption> op = new List<GUILayoutOption>(options);
                    op.Add(GUILayout.Height(attribute.minLines * EditorGUIUtility.singleLineHeight));
                    GUIStyle style= new GUIStyle(EditorStyles.textArea);
                    style.wordWrap = true;
                    return EditorGUILayout.TextArea((string)value, style,op.ToArray());
                }
                else
                {
                    return EditorGUILayout.TextField(label, (string)value);
                }
            }
            else if (typeof(Enum).IsAssignableFrom(type))
            {
                return EditorGUILayout.EnumPopup(label, (Enum)value, options);
            }
            else if (type == typeof(bool))
            {
                return EditorGUILayout.Toggle(label, (bool)value, options);
            }
            else if (type == typeof(Color))
            {
                return EditorGUILayout.ColorField(label, (Color)value, options);
            }
            else if (type == typeof(Bounds))
            {
                return EditorGUILayout.BoundsField(label, (Bounds)value, options);
            }
            else if (type == typeof(AnimationCurve))
            {
                return EditorGUILayout.CurveField(label, (AnimationCurve)value, options);
            }
            else if (type == typeof(Rect))
            {
                return EditorGUILayout.RectField(label, (Rect)value, options);
            }
            else if (type == typeof(Vector2))
            {
                return EditorGUILayout.Vector2Field(label, (Vector2)value, options);
            }
            else if (type == typeof(Vector3))
            {
                return EditorGUILayout.Vector3Field(label, (Vector3)value, options);
            }
            else if (type == typeof(Vector4))
            {
                return EditorGUILayout.Vector4Field(label, (Vector4)value, options);
            }
            else if (type == typeof(LayerMask))
            {
                return LayerMaskField(label, (LayerMask)value, options);
            }
            else if (typeof(UnityEngine.Object).IsAssignableFrom(type))
            {
                return EditorGUILayout.ObjectField(label, (UnityEngine.Object)value, type, true, options);
            }
            else if (typeof(IList).IsAssignableFrom(type))
            {
                if (EditorTools.Foldout(type.Name + label.text, label))
                {
                    EditorTools.BeginIndent(1, true);
                    Type elementType = Utility.GetElementType(type);
                    IList list = (IList)value;
                    EditorGUI.BeginChangeCheck();
                    int size = EditorGUILayout.IntField("Size", list.Count);
                    size = Mathf.Clamp(size, 0, int.MaxValue);

                    if (size != list.Count)
                    {

                        Array array = Array.CreateInstance(elementType, size);
                        int index = 0;
                        while (index < size)
                        {
                            object item = null;
                            if (index < list.Count)
                            {
                                item = list[index];
                            }
                            else
                            {
                                if (Type.GetTypeCode(elementType) == TypeCode.String)
                                {
                                    item = string.Empty;
                                }
                                else
                                {
                                    item = Activator.CreateInstance(elementType, true);
                                }

                            }
                            array.SetValue(item, index);
                            index++;

                        }
                        if (type.IsArray)
                        {
                            list = array;
                        }
                        else
                        {
                            list.Clear();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array.GetValue(i));
                            }
                        }
                    }

                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i] = DrawField(new GUIContent("Element " + i), list, list[i], field,options);
                    }
                    EditorTools.EndIndent();
                    return list;
                }
                return value;
            }
           
            if (EditorTools.Foldout(type.Name + label.text, label))
            {
                EditorTools.BeginIndent(1, true);
                value = DrawFields(value);
                EditorTools.EndIndent();
            }
            return value;
        }

        public static float CalcHeight(object obj)
        {
            if (obj == null) { return 0f; }
            float height = 0f;
            Type type = obj.GetType();

            FieldInfo[] fields = type.GetSerializedFields().Where(x => !x.HasAttribute(typeof(HideInInspector))).GroupBy(x => x.Name).Select(x => x.First()).ToArray();

            for (int j = 0; j < fields.Length; j++)
            {
                FieldInfo field = fields[j];
                TooltipAttribute attribute = field.GetCustomAttribute<TooltipAttribute>();
                string tooltip = attribute != null ? attribute.tooltip : string.Empty;
                GUIContent label = new GUIContent(ObjectNames.NicifyVariableName(field.Name), tooltip);
                object value = field.GetValue(obj);

                if (value == null)
                {
                    if (Type.GetTypeCode(field.FieldType) == TypeCode.String)
                    {
                        value = string.Empty;
                    }
                    else if (typeof(IList).IsAssignableFrom(field.FieldType))
                    {
                        value = Activator.CreateInstance(field.FieldType, new object[] { 0 });
                    }
                    else if (typeof(UnityEngine.Object).IsAssignableFrom(field.FieldType))
                    {

                    }
                    else
                    {
                        value = System.Activator.CreateInstance(field.FieldType);
                    }
                    field.SetValue(obj, value);
                }

                height += EditorTools.CalcHeight(label, obj, value, field);
               

            }
            return height;
        }

        public static float CalcHeight(GUIContent label, object obj, object value, FieldInfo field)
        {
            float height = 0f;

            Type type = field.FieldType;
            if (value != null)
            {
                type = value.GetType();
            }
            CustomDrawer customDrawer = GetCustomDrawer(type);
            if (customDrawer != null)
            {
                customDrawer.declaringObject = obj;
                customDrawer.fieldInfo = field;
                customDrawer.value = value;
                customDrawer.OnGUI(label);

                return 0f;
            }

            if (type == typeof(string))
            {
                if (field.HasAttribute(typeof(TextAreaAttribute)))
                {
                    TextAreaAttribute attribute = field.GetCustomAttribute<TextAreaAttribute>();
                    return attribute.minLines * EditorGUIUtility.singleLineHeight + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
                else
                {
                    return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
            }
            else if (typeof(IList).IsAssignableFrom(type))
            {
                if (EditorPrefs.GetBool(type.Name + label.text))
                {
                  
                    Type elementType = Utility.GetElementType(type);
                    IList list = (IList)value;
                    int size = list.Count;
                    size = Mathf.Clamp(size, 0, int.MaxValue);

                    if (size != list.Count)
                    {

                        Array array = Array.CreateInstance(elementType, size);
                        int index = 0;
                        while (index < size)
                        {
                            object item = null;
                            if (index < list.Count)
                            {
                                item = list[index];
                            }
                            else
                            {
                                if (Type.GetTypeCode(elementType) == TypeCode.String)
                                {
                                    item = string.Empty;
                                }
                                else
                                {
                                    item = Activator.CreateInstance(elementType, true);
                                }

                            }
                            array.SetValue(item, index);
                            index++;

                        }
                        if (type.IsArray)
                        {
                            list = array;
                        }
                        else
                        {
                            list.Clear();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array.GetValue(i));
                            }
                        }
                    }

                    for (int i = 0; i < list.Count; i++)
                    {
                        height += CalcHeight(new GUIContent("Element " + i), list, list[i], field);
                    }
                    return height+2*EditorGUIUtility.singleLineHeight+ EditorGUIUtility.standardVerticalSpacing;
                }
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            else {
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
        }
           
        private static CustomDrawer GetCustomDrawer(Type type)
        {
            Type drawerType = GetDrawerTypeForType(type);
            if (drawerType != null)
            {
                CustomDrawer drawer;
                if (!EditorTools.m_Drawers.TryGetValue(drawerType, out drawer))
                {
                    drawer = (CustomDrawer)System.Activator.CreateInstance(drawerType);
                    EditorTools.m_Drawers.Add(drawerType, drawer);
                }
                return drawer;
            }
            return null;
        }

        private static Type GetDrawerTypeForType(Type type)
        {
            EditorTools.DrawerKeySet drawerKeySet;
            Type type1;
            if (EditorTools.m_DrawerTypeForType == null)
            {
                EditorTools.BuildDrawerTypeForTypeDictionary();
            }
            EditorTools.m_DrawerTypeForType.TryGetValue(type, out drawerKeySet);
            if (drawerKeySet.drawer == null)
            {
                if (type.IsGenericType)
                {
                    EditorTools.m_DrawerTypeForType.TryGetValue(type.GetGenericTypeDefinition(), out drawerKeySet);
                }
                type1 = drawerKeySet.drawer;
            }
            else
            {
                type1 = drawerKeySet.drawer;
            }
            return type1;
        }

        private static void BuildDrawerTypeForTypeDictionary()
        {
            EditorTools.m_DrawerTypeForType = new Dictionary<Type, EditorTools.DrawerKeySet>();
            foreach (Type typesDerivedFrom in TypeCache.GetTypesDerivedFrom<CustomDrawer>())
            {
                object[] customAttributes = typesDerivedFrom.GetCustomAttributes(typeof(CustomDrawerAttribute), true);
                for (int i = 0; i < customAttributes.Length; i++)
                {
                    CustomDrawerAttribute customDrawer = (CustomDrawerAttribute)customAttributes[i];
                    Dictionary<Type, EditorTools.DrawerKeySet> sDrawerTypeForType = EditorTools.m_DrawerTypeForType;
                    Type mType = customDrawer.Type;
                    EditorTools.DrawerKeySet drawerKeySet = new EditorTools.DrawerKeySet()
                    {
                        drawer = typesDerivedFrom,
                        type = customDrawer.Type
                    };
                    sDrawerTypeForType[mType] = drawerKeySet;
                    if (customDrawer.UseForChildren)
                    {
                        foreach (Type type in TypeCache.GetTypesDerivedFrom(customDrawer.Type))
                        {
                            if ((!EditorTools.m_DrawerTypeForType.ContainsKey(type) ? true : !customDrawer.Type.IsAssignableFrom(EditorTools.m_DrawerTypeForType[type].type)))
                            {
                                Dictionary<Type, EditorTools.DrawerKeySet> types = EditorTools.m_DrawerTypeForType;
                                drawerKeySet = new EditorTools.DrawerKeySet()
                                {
                                    drawer = typesDerivedFrom,
                                    type = customDrawer.Type
                                };
                                types[type] = drawerKeySet;
                            }
                        }
                    }
                }
            }
        }

       
        public static IEnumerable<SerializedProperty> EnumerateChildProperties(this SerializedProperty property)
        {
            var iterator = property.Copy();
            var end = iterator.GetEndProperty();
            if (iterator.NextVisible(enterChildren: true))
            {
                do
                {
                    if (SerializedProperty.EqualContents(iterator, end))
                        yield break;

                    yield return iterator;
                }
                while (iterator.NextVisible(enterChildren: false));
            }
        }

        public static object Duplicate(object source) {
            object duplicate = Activator.CreateInstance(source.GetType());
            FieldInfo[] fields = source.GetType().GetSerializedFields();
            for (int i = 0; i < fields.Length; i++) {
                fields[i].SetValue(duplicate, fields[i].GetValue(source));
            }
            return duplicate;
        }

        private struct DrawerKeySet
        {
            public Type drawer;

            public Type type;
        }

        public static bool IsDocked(this EditorWindow window)
        {
            BindingFlags fullBinding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            MethodInfo isDockedMethod = typeof(EditorWindow).GetProperty("docked", fullBinding).GetGetMethod(true);
            return (bool)isDockedMethod.Invoke(window, null);
        }

        private static class Styles {
            public static GUIStyle seperator;
            public static Texture2D rightArrow;
            public static GUIStyle leftTextButton;
            public static GUIStyle leftTextToolbarButton;
            public static GUIStyle inspectorTitle;
            public static GUIStyle inspectorTitleText;
            public static GUIStyle inspectorBigTitle;


            static Styles() {
                Styles.seperator = new GUIStyle("IN Title"){
                    fixedHeight = 1f
                };
                Styles.rightArrow = ((GUIStyle)"AC RightArrow").normal.background;
                Styles.leftTextButton = new GUIStyle("Button"){
                    alignment = TextAnchor.MiddleLeft
                };
                Styles.leftTextToolbarButton = new GUIStyle(EditorStyles.toolbarButton)
                {
                    alignment = TextAnchor.MiddleLeft
                };
                Styles.inspectorTitle = new GUIStyle("IN Foldout")
                {
                    overflow = new RectOffset(0, 0, -3, 0),
                    fixedWidth = 0,
                    fixedHeight = 20
                };
                Styles.inspectorTitleText = new GUIStyle("IN TitleText");
                Styles.inspectorBigTitle = new GUIStyle("IN BigTitle");
            }
        }
    }
}