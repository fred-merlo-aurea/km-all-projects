using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class Suppressed
    {
        private static Entity.Suppressed Get(SqlCommand cmd)
        {
            Entity.Suppressed retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Suppressed();
                        DynamicBuilder<Entity.Suppressed> builder = DynamicBuilder<Entity.Suppressed>.CreateBuilder(rdr);
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
        private static List<Entity.Suppressed> GetList(SqlCommand cmd)
        {
            List<Entity.Suppressed> retList = new List<Entity.Suppressed>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.Suppressed retItem = new Entity.Suppressed();
                        DynamicBuilder<Entity.Suppressed> builder = DynamicBuilder<Entity.Suppressed>.CreateBuilder(rdr);
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
        public static bool SaveBulkSqlInsert(List<Entity.Suppressed> list, KMPlatform.Object.ClientConnections client)
        {

            Core_AMS.Utilities.BulkDataReader<Entity.Suppressed> bdr = new Core_AMS.Utilities.BulkDataReader<Entity.Suppressed>(list);

            DataTable dt = Core_AMS.Utilities.BulkDataReader<Entity.Suppressed>.ToDataTable(list);
            bool done = true;
            SqlConnection conn = DataFunctions.GetClientSqlConnection(client);

            try
            {
                conn.Open();
                SqlBulkCopy bc = default(SqlBulkCopy);
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "Suppressed");
                bc.BatchSize = 1000;
                bc.BulkCopyTimeout = 0;

                bc.ColumnMappings.Add("STRecordIdentifier", "STRecordIdentifier");
                bc.ColumnMappings.Add("SFRecordIdentifier", "SFRecordIdentifier");
                bc.ColumnMappings.Add("Source", "Source");
                bc.ColumnMappings.Add("IsSuppressed", "IsSuppressed");
                bc.ColumnMappings.Add("IsEmailMatch", "IsEmailMatch");
                bc.ColumnMappings.Add("IsPhoneMatch", "IsPhoneMatch");
                bc.ColumnMappings.Add("IsAddressMatch", "IsAddressMatch");
                bc.ColumnMappings.Add("IsCompanyMatch", "IsCompanyMatch");
                bc.ColumnMappings.Add("ProcessCode", "ProcessCode");
                bc.ColumnMappings.Add("DateCreated", "DateCreated");
                bc.ColumnMappings.Add("DateUpdated", "DateUpdated");
                bc.ColumnMappings.Add("UpdatedByUserID", "UpdatedByUserID");

                bc.WriteToServer(dt);
                bc.Close();
            }
            catch (Exception ex)
            {
                done = false;
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return done;
        }

        public static int PerformSuppression(string xml, KMPlatform.Object.ClientConnections client, int sourceFileID, string processCode, string suppFileName)
        {
            int suppCount = 0;

            SqlConnection conn = DataFunctions.GetClientSqlConnection(client);
            try
            {
                SqlCommand cmd = new SqlCommand("job_Suppression", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Xml", xml);
                cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
                cmd.Parameters.AddWithValue("@ProcessCode", processCode.ToString());
                cmd.Parameters.AddWithValue("@SuppFileName", suppFileName.ToString());

                suppCount = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return suppCount;
        }
    }
}
