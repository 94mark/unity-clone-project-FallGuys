using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames
{
    public interface ISelectable
    {
        bool enabled { get; }
        Vector3 position { get; }
        void OnSelect();
        void OnDeselect();
    }
}