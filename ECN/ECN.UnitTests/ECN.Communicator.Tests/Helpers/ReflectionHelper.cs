using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Moq;

namespace ECN.Communicator.Tests.Helpers
{
    /// <summary>
    ///    Class to extend <see cref="System.Reflection"></see> 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ReflectionHelper
    {
        /// <summary>
        ///     Compare List Contents in same order
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static bool IsListContentMatched<T>(this List<T> expected, List<T> actual)
        {
            bool isMatched = true;
            for (int listIndex = 0; listIndex < expected.Count; listIndex++)
            {
                PropertyInfo[] expectedProperties = expected[listIndex].GetType().GetAllProperties();
                PropertyInfo[] actualProperties = actual[listIndex].GetType().GetAllProperties();

                for (int i = 0; i < expectedProperties.Length; i++)
                {
                    object expectedValue = expectedProperties[i].GetValue(expected[listIndex]) ?? string.Empty;
                    object actualValue = actualProperties[i].GetValue(actual[listIndex]) ?? string.Empty;
                    if (!expectedValue.Equals(actualValue))
                    {
                        Console.WriteLine(
                            $"Expected Property({expectedProperties[i].Name}) Value: {expectedValue} is not matched with actual value {actualValue} ");
                        isMatched = false;
                    }
                }
            }

            return isMatched;
        }

        /// <summary>
        ///     Compare object content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static bool IsContentMatched<T>(this T expected, T actual)
        {
            bool isMatched = true;
            PropertyInfo[] expectedProperties = expected.GetType().GetAllProperties();
            PropertyInfo[] actualProperties = actual.GetType().GetAllProperties();

            for (int i = 0; i < expectedProperties.Length; i++)
            {
                object expectedValue = expectedProperties[i].GetValue(expected) ?? string.Empty;
                object actualValue = actualProperties[i].GetValue(actual) ?? string.Empty;
                if (!expectedValue.Equals(actualValue))
                {
                    Console.WriteLine(
                        $"Expected Property({expectedProperties[i].Name}) Value: {expectedValue} is not matched with actual value {actualValue} ");
                    isMatched = false;
                }
            }

            return isMatched;
        }

        /// <summary>
        ///     Get All Methods in Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MethodInfo[] GetAllMethods(this Type type)
        {
            MethodInfo[] result = type.GetMethods(BindingFlags());

            Type parentType = type.BaseType;
            if (parentType != null)
            {
                MethodInfo[] parentClassMethods = parentType.GetAllMethods();
                result = result.Concat(parentClassMethods).ToArray();
            }

            return result;
        }

        /// <summary>
        ///     Call Protected or Private Class methods
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <param name="parametersValues"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static object CallMethod(this Type type, string methodName, object[] parametersValues,
            object instance = null)
        {
            MethodInfo methodInfo = type.GetAllMethods().FirstOrDefault(x => x.Name == methodName);
            if (methodInfo != null)
            {
                if (methodInfo.ReturnType != typeof(void))
                {
                    return methodInfo.Invoke(methodInfo.IsStatic ? null : instance, parametersValues);
                }

                methodInfo.Invoke(methodInfo.IsStatic ? null : instance, parametersValues);
            }

            return null;
        }

		public static object CallOverloadedMethod(this Type type, string methodName, object[] parametersValues, int paramCount,
			object instance = null)
		{
			MethodInfo methodInfo = type.GetAllMethods().FirstOrDefault(x => x.Name == methodName && x.GetParameters().Count() == paramCount);
			if (methodInfo != null)
			{
				if (methodInfo.ReturnType != typeof(void))
				{
					return methodInfo.Invoke(methodInfo.IsStatic ? null : instance, parametersValues);
				}

				methodInfo.Invoke(methodInfo.IsStatic ? null : instance, parametersValues);
			}

			return null;
		}

		/// <summary>
		///     Get all properties for specific type
		/// </summary>
		/// <param name="type">Class type</param>
		/// <returns>Properties</returns>
		public static PropertyInfo[] GetAllProperties(this Type type)
        {
            return type.GetAllProperties(BindingFlags());
        }

        public static PropertyInfo[] GetAllProperties(this Type type, BindingFlags bindingFlags)
        {
            var propertyInfos = type.GetProperties(bindingFlags);

            var parentType = type.BaseType;
            if (parentType != null)
            {
                var parentPropertyInfo = GetAllProperties(parentType, bindingFlags);
                propertyInfos = propertyInfos.Concat(parentPropertyInfo).ToArray();
            }

            return propertyInfos.OrderBy(x => x.Name).ToArray();
        }

        /// <summary>
        ///     Set Property Value
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="instance">Generic Type Instance</param>
        /// <param name="propertyName">Public Property Name</param>
        /// <param name="value">Value To Set</param>
        public static void SetProperty<T>(this T instance, string propertyName, object value)
        {
            instance.GetType().GetProperty(propertyName)?.SetValue(instance, value);
        }

        public static void SetField<T>(this T instance, string fieldName, object value)
        {
            instance.GetType().GetField(fieldName, BindingFlags())?.SetValue(instance, value);
        }

        public static object GetField<T>(this T instance, string fieldName)
        {
            return instance.GetType().GetField(fieldName, BindingFlags())?.GetValue(instance);
        }

        /// <summary>
        ///     Set Property Value
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="instance">Generic Type Instance</param>
        /// <param name="propertyName">Public Property Name</param>
        /// <param name="index">List Index</param>
        public static void SetProperty<T>(this T instance, string propertyName, int index)
        {
            List<PropertyInfo> allProperties = instance.GetType().GetAllProperties().ToList();
            PropertyInfo propertyInfo = allProperties.FirstOrDefault(x => x.Name == propertyName);
            propertyInfo?.SetValue(instance,
                propertyInfo.PropertyType.CreateInstance(index));
        }

        /// <summary>
        ///     Get Property Value
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="instance">Generic Type Instance</param>
        /// <param name="propertyName">Public Property Name</param>
        public static object GetPropertyValue<T>(this T instance, string propertyName)
        {
            return instance.GetType().GetProperty(propertyName)?.GetValue(instance);
        }

        /// <summary>
        ///     Generate value for primitive types
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="type">Type</param>
        /// <param name="index">Field or Property index</param>
        /// <returns>Generated value</returns>
        public static dynamic GetValue(this Type type, string parameterName, int index = 0)
        {
            if (type == null)
            {
                return null;
            }

            type = type.FullName != null && type.FullName.Contains("System.Boolean&") ? typeof(bool) : type;
            type = type.FullName != null && type.FullName.Contains("System.String&") ? typeof(string) : type;

            parameterName = parameterName.ToLower();

            if (type == typeof(sbyte) || type == typeof(sbyte?))
            {
                return sbyte.Parse((1 + index).ToString());
            }
            if (type == typeof(short) || type == typeof(short?))
            {
                return short.Parse((2 + index).ToString());
            }
            if (type == typeof(ushort) || type == typeof(ushort?))
            {
                return ushort.Parse((3 + index).ToString());
            }
            if (type == typeof(int) || type == typeof(int?))
            {
                return int.Parse((4 + index).ToString());
            }
            if (type == typeof(uint) || type == typeof(uint?))
            {
                return uint.Parse((5 + index).ToString());
            }
            if (type == typeof(long) || type == typeof(long?))
            {
                return long.Parse((6 + index).ToString());
            }
            if (type == typeof(ulong) || type == typeof(ulong?))
            {
                return ulong.Parse((7 + index).ToString());
            }
            if (type == typeof(double) || type == typeof(double?))
            {
                return double.Parse((8.0 + index).ToString(CultureInfo.InvariantCulture));
            }
            if (type == typeof(float) || type == typeof(float?))
            {
                return float.Parse((9.0 + index).ToString(CultureInfo.InvariantCulture));
            }
            if (type == typeof(decimal) || type == typeof(decimal?))
            {
                return decimal.Parse((10.0 + index).ToString(CultureInfo.InvariantCulture));
            }

            if (type == typeof(char) || type == typeof(char?))
            {
                return (char)index;
            }

            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return GetDateTime(index);
            }

            if (parameterName.Contains("xml") && type == typeof(string))
            {
                return "<UnitTest>" + index + "</UnitTest>";
            }

            if (parameterName.EndsWith("culture") && type == typeof(string))
            {
                return "en-US";
            }

            if (type == typeof(string))
            {
                return "UnitTest_Index = " + index;
            }

            if (type == typeof(bool) || type == typeof(bool?))
            {
                return bool.Parse((index % 2 == 0).ToString());
            }

            if (type == typeof(Uri))
            {
                return new Uri("https://www.google.com.pk/FieldIndex=" + index);
            }

            if (type.IsAssignableFrom(typeof(Exception)))
            {
                return new SystemException($"{parameterName}_{index}");
            }

            if (IsACollection(type) &&
                (type.GetElementType() != typeof(Type) || type.GetGenericArguments()[0] != typeof(Type)))
            {
                return type.GetElementType()?.GetArrayObjects(index) ??
                       type.GetGenericArguments()[0].GetListObjects(index);
            }

            return null;
        }

