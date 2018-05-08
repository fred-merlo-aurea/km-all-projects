using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class IssueCompDetail
    {
        private static Entity.IssueCompDetail Get(SqlCommand cmd)
        {
            Entity.IssueCompDetail retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.IssueCompDetail();
                        DynamicBuilder<Entity.IssueCompDetail> builder = DynamicBuilder<Entity.IssueCompDetail>.CreateBuilder(rdr);
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
        private static List<Entity.IssueCompDetail> GetList(SqlCommand cmd)
        {
            List<Entity.IssueCompDetail> retList = new List<Entity.IssueCompDetail>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.IssueCompDetail retItem = new Entity.IssueCompDetail();
                        DynamicBuilder<Entity.IssueCompDetail> builder = DynamicBuilder<Entity.IssueCompDetail>.CreateBuilder(rdr);
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
        private static List<int> GetIntList(SqlCommand cmd)
        {
            List<int> retList = new List<int>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
            {
                if (rdr != null)
                {
                    while (rdr.Read())
                    {
                        retList.Add(rdr.GetInt32(0));
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        public static List<int> GetByFilter(string xml, string adHocXml, int issueCompID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "rpt_GetSubscriptionIDs_Copies_From_Filter_XML";
            cmd.CommandText = "e_IssueCompDetail_ID_From_Filter_XML";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@FilterString", xml);
            cmd.Parameters.AddWithValue("@AdHocXML", adHocXml);
            cmd.Parameters.AddWithValue("@IssueCompID", issueCompID);

            return GetIntList(cmd);
        }

        public static bool ArchiveAll(int productID, int issueID, string compimb, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Issue_Archive_All_Comps";
            cmd.Parameters.Add(new SqlParameter("@ProductID", productID));
            cmd.Parameters.Add(new SqlParameter("@IssueID", issueID));
            cmd.Parameters.Add(new SqlParameter("@CompIMBSequences", compimb));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static List<Entity.IssueCompDetail> Select(int issueCompID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.IssueCompDetail> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueCompDetail_Select";
            cmd.Parameters.AddWithValue("@IssueCompID", issueCompID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }

        public static bool Clear(int issueCompID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueCompDetail_Clear";
            cmd.Parameters.AddWithValue("@IssueCompID", issueCompID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static int Save(Entity.IssueCompDetail x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueCompDetail_Save";
            cmd.Parameters.Add(new SqlParameter("@IssueCompDetailId", x.IssueCompDetailId));
            cmd.Parameters.Add(new SqlParameter("@IssueCompId", x.IssueCompID));
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
            cmd.Parameters.Add(new SqlParameter("@IsAddressValidated", x.IsAddressValidated));
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
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static DataTable Select_For_Export(int issueID, string cols, string subs, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueCompDetail_Select_For_Export";
            cmd.Parameters.AddWithValue("@IssueID", issueID);
            cmd.Parameters.AddWithValue("@Columns", cols);
            cmd.Parameters.AddWithValue("@Subs", subs);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
        }
    }
}
