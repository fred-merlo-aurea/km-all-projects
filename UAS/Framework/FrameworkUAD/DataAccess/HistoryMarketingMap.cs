using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class HistoryMarketingMap
    {
        public static List<Entity.HistoryMarketingMap> Select(int PubSubscriptionID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.HistoryMarketingMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_HistoryMarketingMap_Select_PubSubscriptionID";
            cmd.Parameters.Add(new SqlParameter("@PubSubscriptionID", PubSubscriptionID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.HistoryMarketingMap Get(SqlCommand cmd)
        {
            Entity.HistoryMarketingMap retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.HistoryMarketingMap();
                        DynamicBuilder<Entity.HistoryMarketingMap> builder = DynamicBuilder<Entity.HistoryMarketingMap>.CreateBuilder(rdr);
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
        private static List<Entity.HistoryMarketingMap> GetList(SqlCommand cmd)
        {
            List<Entity.HistoryMarketingMap> retList = new List<Entity.HistoryMarketingMap>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.HistoryMarketingMap retItem = new Entity.HistoryMarketingMap();
                        DynamicBuilder<Entity.HistoryMarketingMap> builder = DynamicBuilder<Entity.HistoryMarketingMap>.CreateBuilder(rdr);
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
        public static int Save(Entity.HistoryMarketingMap x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_HistoryMarketingMap_Save";
            cmd.Parameters.Add(new SqlParameter("@HistoryMarketingMapID", x.HistoryMarketingMapID));
            cmd.Parameters.Add(new SqlParameter("@MarketingID", x.MarketingID));
            cmd.Parameters.Add(new SqlParameter("@PubSubscriptionID", x.PubSubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@PublicationID", x.PublicationID));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static List<Entity.HistoryMarketingMap> SaveBulkUpdate(List<Entity.HistoryMarketingMap> list, KMPlatform.Object.ClientConnections client)
        {
            StringBuilder sbXML = new StringBuilder();
            sbXML.AppendLine("<XML>");
            foreach (Entity.HistoryMarketingMap x in list)
            {
                sbXML.AppendLine("<HistoryMarketingMap>");

                sbXML.AppendLine("<MarketingID>" + x.MarketingID.ToString() + "</MarketingID>");
                sbXML.AppendLine("<PubSubscriptionID>" + x.PubSubscriptionID.ToString() + "</PubSubscriptionID>");
                sbXML.AppendLine("<PublicationID>" + x.PublicationID.ToString() + "</PublicationID>");
                sbXML.AppendLine("<IsActive>" + x.IsActive.ToString() + "</IsActive>");
                sbXML.AppendLine("<DateCreated>" + x.DateCreated.ToString() + "</DateCreated>");
                sbXML.AppendLine("<CreatedByUserID>" + x.CreatedByUserID.ToString() + "</CreatedByUserID>");

                sbXML.AppendLine("</HistoryMarketingMap>");
            }
            sbXML.AppendLine("</XML>");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_HistoryMarketingMap_BulkSave";
            cmd.Parameters.Add(new SqlParameter("@xml", sbXML.ToString()));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
    }
}
