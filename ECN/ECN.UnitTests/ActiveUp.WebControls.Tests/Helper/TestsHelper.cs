#if !FX1_1
#define NOT_FX1_1
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Shouldly;
using ReflectionFlags = System.Reflection.BindingFlags;

namespace ActiveUp.WebControls.Tests.Helper
{
    public static class TestsHelper
    {
        [Conditional("NOT_FX1_1")]
        public static void AssertNotFX1(string expectedValue, string actualValue)
        {
            expectedValue.ShouldBe(actualValue);
        }

        [Conditional("FX1_1")]
        public static void AssertFX1(string expectedValue, string actualValue)
        {
            expectedValue.ShouldBe(actualValue);
        }

        /// <summary>
        /// Call Protected or Private Class methods
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
        /// Get All Methods in Type
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
        /// Get Field Value
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="instance">Generic Type Instance</param>
        /// <param name="fieldName">Field Name</param>
        public static object GetFieldValue<T>(this T instance, string fieldName)
        {
            return instance.GetType().GetAllFields().FirstOrDefault(x => x.Name == fieldName)?.GetValue(instance);
        }

        private static BindingFlags BindingFlags()
        {
            return ReflectionFlags.Instance | ReflectionFlags.NonPublic |
                   ReflectionFlags.Public | ReflectionFlags.Static;
        }

        /// <summary>
        ///  Get all fields for specific type
        /// </summary>
        /// <param name="type">Class type</param>
        /// <returns>Properties</returns>
        public static FieldInfo[] GetAllFields(this Type type)
        {
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags());

            var parentType = type.BaseType;
            if (parentType != null)
            {
                FieldInfo[] parentFieldInfos = parentType.GetAllFields();
                fieldInfos = fieldInfos.Concat(parentFieldInfos).ToArray();
            }

            return fieldInfos.OrderBy(x => x.Name).ToArray();
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
        ///     Get all properties for specific type
        /// </summary>
        /// <param name="type">Class type</param>
        /// <returns>Properties</returns>
        public static PropertyInfo[] GetAllProperties(this Type type)
        {
            var propertyInfos = type.GetProperties(BindingFlags());
            propertyInfos = propertyInfos
                .Where(x => x.GetMethod != null && x.SetMethod != null)
                .ToArray();

            var parentType = type.BaseType;
            if (parentType != null)
            {
                var parentPropertyInfo = parentType.GetAllProperties();
                propertyInfos = propertyInfos.Concat(parentPropertyInfo).ToArray();
            }

            return propertyInfos.OrderBy(x => x.Name).ToArray();
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
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            List<FieldInfo> allFields = instance.GetType().GetAllFields().ToList();
            FieldInfo fieldInfo = allFields.FirstOrDefault(x => x.Name == fieldname);
            fieldInfo?.SetValue(instance, value);
        }
    }
}
