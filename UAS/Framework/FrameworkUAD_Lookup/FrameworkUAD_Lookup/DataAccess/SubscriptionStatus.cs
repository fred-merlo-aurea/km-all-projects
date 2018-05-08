using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAD_Lookup.DataAccess
{
    [Serializable]
    public class SubscriptionStatus
    {
        public static List<Entity.SubscriptionStatus> Select()
        {
            //if (CacheUtil.IsCacheEnabled())
            //{
            //    string DatabaseName = "UAD_LOOKUP";

            //    List<Entity.SubscriptionStatus> retItem = (List<Entity.SubscriptionStatus>) CacheUtil.GetFromCache("SubscriptionStatus", DatabaseName);

            //    if (retItem == null)
            //    {
            //        retItem = GetData();

            //        CacheUtil.AddToCache("SubscriptionStatus", retItem, DatabaseName);
            //    }

            //    return retItem;
            //}
            //else
            //{
                return GetData();
            //}
        }
        public static Entity.SubscriptionStatus Select(int categoryCodeID, int transactionCodeID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionStatus_Select_CatCodeID_TranCodeID";
            cmd.Parameters.AddWithValue("CategoryCodeID", categoryCodeID);
            cmd.Parameters.AddWithValue("TransactionCodeID", transactionCodeID);
            return Get(cmd);
        }
        public static Entity.SubscriptionStatus Select(int subscriptionStatusID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionStatus_Select_SubscriptionStatusID";
            cmd.Parameters.AddWithValue("@SubscriptionStatusID", subscriptionStatusID);
            return Get(cmd);
        }
        private static Entity.SubscriptionStatus Get(SqlCommand cmd)
        {
            Entity.SubscriptionStatus retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SubscriptionStatus();
                        DynamicBuilder<Entity.SubscriptionStatus> builder = DynamicBuilder<Entity.SubscriptionStatus>.CreateBuilder(rdr);
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
        private static List<Entity.SubscriptionStatus> GetData()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionStatus_Select";
            List<Entity.SubscriptionStatus> retList = new List<Entity.SubscriptionStatus>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.SubscriptionStatus retItem = new Entity.SubscriptionStatus();
                        DynamicBuilder<Entity.SubscriptionStatus> builder = DynamicBuilder<Entity.SubscriptionStatus>.CreateBuilder(rdr);
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
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }

        public static int Save(Entity.SubscriptionStatus x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionStatus_Save";
            cmd.Parameters.Add(new SqlParameter("@SubscriptionStatusID", x.SubscriptionStatusID));
            cmd.Parameters.Add(new SqlParameter("@StatusName", x.StatusName));
            cmd.Parameters.Add(new SqlParameter("@StatusCode", x.StatusCode));
            cmd.Parameters.Add(new SqlParameter("@Color", x.Color));
            cmd.Parameters.Add(new SqlParameter("@Icon", x.Icon));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAD_Lookup.ToString()));
        }
    }
}
