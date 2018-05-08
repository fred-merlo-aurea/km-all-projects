﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Moq;
using NUnit.Framework;
using ReflectionFlags = System.Reflection.BindingFlags;

namespace ECN.TestHelpers
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
        /// <param name="ignorePropertiesNames">Ignore Properties Names</param>
        /// <returns></returns>
        public static bool IsListContentMatched<T>(this List<T> expected, List<T> actual,
            params string[] ignorePropertiesNames)
        {
            var isMatched = true;
            for (int listIndex = 0; listIndex < expected.Count; listIndex++)
            {
                isMatched = expected[listIndex].IsContentMatched(actual[listIndex], ignorePropertiesNames);
            }

            return isMatched;
        }

        /// <summary>
        ///     Compare object content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="ignorePropertiesNames">Properties to be ignored in comparing</param>
        /// <returns></returns>
        public static bool IsContentMatched<T>(this T expected, T actual, params string[] ignorePropertiesNames)
        {
            var isMatched = expected.Equals(actual) &&
                            expected.GetHashCode().Equals(actual.GetHashCode());

            var allPropertiesMatched = true;

            PropertyInfo[] expectedProperties = expected.GetType().GetAllProperties();
            PropertyInfo[] actualProperties = actual.GetType().GetAllProperties();

            for (int i = 0; i < expectedProperties.Length; i++)
            {
                object expectedValue = expectedProperties[i].GetValue(expected) ?? string.Empty;
                object actualValue = actualProperties[i].GetValue(actual) ?? string.Empty;
                if (!expectedValue.Equals(actualValue))
                {
                    if (ReferenceEquals(expectedValue, actualValue) ||
                        ignorePropertiesNames.Any(x => x == expectedProperties[i].Name))
                    {
                        continue;
                    }

                    if (expectedValue.GetType().IsGenericType &&
                        expectedValue.GetType().GetGenericTypeDefinition() == typeof(Dictionary<,>))
                    {
                        var expectedDictionary = (IDictionary)expectedValue;
                        var actualDictionary = (IDictionary)actualValue;

                        CollectionAssert.AreEquivalent(expectedDictionary, actualDictionary);
                        continue;
                    }

                    if (expectedValue.GetType().IsGenericType &&
                        expectedValue.GetType().GetGenericTypeDefinition() == typeof(List<>))
                    {
                        var expectedList = (IList)expectedValue;
                        var actualList = (IList)actualValue;

                        for (var index = 0; index < expectedList.Count; index++)
                        {
                            if (!expectedList[index].IsContentMatched(actualList[index]))
                            {
                                return false;
                            }
                        }
                        continue;
                    }

                    Console.WriteLine(
                        $"Class {expected.GetType().FullName} Expected Property({expectedProperties[i].Name}) Value: {expectedValue} is not matched with actual value {actualValue} ");
                    allPropertiesMatched = false;
                }
            }

            if (isMatched && !allPropertiesMatched)
            {
                Console.WriteLine("Equal Method is not checking all Properties - Please review Equal method code");
            }

            return isMatched || allPropertiesMatched;
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
        public static object CallMethod(this Type type, string methodName, object[] parametersValues = null,
            object instance = null)
        {
            parametersValues = parametersValues ?? new object[0];
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

        /// <summary>
        ///     Call Protected or Private Class Overloaded methods
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="parametersValues"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static object CallOverloadedMethod(this Type type, Type[] parameterTypes, string methodName, object[] parametersValues = null,
            object instance = null)
        {
            parametersValues = parametersValues ?? new object[0];
            parameterTypes = parameterTypes ?? Type.EmptyTypes;

            var methodInfo = type.GetMethod(methodName, BindingFlags(), Type.DefaultBinder, parameterTypes, null);
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
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags());
            propertyInfos = propertyInfos
                            .Where(x => x.GetMethod != null && x.SetMethod != null)
                            .ToArray();

            Type parentType = type.BaseType;
            if (parentType != null)
            {
                PropertyInfo[] parentPropertyInfo = parentType.GetAllProperties();
                propertyInfos = propertyInfos.Concat(parentPropertyInfo).ToArray();
            }

            return propertyInfos.OrderBy(x => x.Name).ToArray();
        }

        /// <summary>
        ///     Get all fields for specific type
        /// </summary>
        /// <param name="type">Class type</param>
        /// <returns>Properties</returns>
        public static FieldInfo[] GetAllFields(this Type type)
        {
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags());

            var parentType = type.BaseType;
            if (parentType != null)
            {
                FieldInfo[] parentFieldInfos = parentType.GetFields();
                fieldInfos = fieldInfos.Concat(parentFieldInfos).ToArray();
            }

            return fieldInfos.OrderBy(x => x.Name).ToArray();
        }

        /// <summary>
        ///     Set Property Value
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="instance">Generic Type Instance</param>
        /// <param name="propertyName">Property Name</param>
        /// <param name="value">Value To Set</param>
        public static void SetProperty<T>(this T instance, string propertyName, object value)
        {
            List<PropertyInfo> allProperties = instance.GetType().GetAllProperties().ToList();
            PropertyInfo propertyInfo = allProperties.FirstOrDefault(x => x.Name == propertyName);
            propertyInfo?.SetValue(instance, value);
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
                propertyInfo.PropertyType.CreateInstance(false, index));
        }

        /// <summary>
        ///     Get Property Value
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="instance">Generic Type Instance</param>
        /// <param name="propertyName">Property Name</param>
        public static object GetPropertyValue<T>(this T instance, string propertyName)
        {
            return instance.GetType().GetAllProperties().FirstOrDefault(x => x.Name == propertyName)
                ?.GetValue(instance);
        }

        /// <summary>
        ///     Get Field Index
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="propertyName">Field Name</param>
        public static int GetPropertyIndex(this Type type, string propertyName)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var infos = type.GetAllProperties();
            for (var index = 0; index < infos.Length; index++)
            {
                var property = infos[index];

                if (property.Name == propertyName)
                {
                    return index;
                }
            }

            return -1;
        }

        /// <summary>
        ///     Get Property Value
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="instance">Generic Type Instance</param>
        /// <param name="propertyIndex">Field Index</param>
        public static object GetPropertyValue<T>(this T instance, int propertyIndex)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var properties = instance.GetType().GetAllProperties();
            if (propertyIndex >= 0 && propertyIndex < properties.Length)
            {
                return properties[propertyIndex].GetValue(instance);
            }

            throw new ArgumentOutOfRangeException(nameof(propertyIndex));
        }

        /// <summary>
        ///     Set Field Value
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="instance">Generic Type Instance</param>
        /// <param name="fieldname">Field Name</param>
        /// <param name="value">value</param>
        public static void SetField<T>(this T instance, string fieldname, object value)
        {
            List<FieldInfo> allFields = instance.GetType().GetAllFields().ToList();
            FieldInfo fieldInfo = allFields.FirstOrDefault(x => x.Name == fieldname);
            fieldInfo?.SetValue(instance, value);
        }

        /// <summary>
        ///     Set Field Value
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="instance">Generic Type Instance</param>
        /// <param name="fieldname">Field Name</param>
        /// <param name="index">List Index</param>
        public static void SetField<T>(this T instance, string fieldname, int index)
        {
            List<FieldInfo> allFields = instance.GetType().GetAllFields().ToList();
            FieldInfo fieldInfo = allFields.FirstOrDefault(x => x.Name == fieldname);
            fieldInfo?.SetValue(instance, fieldInfo.FieldType.CreateInstance(false, index));
        }

        /// <summary>
        ///     Get Field Value
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="instance">Generic Type Instance</param>
        /// <param name="fieldName">Field Name</param>
        public static object GetFieldValue<T>(this T instance, string fieldName)
        {
            return instance.GetType().GetAllFields().FirstOrDefault(x => x.Name == fieldName)?.GetValue(instance);
        }

        /// <summary>
        /// Get Default Value
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static dynamic GetDefaultValue(this Type type)
        {
            return type.GetValue(string.Empty, 0, true);
        }

        /// <summary>
        ///     Generate value for primitive types
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="type">Type</param>
        /// <param name="index">Field or Property index</param>
        /// <param name="setDefaultValues">Set Default Values If true</param>
        /// <returns>Generated value</returns>
        public static dynamic GetValue(this Type type, string parameterName, int index = 0,
            bool setDefaultValues = false)
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
                return setDefaultValues ? 0 : sbyte.Parse((1 + index).ToString());
            }
            if (type == typeof(short) || type == typeof(short?))
            {
                return setDefaultValues ? 0 : short.Parse((2 + index).ToString());
            }
            if (type == typeof(ushort) || type == typeof(ushort?))
            {
                return setDefaultValues ? 0 : ushort.Parse((3 + index).ToString());
            }
            if (type == typeof(int) || type == typeof(int?))
            {
                return setDefaultValues ? 0 : int.Parse((4 + index).ToString());
            }
            if (type == typeof(uint) || type == typeof(uint?))
            {
                return setDefaultValues ? 0 : uint.Parse((5 + index).ToString());
            }
            if (type == typeof(long) || type == typeof(long?))
            {
                return setDefaultValues ? 0L : long.Parse((6 + index).ToString());
            }
            if (type == typeof(ulong) || type == typeof(ulong?))
            {
                return setDefaultValues ? 0 : ulong.Parse((7 + index).ToString());
            }
            if (type == typeof(double) || type == typeof(double?))
            {
                return setDefaultValues ? 0.0D : double.Parse((8.0 + index).ToString(CultureInfo.InvariantCulture));
            }
            if (type == typeof(float) || type == typeof(float?))
            {
                return setDefaultValues ? 0.0F : float.Parse((9.0 + index).ToString(CultureInfo.InvariantCulture));
            }
            if (type == typeof(decimal) || type == typeof(decimal?))
            {
                return setDefaultValues ? 0M : decimal.Parse((10.0 + index).ToString(CultureInfo.InvariantCulture));
            }

            if (type == typeof(char) || type == typeof(char?))
            {
                return setDefaultValues ? '\0' : (char)index;
            }

            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return setDefaultValues
                    ? type == typeof(DateTime?)
                        ? (DateTime?)null
                        : DateTime.MinValue
                    : GetDateTime(index);
            }

            if (parameterName.Contains("xml") && type == typeof(string))
            {
                return setDefaultValues ? null : "<UnitTest>" + index + "</UnitTest>";
            }

            if (parameterName.EndsWith("culture") && type == typeof(string))
            {
                return setDefaultValues ? null : "en-US";
            }

            if (type == typeof(string))
            {
                return setDefaultValues ? null : "UnitTest_Index = " + index;
            }

            if (type == typeof(bool) || type == typeof(bool?))
            {
                return !setDefaultValues && bool.Parse((index % 2 == 0).ToString());
            }

            if (type == typeof(Uri))
            {
                return setDefaultValues ? null : new Uri("https://www.google.com.pk/FieldIndex=" + index);
            }

            if (type.IsAssignableFrom(typeof(Exception)))
            {
                return setDefaultValues ? null : new SystemException($"{parameterName}_{index}");
            }

            if (type == typeof(Guid) || type == typeof(Guid?))
            {
                return setDefaultValues ? type == typeof(Guid?) ? (Guid?)null : Guid.Empty : GetGuid(index);
            }

            if (IsACollection(type) &&
                (type.GetElementType() != typeof(Type) || type.GetGenericArguments()[0] != typeof(Type)))
            {
                return setDefaultValues
                    ? null
                    : type.GetElementType()?.GetArrayObjects(index) ??
                      type.GetGenericArguments()[0].GetListObjects(index);
            }

            return null;
        }

        /// <summary>
        ///     Create Instance of Specific Type
        /// </summary>
        /// <param name="type">Class Type</param>
        /// <param name="setDefaultValues"></param>
        /// <param name="listIndex"></param>
        /// <param name="levelInitializeUninitializedMembers">Level to initialize nested properties and fields</param>
        /// <returns>Instance object</returns>
        public static dynamic CreateInstance(this Type type, bool setDefaultValues = false, int listIndex = 0,
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
                    InitializeUninitializedProperties(type, instance, listIndex, levelInitializeUninitializedMembers,
                        setDefaultValues);
                }
                catch (Exception exp)
                {
                    Debug.WriteLine(exp.ToString());
                }
            }

            return instance;
        }

        public static TType CreateInstance<TType>(params object[] parameters)
        {
            return (TType)typeof(TType).InvokeMember(
                string.Empty,
                ReflectionFlags.NonPublic | ReflectionFlags.CreateInstance | ReflectionFlags.Instance,
                null,
                null,
                parameters);
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

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        /// <summary>
        ///     Convert DataTable to List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList<T> ConvertDataTable<T>(this DataTable dt)
        {
            return (from DataRow row in dt.Rows select row.GetItem<T>()).ToList();
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
            int levelInitializeUninitializedMembers, bool setDefaultValues)
        {
            PropertyInfo[] propertyInfos = type.GetAllProperties();
            for (int index = 0; index < propertyInfos.Length; index++)
            {
                PropertyInfo propertyInfo = propertyInfos[index];

                try
                {
                    Type typeInfo = propertyInfo.PropertyType;
                    dynamic defaultValue = propertyInfo.GetValue(instance);

                    if (typeInfo.FullName != null &&
                        !typeInfo.FullName.Contains("System.Threading"))
                    {
                        if (typeInfo == typeof(Type))
                        {
                            propertyInfo.SetValue(instance, instance.GetType());
                            continue;
                        }

                        dynamic value = GetValue(typeInfo, propertyInfo.Name, index + listIndex, setDefaultValues);
                        propertyInfo.SetValue(instance,
                            value != null || setDefaultValues
                                ? value
                                : (defaultValue ?? type.CreateInstance(false, listIndex,
                                       levelInitializeUninitializedMembers)));
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

            if (type == typeof(Guid))
            {
                return GetGuid(fieldIndex);
            }

            return type == typeof(string) ? $"Test Output-{value}" : CreateInstanceOfTypeHavingDefaultConstructor(type);
        }

        private static dynamic GetGuid(int fieldIndex)
        {
            string guid = "00000000-0000-0000-0000-000000000000";
            return Guid.Parse(fieldIndex + guid.Substring(fieldIndex.ToString().Length, guid.Length - 1));
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
            return ReflectionFlags.Instance | ReflectionFlags.NonPublic |
                   ReflectionFlags.Public | ReflectionFlags.Static;
        }

        private static T GetItem<T>(this DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName && pro.CanWrite)
                    {
                        pro.SetValue(obj, dr[column.ColumnName] == DBNull.Value ? null : dr[column.ColumnName], null);
                    }
                }
            }
            return obj;
        }
    }
}