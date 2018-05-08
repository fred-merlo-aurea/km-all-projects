using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using KM.Common;

namespace KMPS.MD.Objects
{
    public class SubscriberAdhoc
    {
        public SubscriberAdhoc() { }

        #region Properties
        public string AdhocField { get; set; }
        public string AdhocValue { get; set; }
        #endregion

        #region Data
        public static List<SubscriberAdhoc> GetForRecordView(KMPlatform.Object.ClientConnections clientconnection, int subscriptionID)
        {
            List<SubscriberAdhoc> retList = new List<SubscriberAdhoc>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_SubscriberExtenstion_getbySubscriberID_forRecordView", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@subscriptionID", subscriptionID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<SubscriberAdhoc> builder = DynamicBuilder<SubscriberAdhoc>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    SubscriberAdhoc x = builder.Build(rdr);
                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return retList;
        }

        public static List<SubscriberAdhoc> Get(KMPlatform.Object.ClientConnections clientconnection, int subscriptionID)
        {
            List<SubscriberAdhoc> retList = new List<SubscriberAdhoc>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_SubscriptionsExtension_Select_SubscriptionID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@subscriptionID", subscriptionID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<SubscriberAdhoc> builder = DynamicBuilder<SubscriberAdhoc>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    SubscriberAdhoc x = builder.Build(rdr);
                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return retList;
        }

        #endregion
    }
}

