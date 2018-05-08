using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AUT.ConfigureTestProjects.StaticTypes;

namespace AUT.ConfigureTestProjects.Analyzer
{
    [ExcludeFromCodeCoverage]
    public static class ReflectionAnalysis
    {
        public static MethodInfo GetNonPublicInstanceMethodInfo<T>(T instance, string name)
        {
            return instance.GetType().GetMethod(name, ReflectionConstants.NonPublicInstanceFlag);
        }

        public static MethodInfo GetPublicMethodInfo<T>(T instance, string name)
        {
            return instance.GetType().GetMethod(name, ReflectionConstants.PublicFlag);
        }

        public static FieldInfo GetNonPublicInstanceFieldInfo<T>(T instance, string name)
        {
            return instance.GetType().GetField(name, ReflectionConstants.NonPublicInstanceFlag);
        }

        public static FieldInfo GetNonPublicStaticFieldInfo<T>(T instance, string name)
        {
            return instance.GetType().GetField(name, ReflectionConstants.NonPublicStaticFlag);
        }

        public static PropertyInfo GetNonPublicPropertyInfo<T>(T instance, string name)
        {
            return instance.GetType().GetProperty(name, ReflectionConstants.PublicFlag);
        }

        public static PropertyInfo GetPublicInstancePropertyInfo<T>(T instance, string name)
        {
            return instance.GetType().GetProperty(name, ReflectionConstants.PublicFlag);
        }

        public static FieldInfo GetPublicInstanceFieldInfo<T>(T instance, string name)
        {
            return instance.GetType().GetField(name, ReflectionConstants.PublicInstanceFlag);
        }

        public static MethodInfo GetNonPublicStaticMethodInfo<T>(T instance, string name)
        {
            return instance.GetType().GetMethod(name, ReflectionConstants.NonPublicStaticFlag);
        }

        public static bool IsFieldDataSame<T, TData>(T instance, FieldInfo field, TData expectingData) where TData : class
        {
            var value = field.GetValue(instance) as TData;

            return value == expectingData;
        }

        public static Dictionary<string, LocalVariableInfo> GetLocalVariablesDictionary(MethodInfo methodInfo, string localVariableName)

        {
            var localVariableInfos = methodInfo.GetMethodBody()?.LocalVariables;

            if (localVariableInfos != null)
            {
                var dictionary = new Dictionary<string, LocalVariableInfo>(localVariableInfos.Count);

                foreach (var localVariable in localVariableInfos)
                {
                    dictionary[localVariable.ToString()] = localVariable;
                }

                return dictionary;
            }

            return null;
        }

        public static bool IsPropertyDataSame<T, TData>(T instance, FieldInfo field, TData expectingData) where TData : class
        {
            var value = field.GetValue(instance) as TData;

            return value == expectingData;
        }
    }
}