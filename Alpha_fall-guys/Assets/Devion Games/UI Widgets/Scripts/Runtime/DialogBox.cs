using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DevionGames.UIWidgets
{
    public class DialogBox : UIWidget
    {
        /// <summary>
		/// Closes the window when a button is clicked.
		/// </summary>
		public bool autoClose = true;
        [Header("Reference")]
        /// <summary>
        /// The title component reference
        /// </summary>
        public Text title;
        /// <summary>
        /// The text component reference
        /// </summary>
        public Text text;
        /// <summary>
        /// The icon sprite reference
        /// </summary>
        public Image icon;
        /// <summary>
        /// The button prefab reference
        /// </summary>
        public Button button;

        protected List<Button> buttonCache = new List<Button>();
        protected GameObject m_IconParent;

        protected override void OnAwake()
        {
            base.OnAwake();
            if(icon != null)
                m_IconParent = icon.GetComponentInParent<LayoutElement>().gameObject;

        }

        public virtual void Show(NotificationOptions settings, UnityAction<int> result, params string[] buttons)
        {
            Show(settings.title, WidgetUtility.ColorString(settings.text, settings.color), settings.icon, result, buttons);
        }

        /// <summary>
        /// Show the MessageBox
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="text">Text.</param>
        /// <param name="buttons">Buttons.</param>
        public virtual void Show(string title, string text, params string[] buttons)
        {
            Show(title, text, null, null, buttons);
        }

        /// <summary>
        /// Show the MessageBox
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="text">Text.</param>
        /// <param name="result">Result.</param>
        /// <param name="buttons">Buttons.</param>
        public virtual void Show(string title, string text, UnityAction<int> result, params string[] buttons)
        {
            Show(title, text, null, result, buttons);
        }

        /// <summary>
        /// Show the MessageBox
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="text">Text.</param>
        /// <param name="icon">Icon.</param>
        /// <param name="result">Result.</param>
        /// <param name="buttons">Buttons.</param>
        public virtual void Show(string title, string text, Sprite icon, UnityAction<int> result, params string[] buttons)
        {
            for (int i = 0; i < buttonCache.Count; i++)
            {
                buttonCache[i].onClick.RemoveAllListeners();
                buttonCache[i].gameObject.SetActive(false);
            }
            if (this.title != null)
            {
                if (!string.IsNullOrEmpty(title))
                {
                    this.title.text = title;
                    this.title.gameObject.SetActive(true);
                }
                else
                {
                    this.title.gameObject.SetActive(false);
                }
            }
            if (this.text != null)
            {
                this.text.text = text;
            }

            if (this.icon != null)
            {
                if (icon != null)
                {
                    this.icon.overrideSprite = icon;
                    this.m_IconParent.SetActive(true);
                }
                else
                {
                    this.m_IconParent.SetActive(false);
                }
            }
            base.Show();
            button.gameObject.SetActive(false);
            for (int i = 0; i < buttons.Length; i++)
            {
                string caption = buttons[i];
                int index = i;
                AddButton(caption).onClick.AddListener(delegate () {
                    if (this.autoClose)
                    {
                        base.Close();
                    }
                    if (result != null)
                    {
                        result.Invoke(index);
                    }
                });
            }
        }

        private Button AddButton(string text)
        {
            Button mButton = buttonCache.Find(x => !x.isActiveAndEnabled);
            if (mButton == null)
            {
                mButton = Instantiate(button) as Button;
                buttonCache.Add(mButton);
            }
            mButton.gameObject.SetActive(true);
            mButton.onClick.RemoveAllListeners();
            mButton.transform.SetParent(button.transform.parent, false);
            Text[] buttonTexts = mButton.GetComponentsInChildren<Text>(true);
            if (buttonTexts.Length > 0)
            {
                buttonTexts[0].text = text;
            }
            return mButton;
        }
    }
}