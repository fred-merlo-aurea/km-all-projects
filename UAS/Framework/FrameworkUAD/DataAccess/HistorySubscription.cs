using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class HistorySubscription
    {
        public static List<Entity.HistorySubscription> Select(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_HistorySubscription_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }


        //public static bool UpdateIsUadUpdated(List<int> list, KMPlatform.Object.ClientConnections client)
        //{
        //    //bool done = true;

        //    StringBuilder sbXML = new StringBuilder();
        //    sbXML.AppendLine("<XML>");
        //    foreach (int x in list)
        //    {
        //        sbXML.AppendLine("<HistorySubscriptionID>");

        //        sbXML.AppendLine("<HistorySubscriptionID>" + x.ToString() + "</HistorySubscriptionID>");

        //        sbXML.AppendLine("</HistorySubscriptionID>");
        //    }
        //    sbXML.AppendLine("</XML>");

        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_HistorySubscription_BulkUpdate_IsUadUpdated";
        //    cmd.Parameters.Add(new SqlParameter("@xml", sbXML.ToString()));
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);

        //    return DataFunctions.ExecuteNonQuery(cmd);
        //}
        private static Entity.HistorySubscription Get(SqlCommand cmd)
        {
            Entity.HistorySubscription retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.HistorySubscription();
                        DynamicBuilder<Entity.HistorySubscription> builder = DynamicBuilder<Entity.HistorySubscription>.CreateBuilder(rdr);
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
        private static List<Entity.HistorySubscription> GetList(SqlCommand cmd)
        {
            List<Entity.HistorySubscription> retList = new List<Entity.HistorySubscription>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.HistorySubscription retItem = new Entity.HistorySubscription();
                        DynamicBuilder<Entity.HistorySubscription> builder = DynamicBuilder<Entity.HistorySubscription>.CreateBuilder(rdr);
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

        public static int Save(Entity.HistorySubscription x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_HistorySubscription_Save";
            cmd.Parameters.Add(new SqlParameter("@HistorySubscriptionID", x.HistorySubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@PubSubscriptionID", x.PubSubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@SubscriptionID", x.SubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@PubID", (object)x.PubID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@demo7", (object)x.Demo7 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Qualificationdate", (object)x.QualificationDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PubQSourceID", (object)x.PubQSourceID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PubCategoryID", (object)x.PubCategoryID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PubTransactionID", (object)x.PubTransactionID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Email", (object)x.Email ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EmailStatusID", (object)x.EmailStatusID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@StatusUpdatedDate", (object)x.StatusUpdatedDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@StatusUpdatedReason", (object)x.StatusUpdatedReason ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsComp", (object)x.IsComp ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SubscriptionStatusID", (object)x.SubscriptionStatusID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ExternalKeyID", (object)x.ExternalKeyID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FirstName", (object)x.FirstName ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@LastName", (object)x.LastName ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Company", (object)x.Company ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Title", (object)x.Title ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Occupation", (object)x.Occupation ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AddressTypeID", (object)x.AddressTypeID ?? DBNull.Value));
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
            cmd.Parameters.Add(new SqlParameter("@AddressValidationDate", (object)x.AddressValidationDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AddressValidationSource", (object)x.AddressValidationSource ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AddressValidationMessage", (object)x.AddressValidationMessage ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Phone", (object)x.Phone ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Fax", (object)x.Fax ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Mobile", (object)x.Mobile ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Website", (object)x.Website ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Birthdate", (object)x.Birthdate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Age", (object)x.Age ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Income", (object)x.Income ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Gender", (object)x.Gender ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PhoneExt", (object)x.PhoneExt ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsInActiveWaveMailing", x.IsInActiveWaveMailing));
            cmd.Parameters.Add(new SqlParameter("@WaveMailingID", x.WaveMailingID));
            cmd.Parameters.Add(new SqlParameter("@AddressTypeCodeId", (object)x.AddressTypeCodeId ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AddressLastUpdatedDate", (object)x.AddressLastUpdatedDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AddressUpdatedSourceTypeCodeId", (object)x.AddressUpdatedSourceTypeCodeId ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IGrp_No", (object)x.IGrp_No ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SFRecordIdentifier", (object)x.SFRecordIdentifier ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SubSrcID", (object)x.SubSrcID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Par3CID", (object)x.Par3CID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SequenceID", (object)x.SequenceID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AddRemoveID", (object)x.AddRemoveID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Copies", (object)x.Copies ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@GraceIssues", (object)x.GraceIssues ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@IsAddressValidated", x.IsAddressValidated));
            cmd.Parameters.Add(new SqlParameter("@IsPaid", x.IsPaid));
            cmd.Parameters.Add(new SqlParameter("@IsSubscribed", x.IsSubscribed));
            cmd.Parameters.Add(new SqlParameter("@MemberGroup", (object)x.MemberGroup ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@OnBehalfOf", (object)x.OnBehalfOf ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@OrigsSrc", (object)x.OrigsSrc ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Status", (object)x.Status ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Verified", (object)x.Verified ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SubscriberSourceCode", (object)x.SubscriberSourceCode ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SubGenSubscriberID", (object)x.SubGenSubscriberID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@MailPermission", (object)x.MailPermission ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FaxPermission", (object)x.FaxPermission ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PhonePermission", (object)x.PhonePermission ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@OtherProductsPermission", (object)x.OtherProductsPermission ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EmailRenewPermission", (object)x.EmailRenewPermission ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ThirdPartyPermission", (object)x.ThirdPartyPermission ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@TextPermission", (object)x.TextPermission ?? DBNull.Value));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
    }
}
