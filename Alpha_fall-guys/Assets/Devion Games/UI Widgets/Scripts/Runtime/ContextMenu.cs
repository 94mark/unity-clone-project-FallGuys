using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DevionGames.UIWidgets
{
	public class ContextMenu : UIWidget
	{
		[Header ("Reference")]
		[SerializeField]
		protected MenuItem m_MenuItemPrefab= null;
		protected List<MenuItem> itemCache = new List<MenuItem> ();

		public override void Show ()
		{
			m_RectTransform.position = Input.mousePosition;
			base.Show ();
		}

		protected override void Update ()
		{
			base.Update();
			if (m_CanvasGroup.alpha > 0f && (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1) || Input.GetMouseButtonDown (2))) {

				var pointer = new PointerEventData (EventSystem.current);
				pointer.position = Input.mousePosition;
				var raycastResults = new List<RaycastResult> ();
				EventSystem.current.RaycastAll (pointer, raycastResults);

				for (int i = 0; i < raycastResults.Count; i++) {
					MenuItem item = raycastResults [i].gameObject.GetComponent<MenuItem> ();
					if (item != null) {
						item.OnPointerClick (pointer);
						return;
					}
				}

				Close ();
			}
		}

		public virtual void Clear ()
		{
			for (int i = 0; i < itemCache.Count; i++) {
				itemCache [i].gameObject.SetActive (false);
			}
		}

		public virtual MenuItem AddMenuItem (string text, UnityAction used)
		{
			MenuItem item = itemCache.Find (x => !x.gameObject.activeSelf);

			if (item == null) {
				Debug.Log(text);
				item = Instantiate (m_MenuItemPrefab) as MenuItem;
				itemCache.Add (item);
			}
			Text itemText = item.GetComponentInChildren<Text> ();

			if (itemText != null) {
				itemText.text = text;
			}
			item.onTrigger.RemoveAllListeners ();
			item.gameObject.SetActive (true);
			item.transform.SetParent (m_RectTransform, false);
			item.onTrigger.AddListener (delegate() {
				Close ();
				if (used != null) {
					used.Invoke ();
				}
			});
			return item;
		}
	}
}