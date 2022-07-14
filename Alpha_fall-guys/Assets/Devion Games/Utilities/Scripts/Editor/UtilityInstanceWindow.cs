using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

namespace DevionGames{
	/// <summary>
	/// Utility editor window.
	/// </summary>
	public class UtilityInstanceWindow : EditorWindow {
		public static UtilityInstanceWindow instance;
		private System.Action onClose;
		private System.Action onGUI;
		private Vector2 scroll;

		public static UtilityInstanceWindow ShowWindow(string title, System.Action onGUI){
			return ShowWindow (title, new Vector2 (227,200), onGUI, null);
		}
		
		public static UtilityInstanceWindow ShowWindow(string title,Vector2 size, System.Action onGUI){
			return ShowWindow (title, size, onGUI, null);
		}

		public static UtilityInstanceWindow ShowWindow(string title,Vector2 size, System.Action onGUI, System.Action onClose){
			UtilityInstanceWindow window = EditorWindow.GetWindow<UtilityInstanceWindow>(true,title);
			window.minSize = size;
			window.onGUI = onGUI;
			window.onClose = onClose;
			return window;
		}

		private void OnEnable(){
			instance = this;
			AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
		}


        private void OnDisable()
        {
			AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
		}

        public static void CloseWindow(){
			if (UtilityInstanceWindow.instance != null) {
				UtilityInstanceWindow.instance.Close();
			}
		}


		private void OnAfterAssemblyReload()
		{
			Close();
		}

		private void OnDestroy(){
			if (onClose != null) {
				onClose.Invoke();
			}
		}

		private void OnGUI(){
			scroll = EditorGUILayout.BeginScrollView (scroll);
			if (onGUI != null) {
				onGUI.Invoke ();
			}
			EditorGUILayout.EndScrollView ();
            //Focus();
		}
	}
}