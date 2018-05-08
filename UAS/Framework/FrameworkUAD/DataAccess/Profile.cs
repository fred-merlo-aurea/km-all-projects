using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    [Serializable]
    public class Profile
    {
        public static List<Entity.Profile> Search(string searchValue, List<Entity.Profile> searchList)
        {
            searchValue = searchValue.ToLower();
            List<Entity.Profile> matchList = new List<Entity.Profile>();

            matchList.AddRange(searchList.FindAll(x => x.FirstName != null && x.FirstName.ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.LastName != null && x.LastName.ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.Company != null && x.Company.ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.Title != null && x.Title.ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.Address1 != null && x.Address1.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.Address2 != null && x.Address2.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.City != null && x.City.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.RegionCode != null && x.RegionCode.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.ZipCode != null && x.ZipCode.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.Plus4 != null && x.Plus4.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.CarrierRoute != null && x.CarrierRoute.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.County != null && x.County.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.Country != null && x.Country.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.IsAddressValidated.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.AddressValidationDate != null && x.AddressValidationDate.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.AddressValidationSource != null && x.AddressValidationSource.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.AddressValidationMessage != null && x.AddressValidationMessage.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.Email != null && x.Email.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.Phone != null && x.Phone.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.Fax != null && x.Fax.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.Mobile != null && x.Mobile.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.Website != null && x.Website.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.DateCreated != null && x.DateCreated.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.DateUpdated != null && x.DateUpdated.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.CreatedByUserID.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.UpdatedByUserID != null && x.UpdatedByUserID.ToString().ToLower().Contains(searchValue)));
            return matchList.Distinct().ToList();
        }
        public static List<Entity.Profile> Search(string searchValue, string searchFields, string orderBy, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Profile_Search";
            cmd.Parameters.AddWithValue("@Search", searchValue);
            cmd.Parameters.AddWithValue("@SearchFields", searchFields);
            cmd.Parameters.AddWithValue("@OrderBy", orderBy);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static Entity.Profile Select(int profileID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Profile_Select_ProfileID";
            cmd.Parameters.AddWithValue("@ProfileID", profileID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Get(cmd);
        }
        public static List<Entity.Profile> SelectPublisher(int publisherID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscriber_Select_PublisherID";
            cmd.Parameters.AddWithValue("@PublisherID", publisherID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.Profile> SelectProspect(int publicationID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscriber_Select_Prospect_PublicationID";
            cmd.Parameters.AddWithValue("@PublicationID", publicationID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.Profile> SelectPublicationSubscribed(int publicationID, bool isSubscribed, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscriber_Select_PublicationID_IsSubscribed";
            cmd.Parameters.AddWithValue("@PublicationID", publicationID);
            cmd.Parameters.AddWithValue("@IsSubscribed", isSubscribed);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.Profile> SelectPublicationProspect(int publicationID, bool isProspect, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscriber_Select_PublicationID_IsProspect";
            cmd.Parameters.AddWithValue("@PublicationID", publicationID);
            cmd.Parameters.AddWithValue("@IsProspect", isProspect);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.Profile> SelectPublication(int publicationID, bool isSubscribed, bool isProspect, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscriber_Select_PublicationID_IsSubscribed_IsProspect";
            cmd.Parameters.AddWithValue("@PublicationID", publicationID);
            cmd.Parameters.AddWithValue("@IsSubscribed", isSubscribed);
            cmd.Parameters.AddWithValue("@IsProspect", isProspect);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.Profile> SelectPublication(int publicationID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscriber_SelectPublication";
            cmd.Parameters.AddWithValue("@PublicationID", publicationID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        private static Entity.Profile Get(SqlCommand cmd)
        {
            Entity.Profile retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Profile();
                        DynamicBuilder<Entity.Profile> builder = DynamicBuilder<Entity.Profile>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            //retItem.TempSubscriptions = new List<TempSubscription>();
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
        private static List<Entity.Profile> GetList(SqlCommand cmd)
        {
            List<Entity.Profile> retList = new List<Entity.Profile>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.Profile retItem = new Entity.Profile();
                        DynamicBuilder<Entity.Profile> builder = DynamicBuilder<Entity.Profile>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            //retItem.TempSubscriptions = new List<TempSubscription>();
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

        public static int Save(Entity.Profile x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Profile_Save";
            cmd.Parameters.Add(new SqlParameter("@ProfileID", x.ProfileID));
            cmd.Parameters.Add(new SqlParameter("@FirstName", x.FirstName));
            cmd.Parameters.Add(new SqlParameter("@LastName", x.LastName));
            cmd.Parameters.Add(new SqlParameter("@Company", x.Company));
            cmd.Parameters.Add(new SqlParameter("@Title", x.Title));
            cmd.Parameters.Add(new SqlParameter("@Occupation", x.Occupation));
            cmd.Parameters.Add(new SqlParameter("@AddressTypeID", x.AddressTypeID));
            cmd.Parameters.Add(new SqlParameter("@Address1", x.Address1));
            cmd.Parameters.Add(new SqlParameter("@Address2", x.Address2));
            cmd.Parameters.Add(new SqlParameter("@City", x.City));
            cmd.Parameters.Add(new SqlParameter("@RegionCode", x.RegionCode));
            cmd.Parameters.Add(new SqlParameter("@RegionID", x.RegionID));
            cmd.Parameters.Add(new SqlParameter("@ZipCode", x.ZipCode));
            cmd.Parameters.Add(new SqlParameter("@Plus4", x.Plus4));
            cmd.Parameters.Add(new SqlParameter("@CarrierRoute", x.CarrierRoute));
            cmd.Parameters.Add(new SqlParameter("@County", x.County));
            cmd.Parameters.Add(new SqlParameter("@Country", x.Country));
            cmd.Parameters.Add(new SqlParameter("@CountryID", x.CountryID));
            cmd.Parameters.Add(new SqlParameter("@Latitude", x.Latitude));
            cmd.Parameters.Add(new SqlParameter("@Longitude", x.Longitude));
            cmd.Parameters.Add(new SqlParameter("@IsAddressValidated", x.IsAddressValidated));
            cmd.Parameters.Add(new SqlParameter("@AddressValidationDate", (object)x.AddressValidationDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AddressValidationSource", x.AddressValidationSource));
            cmd.Parameters.Add(new SqlParameter("@AddressValidationMessage", x.AddressValidationMessage));
            cmd.Parameters.Add(new SqlParameter("@Email", x.Email));
            cmd.Parameters.Add(new SqlParameter("@Phone", x.Phone));
            cmd.Parameters.Add(new SqlParameter("@Fax", x.Fax));
            cmd.Parameters.Add(new SqlParameter("@Mobile", x.Mobile));
            cmd.Parameters.Add(new SqlParameter("@Website", x.Website));
            cmd.Parameters.Add(new SqlParameter("@Age", x.Age));
            cmd.Parameters.Add(new SqlParameter("@BirthDate", x.BirthDate));
            cmd.Parameters.Add(new SqlParameter("@Income", x.Income));
            cmd.Parameters.Add(new SqlParameter("@Gender", x.Gender));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DatabaseSource", x.DatabaseSource));
            cmd.Parameters.Add(new SqlParameter("@DatabaseTable", x.DatabaseTable));
            cmd.Parameters.Add(new SqlParameter("@TableID", x.TableID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
    }
}
