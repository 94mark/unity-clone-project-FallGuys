using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames
{
	[System.Serializable]
	public class IntVariable : Variable
	{
		[SerializeField]
		private int m_Value;

		public int Value {
			get{ return this.m_Value; }
			set{ this.m_Value = value; }
		}

		public override object RawValue {
			get {
				return this.m_Value;
			}
			set {
				this.m_Value = (int)value;
			}
		}

		public override System.Type type {
			get {
				return typeof(int);
			}
		}

		public IntVariable ()
		{
		}

		public IntVariable (string name) : base (name)
		{
		}

		public static implicit operator IntVariable(int value)
		{
			return new IntVariable()
			{
				Value = value
			};
		}

		public static implicit operator int(IntVariable value)
		{
			return value.Value;
		}
	}
}