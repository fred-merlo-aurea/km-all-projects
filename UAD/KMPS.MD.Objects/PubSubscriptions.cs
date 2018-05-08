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
    public class PubSubscriptions
    {
        public PubSubscriptions() { }
        #region Properties
        public int PubSubscriptionID { get; set; }
        public int SubscriptionID { get; set; }
        public int PubID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string Address1 { get; set; } 
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string RegionCode { get; set; }
        public string ZipCode { get; set; }
        public string Plus4 { get; set; }
        public string ForZip { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public int? CountryID { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public DateTime? QualificationDate { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? PubCategoryID { get; set; }
        public int? PubTransactionID { get; set; }
        public int? PubQSourceID { get; set; }
        public int? UpdatedByUserID { get; set; }
        #endregion

        #region Data

        public static PubSubscriptions GetByPubIDSubscriptionID(KMPlatform.Object.ClientConnections clientconnection, int pubID, int subscriptionID)
        {
            PubSubscriptions retItem = new PubSubscriptions();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_PubSubscriptions_Select_PubID_SubscriptionID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SubscriptionID", subscriptionID);
            cmd.Parameters.AddWithValue("@PubID", pubID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<PubSubscriptions> builder = DynamicBuilder<PubSubscriptions>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    retItem = builder.Build(rdr);
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
            return retItem;
        }

        public static int GetUniqueProductEmailsCount(KMPlatform.Object.ClientConnections clientconnection, StringBuilder Queries, int pubID)
        {
            SqlCommand cmd = new SqlCommand("sp_GetSubscriberUniqueProductEmailsCount");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Queries", Queries.ToString()));
            cmd.Parameters.Add(new SqlParameter("@PubID", pubID));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }
        #endregion

        #region CRUD
        public static void UpdateEmailStatus(KMPlatform.Object.ClientConnections clientconnection, string EmailStatusID, int PubSubscriptionID, string Email)
        {
            SqlCommand cmd = new SqlCommand("sp_PubSubscriptions_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PubSubscriptionID", PubSubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@EmailStatusID", EmailStatusID));
            cmd.Parameters.Add(new SqlParameter("@Email", Email));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }

        public static void UpdateAddress(KMPlatform.Object.ClientConnections clientconnection, PubSubscriptions ps, string emailStatusID)
        {
            SqlCommand cmd = new SqlCommand("e_ProductSubscription_Update_Address");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PubSubscriptionID", ps.PubSubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@Address1", (object)ps.Address1 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Address2", (object)ps.Address2 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Address3", (object)ps.Address3 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@City", (object)ps.City ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RegionCode", (object)ps.RegionCode ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ZipCode", (object)ps.ZipCode ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CountryID", (object)ps.CountryID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Phone", (object)ps.Phone ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Fax", (object)ps.Fax ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Email", (object)ps.Email ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EmailStatusID", (object)emailStatusID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", ps.UpdatedByUserID));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }

        #endregion
    }

}