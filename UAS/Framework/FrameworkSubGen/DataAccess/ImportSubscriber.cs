using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM;
using KM.Common;
using KM.Common.Data;

namespace FrameworkSubGen.DataAccess
{
    public class ImportSubscriber
    {
        public static List<Entity.ImportSubscriber> Select(int accountId, bool isMergedToUAD)
        {
            List<Entity.ImportSubscriber> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ImportSubscriber_Select_accountId_IsMergedToUAD";
            cmd.Parameters.AddWithValue("@accountId", accountId);
            cmd.Parameters.AddWithValue("@IsMergedToUAD", isMergedToUAD);
            retItem = GetList(cmd);
            return retItem;
        }
        public static int Save(Entity.ImportSubscriber sub)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ImportSubscriber_Save";
            //cmd.Parameters.AddWithValue("@xml", xml);
            cmd.Connection = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.SubGenData.ToString());

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static bool UpdateMergedToUAD(string xml)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ImportSubscriberFile_UpdateMergedToUAD";
            cmd.Parameters.AddWithValue("@xml", xml);
            cmd.Connection = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.SubGenData.ToString());

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool JobImportSubscriberFile(string xml)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_ImportSubscriberFile";
            cmd.Parameters.AddWithValue("@xml", xml);
            cmd.Connection = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.SubGenData.ToString());

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        private static Entity.ImportSubscriber Get(SqlCommand cmd)
        {
            Entity.ImportSubscriber retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.SubGenData.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ImportSubscriber();
                        DynamicBuilder<Entity.ImportSubscriber> builder = DynamicBuilder<Entity.ImportSubscriber>.CreateBuilder(rdr);
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
        private static List<Entity.ImportSubscriber> GetList(SqlCommand cmd)
        {
            List<Entity.ImportSubscriber> retList = new List<Entity.ImportSubscriber>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.SubGenData.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.ImportSubscriber retItem = new Entity.ImportSubscriber();
                        DynamicBuilder<Entity.ImportSubscriber> builder = DynamicBuilder<Entity.ImportSubscriber>.CreateBuilder(rdr);
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
    }
}
