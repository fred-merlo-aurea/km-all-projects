using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class ImportError
    {
        public static List<Entity.ImportError> Select(string processCode, int sourceFileID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ImportError_Select_ProcessCode_SourceFileID";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }


        public static Entity.ImportError Get(SqlCommand cmd)
        {
            Entity.ImportError retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ImportError();
                        DynamicBuilder<Entity.ImportError> builder = DynamicBuilder<Entity.ImportError>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
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
            return retItem;
        }
        public static List<Entity.ImportError> GetList(SqlCommand cmd)
        {
            List<Entity.ImportError> retList = new List<Entity.ImportError>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.ImportError retItem = new Entity.ImportError();
                        DynamicBuilder<Entity.ImportError> builder = DynamicBuilder<Entity.ImportError>.CreateBuilder(rdr);
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

        public static int Save(Entity.ImportError x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ImportError_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@SourceFileID", x.SourceFileID);
            cmd.Parameters.AddWithValue("@RowNumber", (object)x.RowNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FormattedException", (object)x.FormattedException ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ClientMessage", (object)x.ClientMessage ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@MAFField", (object)x.MAFField ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BadDataRow", (object)x.BadDataRow ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ThreadID", (object)x.ThreadID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DateCreated", (object)x.DateCreated ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ProcessCode", (object)x.ProcessCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsDimensionError", x.IsDimensionError);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static bool SaveBulkSqlInsert(List<Entity.ImportError> list, KMPlatform.Object.ClientConnections client)
        {
            int sourceFileID = list.FirstOrDefault().SourceFileID;
            string processCode = list.FirstOrDefault().ProcessCode;
            DataTable dt = Core_AMS.Utilities.BulkDataReader<Entity.ImportError>.ToDataTable(list);
            bool done = true;
            SqlConnection conn = DataFunctions.GetClientSqlConnection(client);
            try
            {
                conn.Open();
                SqlBulkCopy bc = default(SqlBulkCopy);
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "ImportError");
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
            int sourceFileID = x.SourceFileID;
            string processCode = x.ProcessCode;
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
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "ImportError");
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
