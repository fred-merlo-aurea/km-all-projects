using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class SubscriberOpenActivity
    {
        public SubscriberOpenActivity() { }
        #region Properties
        public int ClickActivityID { get; set; }
        public int PubSubscriptionID { get; set; }
        public int BlastID { get; set; }
        public DateTime ActivityDate { get; set; }
        public int SubscriptionID{ get; set; }
        #endregion

        #region Data
        public static List<SubscriberOpenActivity> GetByPubSubscriptionID(KMPlatform.Object.ClientConnections clientconnection, int PubSubscriptionID, int pubID)
        {
            List<SubscriberOpenActivity> retList = new List<SubscriberOpenActivity>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_SubscriberOpenActivity_Select_PubSubscriptionID_PubID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PubSubscriptionID ", PubSubscriptionID);
            cmd.Parameters.AddWithValue("@PubID ", pubID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<SubscriberOpenActivity> builder = DynamicBuilder<SubscriberOpenActivity>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    SubscriberOpenActivity x = builder.Build(rdr);
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
