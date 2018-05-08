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
    public class SubscriberDimension
    {
        public SubscriberDimension()
        {
        }
        #region Properties
        public int SubscriptionID { get; set; }
        public string DisplayName { get; set; }
        public string MasterValue { get; set; }
        public string MasterDesc { get; set; }
        #endregion

        #region Data
        public static List<SubscriberDimension> GetSubscriberDimensionForExport(KMPlatform.Object.ClientConnections clientconnection, int subscriptionID, int brandID)
        {
            List<SubscriberDimension> retList = new List<SubscriberDimension>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_GetSubscriberDimensionForExport", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SubscriptionID", subscriptionID);
            cmd.Parameters.AddWithValue("@BrandID", brandID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<SubscriberDimension> builder = DynamicBuilder<SubscriberDimension>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    SubscriberDimension x = builder.Build(rdr);
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

        public static List<SubscriberDimension> GetSubscriberDimension(KMPlatform.Object.ClientConnections clientconnection, int subscriptionID, int brandID)
        {
            List<SubscriberDimension> retList = new List<SubscriberDimension>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_GetSubscriberDimension", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SubscriptionID", subscriptionID);
            cmd.Parameters.AddWithValue("@BrandID", brandID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<SubscriberDimension> builder = DynamicBuilder<SubscriberDimension>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    SubscriberDimension x = builder.Build(rdr);
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
