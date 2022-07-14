using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using UnityEngine.Events;

namespace DevionGames
{
    [InitializeOnLoad]
    public class ModuleManagerWindow : EditorWindow
    {
        private const float LIST_MIN_WIDTH = 280f;
        private const float LIST_MAX_WIDTH = 600f;
        private const float LIST_RESIZE_WIDTH = 10f;

        private static string m_ModuleTxtPath = "https://deviongames.com/modules/modules.txt";
        private Rect m_SidebarRect = new Rect(0, 30, 280, 1000);
        private Vector2 m_ScrollPosition;
        private string m_SearchString = "Search...";
        private Vector2 m_SidebarScrollPosition;
        private ModuleItem selectedItem;
        private ModuleItem[] m_Items= new ModuleItem[0];
        private static string tempPath { get { return CombinePath(Application.dataPath, "..", "Temp", "Modules"); } }

        private int m_SelectedChangeLog;

        [MenuItem("Tools/Devion Games/Module Manager", false, -1000)]
        public static void ShowWindow()
        {
            ModuleManagerWindow window = EditorWindow.GetWindow<ModuleManagerWindow>(false, "Module Manager");
            window.minSize = new Vector2(500f, 300f);
            StartBackgroundTask(RequestModules(delegate(ModuleItem[] items) { window.m_Items = items; }));
        }

        static ModuleManagerWindow()
        {
                EditorApplication.update += UpdateCheck;

        }

        private static void UpdateCheck()
        {
           
            if (EditorApplication.timeSinceStartup > 5.0 && EditorApplication.timeSinceStartup < 10.0)
            {
                bool checkUpdates = EditorPrefs.GetBool("ModuleUpdateCheck", true);
                if (checkUpdates)
                {
                    StartBackgroundTask(RequestModules(delegate (ModuleItem[] items)
                    {
                        List<ModuleItem> updatedModules = new List<ModuleItem>();
                        for (int i = 0; i < items.Length; i++)
                        {
                            ModuleItem current = items[i];
                            if (current.IsInstalled && current.InstalledModule.version != current.version)
                            {
                                updatedModules.Add(current);
                            }
                        }

                        if (updatedModules.Count > 0)
                        {
                            UpdateNotificationWindow.ShowWindow(updatedModules.ToArray());
                        }
                    }));
                }
                EditorApplication.update -= UpdateCheck;
            }
        }

        private static IEnumerator RequestModules(UnityAction<ModuleItem[]> result)
        {
            using (UnityWebRequest w = UnityWebRequest.Get(m_ModuleTxtPath))
            {
                yield return w.SendWebRequest();
                while (!w.isDone) { yield return null; }

                ModuleItem[] items = JsonHelper.FromJson<ModuleItem>(w.downloadHandler.text);

                for (int i = 0; i < items.Length; i++)
                {
                    items[i].Initialize();
                }
                result.Invoke(items);
            }
        }