        /// <summary>
        ///     Create Instance of Specific Type
        /// </summary>
        /// <param name="type">Class Type</param>
        /// <param name="listIndex"></param>
        /// <param name="levelInitializeUninitializedMembers">Level to initialize nested properties and fields</param>
        /// <returns>Instance object</returns>
        public static dynamic CreateInstance(this Type type, int listIndex = 0,
            int levelInitializeUninitializedMembers = 1)
        {
            dynamic instance = null;
            if (type.IsAbstract || type.IsInterface)
            {
                instance = GetMock(type);
            }

            if (instance == null)
            {
                instance = GetValue(type, type.Name, listIndex);
                levelInitializeUninitializedMembers = instance != null ? 0 : levelInitializeUninitializedMembers;
            }

            if (instance == null)
            {
                instance = CreateInstanceOfTypeHavingDefaultConstructor(type);
            }

            if (instance == null)
            {
                instance = CreateUninitializedObject(type);
            }

            if (instance != null && levelInitializeUninitializedMembers > 0)
            {
                try
                {
                    levelInitializeUninitializedMembers = levelInitializeUninitializedMembers - 1;
                    InitializeUninitializedProperties(type, instance, listIndex, levelInitializeUninitializedMembers);
                }
                catch (Exception exp)
                {
                    Debug.WriteLine(exp.ToString());
                }
            }

            return instance;
        }

