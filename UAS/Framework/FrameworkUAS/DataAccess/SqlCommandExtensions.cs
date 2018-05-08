using System;
using KM.Common;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public static class SqlCommandExtensions
    {
        private const string ExceptionConnectionStringNullOrWhiteSpace = "Connection string is null or white space.";
        private const string ExceptionConnectionNullInSqlCommand = "Connection is null in SqlCommand parameter.";

        public static List<T> GetList<T>(this SqlCommand cmd, ConnectionString connString)
        {
            List<T> retList = new List<T>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd,connString.ToString()))
                {
                    if (rdr != null)
                    {
                        T retItem = default(T);
                        DynamicBuilder<T> builder = DynamicBuilder<T>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            if (retItem != null)
                            {
                                retList.Add(retItem);
                            }
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }

        public static List<T> GetList<T>(this SqlCommand command, string connString)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (string.IsNullOrWhiteSpace(connString))
            {
                throw new ArgumentException(ExceptionConnectionStringNullOrWhiteSpace);
            }

            command.Connection = KM.Common.DataFunctions.GetSqlConnection(connString);

            return GetList<T>(command);
        }

        public static List<T> GetList<T>(this SqlCommand cmd, KMPlatform.Entity.Client client)
        {
            List<T> retList = new List<T>();
            try
            {
                cmd.Connection = KMPlatform.DataAccess.DataFunctions.GetClientSqlConnection(client);
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReaderNullIfEmpty(cmd))
                {
                    if (rdr != null)
                    {
                        T retItem = default(T);
                        DynamicBuilder<T> builder = DynamicBuilder<T>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            if (retItem != null)
                            {
                                retList.Add(retItem);
                            }
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }
        public static List<T> GetList<T>(this SqlCommand command, KMPlatform.Object.ClientConnections client)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            command.Connection = KMPlatform.DataAccess.DataFunctions.GetClientSqlConnection(client);

            return GetList<T>(command);
        }

        private static List<T> GetList<T>(this SqlCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (command.Connection == null)
            {
                throw new ArgumentException(ExceptionConnectionNullInSqlCommand);
            }

            var list = new List<T>();

            try
            {
                using (var reader = KM.Common.DataFunctions.ExecuteReaderNullIfEmpty(command))
                {
                    if (reader != null)
                    {
                        var builder = DynamicBuilder<T>.CreateBuilder(reader);

                        while (reader.Read())
                        {
                            var item = builder.Build(reader);
                            if (item != null)
                            {
                                list.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Error is ignored in original code.
                Trace.WriteLine(ex);
            }
            finally
            {
                // Possible issue: Disposing command and connection not in the method to create them.
                command.Connection.Close();
                command.Dispose();
            }

            return list;
        }
    }
}
