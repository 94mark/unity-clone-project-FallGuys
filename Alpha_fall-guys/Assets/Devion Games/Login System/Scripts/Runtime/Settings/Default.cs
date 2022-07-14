using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.LoginSystem.Configuration
{
    [System.Serializable]
    public class Default : Settings
    {
        public override string Name
        {
            get
            {
                return "Default";
            }
        }


        [Header("Default Settings:")]
        public bool skipLogin = false;
        public bool loadSceneOnLogin = true;
        public string sceneToLoad = "SciFi Login Successful";
        public bool debug = true;
    }
}