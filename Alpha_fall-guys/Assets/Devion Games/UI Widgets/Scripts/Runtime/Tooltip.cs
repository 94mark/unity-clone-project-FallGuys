using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace DevionGames.UIWidgets{
	public class Tooltip : UIWidget {
        [Header("Behaviour")]
        /// <summary>
        /// Update position to follow mouse 
        /// </summary>
        [SerializeField]
        protected bool m_UpdatePosition=true;
        [SerializeField]
        protected Vector2 m_PositionOffset = Vector2.zero;

        [Header("Reference")]
        /// <summary>
		/// The Text component to display tooltip title.
		/// </summary>
        [SerializeField]
		protected Text m_Title;
        /// <summary>
        /// The Text component to display tooltip text.
        /// </summary>
        [SerializeField]
        protected Text m_Text;
		/// <summary>
		/// The Image component to display the icon.
		/// </summary>
        [SerializeField]
		protected Image m_Icon;
		/// <summary>
		/// The background image.
		/// </summary>
        [SerializeField]
		protected Image m_Background;
        /// <summary>
        /// StringPairSlot prefab to display a string - string pair 
        /// </summary>
        [SerializeField]
        protected StringPairSlot m_SlotPrefab;
   
        protected Canvas m_Canvas;
        protected Transform m_SlotParent;
        protected float m_Width = 300f;
        protected List<StringPairSlot> m_SlotCache;

        protected bool _updatePosition;

        protected override void OnStart()
        {
            base.OnStart();
            this.m_SlotCache = new List<StringPairSlot>();
            this.m_Canvas = GetComponentInParent<Canvas>();
            this.m_SlotParent = this.m_SlotPrefab.transform.parent;
            this.m_Width = m_RectTransform.sizeDelta.x;
            if (IsVisible){
                Close();
            }
        }

        protected override void Update()
        {
            base.Update();
            if (this.m_UpdatePosition && m_CanvasGroup.alpha > 0f && _updatePosition)
            {
                UpdatePosition();
            }
        }

        protected void UpdatePosition()
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_Canvas.transform as RectTransform, Input.mousePosition, this.m_Canvas.worldCamera, out pos);
            Vector2 offset = Vector2.zero;

            if (Input.mousePosition.x < m_RectTransform.sizeDelta.x){
                offset += new Vector2(m_RectTransform.sizeDelta.x * 0.5f, 0);
            }else{
                offset += new Vector2(-m_RectTransform.sizeDelta.x * 0.5f, 0);
            }
            if (Screen.height - Input.mousePosition.y > m_RectTransform.sizeDelta.y){
                offset += new Vector2(0, m_RectTransform.sizeDelta.y * 0.5f);
            }else{
                offset += new Vector2(0, -m_RectTransform.sizeDelta.y * 0.5f);
            }
            pos = pos + offset+ this.m_PositionOffset;
            transform.position = this.m_Canvas.transform.TransformPoint(pos);
            Focus();
        }

        public override void Show()
        {
            base.Show();
            UpdatePosition();
            _updatePosition = true;
            m_CanvasGroup.interactable = false;
            m_CanvasGroup.blocksRaycasts = false;
        }

        public virtual void Show(string text)
        {
            Show(string.Empty, text, null, null, this.m_Width, true);
        }

        public virtual void Show(string title, string text)
        {
            Show(title, text, null, null, this.m_Width, true);
        }

        public virtual void Show(string title, string text, Sprite icon, List<KeyValuePair<string, string>> pairs)
        {
            Show(title, text, icon, pairs, this.m_Width, true);
        }

        public virtual void Show(string title, string text, Sprite icon, List<KeyValuePair<string, string>> pairs, float width, bool showBackground)
        {
            if (!string.IsNullOrEmpty(title))
            {
                this.m_Title.gameObject.SetActive(true);
                this.m_Title.text = title;
            }
            else
            {
                this.m_Title.gameObject.SetActive(false);
            }

            this.m_Text.text = text;

            if (icon != null)
            {
                this.m_Icon.overrideSprite = icon;
                this.m_Icon.transform.parent.gameObject.SetActive(true);
            }
            else
            {
                this.m_Icon.transform.parent.gameObject.SetActive(false);
            }

            if (pairs != null && pairs.Count > 0)
            {
                for (int i = 0; i < this.m_SlotCache.Count;i++) {
                    this.m_SlotCache[i].gameObject.SetActive(false);
                }
               
                while (pairs.Count > this.m_SlotCache.Count)
                {
                    CreateSlot();
                }

                for (int i = 0; i < pairs.Count; i++)
                {
                    StringPairSlot slot = this.m_SlotCache[i];
                    slot.gameObject.SetActive(true);
                    slot.Target = pairs[i];
                }
                this.m_SlotParent.gameObject.SetActive(true);
            }
            else
            {
                this.m_SlotParent.gameObject.SetActive(false);
            }
            m_RectTransform.sizeDelta = new Vector2(width, m_RectTransform.sizeDelta.y);
           
            this.m_Background.gameObject.SetActive(showBackground);

            Show();
        }

        /// <summary>
        /// Close this widget.
        /// </summary>
        public override void Close()
        {
            base.Close();
            _updatePosition = false;
        }

        protected virtual StringPairSlot CreateSlot()
        {
            if (this.m_SlotPrefab != null)
            {
                GameObject go = (GameObject)Instantiate(this.m_SlotPrefab.gameObject);
                go.SetActive(true);
                go.transform.SetParent(this.m_SlotParent, false);
                StringPairSlot slot = go.GetComponent<StringPairSlot>();
                this.m_SlotCache.Add(slot);

                return slot;
            }
            Debug.LogWarning("[Tooltip] Please ensure that the slot prefab is set in the inspector.");
            return null;
        }

    }
}