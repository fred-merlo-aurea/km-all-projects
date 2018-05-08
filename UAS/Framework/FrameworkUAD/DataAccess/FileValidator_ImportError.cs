using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FrameworkUAD.DataAccess
{
    public class FileValidator_ImportError
    {
        public static List<Entity.FileValidator_ImportError> Select(string processCode, int sourceFileID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FileValidator_ImportError_Select_ProcessCode_SourceFileID";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.FileValidator_ImportError> GetList(SqlCommand cmd)
        {
            List<Entity.FileValidator_ImportError> retList = new List<Entity.FileValidator_ImportError>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.FileValidator_ImportError retItem = new Entity.FileValidator_ImportError();
                        DynamicBuilder<Entity.FileValidator_ImportError> builder = DynamicBuilder<Entity.FileValidator_ImportError>.CreateBuilder(rdr);
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
        public static bool SaveBulkSqlInsert(List<Entity.ImportError> list, KMPlatform.Object.ClientConnections client)
        {
            DataTable dt = Core_AMS.Utilities.BulkDataReader<Entity.ImportError>.ToDataTable(list);
            bool done = true;
            SqlConnection conn = DataFunctions.GetClientSqlConnection(client);
            try
            {
                conn.Open();
                SqlBulkCopy bc = default(SqlBulkCopy);
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "FileValidator_ImportError");
                bc.BatchSize = 1000;
                bc.BulkCopyTimeout = 0;

                bc.ColumnMappings.Add("SourceFileID", "SourceFileID");
                bc.ColumnMappings.Add("RowNumber", "RowNumber");
                bc.ColumnMappings.Add("FormattedException", "FormattedException");
                bc.ColumnMappings.Add("ClientMessage", "ClientMessage");
                bc.ColumnMappings.Add("MAFField", "MAFField");
                bc.ColumnMappings.Add("BadDataRow", "BadDataRow");
                bc.ColumnMappings.Add("ThreadID", "ThreadID");
                bc.ColumnMappings.Add("DateCreated", "DateCreated");
                bc.ColumnMappings.Add("ProcessCode", "ProcessCode");
                bc.ColumnMappings.Add("IsDimensionError", "IsDimensionError");

                bc.WriteToServerAsync(dt);
                bc.Close();
            }
            catch
            {
                done = false;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return done;
        }
        public static bool SaveBulkSqlInsert(Entity.ImportError x, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ImportError> list = new List<Entity.ImportError>();
            list.Add(x);
            DataTable dt = Core_AMS.Utilities.BulkDataReader<Entity.ImportError>.ToDataTable(list);
            bool done = true;
            SqlConnection conn = DataFunctions.GetClientSqlConnection(client);
            try
            {
                conn.Open();
                SqlBulkCopy bc = default(SqlBulkCopy);
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "FileValidator_ImportError");
                bc.BatchSize = 1000;
                bc.BulkCopyTimeout = 0;

                bc.ColumnMappings.Add("SourceFileID", "SourceFileID");
                bc.ColumnMappings.Add("RowNumber", "RowNumber");
                bc.ColumnMappings.Add("FormattedException", "FormattedException");
                bc.ColumnMappings.Add("ClientMessage", "ClientMessage");
                bc.ColumnMappings.Add("MAFField", "MAFField");
                bc.ColumnMappings.Add("BadDataRow", "BadDataRow");
                bc.ColumnMappings.Add("ThreadID", "ThreadID");
                bc.ColumnMappings.Add("DateCreated", "DateCreated");
                bc.ColumnMappings.Add("ProcessCode", "ProcessCode");
                bc.ColumnMappings.Add("IsDimensionError", "IsDimensionError");

                bc.WriteToServerAsync(dt);
                bc.Close();
            }
            catch
            {
                done = false;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return done;
        }
    }
}