        /// <summary>
        ///     Create Uninitialized Object
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static dynamic CreateUninitializedObject(this Type type)
        {
            dynamic instance = null;

            try
            {
                instance = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
            }

            return instance;
        }

        /// <summary>
        ///  Get Mock Of Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static dynamic GetMock(this Type type)
        {
            dynamic mock = null;
            try
            {
                mock = Activator.CreateInstance(typeof(Mock<>).MakeGenericType(type));
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
            }

            return mock;
        }

        private static dynamic CreateInstanceOfTypeHavingDefaultConstructor(this Type type)
        {
            return CreateInstanceOfTypeHavingPublicDefaultConstructor(type) ??
                   CreateInstanceOfTypeHavingPrivateDefaultConstructor(type);
        }

        private static dynamic CreateInstanceOfTypeHavingPublicDefaultConstructor(this Type type)
        {
            dynamic instance = null;
            if (IsPublicDefaultConstructorExist(type))
            {
                try
                {
                    instance = Activator.CreateInstance(type);
                }
                catch (Exception exp)
                {
                    Debug.WriteLine(exp.ToString());
                }
            }

            return instance;
        }

        private static bool IsPublicDefaultConstructorExist(this Type classType)
        {
            ConstructorInfo constructorInfo = classType.GetConstructor(Type.EmptyTypes);

            return constructorInfo != null && constructorInfo.IsPublic;
        }

        private static dynamic CreateInstanceOfTypeHavingPrivateDefaultConstructor(this Type type)
        {
            dynamic instance = null;
            try
            {
                instance = Activator.CreateInstance(type, BindingFlags(), null, new object[] { }, null);
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.ToString());
            }

