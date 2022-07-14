using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace DevionGames
{
	[System.Serializable]
	public class ObjectProperty : INameable
	{
		[SerializeField]
		private string name= string.Empty;

		public string Name {
			get{ return this.name; }
			set{ this.name = value; }
		}

		[SerializeField]
		private int typeIndex;

		public Type SerializedType {
			get {
				return SupportedTypes [typeIndex];
			}
		}

		
		public string stringValue;
		public int intValue;

		public float floatValue;
		public Color colorValue;
		public bool boolValue;
		public UnityEngine.Object objectReferenceValue;
		public Vector2 vector2Value;
		public Vector3 vector3Value;
		public bool show;
		public Color color = Color.white;

		public object GetValue ()
		{
			Type mType = SerializedType;
			
			if (mType == null) {
				return null;			
			} else if (mType == typeof(string)) {
				return stringValue;		
			} else if (mType == typeof(bool)) {
				return boolValue;
			} else if (mType == typeof(Color)) {
				return colorValue;
			} else if (mType == typeof(float)) {
				return floatValue;
			} else if (typeof(UnityEngine.Object).IsAssignableFrom (mType)) {
				return objectReferenceValue;
			} else if (mType == typeof(int)) {
				return intValue;
			} else if (mType == typeof(Vector2)) {
				return vector2Value;
			} else if (mType == typeof(Vector3)) {
				return vector3Value;
			} else {
				return null;
			}
		}

		public void SetValue (object value)
		{
			if (value is string)
			{
				typeIndex = 0;
				stringValue = (string)value;
			}
			else if (value is bool)
			{
				typeIndex = 1;
				boolValue = (bool)value;
			}
			else if (value is Color)
			{
				typeIndex = 2;
				colorValue = (Color)value;
			}
			else if (value is float || value is double)
			{
				typeIndex = 3;
				floatValue = System.Convert.ToSingle(value);
			}
			else if (typeof(UnityEngine.Object).IsAssignableFrom(value.GetType()))
			{
				typeIndex = 4;
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
				typeIndex = 5;
				intValue = System.Convert.ToInt32(value);
			}
			else if (value is Vector2)
			{
				typeIndex = 6;
				vector2Value = (Vector2)value;
			}
			else if (value is Vector3)
			{
				typeIndex = 7;
				vector3Value = (Vector3)value;
			}
			else {
				Debug.LogWarning("Type is not supported "+value);
			}
		}

		public static string GetPropertyName (Type mType)
		{
			if (mType == typeof(string)) {
				return "stringValue";		
			} else if (mType == typeof(bool)) {
				return "boolValue";
			} else if (mType == typeof(Color)) {
				return "colorValue";
			} else if (mType == typeof(float)) {
				return "floatValue";
			} else if (typeof(UnityEngine.Object).IsAssignableFrom (mType)) {
				return "objectReferenceValue";
			} else if (mType == typeof(int)) {
				return "intValue";
			} else if (mType == typeof(Vector2)) {
				return "vector2Value";
			} else if (mType == typeof(Vector3)) {
				return "vector3Value";
			} 
			return string.Empty;
		}

		public string ToString (string format)
		{
			return SerializedType == typeof(float) ? floatValue.ToString (format) : GetValue ().ToString ();
		}

		public static Type[] SupportedTypes {
			get {
				return new Type[8] {
					typeof(string),
					typeof(bool),
					typeof(Color),
					typeof(float),
					typeof(UnityEngine.Object),
					typeof(int),
					typeof(Vector2),
					typeof(Vector3),
				};
			}
		}

		public static string[] DisplayNames {
			get {
				return new string[8] {
					"String",
					"Bool",
					"Color",
					"Float",
					"Object",
					"Int",
					"Vector2",
					"Vector3",
				};
			}
		}
	}
}