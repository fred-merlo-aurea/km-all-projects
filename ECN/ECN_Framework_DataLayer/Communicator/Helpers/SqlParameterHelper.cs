using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator.Helpers
{
    public static class SqlParameterHelper<TFillParameters> where TFillParameters : class
    {
        private const string IdProperty = "ID";
        private const string IdRegex = "Id$";
        private const string ExceptionProcedureNameNullOrWhiteSpace = "Procedure name is null or white space.";

        public static SqlCommand CreateCommand(TFillParameters fillLayoutContentArgs, string procedureName)
        {
            Guard.NotNull(fillLayoutContentArgs, nameof(fillLayoutContentArgs));
            
            if (string.IsNullOrWhiteSpace(procedureName))
            {
                throw new ArgumentException(ExceptionProcedureNameNullOrWhiteSpace);
            }

            var command = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = procedureName
            };

            foreach (var prop in typeof(TFillParameters).GetProperties())
            {
                var propertyName = Regex.Replace(prop.Name, IdRegex, IdProperty);
                var propertyValue = prop.GetValue(fillLayoutContentArgs);

                if (!string.IsNullOrWhiteSpace(propertyValue?.ToString()))
                {
                    command.Parameters.AddWithValue($"@{propertyName}", propertyValue);
                }
            }
            return command;
        }
    }
}
