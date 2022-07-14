using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DevionGames
{
    public class SelectableObject : CallbackHandler, ISelectable
    {
        public static SelectableObject current;

        private Transform m_Transform;

        public Vector3 position { get { return this.m_Transform != null ? this.m_Transform.position : Vector3.zero; } }

        public override string[] Callbacks => new string[] {"OnSelect","OnDeselect" };

        protected virtual void Awake()
        {
            this.m_Transform = transform;    
        }

        protected virtual void Start() { }

        public void OnSelect()
        {
            current = this;
            Execute("OnSelect", new CallbackEventData());
          
        }

        public void OnDeselect()
        {
            Execute("OnDeselect", new CallbackEventData());
            current = null;
        }

        private void OnDestroy()
        {
            if (current == this)
                OnDeselect();
        }

    }
}