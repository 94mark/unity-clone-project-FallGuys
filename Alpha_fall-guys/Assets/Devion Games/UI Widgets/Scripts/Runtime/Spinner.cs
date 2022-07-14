using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

namespace DevionGames.UIWidgets{
	public class Spinner : MonoBehaviour {
		public float changeDelay = 0.1f;
		[SerializeField]
		protected float m_Current;
		public float current{
			get{
				return this.m_Current;
			}
			set{
                if (this.m_Current != value)
                {
                    this.m_Current = value;
                    onChange.Invoke(value);
                    m_OnChange.Invoke(Mathf.RoundToInt(value).ToString());

                }

			}
		}
		public float step = 1.0f;
		public float min;
		public float max;
		public SpinnerEvent onChange=new SpinnerEvent();
		public SpinnerTextEvent m_OnChange=new SpinnerTextEvent();

		protected IEnumerator coroutine;

        public void SetCurrent(string value) {
            if (string.IsNullOrEmpty(value)) {
                value = "0";
            }
           current = Mathf.Clamp(System.Convert.ToSingle(value),min,max);
        }

		public void StartIncrease(){
			Stop ();
			coroutine = Increase ();
			StartCoroutine (coroutine);
		}

		public void StartDecrease(){
			Stop ();
			coroutine = Decrease ();
			StartCoroutine (coroutine);
		}

		public void Stop(){
			if (coroutine != null) {
				StopCoroutine(coroutine);
			}
		}

		public IEnumerator Increase(){
			while (true) {
				current = Mathf.Clamp (current+step, min, max);
				yield return new WaitForSeconds(changeDelay);
			}
		}

		public IEnumerator Decrease(){
			while (true) {
				current = Mathf.Clamp (current-step, min, max);
				yield return new WaitForSeconds(changeDelay);
			}
		}

		[System.Serializable]
		public class SpinnerEvent : UnityEvent<float>{} 
		
		[System.Serializable]
		public class SpinnerTextEvent : UnityEvent<string>{} 
	}
}