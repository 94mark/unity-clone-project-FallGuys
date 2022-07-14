using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames
{
	[System.Serializable]
	public class ObjectVariable : Variable
	{
		[SerializeField]
		private Object m_Value;

		public Object Value {
			get{ return this.m_Value; }
			set{ this.m_Value = value; }
		}

		public override object RawValue {
			get {
				return this.m_Value;
			}
			set {
				this.m_Value = (Object)value;
			}
		}

		public override System.Type type {
			get {
				return typeof(Object);
			}
		}

		public ObjectVariable ()
		{
		}

		public ObjectVariable (string name) : base (name)
		{
		}

		public static implicit operator ObjectVariable(Object value)
		{
			return new ObjectVariable()
			{
				Value = value
			};
		}

		public static implicit operator Object(ObjectVariable value)
		{
			return value.Value;
		}
	}
}