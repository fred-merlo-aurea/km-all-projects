using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAD_Lookup.DataAccess
{
    public class SubscriptionStatusMatrix
    {
        public static List<Entity.SubscriptionStatusMatrix> Select()
        {
            //if (CacheUtil.IsCacheEnabled())
            //{
            //    string DatabaseName = "UAD_LOOKUP";

            //    List<Entity.SubscriptionStatusMatrix> retItem = (List<Entity.SubscriptionStatusMatrix>) CacheUtil.GetFromCache("SubscriptionStatusMatrix", DatabaseName);

            //    if (retItem == null)
            //    {
            //        retItem = GetData();

            //        CacheUtil.AddToCache("SubscriptionStatusMatrix", retItem, DatabaseName);
            //    }

            //    return retItem;
            //}
            //else
            //{
                return GetData();
            //}
        }
        public static Entity.SubscriptionStatusMatrix Select(int subscriptionStatusID, int categoryCodeID, int transactionCodeID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionStatusMatrix_Select_SubscriptionStatusID_CatID_TranID";
            cmd.Parameters.AddWithValue("@SubscriptionStatusID", subscriptionStatusID);
            cmd.Parameters.AddWithValue("@CategoryCodeID", categoryCodeID);
            cmd.Parameters.AddWithValue("@TransactionCodeID", transactionCodeID);

            return Get(cmd);
        }
        private static Entity.SubscriptionStatusMatrix Get(SqlCommand cmd)
        {
            Entity.SubscriptionStatusMatrix retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SubscriptionStatusMatrix();
                        DynamicBuilder<Entity.SubscriptionStatusMatrix> builder = DynamicBuilder<Entity.SubscriptionStatusMatrix>.CreateBuilder(rdr);
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
        private static List<Entity.SubscriptionStatusMatrix> GetData()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionStatusMatrix_Select";
            List<Entity.SubscriptionStatusMatrix> retList = new List<Entity.SubscriptionStatusMatrix>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.SubscriptionStatusMatrix retItem = new Entity.SubscriptionStatusMatrix();
                        DynamicBuilder<Entity.SubscriptionStatusMatrix> builder = DynamicBuilder<Entity.SubscriptionStatusMatrix>.CreateBuilder(rdr);
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
            catch (Exception ex)
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

        public static int Save(Entity.SubscriptionStatusMatrix x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionStatusMatrix_Save";
            cmd.Parameters.Add(new SqlParameter("@StatusMatrixID", x.StatusMatrixID));
            cmd.Parameters.Add(new SqlParameter("@SubscriptionStatusID", x.SubscriptionStatusID));
            cmd.Parameters.Add(new SqlParameter("@CategoryCodeID", x.CategoryCodeID));
            cmd.Parameters.Add(new SqlParameter("@TransactionCodeID", x.TransactionCodeID));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAD_Lookup.ToString()));
        }
    }
}
