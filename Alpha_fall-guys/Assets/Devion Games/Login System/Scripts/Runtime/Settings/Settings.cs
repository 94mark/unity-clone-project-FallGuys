using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.LoginSystem.Configuration
{
    [System.Serializable]
    public abstract class Settings : ScriptableObject, INameable
    {
        public virtual string Name
        {
            get { return "Settings"; }
            set { }
        }

        public virtual int Order {
            get { return 0; }
            set { }
        }
    }
}