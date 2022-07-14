using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DevionGames.UIWidgets
{
	public class Progressbar : UIWidget
	{
		[Header ("Reference")]
		[SerializeField]
		protected Image progressbar;
		[SerializeField]
		protected Text m_ProgressbarTitle;
		[SerializeField]
		protected Text progressLabel;
		[SerializeField]
		protected string format = "F0";

		protected override void OnStart ()
		{
			progressbar.type = Image.Type.Filled;
		}

		public virtual void SetProgress (float progress)
		{
			progressbar.fillAmount = progress;
			if (progressLabel != null) {
				progressLabel.text = (progress * 100f).ToString (format) + "%";
			}
		}

		public override void Show()
		{
			this.Show("");
		}

		public virtual void Show(string title)
		{
			if (this.m_ProgressbarTitle != null) {
				this.m_ProgressbarTitle.text = title;
			}
			progressbar.fillAmount = 0f;
			if (progressLabel != null)
			{
				progressLabel.text = "0%";
			}
			base.Show();
		}

		
	}
}