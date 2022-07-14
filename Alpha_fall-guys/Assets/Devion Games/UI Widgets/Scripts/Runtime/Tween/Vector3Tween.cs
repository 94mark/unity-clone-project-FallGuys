using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace DevionGames.UIWidgets{
	internal struct Vector3Tween : ITweenValue
	{
		private Vector3Tween.Vector3TweenCallback m_Target;
		private Vector3Tween.Vector3TweenFinishCallback m_OnFinish;

		private EasingEquations.EaseType m_EaseType;
		public EasingEquations.EaseType easeType{
			get{return this.m_EaseType;}
			set{this.m_EaseType = value;}
		}
		private Vector3 m_StartValue;
		public Vector3 startValue{
			get{return this.m_StartValue;}
			set{this.m_StartValue = value;}
		}
		private Vector3 m_TargetValue;
		public Vector3 targetValue{
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
			float x = EasingEquations.GetValue (easeType, startValue.x, targetValue.x, floatPercentage);
			float y = EasingEquations.GetValue (easeType, startValue.y, targetValue.y, floatPercentage);
			float z = EasingEquations.GetValue (easeType, startValue.z, targetValue.z, floatPercentage);
			this.m_Target.Invoke(new Vector3(x,y,z));
		}

		public void AddOnChangedCallback(UnityAction<Vector3> callback)
		{
			if (m_Target == null)
				m_Target = new Vector3TweenCallback();
			
			m_Target.AddListener (callback);
		}
		
		public void AddOnFinishCallback(UnityAction callback)
		{
			if (m_OnFinish == null)
				m_OnFinish = new Vector3TweenFinishCallback();
			
			m_OnFinish.AddListener (callback);
		}

		public void OnFinish()
		{
			if (m_OnFinish != null)
				m_OnFinish.Invoke();
		}

		public class Vector3TweenCallback : UnityEvent<Vector3>
		{
			public Vector3TweenCallback()
			{
			}
		}

		public class Vector3TweenFinishCallback : UnityEvent
		{
			public Vector3TweenFinishCallback()
			{
			}
		}
	}
}