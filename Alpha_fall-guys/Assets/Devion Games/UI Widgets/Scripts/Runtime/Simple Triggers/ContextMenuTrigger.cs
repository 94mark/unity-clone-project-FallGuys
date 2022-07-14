using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevionGames.UIWidgets;
using ContextMenu = DevionGames.UIWidgets.ContextMenu;
using UnityEngine.EventSystems;

public class ContextMenuTrigger : MonoBehaviour, IPointerDownHandler
{
    private ContextMenu m_ContextMenu;

    public string[] menu;

    // Start is called before the first frame update
    private void Start()
    {
        this.m_ContextMenu = WidgetUtility.Find<ContextMenu>("ContextMenu");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            this.m_ContextMenu.Clear();
            for (int i = 0; i < menu.Length; i++)
            {
                string menuItem = menu[i];
                m_ContextMenu.AddMenuItem(menuItem, delegate { Debug.Log("Used - " + menuItem); });
            }
            this.m_ContextMenu.Show();
        }
    }
}
