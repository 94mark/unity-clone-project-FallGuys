using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

namespace DevionGames.UIWidgets{
	public class Joystick : Selectable, IDragHandler {
		[SerializeField]
		protected RectTransform handle=null;
		[SerializeField]
		protected float radius = 90f;
		[SerializeField]
		protected float returnSpeed=4f;
		[SerializeField]
		protected string horizontalAxis="Horizontal";
		[SerializeField]
		protected string verticalAxis="Vertical";

		public JoystickEvent onChange;

		public Vector2 position
		{
			get
			{
				Vector2 pos = handle.anchoredPosition.normalized;
				if (handle.anchoredPosition.magnitude < radius){
					pos = handle.anchoredPosition / radius;
				}
				if(pos.sqrMagnitude < 0.1f && (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject())){
					pos = new Vector2(Input.GetAxis(horizontalAxis),Input.GetAxis(verticalAxis));
				}
				return pos;
			}
		}

		protected RectTransform parentTransform;
		protected bool returnHandle;
		
		protected override void Start(){
			base.Start ();
			parentTransform = GetComponentInParent<RectTransform> ();
			returnHandle = true;
		}

		public void OnDrag (PointerEventData eventData)
		{
			Vector2 pos;
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle (parentTransform, eventData.position, eventData.pressEventCamera, out pos)) {

				handle.localPosition=pos;
				Vector2 handleOffset = handle.anchoredPosition;
				if (handleOffset.magnitude > radius)
				{
					handleOffset = handleOffset.normalized * radius;
					handle.anchoredPosition = handleOffset;
				}
			}

		}
		
		public override void OnPointerDown (PointerEventData eventData)
		{
			base.OnPointerDown (eventData);
			returnHandle = false;
			OnDrag (eventData);
		}

		public override void OnPointerUp (PointerEventData eventData)
		{
			base.OnPointerUp (eventData);
			returnHandle = true;
		}

		private void Update()
		{
			if (returnHandle) {
				if (handle.anchoredPosition.magnitude > Mathf.Epsilon) {
					handle.anchoredPosition -= new Vector2 (handle.anchoredPosition.x * returnSpeed, handle.anchoredPosition.y * returnSpeed) * Time.deltaTime;
				}
			} 
			onChange.Invoke(position);
		}

		[System.Serializable]
		public class JoystickEvent:UnityEvent<Vector2>{

		}
	}
}