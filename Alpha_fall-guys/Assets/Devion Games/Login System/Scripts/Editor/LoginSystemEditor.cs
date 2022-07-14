using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace DevionGames.LoginSystem
{
	public class LoginSystemEditor : EditorWindow
	{

		public static void ShowWindow()
		{

			LoginSystemEditor[] objArray = Resources.FindObjectsOfTypeAll<LoginSystemEditor>();
			LoginSystemEditor editor = (objArray.Length <= 0 ? ScriptableObject.CreateInstance<LoginSystemEditor>() : objArray[0]);

			editor.hideFlags = HideFlags.HideAndDontSave;
			editor.minSize = new Vector2(690, 300);
#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1
			editor.title="Item System";
#else
			editor.titleContent = new GUIContent("Login System");
#endif
			editor.SelectDatabase();
		}

		public static LoginSystemEditor instance;

		private LoginConfigurations database;
		private static LoginConfigurations db;
		public static LoginConfigurations Database
		{
			get
			{
				if (LoginSystemEditor.instance != null)
				{
					db = LoginSystemEditor.instance.database;
				}
				return db;
			}
			set
			{
				db = value;
				if (LoginSystemEditor.instance != null)
				{
					LoginSystemEditor.instance.database = value;
				}
			}
		}

		private List<ICollectionEditor> childEditors;

		[SerializeField]
		private int toolbarIndex;

		private string[] toolbarNames
		{
			get
			{
				string[] items = new string[childEditors.Count];
				for (int i = 0; i < childEditors.Count; i++)
				{
					items[i] = childEditors[i].ToolbarName;
				}
				return items;
			}

		}

		private void OnEnable()
		{
			instance = this;
			if (database == null)
			{
				SelectDatabase();
			}
			ResetChildEditors();
		}

		private void OnDestroy()
		{
			if (childEditors != null)
			{
				for (int i = 0; i < childEditors.Count; i++)
				{
					childEditors[i].OnDestroy();
				}
			}
			instance = null;
			db = null;
		}

        private void Update()
        {
			Repaint();
        }

        private void OnGUI()
		{
			if (childEditors != null)
			{
				EditorGUILayout.Space();
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				toolbarIndex = GUILayout.Toolbar(toolbarIndex, toolbarNames, GUILayout.MinWidth(200));
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				childEditors[toolbarIndex].OnGUI(new Rect(0f, 30f, position.width, position.height - 30f));
			}
		}

		private void SelectDatabase()
		{
			string searchString = "Search...";
			LoginConfigurations[] databases = EditorTools.FindAssets<LoginConfigurations>();

			UtilityInstanceWindow.ShowWindow("Select Configuration", delegate () {
				searchString = EditorTools.SearchField(searchString);

				for (int i = 0; i < databases.Length; i++)
				{
					if (!string.IsNullOrEmpty(searchString) && !searchString.Equals("Search...") && !databases[i].name.Contains(searchString))
					{
						continue;
					}
					databases[i].settings = databases[i].settings.OrderBy(x => x.Order).ToList();

					GUIStyle style = new GUIStyle("button");
					style.wordWrap = true;
					if (GUILayout.Button(AssetDatabase.GetAssetPath(databases[i]), style))
					{
						database = databases[i];
						ResetChildEditors();
						Show();
						UtilityInstanceWindow.CloseWindow();
					}
				}
				GUILayout.FlexibleSpace();
				Color color = GUI.backgroundColor;
				GUI.backgroundColor = Color.green;
				if (GUILayout.Button("Create"))
				{
					LoginConfigurations db = EditorTools.CreateAsset<LoginConfigurations>(true);
					if (db != null)
					{
						ArrayUtility.Add<LoginConfigurations>(ref databases, db);
					}
				}
				GUI.backgroundColor = color;
			});

		}

		private void ResetChildEditors()
		{
			if (database != null)
			{
				if (childEditors != null)
				{
					for (int i = 0; i < childEditors.Count; i++)
					{
						childEditors[i].OnDestroy();
					}
				}
				childEditors = new List<ICollectionEditor>();
				childEditors.Add(new Configuration.LoginSettingsEditor(database, database.settings));
			}
		}
	}
}