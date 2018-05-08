using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class SubscriptionSearchResult
    {
        public static List<Object.SubscriptionSearchResult> Select(int subscriberID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_SubscriptionSearchResult_Select_SubscriberID";
            cmd.Parameters.AddWithValue("@SubscriberID", subscriberID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Object.SubscriptionSearchResult> SelectMultiple(List<int> list, KMPlatform.Object.ClientConnections client)
        {


            StringBuilder sbXML = new StringBuilder();
            sbXML.AppendLine("<XML>");
            foreach (int x in list)
            {
                sbXML.AppendLine("<SubscriptionSearchResult>");
                sbXML.AppendLine("<SubscriptionID>" + x.ToString() + "</SubscriptionID>");
                sbXML.AppendLine("</SubscriptionSearchResult>");
            }
            sbXML.AppendLine("</XML>");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_SubscriptionSearchResult_Select_SubscriberID_Multiple";
            cmd.Parameters.Add(new SqlParameter("@xml", sbXML.ToString()));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            //int rowCount = Convert.ToInt32(DataFunctions.ExecuteNonQuery(cmd));

            return GetList(cmd);
        }
        private static List<Object.SubscriptionSearchResult> GetList(SqlCommand cmd)
        {
            List<Object.SubscriptionSearchResult> retList = new List<Object.SubscriptionSearchResult>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Object.SubscriptionSearchResult retItem = new Object.SubscriptionSearchResult();
                        DynamicBuilder<Object.SubscriptionSearchResult> builder = DynamicBuilder<Object.SubscriptionSearchResult>.CreateBuilder(rdr);
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
    }
}
