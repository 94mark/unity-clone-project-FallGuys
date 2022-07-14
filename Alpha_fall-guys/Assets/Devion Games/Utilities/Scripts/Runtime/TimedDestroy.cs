using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DevionGames
{
    public class TimedDestroy : MonoBehaviour
    {
        [SerializeField]
        private float m_Delay = 1f;
        [SerializeField]
        private UnityEvent m_OnDestroy = null;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(this.m_Delay);
            this.m_OnDestroy?.Invoke();
            Destroy(gameObject);
        }
    }
}