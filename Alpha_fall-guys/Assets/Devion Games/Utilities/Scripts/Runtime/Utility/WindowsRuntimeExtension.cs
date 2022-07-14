using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using System.Linq;
using System.Collections.Generic;

public static class WindowsRuntimeExtension {
	#if NETFX_CORE
	public static FieldInfo GetField(this Type type,string name){
		return type.GetRuntimeField (name);
	}

	public static FieldInfo[] GetFields(this Type type){
		return type.GetRuntimeFields ().ToArray();
	}

	public static FieldInfo[] GetFields(this Type type, BindingFlags bindingFlags)
	{
		Type baseType;
		List<FieldInfo> fieldInfos = new List<FieldInfo>();
		Boolean flag = false;
		while (type != null)
		{
			foreach (FieldInfo runtimeField in type.GetRuntimeFields())
			{
				if (!WindowsRuntimeExtension.MatchBindingFlags(runtimeField.IsPublic, runtimeField.IsPrivate, runtimeField.IsStatic, bindingFlags, flag))
				{
					continue;
				}
				fieldInfos.Add(runtimeField);
			}
			if ((bindingFlags & BindingFlags.DeclaredOnly) == BindingFlags.Default)
			{
				baseType = type.GetTypeInfo().BaseType;
			}
			else
			{
				baseType = null;
			}
			type = baseType;
			flag = true;
		}
		return fieldInfos.ToArray();
	}

	public static PropertyInfo GetProperty(this Type type,string name){
		return type.GetRuntimeProperty (name);
	}

	public static PropertyInfo[] GetProperties(this Type type){
		return type.GetRuntimeProperties().ToArray();
	}

	public static PropertyInfo[] GetProperties(this Type type, BindingFlags bindingFlags)
	{
		Type baseType;
		List<PropertyInfo> propertyInfos = new List<PropertyInfo>();
		bool flag = false;
		while (type != null)
		{
			foreach (PropertyInfo runtimeProperty in type.GetRuntimeProperties())
			{
				MethodInfo getMethod = runtimeProperty.GetMethod ?? runtimeProperty.SetMethod;
				if (!WindowsRuntimeExtension.MatchBindingFlags(getMethod.IsPublic, getMethod.IsPrivate, getMethod.IsStatic, bindingFlags, flag))
				{
					continue;
				}
				propertyInfos.Add(runtimeProperty);
			}
			if ((bindingFlags & BindingFlags.DeclaredOnly) == BindingFlags.Default)
			{
				baseType = type.GetTypeInfo().BaseType;
			}
			else
			{
				baseType = null;
			}
			type = baseType;
			flag = true;
		}
		return propertyInfos.ToArray();
	}

	public static MethodInfo GetMethod(this Type type,string methodName){
		return type.GetTypeInfo().GetDeclaredMethod(methodName);
	}

	public static MethodInfo[] GetMethods(this Type type,BindingFlags flags){
		Type baseType;
		List<MethodInfo> infos = new List<MethodInfo>();
		bool flag = false;
		while (type != null)
		{
			foreach (MethodInfo runtimeMethod in type.GetRuntimeMethods())
			{
				if (!WindowsRuntimeExtension.MatchBindingFlags(runtimeMethod.IsPublic, runtimeMethod.IsPrivate, runtimeMethod.IsStatic, flags, flag))
				{
					continue;
				}
				infos.Add(runtimeMethod);
			}
			if ((flags & BindingFlags.DeclaredOnly) == BindingFlags.Default)
			{
				baseType = type.GetTypeInfo().BaseType;
			}
			else
			{
				baseType = null;
			}
			type = baseType;
			flag = true;
		}
		return infos.ToArray();
	}

	public static bool IsAssignableFrom(this Type type,Type c){
		return type.GetTypeInfo().IsAssignableFrom(c.GetTypeInfo());
	}

	public static bool IsSubclassOf(this Type type,Type c){
		return type.GetTypeInfo().IsSubclassOf(c);
	}


	public static Type[] GetGenericArguments(this Type type){
		return type.GetTypeInfo().GenericTypeArguments;
	}

	public static object[] GetCustomAttributes(this Type type,bool inherit){
		return type.GetTypeInfo().GetCustomAttributes(inherit).Cast<object>().ToArray();
	}

	public static Type[] GetTypes(this Assembly asm){
		return asm.DefinedTypes.Cast<Type>().ToArray();
	}

	public static ConstructorInfo GetConstructor(this Type type, Type[] args)
	{
		ConstructorInfo constructorInfo;
		using (IEnumerator<ConstructorInfo> enumerator = type.GetTypeInfo().DeclaredConstructors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ConstructorInfo current = enumerator.Current;
				if (!WindowsRuntimeExtension.ParametersMatch(current.GetParameters(), args))
				{
					continue;
				}
				constructorInfo = current;
				return constructorInfo;
			}
			return null;
		}
	}
	#endif

	public static Type GetBaseType(this Type type)
	{
		#if NETFX_CORE
		return type.GetTypeInfo().BaseType;
		#else
		return type.BaseType;
		#endif
	}
		

	public static Assembly GetAssembly(this Type type)
	{
		#if NETFX_CORE
		return type.GetTypeInfo().Assembly;
		#else
		return type.Assembly;
		#endif
	}

	public static bool IsValueType(this Type type)
	{
		#if NETFX_CORE
		return type.GetTypeInfo().IsValueType;
		#else
		return type.IsValueType;
		#endif
	}

	public static bool IsGenericType(this Type type)
	{
		#if NETFX_CORE
		return type.GetTypeInfo().IsGenericType;
		#else
		return type.IsGenericType;
		#endif
	}

	private static bool ParametersMatch(ParameterInfo[] parameters, Type[] args)
	{
		if ((Int32)parameters.Length != (Int32)args.Length)
		{
			return false;
		}
		bool flag = true;
		Int32 num = 0;
		while (num < (Int32)parameters.Length)
		{
			Type parameterType = parameters[num].ParameterType;
			Type type = args[num];
			if ((System.Object)parameterType == (System.Object)type)
			{
				num++;
			}
			else
			{
				flag = false;
				break;
			}
		}
		return flag;
	}

	public static bool MatchBindingFlags(bool isPublic, bool isPrivate, bool isStatic, BindingFlags bindingAttr, bool baseClass)
	{
		if (isPublic)
		{
			if ((bindingAttr & BindingFlags.Public) == BindingFlags.Default)
			{
				return false;
			}
		}
		else if ((bindingAttr & BindingFlags.NonPublic) == BindingFlags.Default)
		{
			return false;
		}
		if (isStatic)
		{
			if ((bindingAttr & BindingFlags.Static) == BindingFlags.Default)
			{
				return false;
			}
			if (baseClass)
			{
				if ((bindingAttr & BindingFlags.FlattenHierarchy) == BindingFlags.Default)
				{
					return false;
				}
				if (isPrivate)
				{
					return false;
				}
			}
		}
		else if ((bindingAttr & BindingFlags.Instance) == BindingFlags.Default)
		{
			return false;
		}
		return true;
	}

}
