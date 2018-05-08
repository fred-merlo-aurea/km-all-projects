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
    public class SubscriberPubs
    {
        public SubscriberPubs()
        {
        }
        #region Properties
        public int SubscriptionID { get; set; }
        public string PubCode { get; set; }
        public string pubname { get; set; }
        public string ColumnReference { get; set; }
        public string PubTypeDisplayname { get; set; }
        public int PubSubscriptionID { get; set; }
        public int PubID { get; set; }
        public int PubTypeID { get; set; }
        public int SortOrder { get; set; }
        public int EmailStatusID { get; set; }
        public string Email { get; set; }
        public DateTime Qualificationdate { get; set; }
        public DateTime StatusUpdatedDate { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string RegionCode { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public int? CountryID { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public bool IsCirc { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        #endregion

        #region Data
        public static List<SubscriberPubs> GetSubscriberPubsForExport(KMPlatform.Object.ClientConnections clientconnection, int subscriptionID, int brandID)
        {
            List<SubscriberPubs> retList = new List<SubscriberPubs>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_GetSubscriberPubsForExport", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SubscriptionID", subscriptionID);
            cmd.Parameters.AddWithValue("@BrandID", brandID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<SubscriberPubs> builder = DynamicBuilder<SubscriberPubs>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    SubscriberPubs x = builder.Build(rdr);
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

        public static List<SubscriberPubs> GetSubscriberPubs(KMPlatform.Object.ClientConnections clientconnection, int subscriptionID, int brandID)
        {
            List<SubscriberPubs> retList = new List<SubscriberPubs>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_GetSubscriberPubs", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SubscriptionID", subscriptionID);
            cmd.Parameters.AddWithValue("@BrandID", brandID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<SubscriberPubs> builder = DynamicBuilder<SubscriberPubs>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    SubscriberPubs x = builder.Build(rdr);
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