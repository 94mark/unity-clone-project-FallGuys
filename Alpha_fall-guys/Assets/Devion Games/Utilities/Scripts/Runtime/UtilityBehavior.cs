using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames
{
    public class UtilityBehavior : MonoBehaviour
    {
        public void QuitApplication() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void LoadScene(string scene) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        }

        public void Instantiate(GameObject gameObject) {
            GameObject.Instantiate(gameObject, transform.position, Quaternion.identity);
        }
    }
}