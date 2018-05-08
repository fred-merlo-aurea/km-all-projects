using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class WaveMailingDetail
    {
        public static List<Entity.WaveMailingDetail> SelectIssue(int issueID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.WaveMailingDetail> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_WaveMailingDetail_Select_IssueID";
            cmd.Parameters.Add(new SqlParameter("@IssueID", issueID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static bool UpdateOriginalSubInfo(int productID, int userID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_WaveMailingDetail_Update_Original_Records";
            cmd.Parameters.Add(new SqlParameter("@ProductID", productID));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static int Save(Entity.WaveMailingDetail x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_WaveMailingDetail_Save";
            cmd.Parameters.Add(new SqlParameter("@WaveMailingDetailID", x.WaveMailingDetailID));
            cmd.Parameters.Add(new SqlParameter("@WaveMailingID", x.WaveMailingID));
            cmd.Parameters.Add(new SqlParameter("@PubSubscriptionID", x.PubSubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@SubscriptionID", x.SubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@Demo7", x.Demo7));
            cmd.Parameters.Add(new SqlParameter("@PubCategoryID", x.PubCategoryID));
            cmd.Parameters.Add(new SqlParameter("@PubTransactionID", x.PubTransactionID));
            cmd.Parameters.Add(new SqlParameter("@IsSubscribed", ((object)x.IsSubscribed ?? DBNull.Value)));
            cmd.Parameters.Add(new SqlParameter("@IsPaid", ((object)x.IsPaid ?? DBNull.Value)));
            cmd.Parameters.Add(new SqlParameter("@SubscriptionStatusID", x.SubscriptionStatusID));
            cmd.Parameters.Add(new SqlParameter("@Copies", (object)x.Copies ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FirstName", (object)x.FirstName ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@LastName", (object)x.LastName ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Company", (object)x.Company ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Title", (object)x.Title ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AddressTypeID", (object)x.AddressTypeID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Address1", (object)x.Address1 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Address2", (object)x.Address2 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Address3", (object)x.Address3 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@City", (object)x.City ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RegionCode", (object)x.RegionCode ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RegionID", (object)x.RegionID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ZipCode", (object)x.ZipCode ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Plus4", (object)x.Plus4 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@County", (object)x.County ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Country", (object)x.Country ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CountryID", (object)x.CountryID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Email", (object)x.Email ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Phone", (object)x.Phone ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PhoneExt", (object)x.PhoneExt ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Fax", (object)x.Fax ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Mobile", (object)x.Mobile ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        private static List<Entity.WaveMailingDetail> GetList(SqlCommand cmd)
        {
            List<Entity.WaveMailingDetail> retList = new List<Entity.WaveMailingDetail>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.WaveMailingDetail retItem = new Entity.WaveMailingDetail();
                        DynamicBuilder<Entity.WaveMailingDetail> builder = DynamicBuilder<Entity.WaveMailingDetail>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            if (retItem != null)
                            {
                                retList.Add(retItem);
                            }
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }
    }
}
