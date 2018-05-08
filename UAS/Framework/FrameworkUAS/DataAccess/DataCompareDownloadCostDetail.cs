using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class DataCompareDownloadCostDetail
    {
        public static List<Entity.DataCompareDownloadCostDetail> SelectDataCompareDownloadId(int dcDownloadId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareDownloadCostDetail_Select_DcDownloadId";
            cmd.Parameters.AddWithValue("@dcDownloadId", dcDownloadId);
            return GetList(cmd);
        }
        public static Entity.DataCompareDownloadCostDetail Get(SqlCommand cmd)
        {
            Entity.DataCompareDownloadCostDetail retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.DataCompareDownloadCostDetail();
                        DynamicBuilder<Entity.DataCompareDownloadCostDetail> builder = DynamicBuilder<Entity.DataCompareDownloadCostDetail>.CreateBuilder(rdr);
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
        public static List<Entity.DataCompareDownloadCostDetail> GetList(SqlCommand cmd)
        {
            List<Entity.DataCompareDownloadCostDetail> retList = new List<Entity.DataCompareDownloadCostDetail>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.DataCompareDownloadCostDetail retItem = new Entity.DataCompareDownloadCostDetail();
                        DynamicBuilder<Entity.DataCompareDownloadCostDetail> builder = DynamicBuilder<Entity.DataCompareDownloadCostDetail>.CreateBuilder(rdr);
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
        public static List<Entity.DataCompareDownloadCostDetail> CreateCostDetail(int dcViewId, int dcTypeCodeID, string profileCount, string profileColumns, string demoColumns, int userId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_DataCompare_CreateCostDetail";
            cmd.Parameters.AddWithValue("@dcViewId", dcViewId);
            cmd.Parameters.AddWithValue("@dcTypeCodeID", dcTypeCodeID);
            cmd.Parameters.AddWithValue("@profileCount", profileCount);
            cmd.Parameters.AddWithValue("@profileColumns", profileColumns);
            cmd.Parameters.AddWithValue("@demoColumns", demoColumns);
            cmd.Parameters.AddWithValue("@userId", userId);
            return GetList(cmd);
        }
    }
}
