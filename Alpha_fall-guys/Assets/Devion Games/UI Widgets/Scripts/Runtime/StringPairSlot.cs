using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.UIWidgets
{
    public class StringPairSlot : MonoBehaviour
    {

        [SerializeField]
        protected Text m_Key;
        [SerializeField]
        protected Text m_Value;

        private KeyValuePair<string, string> m_Target;
        public KeyValuePair<string, string> Target {
            get { return this.m_Target; }
            set {
                this.m_Target = value;
                Repaint();
            }
        }
    
        public virtual void Repaint() {
            this.m_Key.text = Target.Key;
            this.m_Value.text = Target.Value;
        }
    }
}