using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    [Serializable]
    public class History
    {
        public static List<Entity.History> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.History> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_History_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.History> Select(int batchID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.History> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_History_Select_BatchID";
            cmd.Parameters.Add(new SqlParameter("@BatchID", batchID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.History> Select(DateTime startDate, DateTime endDate, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.History> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_History_Select_DateCreated";
            cmd.Parameters.Add(new SqlParameter("@StartDate", startDate));
            cmd.Parameters.Add(new SqlParameter("@EndDate", endDate));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.History> SelectBatch(int UserID, int PublicationID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.History> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_History_Select_Active_User_BatchID";
            cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
            cmd.Parameters.Add(new SqlParameter("@PublicationID", PublicationID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<int> UserLogList(int historyID, KMPlatform.Object.ClientConnections client)
        {
            List<int> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_HistoryToUserLog_HistoryID";
            cmd.Parameters.Add(new SqlParameter("@HistoryID", historyID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetIntList(cmd);
            return retItem;
        }
        public static List<int> HistoryResponseList(int historyID, KMPlatform.Object.ClientConnections client)
        {
            List<int> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_HistoryToHistoryResonse_HistoryID";
            cmd.Parameters.Add(new SqlParameter("@HistoryID", historyID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetIntList(cmd);
            return retItem;
        }
        public static List<int> HistoryMarketingMapList(int historyID, KMPlatform.Object.ClientConnections client)
        {
            List<int> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_HistoryToHistoryMarketingMap_HistoryID";
            cmd.Parameters.Add(new SqlParameter("@HistoryID", historyID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetIntList(cmd);
            return retItem;
        }
        private static Entity.History Get(SqlCommand cmd)
        {
            Entity.History retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.History();
                        DynamicBuilder<Entity.History> builder = DynamicBuilder<Entity.History>.CreateBuilder(rdr);
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
        private static List<Entity.History> GetList(SqlCommand cmd)
        {
            List<Entity.History> retList = new List<Entity.History>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.History retItem = new Entity.History();
                        DynamicBuilder<Entity.History> builder = DynamicBuilder<Entity.History>.CreateBuilder(rdr);
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

        private static List<int> GetIntList(SqlCommand cmd)
        {
            List<int> retList = new List<int>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        int retItem = new int();
                        DynamicBuilder<int> builder = DynamicBuilder<int>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            retList.Add(retItem);
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
        public static int Save(Entity.History x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_History_Save";
            cmd.Parameters.Add(new SqlParameter("@HistoryID", x.HistoryID));
            cmd.Parameters.Add(new SqlParameter("@BatchID", x.BatchID));
            cmd.Parameters.Add(new SqlParameter("@BatchCountItem", x.BatchCountItem));
            cmd.Parameters.Add(new SqlParameter("@PublicationID", x.PublicationID));
            cmd.Parameters.Add(new SqlParameter("@PubSubscriptionID", x.PubSubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@SubscriptionID", x.SubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@HistorySubscriptionID", x.HistorySubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@HistoryPaidID", x.HistoryPaidID));
            cmd.Parameters.Add(new SqlParameter("@HistoryPaidBillToID", x.HistoryPaidBillToID));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static bool Insert_History_To_UserLog(int historyID, int userLogID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_HistoryToUserLog_Save";
            cmd.Parameters.Add(new SqlParameter("@HistoryID", historyID));
            cmd.Parameters.Add(new SqlParameter("@UserLogID", userLogID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            int rowCount = Convert.ToInt32(KM.Common.DataFunctions.ExecuteNonQuery(cmd));
            if (rowCount > 0)
                return true;
            else
                return false;
        }
        //public static bool Insert_History_To_HistoryResponse(int historyID, int historyResponseID, KMPlatform.Object.ClientConnections client)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_HistoryToHistoryResponse_Save";
        //    cmd.Parameters.Add(new SqlParameter("@HistoryID", historyID));
        //    cmd.Parameters.Add(new SqlParameter("@HistoryResponseID", historyResponseID));
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);

        //    int rowCount = Convert.ToInt32(DataFunctions.ExecuteNonQuery(cmd));
        //    if (rowCount > 0)
        //        return true;
        //    else
        //        return false;
        //}

        //public static bool Insert_History_To_HistoryResponse_List(int historyID, List<Entity.HistoryResponseMap> list, KMPlatform.Object.ClientConnections client)
        //{

        //    StringBuilder sbXML = new StringBuilder();
        //    sbXML.AppendLine("<XML>");
        //    foreach (Entity.HistoryResponseMap x in list)
        //    {
        //        sbXML.AppendLine("<History>");

        //        sbXML.AppendLine("<HistoryID>" + historyID.ToString() + "</HistoryID>");
        //        sbXML.AppendLine("<HistoryResponseID>" + x.HistoryResponseMapID.ToString() + "</HistoryResponseID>");

        //        sbXML.AppendLine("</History>");
        //    }
        //    sbXML.AppendLine("</XML>");

        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_HistoryToHistoryResponse_BulkSave";
        //    cmd.Parameters.Add(new SqlParameter("@xml", sbXML.ToString()));
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);

        //    int rowCount = Convert.ToInt32(DataFunctions.ExecuteNonQuery(cmd));
        //    if (rowCount > 0)
        //        return true;
        //    else
        //        return false;
        //}

        public static bool Insert_History_To_HistoryMarketingMap_List(int historyID, List<Entity.HistoryMarketingMap> list, KMPlatform.Object.ClientConnections client)
        {
            StringBuilder sbXML = new StringBuilder();
            sbXML.AppendLine("<XML>");
            foreach (Entity.HistoryMarketingMap x in list)
            {
                sbXML.AppendLine("<History>");

                sbXML.AppendLine("<HistoryID>" + historyID.ToString() + "</HistoryID>");
                sbXML.AppendLine("<HistoryMarketingMapID>" + x.HistoryMarketingMapID.ToString() + "</HistoryMarketingMapID>");

                sbXML.AppendLine("</History>");
            }
            sbXML.AppendLine("</XML>");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_HistoryToHistoryMarketingMap_BulkSave";
            cmd.Parameters.Add(new SqlParameter("@xml", sbXML.ToString()));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            int rowCount = Convert.ToInt32(KM.Common.DataFunctions.ExecuteNonQuery(cmd));
            if (rowCount > 0)
                return true;
            else
                return false;
        }

        public static bool Insert_History_To_HistoryMarketingMap(int historyID, int historyMarketingMapID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_HistoryToHistoryMarketingMap_Save";
            cmd.Parameters.Add(new SqlParameter("@HistoryID", historyID));
            cmd.Parameters.Add(new SqlParameter("@HistoryMarketingMapID", historyMarketingMapID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            int rowCount = Convert.ToInt32(KM.Common.DataFunctions.ExecuteNonQuery(cmd));
            if (rowCount > 0)
                return true;
            else
                return false;
        }
    }
}
