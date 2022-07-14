using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevionGames.UIWidgets;
using UnityEngine.EventSystems;

public class RadialMenuTrigger : MonoBehaviour, IPointerDownHandler
{
 
    public Sprite[] menuIcons;

    private RadialMenu m_RadialMenu;

    // Start is called before the first frame update
    private void Start()
    {
        this.m_RadialMenu = WidgetUtility.Find<RadialMenu>("RadialMenu");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.m_RadialMenu.Show(gameObject, menuIcons, delegate (int index) { Debug.Log("Used index - " + index); });
        
    }
}
