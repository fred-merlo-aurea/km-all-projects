using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class DataCompareRun
    {
        public static List<Entity.DataCompareRun> SelectForSourceFile(int sourceFileId)
        {
            List<Entity.DataCompareRun> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareRun_Select_SourceFileId";
            cmd.Parameters.AddWithValue("@sourceFileId", sourceFileId);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.DataCompareRun> SelectForClient(int clientId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareRun_Select_ClientId";
            cmd.Parameters.AddWithValue("@ClientId", clientId);
            return GetList(cmd);
        }
        public static List<Entity.DataCompareRun> SelectForUser(int userId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareRun_Select_UserId";
            cmd.Parameters.AddWithValue("@userId", userId);
            return GetList(cmd);
        }
        public static Entity.DataCompareRun Get(SqlCommand cmd)
        {
            Entity.DataCompareRun retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.DataCompareRun();
                        DynamicBuilder<Entity.DataCompareRun> builder = DynamicBuilder<Entity.DataCompareRun>.CreateBuilder(rdr);
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
        public static List<Entity.DataCompareRun> GetList(SqlCommand cmd)
        {
            List<Entity.DataCompareRun> retList = new List<Entity.DataCompareRun>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.DataCompareRun retItem = new Entity.DataCompareRun();
                        DynamicBuilder<Entity.DataCompareRun> builder = DynamicBuilder<Entity.DataCompareRun>.CreateBuilder(rdr);
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
        public static int Save(Entity.DataCompareRun x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareRun_Save";
            cmd.Parameters.AddWithValue("@DcRunId", x.DcRunId);
            cmd.Parameters.AddWithValue("@ClientId", x.ClientId);
            cmd.Parameters.AddWithValue("@SourceFileId", x.SourceFileId);
            cmd.Parameters.AddWithValue("@FileRecordCount", x.FileRecordCount);
            cmd.Parameters.AddWithValue("@MatchedRecordCount", x.MatchedRecordCount);
            cmd.Parameters.AddWithValue("@UadConsensusCount", x.UadConsensusCount);
            cmd.Parameters.AddWithValue("@ProcessCode", x.ProcessCode);
            cmd.Parameters.AddWithValue("@DateCreated", x.DateCreated);
            cmd.Parameters.AddWithValue("@IsBillable ", x.IsBillable);
            cmd.Parameters.AddWithValue("@Notes", x.Notes);

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}
