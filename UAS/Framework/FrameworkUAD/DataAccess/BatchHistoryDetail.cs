using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAD.DataAccess
{
    [Serializable]
    public class BatchHistoryDetail
    {
        public static List<Object.BatchHistoryDetail> Select(int userID, bool isActive, KMPlatform.Object.ClientConnections client, string clientName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_BatchHistoryDetail_Select_UserID_IsActive";
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            cmd.Parameters.Add(new SqlParameter("@IsActive", isActive));
            cmd.Parameters.AddWithValue("@ClientName", clientName);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Object.BatchHistoryDetail> SelectBatchID(int userID, bool isActive, KMPlatform.Object.ClientConnections client, string clientName, int batchID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_BatchHistoryDetail_Select_UserID_IsActive_BatchID";
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            cmd.Parameters.Add(new SqlParameter("@IsActive", isActive));
            cmd.Parameters.AddWithValue("@ClientName", clientName);
            cmd.Parameters.AddWithValue("@BatchID", batchID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Object.BatchHistoryDetail> SelectBatchID(bool isActive, KMPlatform.Object.ClientConnections client, string clientName, int batchID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_BatchHistoryDetail_Select_IsActive_BatchID";
            cmd.Parameters.Add(new SqlParameter("@IsActive", isActive));
            cmd.Parameters.AddWithValue("@ClientName", clientName);
            cmd.Parameters.AddWithValue("@BatchID", batchID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Object.BatchHistoryDetail> Select(KMPlatform.Object.ClientConnections client, string clientName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_BatchHistoryDetail_Select";
            cmd.Parameters.AddWithValue("@ClientName", clientName);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Object.BatchHistoryDetail> Select(int SubscriptionID, KMPlatform.Object.ClientConnections client, string clientName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_BatchHistoryDetail_Select_SubscriptionID";
            cmd.Parameters.Add(new SqlParameter("@SubscriptionID", SubscriptionID));
            cmd.Parameters.AddWithValue("@ClientName", clientName);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }

        public static List<Object.BatchHistoryDetail> SelectSubscriber(int PubSubscriptionID, KMPlatform.Object.ClientConnections client, string clientName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_BatchHistoryDetail_Select_SubscriberId";
            cmd.Parameters.Add(new SqlParameter("@PubSubscriptionID", PubSubscriptionID));
            cmd.Parameters.AddWithValue("@ClientName", clientName);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Object.BatchHistoryDetail> SelectBatch(int BatchID, string Name, int SequenceID, KMPlatform.Object.ClientConnections client, string clientName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_BatchHistoryDetail_Select_BatchID_Name_Sequence";
            cmd.Parameters.Add(new SqlParameter("@BatchID", BatchID));
            cmd.Parameters.Add(new SqlParameter("@Name", Name));
            cmd.Parameters.Add(new SqlParameter("@SequenceID", SequenceID));
            cmd.Parameters.AddWithValue("@ClientName", clientName);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Object.BatchHistoryDetail> SelectBatch(int BatchID, string Name, int SequenceID, DateTime From, DateTime To, KMPlatform.Object.ClientConnections client, string clientName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_BatchHistoryDetail_Select_BatchID_Name_Sequence_DateRange";
            cmd.Parameters.Add(new SqlParameter("@BatchID", BatchID));
            cmd.Parameters.Add(new SqlParameter("@Name", Name));
            cmd.Parameters.Add(new SqlParameter("@SequenceID", SequenceID));
            cmd.Parameters.Add(new SqlParameter("@From", From));
            cmd.Parameters.Add(new SqlParameter("@To", To));
            cmd.Parameters.AddWithValue("@ClientName", clientName);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        private static Object.BatchHistoryDetail Get(SqlCommand cmd)
        {
            Object.BatchHistoryDetail retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Object.BatchHistoryDetail();
                        DynamicBuilder<Object.BatchHistoryDetail> builder = DynamicBuilder<Object.BatchHistoryDetail>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);

                            //if (retItem != null)
                            //{
                            //    KMPlatform.BusinessLogic.UserLog ulWorker = new KMPlatform.BusinessLogic.UserLog();
                            //    KMPlatform.Entity.UserLog ul = ulWorker.Select(retItem.UserLogID);
                            //    if (ul != null)
                            //    {
                            //        retItem.Object = ul.Object;
                            //        retItem.FromObjectValues = ul.FromObjectValues;
                            //        retItem.ToObjectValues = ul.ToObjectValues;
                            //        retItem.UserLogDateCreated = ul.DateCreated;
                            //    }
                            //}
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
        private static List<Object.BatchHistoryDetail> GetList(SqlCommand cmd)
        {
            List<Object.BatchHistoryDetail> retList = new List<Object.BatchHistoryDetail>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Object.BatchHistoryDetail retItem = new Object.BatchHistoryDetail();
                        DynamicBuilder<Object.BatchHistoryDetail> builder = DynamicBuilder<Object.BatchHistoryDetail>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            if (retItem != null)
                            {
                                //KMPlatform.BusinessLogic.UserLog ulWorker = new KMPlatform.BusinessLogic.UserLog();
                                //KMPlatform.Entity.UserLog ul = ulWorker.Select(retItem.UserLogID);
                                //if (ul != null)
                                //{
                                //    retItem.Object = ul.Object;
                                //    retItem.FromObjectValues = ul.FromObjectValues;
                                //    retItem.ToObjectValues = ul.ToObjectValues;
                                //    retItem.UserLogDateCreated = ul.DateCreated;

                                //    FrameworkUAD_Lookup.BusinessLogic.Code codeW = new FrameworkUAD_Lookup.BusinessLogic.Code();
                                //    FrameworkUAD_Lookup.Entity.Code c = codeW.SelectCodeId(ul.UserLogTypeID);
                                //    if(c != null)
                                //    {
                                //        retItem.UserLogTypeName = c.DisplayName;
                                //    }
                                //}
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
