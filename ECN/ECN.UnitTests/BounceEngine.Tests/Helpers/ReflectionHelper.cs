using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BounceEngine.Tests.Helpers
{
    [ExcludeFromCodeCoverage]
    public class ReflectionHelper
    {
        private const int DefaultSqlErrorInfoNumber = 1;
        private static T Construct<T>(params object[] parameters)
        {
            var ctors = typeof(T).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)ctors.First(ctor => ctor.GetParameters().Length == parameters.Length).Invoke(parameters);
        }

        public static SqlException NewSqlException(int infoNumber = DefaultSqlErrorInfoNumber)
        {
            const byte errorState = 2;
            const byte errorClass = 3;
            const string server = "Server Name";
            const string errorMessage = "Error Message";
            const string procedure = "procedure";
            const string serverVersion = "7.0.0";
            const string sqlErrorCollectionAddMethodName = "Add";
            const string sqlExceptionCreateExceptionMethodName = "CreateException";
            const int lineNumber = 100;
            var collection = Construct<SqlErrorCollection>();
            var error = Construct<SqlError>(infoNumber, errorState, errorClass, server,
                errorMessage, procedure, lineNumber);
            typeof(SqlErrorCollection)
                .GetMethod(sqlErrorCollectionAddMethodName, BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(collection, new object[] { error });
            return typeof(SqlException)
                .GetMethod(sqlExceptionCreateExceptionMethodName, BindingFlags.NonPublic | BindingFlags.Static,
                    null,
                    CallingConventions.ExplicitThis,
                    new[] { typeof(SqlErrorCollection), typeof(string) },
                    new ParameterModifier[] { })
                .Invoke(null, new object[] { collection, serverVersion }) as SqlException;
        }
    }
}
