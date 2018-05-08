using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    [Serializable]
    public class PaidBillTo
    {
        public static Entity.PaidBillTo SelectSubscription(int subscriptionID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PaidBillTo_Select_SubscriptionID";
            cmd.Parameters.AddWithValue("@PubSubscriptionID", subscriptionID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Get(cmd);
        }
        public static Entity.PaidBillTo Select(int subscriptionPaidID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PaidBillTo_Select_SubscriptionPaidID";
            cmd.Parameters.AddWithValue("@SubscriptionPaidID", subscriptionPaidID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Get(cmd);
        }
        private static Entity.PaidBillTo Get(SqlCommand cmd)
        {
            Entity.PaidBillTo retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.PaidBillTo();
                        DynamicBuilder<Entity.PaidBillTo> builder = DynamicBuilder<Entity.PaidBillTo>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
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
            return retItem;
        }
        private static List<Entity.PaidBillTo> GetList(SqlCommand cmd)
        {
            List<Entity.PaidBillTo> retList = new List<Entity.PaidBillTo>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.PaidBillTo retItem = new Entity.PaidBillTo();
                        DynamicBuilder<Entity.PaidBillTo> builder = DynamicBuilder<Entity.PaidBillTo>.CreateBuilder(rdr);
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

        public static int Save(Entity.PaidBillTo x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PaidBillTo_Save";
            cmd.Parameters.Add(new SqlParameter("@PaidBillToID", x.PaidBillToID));
            cmd.Parameters.Add(new SqlParameter("@SubscriptionPaidID", x.SubscriptionPaidID));
            cmd.Parameters.Add(new SqlParameter("@PubSubscriptionID", x.PubSubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@FirstName", (object)x.FirstName ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@LastName", (object)x.LastName ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Company", (object)x.Company ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Title", (object)x.Title ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AddressTypeID", (object)x.AddressTypeId ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Address1", (object)x.Address1 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Address2", (object)x.Address2 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Address3", (object)x.Address3 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@City", (object)x.City ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RegionCode", (object)x.RegionCode ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RegionID", (object)x.RegionID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ZipCode", (object)x.ZipCode ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Plus4", (object)x.Plus4 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CarrierRoute", (object)x.CarrierRoute ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@County", (object)x.County ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Country", (object)x.Country ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CountryID", (object)x.CountryID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Latitude", (object)x.Latitude ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Longitude", (object)x.Longitude ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsAddressValidated", x.IsAddressValidated));
            cmd.Parameters.Add(new SqlParameter("@AddressValidationDate", (object)x.AddressValidationDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AddressValidationSource", (object)x.AddressValidationSource ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AddressValidationMessage", (object)x.AddressValidationMessage ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Email", (object)x.Email ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Phone", (object)x.Phone ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PhoneExt", (object)x.PhoneExt ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Fax", (object)x.Fax ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Mobile", (object)x.Mobile ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Website", (object)x.Website ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
    }
}
