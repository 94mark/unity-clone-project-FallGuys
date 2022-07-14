using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace DevionGames.UIWidgets
{
	/// <summary>
	/// Tooltip trigger to display fixed tooltips
	/// </summary>
	public class TooltipTrigger : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
	{

		/// <summary>
		/// The name of the tooltip instance.
		/// </summary>
		[SerializeField]
		private string instanceName = "Tooltip";
		/// <summary>
		/// Show the background element
		/// </summary>
		[SerializeField]
		private bool showBackground=true;
		//Width to use, Height is set based on width
		[SerializeField]
		private float width = 300;
		/// <summary>
		/// Color of the text.
		/// </summary>
		[SerializeField]
		private Color color = Color.white;
        /// The title to display
		/// </summary>
        public string tooltipTitle;
        /// <summary>
        /// The text to display
        /// </summary>
        [TextArea]
		public string tooltip;
		/// <summary>
		/// Optionally show an icon
		/// </summary>
		public Sprite icon;

        public StringPair[] properties;

		private Tooltip instance;
        private Coroutine m_DelayTooltipCoroutine;
        private List<KeyValuePair<string, string>> m_PropertyPairs;

		/// <summary>
		/// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
		/// </summary>
		private void Start ()
		{
			//Find tooltip instance with name "Tooltip"
			instance = WidgetUtility.Find<Tooltip> (instanceName);
			//Check if an instance of UITooltip is located in scene
			if (enabled && instance == null) {
				//No instance -> disable trigger
				enabled = false;
			}
            this.m_PropertyPairs = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < properties.Length; i++) {
                this.m_PropertyPairs.Add(new KeyValuePair<string, string>(properties[i].key,properties[i].value));
            }
		}

		/// <summary>
		/// Called when the mouse pointer starts hovering the ui element.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnPointerEnter (PointerEventData eventData)
		{
            //Show tooltip
            ShowTooltip();
		}

		/// <summary>
		/// Called when the mouse pointer exits the element
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnPointerExit (PointerEventData eventData)
		{
            //Hide tooltip
            CloseTooltip();
		}

        private IEnumerator DelayTooltip(float delay)
        {
            float time = 0.0f;
            yield return true;
            while (time < delay)
            {
                time += Time.deltaTime;
                yield return true;
            }

   
            instance.Show(WidgetUtility.ColorString(tooltipTitle, color), WidgetUtility.ColorString(tooltip, color), icon, m_PropertyPairs, width, showBackground);
        }

        private void ShowTooltip()
        {

            if (this.m_DelayTooltipCoroutine != null)
            {
                StopCoroutine(this.m_DelayTooltipCoroutine);
            }
            this.m_DelayTooltipCoroutine = StartCoroutine(DelayTooltip(0.3f));

        }

        private void CloseTooltip()
        {
            instance.Close();
            if (this.m_DelayTooltipCoroutine != null)
            {
                StopCoroutine(this.m_DelayTooltipCoroutine);
            }
        }

        [System.Serializable]
        public class StringPair {
            public string key;
            public string value;
        }
    }
}