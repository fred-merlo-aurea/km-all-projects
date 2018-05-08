using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    public class SubscriberVisitActivity
    {
        public SubscriberVisitActivity() { }
        #region Properties
        public int VisitActivityID { get; set; }
        public int SubscriptionID { get; set; }
        public int DomainTrackingID { get; set; }
        public string DomainName { get; set; }
        public string URL { get; set; }
        public DateTime ActivityDate { get; set; }
        #endregion

        #region Data
        public static List<SubscriberVisitActivity> Get(KMPlatform.Object.ClientConnections clientconnection, int SubscriptionID)
        {
            List<SubscriberVisitActivity> retList = new List<SubscriberVisitActivity>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_SubscriberVisitActivity_Select_BySubscriptionID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SubscriptionID ", SubscriptionID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<SubscriberVisitActivity> builder = DynamicBuilder<SubscriberVisitActivity>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    SubscriberVisitActivity x = builder.Build(rdr);
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