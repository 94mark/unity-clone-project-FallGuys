using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace DevionGames.UIWidgets
{
	[RequireComponent (typeof(Image), typeof(CanvasGroup))]
	public class UICursor : MonoBehaviour
	{
		private static UICursor instance;
		
		private RectTransform rectTransform;
		private Image image;
		private CanvasGroup canvasGroup;
		private Canvas canvas;
        private Vector2 defaultSize;
        private Animator animator;
        private int animatorDefaultHash;


		private void Awake ()
		{
            if(instance == null)
			    instance = this; 
		}

		private void Start ()
		{
            animator = GetComponent<Animator>();
   
            rectTransform = GetComponent<RectTransform> ();
			image = GetComponent<Image> ();
			canvas = GetComponentInParent<Canvas> ();
			canvasGroup = GetComponent<CanvasGroup> ();
			canvasGroup.alpha = 0f;
			canvasGroup.blocksRaycasts = false;
			canvasGroup.interactable = false;
            defaultSize = rectTransform.sizeDelta;
            if (animator != null)
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                animatorDefaultHash = stateInfo.shortNameHash;
            }

        }

		void Update ()
		{
			if (canvasGroup.alpha > 0) {
				Vector2 pos;
				RectTransformUtility.ScreenPointToLocalPointInRectangle (canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
				transform.position = canvas.transform.TransformPoint (pos);
			}
		}

		public static void Clear ()
		{	
			Set (null);
            
		}

		public static void Set (Sprite sprite)
		{

            if (instance != null)
            {
                Set(sprite, instance.defaultSize, true, string.Empty);
            }
		}

        public static void Set(Sprite sprite, Vector2 size, bool showCursor, string animatorState)
        {
            if (instance != null && instance.image && instance.gameObject.activeInHierarchy)
            {
                if (sprite != null)
                {
                    instance.rectTransform.SetAsLastSibling();
                    instance.image.sprite = sprite;
                    instance.canvasGroup.alpha = 1f;
                }
                else
                {
                    instance.canvasGroup.alpha = 0f;
                }
                instance.rectTransform.sizeDelta = size;
                Cursor.visible = showCursor;
                instance.SetAnimatorState(animatorState);
            }
        }

        public void SetAnimatorState(string animatorState) {
            if (animator == null) {
                return;
            }
            int hash = Animator.StringToHash(animatorState);
            if (string.IsNullOrEmpty(animatorState))
            {
                animator.CrossFadeInFixedTime(this.animatorDefaultHash, 0.2f);
            }
            else if(animator.HasState(0,hash)){
                animator.CrossFadeInFixedTime(hash,0.2f);
            }
        }

    }
}