        private void OnGUI()
        {
            int index = EditorPrefs.GetInt("ModuleEditorItemIndex", -1);
            if (index != -1 && index < m_Items.Length)
            {
                selectedItem = m_Items[index];
            }
            m_SidebarRect = new Rect(0f, 0f, m_SidebarRect.width, position.height);
            GUILayout.BeginArea(m_SidebarRect, "", Styles.background);
            DoSearchGUI();
            m_SidebarScrollPosition = GUILayout.BeginScrollView(m_SidebarScrollPosition);
            for (int i = 0; i < m_Items.Length; i++)
            {
                ModuleItem currentItem = m_Items[i];
                if (!MatchesSearch(currentItem, m_SearchString))
                {
                    continue;
                }
             
                GUILayout.BeginHorizontal();
                Color backgroundColor = GUI.backgroundColor;
                Color textColor = Styles.selectButtonText.normal.textColor;
                if (selectedItem != null && selectedItem.Equals(currentItem))
                {
                    GUI.backgroundColor = backgroundColor;
                } else {
                    GUI.backgroundColor = Color.clear;
                }

                Styles.selectButtonText.normal.textColor = (selectedItem != null && selectedItem.Equals(currentItem) ? Color.white : textColor);

                using (var h = new EditorGUILayout.HorizontalScope(Styles.selectButton, GUILayout.Height(25)))
                {
                    if (GUI.Button(h.rect, GUIContent.none,Styles.selectButton)) {
                        GUI.FocusControl("");
                        selectedItem = currentItem;
                        EditorPrefs.SetInt("ModuleEditorItemIndex", i);
                        Select(selectedItem);
                    }
                        
                    GUILayout.Label(currentItem.name, Styles.selectButtonText);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label((currentItem.IsInstalled?currentItem.InstalledModule.version:currentItem.version), Styles.selectButtonText);
                    if (currentItem.IsInstalled)
                    {
                        Color color = GUI.color;
                        GUI.color = currentItem.InstalledModule.version == currentItem.version?Color.black:new Color(1f,0.74f,0f,1f);
                        GUILayout.Label(EditorGUIUtility.IconContent("MenuItemOn"), Styles.selectButtonText);
                        GUI.color = color;
                    }else {
                        GUILayout.Space(21);
                    }
                    GUILayout.Space(5);
                }

                GUI.backgroundColor = backgroundColor;
                Styles.selectButtonText.normal.textColor =textColor;
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();


            Rect rect = new Rect(m_SidebarRect.width, m_SidebarRect.y, position.width - m_SidebarRect.width, position.height);

            GUILayout.BeginArea(rect, "");
            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
            if (selectedItem != null)
            {
                DrawItem(selectedItem);
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();
            ResizeSidebar();
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        protected static void OnScriptsReloaded()
        {
            ModuleManagerWindow[] windows = Resources.FindObjectsOfTypeAll<ModuleManagerWindow>();
            if (windows.Length > 0) {
                StartBackgroundTask(RequestModules(delegate(ModuleItem[] items) { windows[0].m_Items = items; }));
            }
        }


        private void Select(ModuleItem item) {
            this.m_SelectedChangeLog = 0;
        }

        private void DrawItem(ModuleItem item) {
            GUILayout.BeginVertical(Styles.thumbnail, GUILayout.Height(50f));
            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            GUILayout.BeginVertical();
           // GUILayout.Space(9f);
            GUILayout.Label(item.Icon, GUILayout.Width(46), GUILayout.Height(46));
            GUILayout.EndVertical();
            GUILayout.Label(item.name, Styles.thumbnailText);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10f);
            GUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);
            GUILayout.Label("<b>Version</b> ", Styles.largeLabel);
            GUILayout.Label(item.version);
            GUILayout.Label("<i>"+item.id+"</i>", Styles.richTextLabel);         
            GUILayout.Space(5f);

            GUILayout.Label("<b>Module Path</b> ", Styles.largeLabel);
            GUILayout.Label("<i>" + item.assetPath + "</i>", Styles.richTextLabel);
 
            GUILayout.Space(5f);
            GUILayout.Label("<b>Author</b> ",Styles.largeLabel);
            GUILayout.Label(item.author);
            GUILayout.Label(item.email);

            GUILayout.Space(5f);

            GUILayout.Label("<b>Description</b> ", Styles.largeLabel);
            GUILayout.Label(item.description, EditorStyles.wordWrappedLabel);

            if (!string.IsNullOrEmpty(item.documentation) && GUILayout.Button("<color=#007bff>Documentation</color>", Styles.richTextLabel))
            {
                Application.OpenURL(item.documentation);
            }
            GUILayout.Space(5f);

            GUILayout.Label("<b>Dependencies ("+item.dependencies.Length+")</b> ", Styles.largeLabel);
            for (int i = 0; i < item.dependencies.Length; i++)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("<color=#007bff>" + item.dependencies[i]+"</color>",Styles.richTextLabel)) {
                    int index = Array.IndexOf(m_Items,m_Items.Where(x => x.id == item.dependencies[i]).FirstOrDefault());
                    GUI.FocusControl("");
                    selectedItem = m_Items[index];
                    EditorPrefs.SetInt("ModuleEditorItemIndex", index);
                    Select(selectedItem);
                }
                if (IsInstanlled(item.dependencies[i]))
                {
                    Color color = GUI.color;
                    GUI.color = Color.black;
                    GUILayout.Label(EditorGUIUtility.IconContent("MenuItemOn"),GUILayout.Width(20));
                    GUI.color = color;
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(5f);

            bool foldout = EditorPrefs.GetBool("ModuleChangelog", false);
            bool state = EditorGUILayout.Foldout(foldout, "Changelog",true,Styles.largeFoldout);
            if (state != foldout) { EditorPrefs.SetBool("ModuleChangelog", state); }
            if (state) {
                EditorGUI.indentLevel += 1;
                string[] versions = item.changelogs.Select(x => x.version).ToArray();
                if (versions.Length > 0)
                {

                    this.m_SelectedChangeLog = EditorGUILayout.Popup("Version", this.m_SelectedChangeLog, versions);
   
                    if (this.m_SelectedChangeLog > -1)
                    {
                        for (int i = 0; i < item.changelogs[this.m_SelectedChangeLog].changes.Length; i++)
                        {
                            EditorGUILayout.LabelField("- "+ item.changelogs[this.m_SelectedChangeLog].changes[i], EditorStyles.wordWrappedLabel);
                        }

                    }
                }
                EditorGUI.indentLevel -= 1;
            }
           

            GUILayout.FlexibleSpace();
            Styles.Seperator();

            if (!item.CanInstall)
            {
                EditorGUILayout.HelpBox("You need to install dependency assets before you can import this package.", MessageType.Warning, true);
            }
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (!string.IsNullOrEmpty(item.assetStore))
            {
                if (GUILayout.Button("Asset Store", GUILayout.Width(80)))
                {
                    Application.OpenURL(item.assetStore);
                }
            }
            else
            {
                string filePath = CombinePath(tempPath, item.name + "_" + item.version.Replace(".", "_") + ".unitypackage");

                if (!File.Exists(filePath) && !item.IsDownloading)
                {
                    if (item.IsInstalled && item.InstalledModule.version != item.version)
                    {
                        if (GUILayout.Button("Update", GUILayout.Width(70)))
                        {
                            StartBackgroundTask(DownloadPackage(item));
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("Downlaod", GUILayout.Width(70)))
                        {
                            StartBackgroundTask(DownloadPackage(item));
                        }
                    }
                }

                if (item.IsDownloading)
                {
                    Rect rect = GUILayoutUtility.GetRect(100, 24);
                    EditorGUI.ProgressBar(new Rect(rect.x, rect.y + 4, 100, 17), item.DownloadProgress, "Downloading...");
                }
                EditorGUI.BeginDisabledGroup(!File.Exists(filePath) || !item.CanInstall);
                if (GUILayout.Button("Import", GUILayout.Width(60)))
                {
                    Import(item);
                }
                EditorGUI.EndDisabledGroup();
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

        }

        private bool IsInstanlled(string moduleID) {
            ModuleItem item= m_Items.Where(x => x.id == moduleID).FirstOrDefault();
            return item != null && item.IsInstalled;
        }

        private static void StartBackgroundTask(IEnumerator update)
        {
            EditorApplication.CallbackFunction callback = null;

            callback = () =>
            {
                try
                {
                    if (update.MoveNext()== false)
                    {
                        EditorApplication.update -= callback;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    EditorApplication.update -= callback;
                }
            };

            EditorApplication.update += callback;
        }

        private IEnumerator DownloadPackage(ModuleItem item) {
            using (UnityWebRequest w = UnityWebRequest.Get(item.downloadPath))
            {
                item.IsDownloading = true;
                item.DownloadProgress = 0f;
                yield return w.SendWebRequest();
                while (!w.isDone) {
                    item.DownloadProgress = w.downloadProgress;
                    Repaint();
                    yield return null;
                }

                string filePath = CombinePath(tempPath, item.name + "_" + item.version.Replace(".", "_") + ".unitypackage");
                if(!Directory.Exists(tempPath))
                    Directory.CreateDirectory(tempPath);

                File.WriteAllBytes(filePath, w.downloadHandler.data);
                item.IsDownloading = false;
            }
        }

        private void Import(ModuleItem item)
        {
            string filePath = CombinePath(tempPath, item.name + "_" + item.version.Replace(".", "_") + ".unitypackage");
            if (!File.Exists(filePath))
            {
                Debug.LogErrorFormat("{0} does not exist.", filePath);
                return;
            }

            AssetDatabase.ImportPackage(filePath, false);
            StartBackgroundTask(RequestModules(delegate (ModuleItem[] items) { m_Items = items; Repaint(); }));
        }

        /*private IEnumerator RequestModules()
        {
            using (UnityWebRequest w = UnityWebRequest.Get(m_ModuleTxtPath))
            {
                yield return w.SendWebRequest();
                while (!w.isDone) { yield return null;}
                items = JsonHelper.FromJson<ModuleItem>(w.downloadHandler.text);
                for (int i = 0; i < items.Length; i++) {
                    items[i].Initialize();
                }
            }
        }*/

        private bool MatchesSearch(ModuleItem item, string search) {
            search = search.ToLower();
            return search.Equals("search...") ||
                item.name.ToLower().Contains(search) ||  
                item.description.ToLower().Contains(search);
        }

        private void DoSearchGUI()
        {
            GUILayout.Space(3f);
            m_SearchString = SearchField(m_SearchString);
            GUILayout.Space(3f);
        }

        private void ResizeSidebar()
        {
            Rect rect = new Rect(m_SidebarRect.width - LIST_RESIZE_WIDTH * 0.5f, m_SidebarRect.y, LIST_RESIZE_WIDTH, m_SidebarRect.height);
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeHorizontal);
            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            Event ev = Event.current;
            switch (ev.rawType)
            {
                case EventType.MouseDown:
                    if (rect.Contains(ev.mousePosition))
                    {
                        GUIUtility.hotControl = controlID;
                        ev.Use();
                    }
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == controlID)
                    {
                        GUIUtility.hotControl = 0;
                        ev.Use();
                    }
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == controlID)
                    {
                        m_SidebarRect.width = ev.mousePosition.x;
                        m_SidebarRect.width = Mathf.Clamp(m_SidebarRect.width, LIST_MIN_WIDTH, LIST_MAX_WIDTH);
                        EditorPrefs.SetFloat("ModuleEditorSidebarWidth", m_SidebarRect.width);
                        ev.Use();
                    }
                    break;
            }
        }

