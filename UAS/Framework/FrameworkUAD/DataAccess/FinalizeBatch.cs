using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    [Serializable]
    public class FinalizeBatch
    {
        public static List<Object.FinalizeBatch> SelectUser(int userId, bool isOpen, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_FinalizeBatch_Select_UserId";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@isOpen", isOpen);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        public static List<Object.FinalizeBatch> SelectAllUser(int userId, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_FinalizeBatch_SelectAll_UserName";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        public static List<Object.FinalizeBatch> SelectDateRange(DateTime startDate, DateTime endDate, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_FinalizeBatch_Select_StartDate_EndDate";
            cmd.Parameters.AddWithValue("@startDate", startDate);
            cmd.Parameters.AddWithValue("@endDate", endDate);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        public static Object.FinalizeBatch SelectBatchId(int batchId, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_FinalizeBatch_Select_BatchId";
            cmd.Parameters.AddWithValue("@batchId", batchId);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return Get(cmd);
        }
        public static Object.FinalizeBatch SelectBatchNumber(int batchNumber, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_FinalizeBatch_Select_BatchNumber";
            cmd.Parameters.AddWithValue("@batchNumber", batchNumber);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return Get(cmd);
        }

        //public static List<Object.FinalizeBatch> Select(int UserID, KMPlatform.Object.ClientConnections client, int clientId, string clientName)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "o_FinalizeBatch_Select";
        //    cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
        //    cmd.Parameters.AddWithValue("@ClientId", clientId);
        //    cmd.Parameters.AddWithValue("@ClientName", clientName);

        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);

        //    return GetList(cmd);
        //}
        //public static List<Object.FinalizeBatch> SelectBatch(int UserID, KMPlatform.Object.ClientConnections client, int clientId, string clientName)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    if (UserID > 0)
        //        cmd.CommandText = "o_FinalizeBatch_SelectAll";
        //    else
        //        cmd.CommandText = "o_FinalizeBatch_SelectAll_NoUser";

        //    cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
        //    cmd.Parameters.AddWithValue("@ClientId", clientId);
        //    cmd.Parameters.AddWithValue("@ClientName", clientName);
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);

        //    return GetList(cmd);
        //}
        //public static Object.FinalizeBatch SelectBatchID(KMPlatform.Object.ClientConnections client, int clientId, string clientName, int batchID)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;            
        //    cmd.CommandText = "o_FinalizeBatch_Select_BatchID";                        
        //    cmd.Parameters.AddWithValue("@BatchID", batchID);
        //    cmd.Parameters.AddWithValue("@ClientId", clientId);
        //    cmd.Parameters.AddWithValue("@ClientName", clientName);
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);

        //    return Get(cmd);
        //}
        //public static Object.FinalizeBatch SelectBatchID(int UserID, KMPlatform.Object.ClientConnections client, int clientId, string clientName, int batchID)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    if (UserID > 0)
        //        cmd.CommandText = "o_FinalizeBatch_SelectAll_BatchID";
        //    else
        //        cmd.CommandText = "o_FinalizeBatch_SelectAll_NoUser_BatchID";

        //    cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
        //    cmd.Parameters.AddWithValue("@ClientId", clientId);
        //    cmd.Parameters.AddWithValue("@ClientName", clientName);
        //    cmd.Parameters.AddWithValue("@BatchID", batchID);
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);

        //    return Get(cmd);
        //}
        //public static List<Object.FinalizeBatch> SelectBatchUserName(int UserID, KMPlatform.Object.ClientConnections client, int clientId, string clientName)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    if (UserID > 0)
        //    {
        //        cmd.CommandText = "o_FinalizeBatch_SelectAll_UserName";
        //        KMPlatform.BusinessLogic.User uWorker = new KMPlatform.BusinessLogic.User();
        //        KMPlatform.Entity.User user = uWorker.SelectUser(UserID,false);
        //        cmd.Parameters.AddWithValue("@UserFullName", user.FullName);
        //    }
        //    else
        //        cmd.CommandText = "o_FinalizeBatch_SelectAll_NoUser";

        //    cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
        //    cmd.Parameters.AddWithValue("@ClientId", clientId);
        //    cmd.Parameters.AddWithValue("@ClientName", clientName);
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);

        //    return GetList(cmd);
        //}
        //public static List<Object.FinalizeBatch> SelectBatchUserName(int userID, KMPlatform.Object.ClientConnections client, int clientId, string clientName, bool isDateFinalized, DateTime? startDate = null, DateTime? endDate = null)
        //{

        //        //retList = DataAccess.FinalizeBatch.SelectBatchUserName(userID, client, clientId, clientName, isDateFinalized);

        //        //retList = DataAccess.FinalizeBatch.SelectBatchUserName(userID, client, clientId, clientName, isDateFinalized, startDate);

        //        //retList = DataAccess.FinalizeBatch.SelectBatchUserName(userID, client, clientId, clientName, isDateFinalized, endDate);

        //        //retList = DataAccess.FinalizeBatch.SelectBatchUserName(userID, client, clientId, clientName, isDateFinalized, startDate, endDate);


        //}



        private static Object.FinalizeBatch Get(SqlCommand cmd)
        {
            Object.FinalizeBatch retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Object.FinalizeBatch();
                        DynamicBuilder<Object.FinalizeBatch> builder = DynamicBuilder<Object.FinalizeBatch>.CreateBuilder(rdr);
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
        private static List<Object.FinalizeBatch> GetList(SqlCommand cmd)
        {
            List<Object.FinalizeBatch> retList = new List<Object.FinalizeBatch>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        KMPlatform.BusinessLogic.User uWorker = new KMPlatform.BusinessLogic.User();
                        List<KMPlatform.Entity.User> allUsers = uWorker.Select(false);

                        Object.FinalizeBatch retItem = new Object.FinalizeBatch();
                        DynamicBuilder<Object.FinalizeBatch> builder = DynamicBuilder<Object.FinalizeBatch>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            if (retItem != null)
                            {
                                if (retItem.UserID > 0 && retItem.UserName.Length == 0)
                                {
                                    KMPlatform.Entity.User u = allUsers.FirstOrDefault(x => x.UserID == retItem.UserID);
                                    if (u != null)
                                    {
                                        retItem.UserName = u.FirstName + " " + u.LastName;
                                    }
                                }
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
