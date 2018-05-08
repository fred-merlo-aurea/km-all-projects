using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    [Serializable]
    public class MarketingMap
    {
        public static List<Entity.MarketingMap> SelectSubscriber(int PubSubscriptionID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.MarketingMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MarketingMap_Select_PubSubscriptionID";
            cmd.Parameters.AddWithValue("@PubSubscriptionID", PubSubscriptionID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.MarketingMap> SelectPublication(int publicationID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.MarketingMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MarketingMap_Select_PublicationID";
            cmd.Parameters.AddWithValue("@PublicationID", publicationID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.MarketingMap Select(int marketingID, int PubSubscriptionID, int publicationID, KMPlatform.Object.ClientConnections client)
        {
            Entity.MarketingMap retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MarketingMap_Select_MarketingID_SubscriberID_PublicationID";
            cmd.Parameters.AddWithValue("@MarketingID", marketingID);
            cmd.Parameters.AddWithValue("@PubSubscriptionID", PubSubscriptionID);
            cmd.Parameters.AddWithValue("@PublicationID", publicationID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = Get(cmd);
            return retItem;
        }
        public static bool SaveBulkUpdate(List<Entity.MarketingMap> list, KMPlatform.Object.ClientConnections client)
        {
            bool done = true;

            StringBuilder sbXML = new StringBuilder();
            sbXML.AppendLine("<XML>");
            foreach (Entity.MarketingMap x in list)
            {
                sbXML.AppendLine("<MarketingMap>");

                sbXML.AppendLine("<MarketingID>" + x.MarketingID.ToString() + "</MarketingID>");
                sbXML.AppendLine("<PubSubscriptionID>" + x.PubSubscriptionID.ToString() + "</PubSubscriptionID>");
                sbXML.AppendLine("<PublicationID>" + x.PublicationID.ToString() + "</PublicationID>");
                sbXML.AppendLine("<IsActive>" + x.IsActive.ToString() + "</IsActive>");
                sbXML.AppendLine("<DateCreated>" + x.DateCreated.ToString() + "</DateCreated>");
                sbXML.AppendLine("<DateUpdated>" + x.DateUpdated.ToString() + "</DateUpdated>");
                sbXML.AppendLine("<CreatedByUserID>" + x.CreatedByUserID.ToString() + "</CreatedByUserID>");
                sbXML.AppendLine("<UpdatedByUserID>" + x.UpdatedByUserID.ToString() + "</UpdatedByUserID>");

                sbXML.AppendLine("</MarketingMap>");
            }
            sbXML.AppendLine("</XML>");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MarketingMap_BulkUpdate";
            cmd.Parameters.Add(new SqlParameter("@xml", sbXML.ToString()));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            int rowCount = Convert.ToInt32(KM.Common.DataFunctions.ExecuteNonQuery(cmd));
            if (rowCount > 0)
                done = true;
            else
                done = false;

            return done;
        }
        private static Entity.MarketingMap Get(SqlCommand cmd)
        {
            Entity.MarketingMap retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.MarketingMap();
                        DynamicBuilder<Entity.MarketingMap> builder = DynamicBuilder<Entity.MarketingMap>.CreateBuilder(rdr);
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
        private static List<Entity.MarketingMap> GetList(SqlCommand cmd)
        {
            List<Entity.MarketingMap> retList = new List<Entity.MarketingMap>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.MarketingMap retItem = new Entity.MarketingMap();
                        DynamicBuilder<Entity.MarketingMap> builder = DynamicBuilder<Entity.MarketingMap>.CreateBuilder(rdr);
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

        public static bool Save(Entity.MarketingMap x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MarketingMap_Save";
            cmd.Parameters.Add(new SqlParameter("@MarketingID", x.MarketingID));
            cmd.Parameters.Add(new SqlParameter("@PubSubscriptionID", x.PubSubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@PublicationID", x.PublicationID));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            int rowCount = Convert.ToInt32(KM.Common.DataFunctions.ExecuteNonQuery(cmd));
            if (rowCount > 0)
                return true;
            else
                return false;
        }
    }
}