        private static string CombinePath(params string[] paths)
        {
            if (paths == null) { return null; }

            string combinedPath = "";

            foreach (var path in paths)
            {
                if (path != null)
                {
                    combinedPath = Path.Combine(combinedPath, path);
                }
            }

            return combinedPath;
        }

       /* private static string SearchField(string search, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            string before = search;
            string after = EditorGUILayout.TextField("", before, "SearchTextField", options);

            if (GUILayout.Button("", "SearchCancelButton", GUILayout.Width(18f)))
            {
                after = "Search...";
                GUIUtility.keyboardControl = 0;
            }
            GUILayout.EndHorizontal();
            return after;
        }*/

        private static string SearchField(string search, params GUILayoutOption[] options)
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
           // EditorGUI.FocusTextInControl("SearchTextFieldFocus");

            GUI.Button(buttonRect, GUIContent.none, (after != "" && after != "Search...") ? "ToolbarSeachCancelButton" : "ToolbarSeachCancelButtonEmpty");
            EditorGUILayout.EndHorizontal();
            return after;
        }

        private static class Styles
        {
            public static GUIStyle minusButton;
            public static GUIStyle selectButton;
            public static GUIStyle background;
            public static GUIStyle seperator;
            public static GUIStyle thumbnail;
            public static GUIStyle thumbnailText;
            public static GUIStyle largeLabel;
            public static GUIStyle largeFoldout;
            public static GUIStyle richTextLabel;
            public static GUIStyle selectButtonText;

