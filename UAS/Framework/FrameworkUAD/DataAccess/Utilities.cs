using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KMPlatform.Object;
using static KM.Common.DataFunctions;

namespace FrameworkUAD.DataAccess
{
    public class Utilities
    {
        public static NameValueCollection ValidateDeleteOrInActive(
            ClientConnections clientConnections,
            string commandText,
            IEnumerable<IDbDataParameter> commandParameter,
            Func<SqlDataReader, NameValueCollection> processAction)
        {
            using (var sqlConnection = DataFunctions.GetClientSqlConnection(clientConnections))
            {
                using (var sqlCommand = CreateStoredProcedureCommand(commandText, commandParameter))
                {
                    sqlConnection.Open();
                    sqlCommand.Connection = sqlConnection;

                    using (var dataReader = sqlCommand.ExecuteReader())
                    {
                        return processAction(dataReader);
                    }
                }
            }
        }

        public static void DeleteCache(ClientConnections clientConnections, string cacheKey)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                var databaseName = DataFunctions.GetDBName(clientConnections);

                if (CacheUtil.GetFromCache(cacheKey, databaseName) != null)
                {
                    CacheUtil.RemoveFromCache(cacheKey, databaseName);
                }
            }
        }

        public static int Save(
            ClientConnections clientConnections,
            string commandText,
            IEnumerable<IDbDataParameter> commandParameter)
        {
            using (var sqlCommand = CreateStoredProcedureCommand(commandText, commandParameter))
            {
                return Convert.ToInt32(ExecuteScalar(sqlCommand, DataFunctions.GetClientSqlConnection(clientConnections)));
            }
        }

        public static int Delete(
            ClientConnections clientConnections,
            string commandText,
            IEnumerable<IDbDataParameter> commandParameter)
        {
            using (var sqlCommand = CreateStoredProcedureCommand(commandText, commandParameter))
            {
                return Execute(sqlCommand, DataFunctions.GetClientSqlConnection(clientConnections));
            }
        }

        private static SqlCommand CreateStoredProcedureCommand(
            string commandText,
            IEnumerable<IDbDataParameter> commandParameter)
        {
            var sqlCommand = new SqlCommand(commandText) { CommandType = CommandType.StoredProcedure, CommandTimeout = 0, };

            foreach (var sqlCommandParameter in commandParameter)
            {
                sqlCommand.Parameters.Add(sqlCommandParameter);
            }

            return sqlCommand;
        }
    }
}