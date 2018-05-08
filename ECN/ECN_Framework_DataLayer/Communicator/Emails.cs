using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class Email
    {
        public static DataTable EmailSearch(string FilterType, string searchTerm, string BaseChannelID, string customerID, int currentPage, int pageSize, string sortColumn, string sortDirection)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_EmailSearch";
            cmd.Parameters.Add(new SqlParameter("@FilterBy", string.Empty));
            cmd.Parameters.Add(new SqlParameter("@FilterType", FilterType));
            cmd.Parameters.Add(new SqlParameter("@searchTerm", searchTerm));
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", BaseChannelID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            cmd.Parameters.Add(new SqlParameter("@currentPage", currentPage));
            cmd.Parameters.Add(new SqlParameter("@pageSize", pageSize));
            cmd.Parameters.Add(new SqlParameter("@SortBy", sortColumn + " " + sortDirection));

            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static ECN_Framework_Entities.Communicator.Email GetByEmailIDGroupID(int emailID, int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select e.*, eg.FormatTypeCode, eg.SubscribeTypeCode, eg.CreatedOn, eg.LastChanged from Emails e with (nolock) join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID where eg.GroupID = @GroupID and eg.EmailID = @EmailID";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return Get(cmd);
        }

        public static int Save(ECN_Framework_Entities.Communicator.Email email)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Email_Save";
            cmd.Parameters.Add(new SqlParameter("@EmailID", email.EmailID));
            cmd.Parameters.Add(new SqlParameter("@Title", email.Title));
            cmd.Parameters.Add(new SqlParameter("@FirstName", email.FirstName));
            cmd.Parameters.Add(new SqlParameter("@LastName", email.LastName));
            cmd.Parameters.Add(new SqlParameter("@FullName", email.FullName));
            cmd.Parameters.Add(new SqlParameter("@Company", email.Company));
            cmd.Parameters.Add(new SqlParameter("@Occupation", email.Occupation));
            cmd.Parameters.Add(new SqlParameter("@Address", email.Address));
            cmd.Parameters.Add(new SqlParameter("@Address2", email.Address2));
            cmd.Parameters.Add(new SqlParameter("@City", email.City));
            cmd.Parameters.Add(new SqlParameter("@State", email.State));
            cmd.Parameters.Add(new SqlParameter("@Zip", email.Zip));
            cmd.Parameters.Add(new SqlParameter("@Country", email.Country));
            cmd.Parameters.Add(new SqlParameter("@Voice", email.Voice));
            cmd.Parameters.Add(new SqlParameter("@Mobile", email.Mobile));
            cmd.Parameters.Add(new SqlParameter("@Fax", email.Fax));
            cmd.Parameters.Add(new SqlParameter("@Website", email.Website));
            cmd.Parameters.Add(new SqlParameter("@Age", email.Age));
            cmd.Parameters.Add(new SqlParameter("@Income", email.Income));
            cmd.Parameters.Add(new SqlParameter("@Gender", email.Gender));
            cmd.Parameters.Add(new SqlParameter("@User1", email.User1));
            cmd.Parameters.Add(new SqlParameter("@User2", email.User2));
            cmd.Parameters.Add(new SqlParameter("@User3", email.User3));
            cmd.Parameters.Add(new SqlParameter("@User4", email.User4));
            cmd.Parameters.Add(new SqlParameter("@User5", email.User5));
            cmd.Parameters.Add(new SqlParameter("@User6", email.User6));
            cmd.Parameters.Add(new SqlParameter("@BirthDate", (object)email.Birthdate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UserEvent1", email.UserEvent1));
            cmd.Parameters.Add(new SqlParameter("@UserEvent2", email.UserEvent2));
            cmd.Parameters.Add(new SqlParameter("@UserEvent1Date", (object)email.UserEvent1Date ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UserEvent2Date", (object)email.UserEvent2Date ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Password", email.Password));
            cmd.Parameters.Add(new SqlParameter("@EmailAddress", email.EmailAddress));
            cmd.Parameters.Add(new SqlParameter("@Notes", email.Notes));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", email.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@BounceScore", email.BounceScore));
            cmd.Parameters.Add(new SqlParameter("@SoftBounceScore", email.BounceScore));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static ECN_Framework_Entities.Communicator.Email GetByEmailID(int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select e.* from Emails e with (nolock) where e.EmailID = @EmailID";
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.Email GetByEmailAddress(string emailAddress, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select Top 1 e.* from Emails e with (nolock) where e.EmailAddress=@EmailAddress and e.CustomerID=@CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.Email GetByEmailAddress(string emailAddress, int customerID, int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select e.* from Emails e with (nolock) where e.EmailID <> @EmailID and e.EmailAddress=@EmailAddress and e.CustomerID=@CustomerID";
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.Email> GetByGroupID(int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select e.*, eg.FormatTypeCode, eg.SubscribeTypeCode, eg.CreatedOn, eg.LastChanged from Emails e with (nolock) join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID where eg.GroupID = @GroupID";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Communicator.Email Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.Email retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.Email();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Email>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }

        private static List<ECN_Framework_Entities.Communicator.Email> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.Email> retList = new List<ECN_Framework_Entities.Communicator.Email>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.Email retItem = new ECN_Framework_Entities.Communicator.Email();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Email>.CreateBuilder(rdr);
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
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        public static bool Exists(int emailID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Email_Exists_ByID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool ExistsByGroup(string emailAddress, int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "IF EXISTS (SELECT TOP 1 e.EmailID FROM [Emails] e WITH (NOLOCK) JOIN [EmailGroups] eg on e.EmailID = eg.EmailID WHERE eg.GroupID = @GroupID AND e.EmailAddress = @EmailAddress) SELECT 1 ELSE SELECT 0";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists_BaseChannel(string emailaddress, int basechannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Email_Exists_BaseChannelID";
            cmd.Parameters.AddWithValue("@EmailAddress", emailaddress);
            cmd.Parameters.AddWithValue("@BaseChannelID", basechannelID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString()) > 0 ? true : false;
        }

        public static bool IsValidEmailAddress(string emailAddress)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.fn_ValidateEmailAddress";
            cmd.Parameters.AddWithValue("@EmailAddr", emailAddress);
            SqlParameter outParam = new SqlParameter("@IsValid", SqlDbType.Bit);
            outParam.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(outParam);
            DataFunctions.Execute(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            return Convert.ToBoolean(cmd.Parameters["@IsValid"].Value);

        }

        public static bool IsValidEmailAddressForBlast(int EmailID, string EmailAddress, int CustomerID, int refGroupID, int refBlastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Email_Exists_ForBlast";
            cmd.Parameters.AddWithValue("@EmailID", EmailID);
            cmd.Parameters.AddWithValue("@EmailAddress", EmailAddress);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@GroupID", refGroupID);
            cmd.Parameters.AddWithValue("@BlastID", refBlastID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(string emailAddress, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Email_Exists_ByEmailAddressAndCust";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(string emailAddress, int customerID, int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Email_Exists_ByEmailAddress";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static DataTable GetColumnNames()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Email_GetColumnNames";
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void UpdateEmailAddress(int groupID, int customerID, string newEmailAddress, string oldEmailAddress, string source)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Email_UpdateEmailAddress";
            cmd.Parameters.Add(new SqlParameter("@GroupID", groupID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            cmd.Parameters.Add(new SqlParameter("@NewEmailAddress", newEmailAddress));
            cmd.Parameters.Add(new SqlParameter("@OldEmailAddress", oldEmailAddress));
            cmd.Parameters.Add(new SqlParameter("@Source", source));
            DataFunctions.Execute(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void MergeProfiles(int OldEmailID, int NewEmailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Email_MergeProfiles";
            cmd.Parameters.Add(new SqlParameter("@OldEmailID", OldEmailID));
            cmd.Parameters.Add(new SqlParameter("@NewEmailID", NewEmailID));
            DataFunctions.Execute(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void UpdateEmail_BaseChannel(string oldEmail, string newEmail, int BaseChannelID, int UserID, string source)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Email_UpdateEmail_BaseChannel";
            cmd.Parameters.AddWithValue("@OldEmail", oldEmail);
            cmd.Parameters.AddWithValue("@NewEmail", newEmail);
            cmd.Parameters.AddWithValue("@BaseChannelID", BaseChannelID);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@Source", source);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void UpdateEmail_Customer(string oldEmail, string newEmail, int CustomerID, int UserID, string source)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Email_UpdateEmail_Customer";
            cmd.Parameters.AddWithValue("@OldEmail", oldEmail);
            cmd.Parameters.AddWithValue("@NewEmail", newEmail);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@Source", source);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetEmailsForWATT_NXTBookSync(int groupID, bool job1, DateTime? dateFrom = null, string Field = "", string FieldValue = "", bool DoFullNXTBookSync = false)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if (job1)//NXTBook JOB 1
            {
                cmd.CommandText = "e_Email_GetEmailsForWATT_NXTBook_Sync";
                cmd.Parameters.AddWithValue("@GroupID", groupID);
                cmd.Parameters.AddWithValue("@Job1", job1);
                if (dateFrom.HasValue)
                    cmd.Parameters.AddWithValue("@DateFrom", dateFrom);
                if (!string.IsNullOrEmpty(Field) && !string.IsNullOrEmpty(FieldValue))
                {
                    cmd.Parameters.AddWithValue("@Field", Field);
                    cmd.Parameters.AddWithValue("@FieldValue", FieldValue);
                }
            }
            else//NXTBook JOB 2
            {
                cmd.CommandText = "e_Email_GetEmailsForWATT_NXTBook_Sync";
                cmd.Parameters.AddWithValue("@GroupID", groupID);
                cmd.Parameters.AddWithValue("@Job1", job1);
                if (dateFrom.HasValue)
                    cmd.Parameters.AddWithValue("@DateFrom", dateFrom);

                cmd.Parameters.AddWithValue("@Field", Field);
                cmd.Parameters.AddWithValue("@FieldValue", FieldValue);

                cmd.Parameters.AddWithValue("@DoFullNXTBookSync", DoFullNXTBookSync);
            }
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
    }
}
