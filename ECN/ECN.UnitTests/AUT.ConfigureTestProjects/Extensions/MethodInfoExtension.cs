using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AUT.ConfigureTestProjects.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class MethodInfoExtension
    {
        public static bool TryToGetResultMethodInfo<T, TReturnData>(this MethodInfo method, T instance, out TReturnData returnData, dynamic[] parameters = null)
        {
            Exception exception;

            return TryToGetResultMethodInfo<T, TReturnData>(method, instance, out returnData, out exception, parameters);
        }

        /// <summary>
        /// Returns true if method invoke 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturnData"></typeparam>
        /// <param name="method"></param>
        /// <param name="instance"></param>
        /// <param name="returnData"></param>
        /// <param name="exception"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static bool TryToGetResultMethodInfo<T, TReturnData>(this MethodInfo method, T instance, out TReturnData returnData, out Exception exception, dynamic[] parameters = null)
        {
            object result;
            returnData = default(TReturnData);
            exception = null;

            try
            {
                result = method.Invoke(instance, parameters);
                returnData = (TReturnData) result;
                return true;
            }
            catch (Exception e)
            {
                exception = e;
                return false;
            }
        }

        /// <summary>
        ///     If got into exeption then returns null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturnData"></typeparam>
        /// <param name="instance"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static TReturnData GetResultMethodInfo<T, TReturnData>(this MethodInfo method, T instance, dynamic[] parameters = null)
        {
            Exception exception;
            return GetResultMethodInfo<T, TReturnData>(method, instance, parameters);
        }

        /// <summary>
        ///     If got into exeption then returns null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturnData"></typeparam>
        /// <param name="instance"></param>
        /// <param name="method"></param>
        /// <param name="exception"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static TReturnData GetResultMethodInfo<T, TReturnData>(this MethodInfo method, T instance, out Exception exception, dynamic[] parameters = null)
        {
            object result;
            TReturnData returnData = default(TReturnData);
            exception = null;

            try
            {
                result = method.Invoke(instance, parameters);
                returnData = (TReturnData) result;
                return returnData;
            }
            catch (Exception e)
            {
                exception = e;
                Trace.TraceError("Error: {0}", e);
            }

            return returnData;
        }

        /// <summary>
        ///     If got into exeption then returns null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturnData"></typeparam>
        /// <param name="instance"></param>
        /// <param name="method"></param>
        /// <param name="exception"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static void InvokeMethodInfo<T>(this MethodInfo method, T instance, out Exception exception, dynamic[] parameters = null)
        {
            object result;
            exception = null;

            try
            {
                method.Invoke(instance, parameters);
            }
            catch (Exception e)
            {
                exception = e;
                Trace.TraceError("Error: {0}", e);
            }
        }

        /// <summary>
        ///     Returns true false based if
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static bool DoesInvokeThrow<T>(this MethodInfo method, T instance, dynamic[] parameters = null)
        {
            try
            {
                method.Invoke(instance, parameters);
                return false;
            }
            catch (Exception e)
            {
                Trace.TraceError("Error: {0}", e);
                return true;
            }
        }

        /// <summary>
        ///     Returns true false based if
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="method"></param>
        /// <param name="exception"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static bool DoesInvokeThrow<T>(this MethodInfo method, T instance, out Exception exception, dynamic[] parameters = null)
        {
            exception = null;
            try
            {
                method.Invoke(instance, parameters);
                return false;
            }
            catch (Exception e)
            {
                exception = e;
                Trace.TraceError("Error: {0}", e);
                return true;
            }
        }


        /// <summary>
        /// Get method thrown exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static Exception GetThrownException<T>(this MethodInfo method, T instance, dynamic[] parameters = null)
        {
            try
            {
                method.Invoke(instance, parameters);

                return null;
            }
            catch (Exception e)
            {
                Trace.TraceError("Error: {0}", e);
                return e;
            }
        }
    }
}
