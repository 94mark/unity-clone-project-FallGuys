using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames
{
    [System.Serializable]
    public class NamedVariable : INameable
    {
        [SerializeField]
        private string m_Name = "New Variable";
        public string Name { get => this.m_Name; set => this.m_Name = value; }

		[TextArea]
		[SerializeField]
		private string m_Description=string.Empty;

		public string Description { get => this.m_Description; set => this.m_Description = value; }

		[SerializeField]
		private NamedVariableType m_VariableType = 0;

		public NamedVariableType VariableType
		{
			get{return this.m_VariableType;}
			set { this.m_VariableType = value; }
		}

		public Type ValueType {
			get {
				switch (VariableType)
				{
					case NamedVariableType.Bool:
						return typeof(bool);
					case NamedVariableType.Color:
						return typeof(Color);
					case NamedVariableType.Float:
						return typeof(float);
					case NamedVariableType.Int:
						return typeof(int);
					case NamedVariableType.Object:
						return typeof(UnityEngine.Object);
					case NamedVariableType.String:
						return typeof(string);
					case NamedVariableType.Vector2:
						return typeof(Vector2);
					case NamedVariableType.Vector3:
						return typeof(Vector3);
				}
				return null;
			}
		}

		public string[] VariableTypeNames {
			get {
				return new string[] {"Bool","Color","Float","Int", "Object", "String","Vector2", "Vector3"};
			}
		}

		public string stringValue = string.Empty;
		public int intValue = 0;
		public float floatValue = 0f;
		public Color colorValue = Color.white;
		public bool boolValue = false;
		public UnityEngine.Object objectReferenceValue = null;
		public Vector2 vector2Value = Vector2.zero;
		public Vector3 vector3Value = Vector3.zero;

		public object GetValue()
		{
			switch (VariableType) {
				case NamedVariableType.Bool:
					return boolValue;
				case NamedVariableType.Color:
					return colorValue;
				case NamedVariableType.Float:
					return floatValue;
				case NamedVariableType.Int:
					return intValue;
				case NamedVariableType.Object:
					return objectReferenceValue;
				case NamedVariableType.String:
					return stringValue;
				case NamedVariableType.Vector2:
					return vector2Value;
				case NamedVariableType.Vector3:
					return vector3Value;
			}
			return null;
		}

		public void SetValue(object value)
		{
			
			if (value is string)
			{
				m_VariableType = NamedVariableType.String;
				stringValue = (string)value;
			}
			else if (value is bool)
			{
				m_VariableType = NamedVariableType.Bool;
				boolValue = (bool)value;
			}
			else if (value is Color)
			{
				m_VariableType = NamedVariableType.Color;
				colorValue = (Color)value;
			}
			else if (value is float || value is double)
			{
				m_VariableType = NamedVariableType.Float;
				floatValue = System.Convert.ToSingle(value);
			}
			else if (value == null || typeof(UnityEngine.Object).IsAssignableFrom(value.GetType()))
			{
				m_VariableType = NamedVariableType.Object;
				objectReferenceValue = (UnityEngine.Object)value;
			}
			else if (value is int
					 || value is uint
					 || value is long
					 || value is sbyte
					 || value is byte
					 || value is short
					 || value is ushort
					 || value is ulong)
			{
				m_VariableType = NamedVariableType.Int;
				intValue = System.Convert.ToInt32(value);
			}
			else if (value is Vector2)
			{
				m_VariableType = NamedVariableType.Vector2;
				vector2Value = (Vector2)value;
			}
			else if (value is Vector3)
			{
				m_VariableType = NamedVariableType.Vector3;
				vector3Value = (Vector3)value;
			}
		}

		public string PropertyPath
		{
			get
			{
				switch (m_VariableType)
				{
					case NamedVariableType.Bool:
						return "boolValue";
					case NamedVariableType.Color:
						return "colorValue";
					case NamedVariableType.Float:
						return "floatValue";
					case NamedVariableType.Int:
						return "intValue";
					case NamedVariableType.Object:
						return "objectReferenceValue";
					case NamedVariableType.String:
						return "stringValue";
					case NamedVariableType.Vector2:
						return "vector2Value";
					case NamedVariableType.Vector3:
						return "vector3Value";
				}
				return string.Empty;
			}
		}
	}

	public enum NamedVariableType : int
	{
		String = 0,
		Bool = 2,
		Color = 3,
		Float = 4,
		Object = 5,
		Int = 6,
		Vector2 = 7,
		Vector3 = 8
	}
}