            return instance;
        }

        private static void InitializeUninitializedProperties(this Type type, dynamic instance, int listIndex,
            int levelInitializeUninitializedMembers)
        {
            PropertyInfo[] propertyInfos = type.GetAllProperties();
            for (int index = 0; index < propertyInfos.Length; index++)
            {
                PropertyInfo propertyInfo = propertyInfos[index];
                try
                {
                    Type typeInfo = propertyInfo.PropertyType;
                    if (typeInfo.FullName != null &&
                        !typeInfo.FullName.Contains("System.Threading"))
                    {
                        if (typeInfo == typeof(Type))
                        {
                            propertyInfo.SetValue(instance, instance.GetType());
                            continue;
                        }

                        dynamic value = GetValue(typeInfo, propertyInfo.Name, index + listIndex);
                        propertyInfo.SetValue(instance, value ?? type.CreateInstance(listIndex, levelInitializeUninitializedMembers));
                    }
                }
                catch (Exception exp)
                {
                    Debug.WriteLine($"Could not Set Property {propertyInfo.Name} because of {exp}");
                }
            }
        }

        private static bool IsACollection(this Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        private static dynamic GetArrayObjects(this Type type, int fieldIndex)
        {
            dynamic objects = Array.CreateInstance(type, 25);

            for (int x = 0; x < objects.Length; x++)
            {
                string value = (x + 1 + fieldIndex).ToString();
                objects[x] = ParseValue(type, fieldIndex, value);
            }

            return objects;
        }

        private static dynamic GetListObjects(this Type type, int fieldIndex)
        {
            dynamic list = Activator.CreateInstance(typeof(List<>).MakeGenericType(type));

            for (int x = 0; x < 25; x++)
            {
                string value = (x + 1 + fieldIndex).ToString();
                list.Add(ParseValue(type, fieldIndex, value));
            }

            return list;
        }

        private static dynamic ParseValue(Type type, int fieldIndex, string value)
        {
            if (type == typeof(int))
            {
                return int.Parse(value);
            }

            if (type == typeof(uint))
            {
                return uint.Parse(value);
            }

            if (type == typeof(double))
            {
                return double.Parse(value);
            }

            if (type == typeof(short))
            {
                return short.Parse(value);
            }

            if (type == typeof(ushort))
            {
                return ushort.Parse(value);
            }

            if (type == typeof(decimal))
            {
                return float.Parse(value);
            }

            if (type == typeof(float))
            {
                return float.Parse(value);
            }

            if (type == typeof(byte))
            {
                return byte.Parse(value);
            }

            if (type == typeof(sbyte))
            {
                return sbyte.Parse(value);
            }

            if (type == typeof(long))
            {
                return long.Parse(value);
            }

            if (type == typeof(ulong))
            {
                return ulong.Parse(value);
            }

            if (type == typeof(bool))
            {
                return int.Parse(value) % 2 == 0;
            }

            if (type == typeof(DateTime))
            {
                return GetDateTime(fieldIndex);
            }

            if (type == typeof(char))
            {
                return (char)int.Parse(value);
            }

            return type == typeof(string) ? $"Test Output-{value}" : null;
        }

        private static DateTime GetDateTime(int fieldIndex)
        {
            DateTime max = DateTime.MaxValue;
            int index = 1 + fieldIndex;
            return new DateTime(Math.Min(max.Year, 2000 + fieldIndex), Math.Min(12, index), Math.Min(27, index),
                Math.Min(24, index), Math.Min(60, index), Math.Min(60, index));
        }

        private static BindingFlags BindingFlags()
        {
            return System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic |
                   System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static;
        }

		public static dynamic CreateInstanceWithValues(this Type type,dynamic FieldsAndValues, int listIndex = 0,
			int levelInitializeUninitializedMembers = 1)
		{
			dynamic instance = null;
			if (type.IsAbstract || type.IsInterface)
			{
				instance = GetMock(type);
			}

			if (instance == null)
			{
				instance = GetValue(type, type.Name, listIndex);
				levelInitializeUninitializedMembers = instance != null ? 0 : levelInitializeUninitializedMembers;
			}

			if (instance == null)
			{
				instance = CreateInstanceOfTypeHavingDefaultConstructor(type);
			}

			if (instance == null)
			{
				instance = CreateUninitializedObject(type);
			}

			if (instance != null && levelInitializeUninitializedMembers > 0)
			{
				try
				{
					levelInitializeUninitializedMembers = levelInitializeUninitializedMembers - 1;
					InitializeUninitializedPropertiesWithValues(type, FieldsAndValues, instance, listIndex, levelInitializeUninitializedMembers);
				}
				catch (Exception exp)
				{
					Debug.WriteLine(exp.ToString());
				}
			}

			return instance;
		}
		private static void InitializeUninitializedPropertiesWithValues(this Type type, dynamic fieldsAndValues, dynamic instance, int listIndex,
			int levelInitializeUninitializedMembers)
		{
			PropertyInfo[] propertyInfos = type.GetAllProperties();
			for (int index = 0; index < propertyInfos.Length; index++)
			{
				PropertyInfo propertyInfo = propertyInfos[index];
				try
				{
					Type typeInfo = propertyInfo.PropertyType;
					if (typeInfo.FullName != null &&
						!typeInfo.FullName.Contains("System.Threading"))
					{
						if (typeInfo == typeof(Type))
						{
							propertyInfo.SetValue(instance, instance.GetType());
							continue;
						}

						if (fieldsAndValues != null && IsPropertyExist(fieldsAndValues, propertyInfo.Name))
						{
							dynamic providedName = propertyInfo.Name;
							dynamic providedValue = fieldsAndValues.GetType().GetProperty(propertyInfo.Name).GetValue(fieldsAndValues, null);
							propertyInfo.SetValue(instance, providedValue ?? type.CreateInstance(listIndex, levelInitializeUninitializedMembers));
						}
						else if (!IsPropertyExist(fieldsAndValues, propertyInfo.Name))
						{
							dynamic value = GetValue(typeInfo, propertyInfo.Name, index + listIndex);
							propertyInfo.SetValue(instance, value ?? type.CreateInstance(listIndex, levelInitializeUninitializedMembers));
						}
					}
				}
				catch (Exception exp)
				{
					Debug.WriteLine($"Could not Set Property {propertyInfo.Name} because of {exp}");
				}
			}
		}

		public static bool IsPropertyExist(dynamic obj, string name)
		{
			if (obj is ExpandoObject)
			{
				return ((IDictionary<string, object>)obj).ContainsKey(name);
			}
			return obj.GetType().GetProperty(name) != null;
		}
	}
}