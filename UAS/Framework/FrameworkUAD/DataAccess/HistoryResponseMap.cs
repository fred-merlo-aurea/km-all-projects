using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class HistoryResponseMap
    {
        public static List<Entity.HistoryResponseMap> Select(int pubSubscriptionID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.HistoryResponseMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_HistoryResponseMap_Select_PubSubscriptionID";
            cmd.Parameters.Add(new SqlParameter("@PubSubscriptionID", pubSubscriptionID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.HistoryResponseMap Get(SqlCommand cmd)
        {
            Entity.HistoryResponseMap retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.HistoryResponseMap();
                        DynamicBuilder<Entity.HistoryResponseMap> builder = DynamicBuilder<Entity.HistoryResponseMap>.CreateBuilder(rdr);
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
        private static List<Entity.HistoryResponseMap> GetList(SqlCommand cmd)
        {
            List<Entity.HistoryResponseMap> retList = new List<Entity.HistoryResponseMap>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.HistoryResponseMap retItem = new Entity.HistoryResponseMap();
                        DynamicBuilder<Entity.HistoryResponseMap> builder = DynamicBuilder<Entity.HistoryResponseMap>.CreateBuilder(rdr);
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
        public static int Save(Entity.HistoryResponseMap x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_HistoryResponseMap_Save";
            cmd.Parameters.Add(new SqlParameter("@PubSubscriptionDetailID", x.PubSubscriptionDetailID));
            cmd.Parameters.Add(new SqlParameter("@PubSubscriptionID", x.PubSubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@SubscriptionID", x.SubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@CodeSheetID", x.CodeSheetID));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@ResponseOther", x.ResponseOther));
            cmd.Parameters.Add(new SqlParameter("@HistorySubscriptionID", x.HistorySubscriptionID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static List<Entity.HistoryResponseMap> SaveBulkUpdate(List<Entity.HistoryResponseMap> list, KMPlatform.Object.ClientConnections client)
        {
            StringBuilder sbXML = new StringBuilder();
            sbXML.AppendLine("<XML>");
            foreach (Entity.HistoryResponseMap x in list)
            {
                sbXML.AppendLine("<HistoryResponseMap>");

                sbXML.AppendLine("<PubSubscriptionDetailID>" + x.PubSubscriptionDetailID.ToString() + "</PubSubscriptionDetailID>");
                sbXML.AppendLine("<PubSubscriptionID>" + x.PubSubscriptionID.ToString() + "</PubSubscriptionID>");
                sbXML.AppendLine("<SubscriptionID>" + x.SubscriptionID.ToString() + "</SubscriptionID>");
                sbXML.AppendLine("<CodeSheetID>" + x.CodeSheetID.ToString() + "</CodeSheetID>");
                sbXML.AppendLine("<DateCreated>" + x.DateCreated.ToString() + "</DateCreated>");
                sbXML.AppendLine("<CreatedByUserID>" + x.CreatedByUserID.ToString() + "</CreatedByUserID>");
                if (x.ResponseOther != null)
                    sbXML.AppendLine("<ResponseOther>" + x.ResponseOther.ToString() + "</ResponseOther>");
                else
                    sbXML.AppendLine("<ResponseOther></ResponseOther>");

                sbXML.AppendLine("<HistorySubscriptionID>" + x.HistorySubscriptionID.ToString() + "</HistorySubscriptionID>");

                sbXML.AppendLine("</HistoryResponseMap>");
            }
            sbXML.AppendLine("</XML>");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_HistoryResponseMap_BulkSave";
            cmd.Parameters.Add(new SqlParameter("@xml", sbXML.ToString()));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }

    }
}
