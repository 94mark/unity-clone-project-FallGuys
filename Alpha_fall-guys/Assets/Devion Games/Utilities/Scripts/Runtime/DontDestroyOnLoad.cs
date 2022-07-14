using UnityEngine;
using System.Collections;

namespace DevionGames{
	public class DontDestroyOnLoad : MonoBehaviour {
		private void Awake(){
			DontDestroyOnLoad (gameObject);
		}
	}
}