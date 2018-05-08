using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FrameworkUAD.DataAccess
{
    public class PubSubscriptionsExtension
    {
        public static DataTable GetAdHoc(KMPlatform.Object.ClientConnections client, int pubSubscriptionID, int pubID)
        {
            DataTable retItem = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "o_GetPubSubscription_Adhocs_PubSubscriptionID";
            cmd.Parameters.AddWithValue("@PubSubscriptionID", pubSubscriptionID);
            cmd.Parameters.AddWithValue("@PubID", pubID);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }

        public static List<string> GetAdHocs(KMPlatform.Object.ClientConnections client, int pubID)
        {
            List<string> retItem = new List<string>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "o_GetPubSubscription_Adhocs";
            cmd.Parameters.AddWithValue("@PubID", pubID);

            //retItem = FrameworkUAS.DataAccess.DataFunctions
            retItem = GetList(cmd);
            return retItem;
        }

        private static List<string> GetList(SqlCommand cmd)
        {
            List<string> retList = new List<string>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        while (rdr.Read())
                        {
                            retList.Add(rdr.GetString(0));
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

        public static bool Save(KMPlatform.Object.ClientConnections client, int pubSubscriptionID, int pubID, List<FrameworkUAD.Object.PubSubscriptionAdHoc> adhocs)
        {
            string xml = "<XML>";
            foreach (FrameworkUAD.Object.PubSubscriptionAdHoc ah in adhocs)
            {
                xml += "<AdHoc><Name>" + ah.AdHocField + "</Name><Value>" + ah.Value + "</Value></AdHoc>";
            }
            xml += "</XML>";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_SavePubSubscription_AdHocs";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@PubSubscriptionID", pubSubscriptionID);
            cmd.Parameters.AddWithValue("@PubID", pubID);
            cmd.Parameters.AddWithValue("@AdHocs", xml);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
    }
}
