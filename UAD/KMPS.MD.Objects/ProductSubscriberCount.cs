using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    public class ProductSubscriberCount
    {
        public ProductSubscriberCount() { }
        #region Properties
        public int SubscriptionID { get; set; }
        public string PubCounts { get; set; }
        #endregion

        #region Data
        public static List<ProductSubscriberCount> Get(KMPlatform.Object.ClientConnections clientconnection, int PubTypeID)
        {
            List<ProductSubscriberCount> retList = new List<ProductSubscriberCount>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("rptProductSubscriberCounts", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PubTypeID", PubTypeID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<ProductSubscriberCount> builder = DynamicBuilder<ProductSubscriberCount>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    ProductSubscriberCount x = builder.Build(rdr);
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