using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class DataCompareView
    {
        public static List<Entity.DataCompareView> SelectForSourceFile(int sourceFileId)
        {
            List<Entity.DataCompareView> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareView_Select_SourceFileId";
            cmd.Parameters.AddWithValue("@sourceFileId", sourceFileId);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.DataCompareView> SelectForClient(int clientId)
        {
            List<Entity.DataCompareView> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareView_Select_ClientId";
            cmd.Parameters.AddWithValue("@clientId", clientId);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.DataCompareView> SelectForUser(int userId)
        {
            List<Entity.DataCompareView> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareView_Select_UserId";
            cmd.Parameters.AddWithValue("@userId", userId);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.DataCompareView> SelectForRun(int dcRunId)
        {
            List<Entity.DataCompareView> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareView_Select_DcRunId";
            cmd.Parameters.AddWithValue("@dcRunId", dcRunId);
            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.DataCompareView Get(SqlCommand cmd)
        {
            Entity.DataCompareView retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.DataCompareView();
                        DynamicBuilder<Entity.DataCompareView> builder = DynamicBuilder<Entity.DataCompareView>.CreateBuilder(rdr);
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
        public static List<Entity.DataCompareView> GetList(SqlCommand cmd)
        {
            List<Entity.DataCompareView> retList = new List<Entity.DataCompareView>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.DataCompareView retItem = new Entity.DataCompareView();
                        DynamicBuilder<Entity.DataCompareView> builder = DynamicBuilder<Entity.DataCompareView>.CreateBuilder(rdr);
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
        public static int Save(Entity.DataCompareView x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareView_Save";
            cmd.Parameters.AddWithValue("@DcViewId", x.DcViewId);
            cmd.Parameters.AddWithValue("@DcRunId", x.DcRunId);
            cmd.Parameters.AddWithValue("@DcTargetCodeId", x.DcTargetCodeId);
            cmd.Parameters.AddWithValue("@DcTargetIdUad", (object)x.DcTargetIdUad ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UadNetCount", x.UadNetCount);
            cmd.Parameters.AddWithValue("@MatchedCount", x.MatchedCount);
            cmd.Parameters.AddWithValue("@NoDataCount", x.NoDataCount);
            cmd.Parameters.AddWithValue("@Cost", x.Cost);
            cmd.Parameters.AddWithValue("@DateCreated", x.DateCreated);
            cmd.Parameters.AddWithValue("@DateUpdated", x.DateUpdated);
            cmd.Parameters.AddWithValue("@CreatedByUserID", x.CreatedByUserID);
            cmd.Parameters.AddWithValue("@UpdatedByUserID", x.UpdatedByUserID);
            cmd.Parameters.AddWithValue("@IsBillable ", x.IsBillable);
            cmd.Parameters.AddWithValue("@Notes", x.Notes);
            cmd.Parameters.AddWithValue("@PaymentStatusId ", x.PaymentStatusId);
            cmd.Parameters.AddWithValue("@PaidDate", x.PaidDate);
            cmd.Parameters.AddWithValue("@DcTypeCodeId ", x.DcTypeCodeId);
            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        #region Object.DataCompareViewCost
        public static Object.DataCompareViewCost GetDataCompareViewCost(int matchCount, int likeCount, int clientId, int userId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_DataCompareViewCost_Select";
            cmd.Parameters.AddWithValue("@matchCount", matchCount);
            cmd.Parameters.AddWithValue("@likeCount", likeCount);
            cmd.Parameters.AddWithValue("@clientId", clientId);
            cmd.Parameters.AddWithValue("@userId", userId);
            return GetCost(cmd);
        }
        public static Object.DataCompareViewCost GetCost(SqlCommand cmd)
        {
            Object.DataCompareViewCost retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Object.DataCompareViewCost();
                        DynamicBuilder<Object.DataCompareViewCost> builder = DynamicBuilder<Object.DataCompareViewCost>.CreateBuilder(rdr);
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
        public static decimal GetDataCompareCost(int userId, int clientId, int count, FrameworkUAD_Lookup.Enums.DataCompareType dcType, FrameworkUAD_Lookup.Enums.DataCompareCost dcCost)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_DataCompare_GetCost";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@clientId", clientId);
            cmd.Parameters.AddWithValue("@count", count);
            cmd.Parameters.AddWithValue("@Match_or_Like", dcType.ToString());
            cmd.Parameters.AddWithValue("@MergePurge_or_Download", dcCost.ToString());
            return Convert.ToDecimal(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
        #endregion

        public static bool Delete(int DcViewID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareView_Delete_DcViewID";
            cmd.Parameters.AddWithValue("@DcViewID", DcViewID);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
    }
}
