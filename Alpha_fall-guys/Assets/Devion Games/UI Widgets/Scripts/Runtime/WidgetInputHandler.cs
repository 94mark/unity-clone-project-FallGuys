using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DevionGames.UIWidgets
{
    public class WidgetInputHandler : MonoBehaviour
    {
       // private static List<UIWidget> m_VisibilityStack = new List<UIWidget>();
        private static Dictionary<KeyCode, List<UIWidget>> m_WidgetKeyBindings;

       /* private void Start()
        {

            m_VisibilityStack = WidgetUtility.FindAll<UIWidget>().Where(x => !x.IsVisible) .ToList();
            m_VisibilityStack.ForEach(x => Debug.Log(x.Name));
            m_VisibilityStack.OrderBy(x => x.transform.GetSiblingIndex());
        }*/

  

        // Update is called once per frame
        void Update()
        {
           /* if (Input.GetKeyDown(KeyCode.Escape)) {
                m_VisibilityStack.OrderBy(x => x.transform.GetSiblingIndex());
                UIWidget widget= m_VisibilityStack.FirstOrDefault(x=>x.IsVisible);
                if(widget != null)
                    widget.Close();
            }*/

            if (m_WidgetKeyBindings == null) {
                return;
            }

            foreach (KeyValuePair<KeyCode, List<UIWidget>> kvp in m_WidgetKeyBindings)
            {
                if (Input.GetKeyDown(kvp.Key)){
                    for (int i = 0; i < kvp.Value.Count; i++) {
                        kvp.Value[i].Toggle();
                        
                    }
                }
            }
        }

        public static void RegisterInput(KeyCode key, UIWidget widget) {
            if (m_WidgetKeyBindings == null) {
                WidgetInputHandler handler = GameObject.FindObjectOfType<WidgetInputHandler>();
                if (handler == null)
                {
                    GameObject handlerObject = new GameObject("WidgetInputHandler");
                    handlerObject.AddComponent<WidgetInputHandler>();
                    handlerObject.AddComponent<SingleInstance>();
                }
                m_WidgetKeyBindings = new Dictionary<KeyCode, List<UIWidget>>();
            }
            if (key == KeyCode.None) {
                return;
            }

            List<UIWidget> widgets;
            if (!m_WidgetKeyBindings.TryGetValue(key, out widgets))
            {
                m_WidgetKeyBindings.Add(key, new List<UIWidget>() { widget });
            }else {
                widgets.RemoveAll(x => x == null);
                widgets.Add(widget);
                m_WidgetKeyBindings[key] = widgets;
            }
        }

        public static void UnregisterInput(KeyCode key, UIWidget widget)
        {
            if (m_WidgetKeyBindings == null)
                return;

            List<UIWidget> widgets;
            if (m_WidgetKeyBindings.TryGetValue(key, out widgets))
            {
                widgets.Remove(widget);
            }
        }
    }
}