            static Styles()
            {
                minusButton = new GUIStyle("OL Minus")
                {
                    margin = new RectOffset(0, 0, 4, 0)
                };
                selectButton = new GUIStyle("MeTransitionSelectHead")
                {
                    alignment = TextAnchor.MiddleLeft,
                    padding= new RectOffset(5,0,0,0),
                    overflow = new RectOffset(0,-1,0,0), 
                };

                selectButtonText = new GUIStyle("MeTransitionSelectHead")
                {
                    alignment = TextAnchor.MiddleLeft,
                    padding = new RectOffset(5, 0, 0, 0),
                    overflow = new RectOffset(0, -1, 0, 0),
                    richText = true
                };
                selectButtonText.normal.background = null;
                selectButtonText.normal.textColor = EditorGUIUtility.isProSkin ? new Color(0.788f, 0.788f, 0.788f, 1f) : new Color(0.047f, 0.047f, 0.047f, 1f);

                background = new GUIStyle("ProfilerLeftPane")
                {
                    padding = new RectOffset(0, 0, 2, 2),
                    overflow= new RectOffset(0, 0, 1, 1)
                };

                seperator = new GUIStyle("IN Title")
                {
                    fixedHeight = 1f,
                    overflow = new RectOffset(),
                    margin = new RectOffset()
                };

                thumbnail = new GUIStyle("IN ThumbnailShadow") {
                    fixedHeight = 50, 
                };

                thumbnailText = new GUIStyle("AM MixerHeader") {
                    alignment = TextAnchor.MiddleLeft,
                    stretchHeight = true,
                    fontSize = 15,
                };

                largeLabel = new GUIStyle(EditorStyles.largeLabel)
                {
                    richText = true,
                    fontSize = 11
                };

                largeFoldout = new GUIStyle(EditorStyles.foldout)
                {
                    richText = true,
                    fontSize = 11,
                    fontStyle = FontStyle.Bold

                };

                richTextLabel = new GUIStyle("label")
                {
                    richText = true,
                    alignment = TextAnchor.MiddleLeft  
                };
                
            }

            public static void Seperator()
            {
                GUIStyle style = new GUIStyle("IN Title");
                style.fixedHeight = 1f;
                GUILayout.Label(GUIContent.none, style);
            }
        }
    }
}