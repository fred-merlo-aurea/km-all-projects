using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    public class SubscriberActivity
    {
        public SubscriberActivity() { }
        #region Properties
        public string Activity { get; set; }
        public DateTime ActivityDate { get; set; }
        public int? BlastID { get; set; }
        public string link { get; set; }
        public string linkAlias { get; set; }
        public string Email { get; set; }
        public string EmailSubject { get; set; }
        public DateTime? SendTime { get; set; }
        #endregion

        #region Data
        public static List<SubscriberActivity> Get(KMPlatform.Object.ClientConnections clientconnection, int SubscriptionID, int BrandID)
        {
            List<SubscriberActivity> retList = new List<SubscriberActivity>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_SubscriberActivity", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SubscriptionID ", SubscriptionID);
            cmd.Parameters.AddWithValue("@BrandID ", BrandID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<SubscriberActivity> builder = DynamicBuilder<SubscriberActivity>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    SubscriberActivity x = builder.Build(rdr);
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

        public static List<SubscriberActivity> GetOpenActivity(KMPlatform.Object.ClientConnections clientconnection, int SubscriptionID, int BrandID)
        {
            List<SubscriberActivity> retList = Get(clientconnection, SubscriptionID, BrandID).FindAll(x => x.Activity.ToUpper() == "OPEN");
            return retList;
        }

        public static List<SubscriberActivity> GetClickActivity(KMPlatform.Object.ClientConnections clientconnection, int SubscriptionID, int BrandID)
        {
            List<SubscriberActivity> retList = Get(clientconnection, SubscriptionID, BrandID).FindAll(x => x.Activity.ToUpper() == "CLICK");
            return retList;
        }

        #endregion
    }
}