using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DevionGames
{
    public static class Utility
    {
		private static Assembly[] m_AssembliesLookup;
		private static Dictionary<string, Type> m_TypeLookup;
		private static Dictionary<Type, FieldInfo[]> m_SerializedFieldInfoLookup;
		private static readonly Dictionary<Type, MethodInfo[]> m_MethodInfoLookup;
		private readonly static Dictionary<MemberInfo, object[]> m_MemberAttributeLookup;
	
		static Utility() {
			Utility.m_AssembliesLookup = GetLoadedAssemblies();
			Utility.m_TypeLookup = new Dictionary<string, Type>();
			Utility.m_SerializedFieldInfoLookup = new Dictionary<Type, FieldInfo[]>();
			Utility.m_MethodInfoLookup = new Dictionary<Type, MethodInfo[]>();
			Utility.m_MemberAttributeLookup = new Dictionary<MemberInfo, object[]>();
		}

		/// <summary>
		/// Gets the Type with the specified name, performing a case-sensitive search.
		/// </summary>
		/// <param name="typeName"></param>
		/// <returns>The type with the specified name, if found; otherwise, null.</returns>
		public static Type GetType(string typeName)
		{
			if (string.IsNullOrEmpty(typeName)) { Debug.LogWarning("Type name should not be null or empty!"); return null; }
			Type type;
			if (m_TypeLookup.TryGetValue(typeName, out type))
			{
				return type;
			}
			type = Type.GetType(typeName);
			if (type == null)
			{
				int num = 0;
				while (num < m_AssembliesLookup.Length)
				{
					type = Type.GetType(string.Concat(typeName, ",", m_AssembliesLookup[num].FullName));
					if (type == null)
					{
						num++;
					}
					else
					{
						break;
					}
				}
			}

			if (type == null)
			{
				foreach (Assembly a in m_AssembliesLookup)
				{
					Type[] assemblyTypes = a.GetTypes();
					for (int j = 0; j < assemblyTypes.Length; j++)
					{
						if (assemblyTypes[j].Name == typeName)
						{
							type = assemblyTypes[j];
							break;
						}
					}
				}
			}

			if (type != null)
			{
				m_TypeLookup.Add(typeName, type);
			}

			return type;
		}

		public static Type GetElementType(Type type)
		{
			Type[] interfaces = type.GetInterfaces();

			return (from i in interfaces
					where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)
					select i.GetGenericArguments()[0]).FirstOrDefault();
		}

		public static MethodInfo[] GetAllMethods(this Type type)
		{
			MethodInfo[] methods = new MethodInfo[0];
			if (type != null && !Utility.m_MethodInfoLookup.TryGetValue(type, out methods))
			{
				methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Concat(GetAllMethods(type.GetBaseType())).ToArray();
				Utility.m_MethodInfoLookup.Add(type, methods);
			}

			return methods;
		}

		public static FieldInfo GetSerializedField(this Type type, string name)
		{
			return type.GetAllSerializedFields().Where(x => x.Name == name).FirstOrDefault();
		}

		public static FieldInfo[] GetAllSerializedFields(this Type type)
		{
			if (type == null)
			{
				return new FieldInfo[0];
			}
			FieldInfo[] fields = GetSerializedFields(type).Concat(GetAllSerializedFields(type.BaseType)).ToArray();
			fields = fields.OrderBy(x => x.DeclaringType.BaseTypesAndSelf().Count()).ToArray();
			return fields;
		}

		public static FieldInfo[] GetSerializedFields(this Type type)
		{
			FieldInfo[] fields;
			if (!Utility.m_SerializedFieldInfoLookup.TryGetValue(type, out fields))
			{
				fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.IsPublic && !x.HasAttribute(typeof(NonSerializedAttribute)) || x.HasAttribute(typeof(SerializeField)) || x.HasAttribute(typeof(SerializeReference))).ToArray();
				fields = fields.OrderBy(x => x.DeclaringType.BaseTypesAndSelf().Count()).ToArray();
				Utility.m_SerializedFieldInfoLookup.Add(type, fields);
			}
			return fields;
		}

		public static IEnumerable<Type> BaseTypesAndSelf(this Type type)
		{
			while (type != null)
			{
				yield return type;
				type = type.BaseType;
			}
		}

		public static IEnumerable<Type> BaseTypes(this Type type)
		{
			while (type != null)
			{
				type = type.BaseType;
				yield return type;

			}
		}

		public static object[] GetCustomAttributes(MemberInfo memberInfo, bool inherit)
		{
			object[] customAttributes;
			if (!Utility.m_MemberAttributeLookup.TryGetValue(memberInfo, out customAttributes))
			{
				customAttributes = memberInfo.GetCustomAttributes(inherit);
				Utility.m_MemberAttributeLookup.Add(memberInfo, customAttributes);
			}
			return customAttributes;
		}

		public static T[] GetCustomAttributes<T>(this MemberInfo memberInfo)
		{
			object[] objArray = Utility.GetCustomAttributes(memberInfo, true);
			List<T> list = new List<T>();
			for (int i = 0; i < (int)objArray.Length; i++)
			{
				if (objArray[i].GetType() == typeof(T) || objArray[i].GetType().IsSubclassOf(typeof(T)))
				{
					list.Add((T)objArray[i]);
				}
			}
			return list.ToArray();
		}

		public static T GetCustomAttribute<T>(this MemberInfo memberInfo)
		{
			object[] objArray =Utility.GetCustomAttributes(memberInfo, true);
			for (int i = 0; i < (int)objArray.Length; i++)
			{
				if (objArray[i].GetType() == typeof(T) || objArray[i].GetType().IsSubclassOf(typeof(T)))
				{
					return (T)objArray[i];
				}
			}
			return default(T);
		}

		public static bool HasAttribute<T>(this MemberInfo memberInfo)
		{
			return memberInfo.HasAttribute(typeof(T));
		}

		public static bool HasAttribute(this MemberInfo memberInfo, Type attributeType)
		{
			object[] objArray = Utility.GetCustomAttributes(memberInfo, true);
			for (int i = 0; i < (int)objArray.Length; i++)
			{
				if (objArray[i].GetType() == attributeType || objArray[i].GetType().IsSubclassOf(attributeType))
				{
					return true;
				}
			}
			return false;
		}

		public static bool Contains(this LayerMask mask, int layer)
		{
			return mask == (mask | (1 << layer));
		}

		private static Assembly[] GetLoadedAssemblies()
		{
#if NETFX_CORE
			var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
			
			List<Assembly> loadedAssemblies = new List<Assembly>();
			
			var folderFilesAsync = folder.GetFilesAsync();
			folderFilesAsync.AsTask().Wait();
			
			foreach (var file in folderFilesAsync.GetResults())
			{
				if (file.FileType == ".dll" || file.FileType == ".exe")
				{
					try
					{
						var filename = file.Name.Substring(0, file.Name.Length - file.FileType.Length);
						AssemblyName name = new AssemblyName { Name = filename };
						Assembly asm = Assembly.Load(name);
						loadedAssemblies.Add(asm);
					}
					catch (BadImageFormatException)
					{
						// Thrown reflecting on C++ executable files for which the C++ compiler stripped the relocation addresses (such as Unity dlls): http://msdn.microsoft.com/en-us/library/x4cw969y(v=vs.110).aspx
					}
				}
			}
			
			return loadedAssemblies.ToArray();
#else
			return AppDomain.CurrentDomain.GetAssemblies();
#endif
		}

		
	}
}