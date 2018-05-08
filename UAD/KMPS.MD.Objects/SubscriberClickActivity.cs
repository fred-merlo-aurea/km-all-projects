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
    public class SubscriberClickActivity
    {
        public SubscriberClickActivity() { }
        #region Properties
        public int ClickActivityID { get; set; }
        public int PubSubscriptionID { get; set; }
        public int BlastID { get; set; }
        public string link { get; set; }
        public string linkAlias { get; set; }
        public string LinkSource { get; set; }
        public string LinkType { get; set; }
        public DateTime ActivityDate { get; set; }
        public int SubscriptionID{ get; set; }
        #endregion

        #region Data
        public static List<SubscriberClickActivity> GetByPubSubscriptionID(KMPlatform.Object.ClientConnections clientconnection, int pubSubscriptionID, int pubID)
        {
            List<SubscriberClickActivity> retList = new List<SubscriberClickActivity>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_SubscriberClickActivity_Select_PubSubscriptionID_PubID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PubSubscriptionID ", pubSubscriptionID);
            cmd.Parameters.AddWithValue("@PubID ", pubID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<SubscriberClickActivity> builder = DynamicBuilder<SubscriberClickActivity>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    SubscriberClickActivity x = builder.Build(rdr);
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
