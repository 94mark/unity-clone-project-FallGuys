using UnityEngine;
using UnityEditor;
using System.Collections;

namespace DevionGames.LoginSystem{
	public static class LoginSystemMenu {
		
		[MenuItem("Tools/Devion Games/Login System/Configurations",false, -1)]
		private static void OpenEditor(){
			LoginSystemEditor.ShowWindow ();
		}

		[MenuItem("Tools/Devion Games/Login System/Create Login Manager",false, 0)]
		private static void CreateLoginManager(){
			GameObject go = new GameObject ("Login Manager");
			go.AddComponent<LoginManager> ();
			Selection.activeGameObject = go;
		}
		
		[MenuItem ("Tools/Devion Games/Login System/Create Login Manager", true)]
		private static bool ValidateCreateLoginManager() {
			return GameObject.FindObjectOfType<LoginManager> () == null;
		}
	}
}