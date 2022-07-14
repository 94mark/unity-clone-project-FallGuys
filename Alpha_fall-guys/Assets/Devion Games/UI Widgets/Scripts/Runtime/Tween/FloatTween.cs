using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace DevionGames.UIWidgets{
	internal struct FloatTween : ITweenValue
	{
		private FloatTween.FloatTweenCallback m_Target;
		private FloatTween.FloatTweenFinishCallback m_OnFinish;

		private EasingEquations.EaseType m_EaseType;
		public EasingEquations.EaseType easeType{
			get{return this.m_EaseType;}
			set{this.m_EaseType = value;}
		}
		private float m_StartValue;
		public float startValue{
			get{return this.m_StartValue;}
			set{this.m_StartValue = value;}
		}
		private float m_TargetValue;
		public float targetValue{
			get{return this.m_TargetValue;}
			set{this.m_TargetValue = value;}
		}
		private float m_Duration;
		public float duration{
			get { return this.m_Duration; }
			set { this.m_Duration = value; }
		}
		private bool m_IgnoreTimeScale;
		public bool ignoreTimeScale{
			get { return this.m_IgnoreTimeScale; }
			set { this.m_IgnoreTimeScale = value; }
		}

		public bool ValidTarget()
		{
			return this.m_Target != null;
		}

		public void TweenValue(float floatPercentage)
		{
			if (!this.ValidTarget()){
				return;
			}
			float value = EasingEquations.GetValue (easeType, startValue, targetValue, floatPercentage);
			this.m_Target.Invoke(value);
           

		}

		public void AddOnChangedCallback(UnityAction<float> callback)
		{
			if (m_Target == null)
				m_Target = new FloatTweenCallback();

			m_Target.AddListener (callback);
		}
		
		public void AddOnFinishCallback(UnityAction callback)
		{
			if (m_OnFinish == null)
				m_OnFinish = new FloatTweenFinishCallback();

            m_OnFinish.AddListener (callback);
		}

		public void OnFinish()
		{
			if (m_OnFinish != null)
				m_OnFinish.Invoke();
		}

		public class FloatTweenCallback : UnityEvent<float>
		{
			public FloatTweenCallback()
			{
			}
		}

		public class FloatTweenFinishCallback : UnityEvent
		{
			public FloatTweenFinishCallback()
			{
			}
		}
	}
}