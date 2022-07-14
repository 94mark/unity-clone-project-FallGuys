using UnityEngine;
using System.Collections;
using UnityEditor;

namespace DevionGames
{
    public interface ICollectionEditor
    {
        string ToolbarName { get; }
        void OnGUI(Rect position);
        void OnEnable();
        void OnDisable();
        void OnDestroy();
      
    }
}