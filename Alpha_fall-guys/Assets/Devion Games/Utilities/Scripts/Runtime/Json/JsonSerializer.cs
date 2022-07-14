using UnityEngine;
using System.Collections;
using System.Text;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DevionGames{
	public static class JsonSerializer {
		/*public static string Serialize(List<object> list, ref List<UnityEngine.Object> objectReferences)
		{

			List<UnityEngine.Object> serializedObjects = new List<UnityEngine.Object>();
			Dictionary<string, object>[] listData = new Dictionary<string, object>[list.Count];

			for (int i = 0; i < list.Count; i++)
			{
				listData[i] = Serialize(list[i], ref serializedObjects);
			}
			objectReferences = serializedObjects ;
			return MiniJSON.Serialize(listData);
		}

		private static Dictionary<string, object> Serialize(object obj, ref List<UnityEngine.Object> objectReferences)
		{
			Dictionary<string, object> data = new Dictionary<string, object>() {
				{ "Type", obj.GetType ().FullName },
			};


			SerializeFields(obj, ref data, ref objectReferences);
			return data;
		}

		private static void SerializeFields(object obj, ref Dictionary<string, object> dic, ref List<UnityEngine.Object> objectReferences)
		{
			if (obj == null)
			{
				return;
			}
			Type type = obj.GetType();
			FieldInfo[] fields = type.GetAllSerializedFields();

			for (int j = 0; j < fields.Length; j++)
			{
				FieldInfo field = fields[j];
				object value = field.GetValue(obj);
				Debug.Log(value);
				SerializeValue(field.Name, value, ref dic, ref objectReferences);
			}
		}


		private static void SerializeValue(string key, object value, ref Dictionary<string, object> dic, ref List<UnityEngine.Object> objectReferences)
		{
			if (value != null && !dic.ContainsKey(key))
			{
				Type type = value.GetType();
				Debug.Log(type);
				if (typeof(UnityEngine.Object).IsAssignableFrom(type))
				{
					UnityEngine.Object unityObject = value as UnityEngine.Object;
					if (!objectReferences.Contains(unityObject))
					{
						objectReferences.Add(unityObject);
					}
					dic.Add(key, objectReferences.IndexOf(unityObject));
				}
				else if (typeof(LayerMask).IsAssignableFrom(type))
				{
					dic.Add(key, ((LayerMask)value).value);
				}
				else if (typeof(Enum).IsAssignableFrom(type))
				{
					dic.Add(key, (Enum)value);
				}
				else if (type.IsPrimitive ||
						 type == typeof(string) ||
						 type == typeof(Vector2) ||
						 type == typeof(Vector3) ||
						 type == typeof(Vector4) ||
						 type == typeof(Color))
				{
					dic.Add(key, value);
				}
				else if (typeof(IList).IsAssignableFrom(type))
				{
					Debug.Log("Serialize List: "+type);
					IList list = (IList)value;
					Dictionary<string, object> s = new Dictionary<string, object>();
					for (int i = 0; i < list.Count; i++)
					{
						SerializeValue(i.ToString(), list[i], ref s, ref objectReferences);
					}
					dic.Add(key, s);
				}
				else
				{
					Dictionary<string, object> data = new Dictionary<string, object>();
					SerializeFields(value, ref data, ref objectReferences);
					dic.Add(key, data);
				}
			}
		}


		public static List<T> Deserialize<T>(string serializationData, List<UnityEngine.Object> objectReferences)
		{
			List<T> list = new List<T>();
			if (string.IsNullOrEmpty(serializationData))
			{
				return list;
			}
			List<object> data = MiniJSON.Deserialize(serializationData) as List<object>;

			for (int i = 0; i < data.Count; i++)
			{

				object obj = Deserialize(data[i] as Dictionary<string, object>, objectReferences);
				list.Add((T)obj);
			}

			return list;
		}


		private static object Deserialize(Dictionary<string, object> data, List<UnityEngine.Object> objectReferences)
		{
			string typeString = (string)data["Type"];
			Type type = Utility.GetType(typeString);
			if (type == null && !string.IsNullOrEmpty(typeString))
			{
				type = Utility.GetType(typeString);
			}

			object obj = System.Activator.CreateInstance(type);
			DeserializeFields(obj, data, objectReferences);
			return obj;
		}

		private static void DeserializeFields(object source, Dictionary<string, object> data, List<UnityEngine.Object> objectReferences)
		{
			if (source == null) { return; }
			Type type = source.GetType();
			FieldInfo[] fields = type.GetAllSerializedFields();

			for (int j = 0; j < fields.Length; j++)
			{
				FieldInfo field = fields[j];
				object value = DeserializeValue(field.Name, source, field, field.FieldType, data, objectReferences);
				if (value != null)
				{
					field.SetValue(source, value);
				}
			}
		}


		private static object DeserializeValue(string key, object source, FieldInfo field, Type type, Dictionary<string, object> data, List<UnityEngine.Object> objectReferences)
		{
			object value;
			if (data.TryGetValue(key, out value))
			{
				Debug.Log("key: "+key+" type:"+type);
				if (typeof(UnityEngine.Object).IsAssignableFrom(type))
				{
					int index = System.Convert.ToInt32(value);
					if (index >= 0 && index < objectReferences.Count)
					{
						return objectReferences[index];
					}
				}
				else if (typeof(LayerMask) == type)
				{
					LayerMask mask = new LayerMask();
					mask.value = (int)value;
					return mask;
				}
				else if (typeof(Enum).IsAssignableFrom(type))
				{
					return Enum.Parse(type, (string)value);
				}
				else if (type.IsPrimitive ||
						 type == typeof(string) ||
						 type == typeof(Vector2) ||
						 type == typeof(Vector3) ||
						 type == typeof(Vector4) ||
						 type == typeof(Quaternion) ||
						 type == typeof(Color))
				{
					Debug.Log("RETURN key: " + key + " type:" + type+" value:"+value);
					return value;
				}
				else if (typeof(IList).IsAssignableFrom(type))
				{
					Dictionary<string, object> dic = value as Dictionary<string, object>;
					Type targetType = typeof(List<>).MakeGenericType(Utility.GetElementType(type));
					IList result = (IList)Activator.CreateInstance(targetType);
					int count = dic.Count;
					for (int i = 0; i < count; i++)
					{
						Type elementType = Utility.GetElementType(type);
						Debug.Log("type:"+type+" element:"+elementType);
						result.Add(DeserializeValue(i.ToString(), source, field, elementType, dic, objectReferences));
					}

					if (type.IsArray)
					{
						Array array = Array.CreateInstance(Utility.GetElementType(type), count);
						result.CopyTo(array, 0);
						return array;
					}
					return result;
				}
				else
				{
					Dictionary<string, object> dic = value as Dictionary<string, object>;
					if (dic.ContainsKey("m_Type"))
					{
						type = Utility.GetType((string)dic["m_Type"]);
					}
					object instance = Activator.CreateInstance(type);

					DeserializeFields(instance, value as Dictionary<string, object>, objectReferences);
					return instance;
				}
			}
			return null;
		}*/


		public static string Serialize(IJsonSerializable[] objs){
			List<object> list = new List<object> ();
			for (int i=0; i<objs.Length; i++) {
				if(objs[i] != null){
					Dictionary<string,object> data = new Dictionary<string, object> ();
					objs[i].GetObjectData (data);
					list.Add(data);
				}
			}
			return MiniJSON.Serialize (list);
		}
		
		public static void Deserialize(string json, IJsonSerializable[] objs){
			if(string.IsNullOrEmpty(json)){
				return;
			}
			List<object> list = MiniJSON.Deserialize (json) as List<object>;
			for (int i = 0; i < list.Count; i++) {
				Dictionary<string,object> data = list[i] as Dictionary<string,object>;
				objs[i].SetObjectData (data);
			}
		}

		public static List<T> Deserialize<T>(string json) where T:IJsonSerializable{
			List<T> result = new List<T> ();
			if(string.IsNullOrEmpty(json)){
				return result;
			}

			List<object> list = MiniJSON.Deserialize (json) as List<object>;
			if (list != null) {
				for (int i = 0; i < list.Count; i++) {
					Dictionary<string,object> data = list [i] as Dictionary<string,object>;
					T obj = default(T);
					if(typeof(ScriptableObject).IsAssignableFrom(typeof(T))){
						obj = (T)(object)ScriptableObject.CreateInstance(typeof(T));
					}else{
						obj = (T)Activator.CreateInstance (typeof(T));
					}
					obj.SetObjectData (data);
					result.Add (obj);
				}
			}
			return result;
		}


		public static string Serialize(IJsonSerializable obj){
			Dictionary<string,object> data = new Dictionary<string, object> ();
			obj.GetObjectData (data);
			return MiniJSON.Serialize (data);
		}

		public static void Deserialize(string json, IJsonSerializable obj){
			if(string.IsNullOrEmpty(json)){
				return;
			}
			Dictionary<string,object> data = MiniJSON.Deserialize (json) as Dictionary<string,object>;
			obj.SetObjectData (data);
		}
	}
}