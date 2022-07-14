using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

namespace DevionGames{
	public class PropertyBinding : MonoBehaviour
	{	
		[SerializeField]
		private PropertyRef m_Source=null;
		[SerializeField]
		private PropertyRef m_Target=null;
		[SerializeField]
		private Execution m_Execution = Execution.Update;
		[SerializeField]
		private float m_Interval = 0.3f;
		
		private void Start(){
			if (m_Execution == Execution.Start) {
				UpdateTarget ();
			}
			if (m_Execution == Execution.Interval) {
				StartCoroutine(IntervalUpdate());
			}
		}
		
		private void Update(){
			if (m_Execution == Execution.Update) {
				UpdateTarget ();
			}
		}
		
		private void LateUpdate(){
			if (m_Execution == Execution.LateUpdate) {
				UpdateTarget ();
			}
		}
		
		private void FixedUpdate(){
			if (m_Execution == Execution.FixedUpdate) {
				UpdateTarget ();
			}
		}
		
		private IEnumerator IntervalUpdate(){
			while (true) {
				yield return new WaitForSeconds(m_Interval);
				UpdateTarget();
			}
		}
		
		public void UpdateTarget ()
		{
			m_Target.SetValue (m_Source.GetValue ());
		}
		
		public enum Execution{
			Start,
			Update,
			LateUpdate,
			FixedUpdate,
			Interval
		}
		
		[System.Serializable]
		public class PropertyRef{
			[SerializeField]
			private Component m_Component= null;
			public Component component{
				get{
					return this.m_Component;
				}
			}
			
			private FieldInfo m_Field;
			private PropertyInfo m_Property;
			
			[SerializeField]
			private string m_PropertyPath=string.Empty;
			public string propertyPath
			{
				get
				{
					return this.m_PropertyPath;
				}
			}
			
			public object GetValue ()
			{
				if (this.m_Field == null && this.m_Property == null) {
					CacheProperty();
				}
				
				if (this.m_Property != null)
				{
					if (this.m_Property.CanRead)
						return this.m_Property.GetValue(this.m_Component, null);
				}
				else if (this.m_Field != null)
				{
					return this.m_Field.GetValue(this.m_Component);
				}
				return null;
			}
			
			public bool SetValue(object value){
				if (this.m_Field == null && this.m_Property == null && !CacheProperty()) {
					return false;
				}
				if (this.m_Field != null){
					this.m_Field.SetValue(this.m_Component, value);
					return true;
				}else if (this.m_Property.CanWrite){
					this.m_Property.SetValue(this.m_Component, value, null);
					return true;
				}
				return false;
			}
			
			private bool CacheProperty ()
			{
				if (this.m_Component != null && !string.IsNullOrEmpty(this.m_PropertyPath))
				{
					Type type = this.m_Component.GetType();
					#if NETFX_CORE
					this.m_Field = type.GetRuntimeField(this.m_PropertyPath);
					this.m_Property = type.GetRuntimeProperty(this.m_PropertyPath);
					#else
					this.m_Field = type.GetField(this.m_PropertyPath);
					this.m_Property = type.GetProperty(this.m_PropertyPath);
					#endif
				}
				else
				{
					this.m_Field = null;
					this.m_Property = null;
				}
				return (this.m_Field != null || this.m_Property != null);
			}
		}
	}
}