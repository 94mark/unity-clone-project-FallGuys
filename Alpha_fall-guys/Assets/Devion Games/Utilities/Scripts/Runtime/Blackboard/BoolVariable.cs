using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames
{
	[System.Serializable]
	public class BoolVariable:Variable
	{
		[SerializeField]
		private bool m_Value;

		public bool Value {
			get{ return this.m_Value; }
			set{ this.m_Value = value; }
		}

		public override object RawValue {
			get {
				return this.m_Value;
			}
			set {
				this.m_Value = (bool)value;
			}
		}

		public override System.Type type {
			get {
				return typeof(bool);
			}
		}

		public BoolVariable ()
		{
		}

		public BoolVariable (string name) : base (name)
		{
		}

		public static implicit operator BoolVariable(bool value)
		{
			return new BoolVariable()
			{
				Value = value
			};
		}

		public static implicit operator bool(BoolVariable value)
		{
			return value.Value;
		}
	}
}