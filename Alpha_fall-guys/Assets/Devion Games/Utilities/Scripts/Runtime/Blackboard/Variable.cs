using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DevionGames
{
	[System.Serializable]
	public abstract class Variable
	{
		[SerializeField]
		private string m_Name = string.Empty;

		public virtual string name {
			get{ return this.m_Name; }
			set{ this.m_Name = value; }
		}

		[SerializeField]
		private bool m_IsShared;

		public virtual bool isShared {
			get{ return this.m_IsShared; }
			set{ this.m_IsShared = value; }
		}

		public virtual bool isNone {
			get{ return (this.m_Name == "None" || string.IsNullOrEmpty (this.m_Name)) && this.m_IsShared; }
		}

		public abstract Type type{ get; }

		public abstract object RawValue {
			get;
			set;
		}

		public Variable ()
		{

		}

		public Variable (string name)
		{
			this.name = name;
		}

	}
}