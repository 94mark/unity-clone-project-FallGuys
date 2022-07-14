using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace DevionGames
{
    public class UpdateNotificationWindow : EditorWindow
    {
        private ModuleItem[] m_UpdatedItems;
        private Texture2D m_Icon;
        public Texture2D Icon
        {
            get
            {
                if (this.m_Icon == null)
                {
                    this.m_Icon = Resources.Load<Texture2D>("ModuleIcon");
                }
                return this.m_Icon;
            }
        }

        public static void ShowWindow(ModuleItem[] updatedItems)
		{
			UpdateNotificationWindow window = EditorWindow.GetWindow<UpdateNotificationWindow>(true, "Module Update Check");
			Vector2 size = new Vector2(520f, 260f);
			window.minSize = size;
            window.maxSize = size;
			window.wantsMouseMove = true;
            window.m_UpdatedItems = updatedItems;
		}



        private void OnGUI()
        {
            if (m_UpdatedItems == null) {
                return;
            }
            EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(Icon, GUILayout.Width(46), GUILayout.Height(46));
            EditorGUILayout.Space();

            ShowUpdates();
         

            EditorGUILayout.EndHorizontal();
            OpenModuleManager();
            CheckForUpdates();
            EditorGUILayout.EndVertical();
        }

        private void ShowUpdates(){
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("There are new module versions available for download.");
            GUILayout.FlexibleSpace();
            for (int i = 0; i < m_UpdatedItems.Length; i++)
            {
                ModuleItem item = m_UpdatedItems[i];
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(item.name);

                EditorGUILayout.LabelField(item.version, GUILayout.Width(120));
                EditorGUILayout.EndHorizontal();

            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();
        }


        private void OpenModuleManager() {
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Open Module Manager", "AC Button"))
            {
                ModuleManagerWindow.ShowWindow();
                Close();
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void CheckForUpdates() {
            EditorGUILayout.Space(20f);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            bool checkUpdates = EditorPrefs.GetBool("ModuleUpdateCheck", true);
            bool flag = EditorGUILayout.ToggleLeft("Check for Updates", checkUpdates, GUILayout.Width(130f));
            if (checkUpdates != flag)
            {
                EditorPrefs.SetBool("ModuleUpdateCheck", flag);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}