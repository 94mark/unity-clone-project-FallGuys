using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

namespace DevionGames{
	/// <summary>
	/// Utility editor window.
	/// </summary>

	public class UtilityWindow : EditorWindow {
		private System.Action onClose;
		private System.Action onGUI;
		private Vector2 scroll;
        private bool useScrollView;

		public static UtilityWindow ShowWindow(string title, System.Action onGUI){
			return ShowWindow (title, new Vector2 (227,200), onGUI, null, true);
		}
		
		public static UtilityWindow ShowWindow(string title,Vector2 size, System.Action onGUI){
			return ShowWindow (title, size, onGUI, null, true);
		}

		public static UtilityWindow ShowWindow(string title,Vector2 size, System.Action onGUI, System.Action onClose, bool useScrollView){
            UtilityWindow window = ScriptableObject.CreateInstance<UtilityWindow>();
            window.hideFlags = HideFlags.HideAndDontSave;
            window.titleContent = new GUIContent(title);
            window.minSize = size;
			window.onGUI = onGUI;
			window.onClose = onClose;
            window.useScrollView=useScrollView;
            window.ShowUtility();
            return window;
		}

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            UtilityWindow[] windows = Resources.FindObjectsOfTypeAll<UtilityWindow>();
            for (int i = 0; i < windows.Length; i++) {
                windows[i].Close();
            }
        }


        private void OnDestroy(){
			if (onClose != null) {
				onClose.Invoke();
			}
		}


		private void OnGUI(){
            if(useScrollView)
			    scroll = EditorGUILayout.BeginScrollView (scroll);

			if (onGUI != null) {
				onGUI.Invoke ();
			}
            if(useScrollView)
			EditorGUILayout.EndScrollView ();
		}
	}
}