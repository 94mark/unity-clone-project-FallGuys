using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DevionGames
{
    public class TimedEnable : MonoBehaviour
    {
        [SerializeField]
        private float m_Delay = 1f;
        [SerializeField]
        private Behaviour m_Combonent=null;
        [SerializeField]
        private bool m_Enable = true;

        private void OnEnable()
        {
            StartCoroutine(WaitAndSetEnabled());
        }

        private IEnumerator WaitAndSetEnabled() {
            yield return new WaitForSeconds(this.m_Delay);
            if (this.m_Combonent != null)
                this.m_Combonent.enabled = this.m_Enable;

            enabled = false;
        }
    }
}