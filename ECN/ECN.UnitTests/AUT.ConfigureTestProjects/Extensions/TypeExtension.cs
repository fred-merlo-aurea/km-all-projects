using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AUT.ConfigureTestProjects.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class TypeExtension
    {
        /// <summary>
        ///     Create only if possible or else return null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Create<T>(this Type type)
        {
            try
            {
                var instantiated = Activator.CreateInstance(type);
                var t2 = (T) instantiated;
                return t2;
            }
            catch (Exception e)
            {
                Trace.TraceError("Error: {0}", e);
                return default(T);
            }
        }

        /// <summary>
        ///     Create only if possible or else return null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Create<T>(this Type type, params dynamic[] dynamicParamtersForConstructor)
        {
            try
            {
                var instantiated = Activator.CreateInstance(type);
                var t2 = (T) instantiated;
                return t2;
            }
            catch (Exception e)
            {
                Trace.TraceError("Error: {0}", e);
                return default(T);
            }
        }

        /// <summary>
        ///     Create only if possible or else return null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Create<T>(this Type type, out Exception exception, params dynamic[] dynamicParamtersForConstructor)
        {
            try
            {
                var instantiated = Activator.CreateInstance(type);
                var t2 = (T) instantiated;
                exception = null;
                return t2;
            }
            catch (Exception e)
            {
                exception = e;
                Trace.TraceError("Error: {0}", e);
                return default(T);
            }
        }

        /// <summary>
        ///     Create only if possible or else return null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static dynamic Create(this Type type)
        {
            try
            {
                var instantiated = Activator.CreateInstance(type);
                return instantiated;
            }
            catch (Exception e)
            {
                Trace.TraceError("Error: {0}", e);
                return null;
            }
        }

        /// <summary>
        ///     Create only if possible or else return null.
        /// </summary>
        /// <returns></returns>
        public static dynamic Create(this Type type, params dynamic[] dynamicParamtersForConstructor)
        {
            try
            {
                var instantiated = Activator.CreateInstance(type, dynamicParamtersForConstructor);
                return instantiated;
            }
            catch (Exception e)
            {
                Trace.TraceError("Error: {0}", e);
                return null;
            }
        }
    }
}