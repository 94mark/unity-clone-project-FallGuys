using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DevionGames.UIWidgets
{
	public class MenuItem : Selectable, IPointerClickHandler
	{

		private UnityEvent m_Trigger = new UnityEvent ();

		public UnityEvent onTrigger {
			get {
				if (m_Trigger == null) {
					m_Trigger = new UnityEvent ();
				}
				return this.m_Trigger;
			}
			set {
				this.m_Trigger = value;
			}
		}

		private void Press ()
		{
			if (!IsActive () || !IsInteractable ())
				return;

			onTrigger.Invoke ();
		}

		public void OnPointerClick (PointerEventData eventData)
		{
			Press ();
		}


		public override void OnPointerEnter (PointerEventData eventData)
		{
			base.OnPointerEnter (eventData);
			DoStateTransition (SelectionState.Highlighted, false);

		}
	}
}