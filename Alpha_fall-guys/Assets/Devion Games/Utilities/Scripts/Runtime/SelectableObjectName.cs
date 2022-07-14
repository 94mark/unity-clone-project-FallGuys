using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames
{
    public class SelectableObjectName : MonoBehaviour
    {
        [SerializeField]
        private Text m_ObjectName = null;

        private void Start()
        {
            if (this.m_ObjectName == null)
                this.m_ObjectName = GetComponent<Text>();
        }

        private void Update()
        {
            if (SelectableObject.current == null) return;

            string current = SelectableObject.current.name;
            if (this.m_ObjectName != null && !current.Equals(this.m_ObjectName.text))
                this.m_ObjectName.text = current;
        }
    }
}