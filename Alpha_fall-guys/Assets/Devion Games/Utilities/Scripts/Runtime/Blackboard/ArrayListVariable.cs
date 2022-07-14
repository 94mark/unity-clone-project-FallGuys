using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames
{
	[System.Serializable]
	public class ArrayListVariable : Variable
	{

		private ArrayList m_Value= new ArrayList();

		public ArrayList Value
		{
			get { return this.m_Value; }
			set { this.m_Value = value; }
		}

		public override object RawValue
		{
			get
			{
				if (this.m_Value == null) {
					this.m_Value = new ArrayList();
				}
				return this.m_Value;
			}
			set
			{
				this.m_Value = (ArrayList)value;
			}
		}

		public override System.Type type
		{
			get
			{
				return typeof(ArrayList);
			}
		}

		public ArrayListVariable()
		{
		}

		public ArrayListVariable(string name) : base(name)
		{
		}

		public static implicit operator ArrayListVariable(ArrayList value)
		{
			return new ArrayListVariable()
			{
				Value = value
			};
		}

		public static implicit operator ArrayList(ArrayListVariable value)
		{
			return value.Value;
		}
	}
}