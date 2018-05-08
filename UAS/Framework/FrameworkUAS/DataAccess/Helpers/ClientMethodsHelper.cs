using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KM.Common.Data;
using KMPlatform.Entity;

namespace FrameworkUAS.DataAccess.Helpers
{
    public static class ClientMethodsHelper
    {
        private const string ParameterXml = "@Xml";
        private const string NullString = "NULL";
        private const string ExceptionProcedureNameNullOrWhiteSpace = "Procedure name is null or white space.";
        private const string ExceptionConnectionStringNullOrWhiteSpace = "Connection string is null or white space.";
        private const string ExceptionColumnNameNullOrWhiteSpace = "Column name is null or white space.";
        private const string ExceptionTableNameNullOrWhiteSpace = "Table name is null or white space.";
        private const int DefaultBatchSize = 2500;
        private const int BulkCopyTimeout = 0;

        public static string GetString(this DataRow row, string columnName)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }

            if (string.IsNullOrWhiteSpace(columnName))
            {
                throw new ArgumentException(ExceptionColumnNameNullOrWhiteSpace);
            }

            var columnValue = row[columnName].ToString();

            return columnValue == NullString ? null : columnValue;
        }

        public static bool ExecuteNonQueryWithXml(string xml, string procedureName, Client client)
        {
            if (string.IsNullOrWhiteSpace(procedureName))
            {
                throw new ArgumentException(ExceptionProcedureNameNullOrWhiteSpace);
            }

            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            var command = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = procedureName,
                Connection = KMPlatform.DataAccess.DataFunctions.GetClientSqlConnection(client)
            };
            command.Parameters.AddWithValue(ParameterXml, xml);

            return KM.Common.DataFunctions.ExecuteNonQuery(command);
        }

        public static bool ExecuteNonQueryWithXml(string xml, string procedureName, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(procedureName))
            {
                throw new ArgumentException(ExceptionProcedureNameNullOrWhiteSpace);
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException(ExceptionConnectionStringNullOrWhiteSpace);
            }

            var command = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = procedureName
            };
            command.Parameters.AddWithValue(ParameterXml, xml);

            return KM.Common.DataFunctions.ExecuteNonQuery(command, connectionString);
        }

        public static void BulkCopy(DataTable dataTable, string tableName, IDictionary<string, string> mappings, int batchSize = DefaultBatchSize)
        {
            if (dataTable == null)
            {
                throw new ArgumentNullException(nameof(dataTable));
            }

            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentException(ExceptionTableNameNullOrWhiteSpace);
            }

            if (mappings == null)
            {
                throw new ArgumentNullException(nameof(mappings));
            }

            using (var connection = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.UAS.ToString()))
            {
                connection.Open();

                using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, null)
                {
                    DestinationTableName = $"[{connection.Database}].[dbo].[{tableName}]",
                    BatchSize = batchSize,
                    BulkCopyTimeout = BulkCopyTimeout
                })
                {
                    foreach (var mapping in mappings)
                    {
                        bulkCopy.ColumnMappings.Add(mapping.Key, mapping.Value);
                    }

                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }
    }
}
