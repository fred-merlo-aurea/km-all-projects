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
    public class PubSubscriptionsDimension
    {
        public PubSubscriptionsDimension()
        {
        }
        #region Properties
        public int ResponseGroupID { get; set; }
        public string ResponseGroupName { get; set; }
        public string ResponseDesc { get; set; }
        #endregion

        #region Data
        public static List<PubSubscriptionsDimension> GetPubSubscriptionsDimension(KMPlatform.Object.ClientConnections clientconnection, int subscriptionID, int pubID)
        {
            List<PubSubscriptionsDimension> retList = new List<PubSubscriptionsDimension>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_GetPubSubscriptionsDimension", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SubscriptionID", subscriptionID);
            cmd.Parameters.AddWithValue("@pubID", pubID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<PubSubscriptionsDimension> builder = DynamicBuilder<PubSubscriptionsDimension>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    PubSubscriptionsDimension x = builder.Build(rdr);
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
