using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.UIWidgets
{
    public class FloatingTextManager : MonoBehaviour
    {
        private static FloatingTextManager current;

        [SerializeField]
        private FloatingText m_Prefab = null;

        private static Dictionary<GameObject, FloatingText> m_FloatingTexts = new Dictionary<GameObject, FloatingText>();

        private void Awake()
        {
            if (FloatingTextManager.current != null){
                Destroy(gameObject);
                return;
            }else{
                FloatingTextManager.current = this;
            }
        }

        public static void Add(GameObject target, string text, Color color, Vector3 offset) {
            if (!m_FloatingTexts.ContainsKey(target))
            {
                var floatingText = Instantiate(FloatingTextManager.current.m_Prefab, FloatingTextManager.current.transform);
                floatingText.SetText(target.transform, text, color, offset);
                floatingText.gameObject.SetActive(true);
                m_FloatingTexts.Add(target,floatingText);
            }
        }

        public static void Remove(GameObject target) {
            if (m_FloatingTexts.ContainsKey(target) && m_FloatingTexts[target] != null)
            {
                Destroy(m_FloatingTexts[target].gameObject);
               m_FloatingTexts.Remove(target);
            }
        }

    }
}