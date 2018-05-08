using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_DataLayer.Communicator.Helpers;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class EmailGroup
    {
        public const string ProcedureEmailGroupDeleteGroupIdEmailId = "e_EmailGroup_Delete_GroupID_EmailID";
        public const string ProcedureEmailGroupDeleteGroupId = "e_EmailGroup_Delete_GroupID";
        public const string ProcedureEmailGroupImportEmails = "e_EmailGroup_ImportEmails";
        public const string ProcedureEmailGroupImportMsEmails = "e_EmailGroup_ImportMSEmails";

        public const string ProcedureEmailGroupImportEmailsPreImportResults =
            "e_EmailGroup_ImportEmails_PreImportResults";

        public const string ProcedureEmailGroupImportEmailsWithDupes = "e_EmailGroup_ImportEmailsWithDupes";

        public static bool Exists(int emailID, int groupID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_Exists_EmailID_GroupID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(string emailAddress, int groupID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_Exists_EmailAddress_GroupID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static int ValidForTracking(int blastID, int emailID)
        {
            int resultCount = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select COUNT(eg.EmailID) " +
                                "from Blast b with (nolock) " +
                                    "join EmailGroups eg with (nolock) on b.GroupID = eg.GroupID " +
                                "where b.BlastID = @BlastID and eg.EmailID = @EmailID and b.StatusCode <> 'Deleted'";
            cmd.Parameters.Add(new SqlParameter("@EmailID", SqlDbType.Int));
            cmd.Parameters["@EmailID"].Value = emailID;
            cmd.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
            cmd.Parameters["@BlastID"].Value = blastID;

            try
            {
                resultCount = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
            }
            catch (Exception)
            {
            }

            return resultCount;
        }

        public static ECN_Framework_Entities.Communicator.EmailGroup GetByEmailIDGroupID(int emailID, int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_Select";
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.EmailGroup GetByEmailAddressGroupID(string emailAddress, int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_Select_EmailAddress_GroupID";
            cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.EmailGroup> GetByEmailID(int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_Select_EmailID";
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.EmailGroup> GetByGroupID(int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_Select_GroupID";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return GetList(cmd);
        }

        public static void Delete(int groupID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryEmailGroups(
                groupID, null, userID, ProcedureEmailGroupDeleteGroupId);
        }

        public static void Delete(int groupID, int emailID , int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryEmailGroups(
                groupID, emailID, userID, ProcedureEmailGroupDeleteGroupIdEmailId);
        }

        public static void DeleteFromMasterSuppressionGroup(int CustomerID, int EmailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_DeleteFromMasterSuppression";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@EmailID", EmailID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static int Save(ECN_Framework_Entities.Communicator.EmailGroup emailGroup, bool emailAddressOnly, string xmlProfile, string xmlUDF, int userID, string fileName = "", string source = "")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_ImportEmails";
            cmd.Parameters.Add(new SqlParameter("@CustomerID", emailGroup.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@GroupID", emailGroup.GroupID));
            cmd.Parameters.Add(new SqlParameter("@xmlProfile", xmlProfile));            
            cmd.Parameters.Add(new SqlParameter("@xmlUDF", xmlUDF));
            cmd.Parameters.Add(new SqlParameter("@formattypecode", emailGroup.FormatTypeCode));
            cmd.Parameters.Add(new SqlParameter("@subscribetypecode", emailGroup.SubscribeTypeCode));
            cmd.Parameters.Add(new SqlParameter("@EmailAddressOnly", emailAddressOnly));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            cmd.Parameters.Add(new SqlParameter("@filename", fileName));

            if (ECN_Framework_Common.Functions.StringFunctions.HasValue(source))
            {
                cmd.Parameters.Add("@source", SqlDbType.VarChar);
                cmd.Parameters["@source"].Value = source;
            }
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString()); 
        }

        private static ECN_Framework_Entities.Communicator.EmailGroup Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.EmailGroup retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.EmailGroup();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.EmailGroup>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Communicator.EmailGroup> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.EmailGroup> retList = new List<ECN_Framework_Entities.Communicator.EmailGroup>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.EmailGroup retItem = new ECN_Framework_Entities.Communicator.EmailGroup();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.EmailGroup>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);

                        retList.Add(retItem);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        public static DataTable ExportFromImportEmails(string filename, string actionCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_ExportFromImportEmails";

            cmd.Parameters.Add("@filename", SqlDbType.VarChar);
            cmd.Parameters["@filename"].Value = filename;

            cmd.Parameters.Add("@actionCode", SqlDbType.VarChar);
            cmd.Parameters["@actionCode"].Value = actionCode;

            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable ImportEmails(int userID, int customerID, int groupID, string xmlProfile, string xmlUDF, string formatTypeCode, string subscribeTypeCode, bool emailAddressOnly, string filename="", string source = "")
        {
            var readAndFillParams = new FillEmailGroupsArgs
            {
                CustomerId = customerID,
                UserId = userID,
                GroupId = groupID,
                XmlProfile = xmlProfile,
                XmlUdf = xmlUDF,
                FormatTypeCode = formatTypeCode,
                SubscribeTypeCode = subscribeTypeCode,
                EmailAddressOnly = emailAddressOnly,
                Filename = filename,
                SourceNotRequired = source
            };
            return CommunicatorMethodsHelper.GetDataTableEmailGroups(readAndFillParams, ProcedureEmailGroupImportEmails);
        }

        public static DataTable ImportEmails_PreImportResults(int customerID, int groupID, string xmlProfile)
        {
            var readAndFillParams = new FillEmailGroupsArgs
            {
                CustomerId = customerID,
                GroupId = groupID,
                XmlProfile = xmlProfile
            };
            return CommunicatorMethodsHelper.GetDataTableEmailGroups(readAndFillParams, ProcedureEmailGroupImportEmailsPreImportResults);
        }

        public static DataTable ImportMSEmails(int userID, int customerID, int groupID, string xmlProfile, string xmlUDF, string formatTypeCode, string subscribeTypeCode, bool emailAddressOnly, string filename = "", string source = "")
        {
            var readAndFillParams = new FillEmailGroupsArgs
            {
                CustomerId = customerID,
                UserId = userID,
                GroupId = groupID,
                XmlProfile = xmlProfile,
                XmlUdf = xmlUDF,
                FormatTypeCode = formatTypeCode,
                SubscribeTypeCode = subscribeTypeCode,
                EmailAddressOnly = emailAddressOnly,
                Filename = filename,
                SourceNotRequired = source
            };
            return CommunicatorMethodsHelper.GetDataTableEmailGroups(readAndFillParams, ProcedureEmailGroupImportMsEmails);
        }

        public static DataTable ImportEmailsWithDupes(int userID, int customerID, int groupID, string xmlProfile, string xmlUDF, string formatTypeCode, string subscribeTypeCode, bool emailAddressOnly, string compositeKey, bool overwriteWithNULL, string source)
        {
            var readAndFillParams = new FillEmailGroupsArgs
            {
                CustomerId = customerID,
                UserId = userID,
                GroupId = groupID,
                XmlProfile = xmlProfile,
                XmlUdf = xmlUDF,
                FormatTypeCode = formatTypeCode,
                SubscribeTypeCode = subscribeTypeCode,
                EmailAddressOnly = emailAddressOnly,
                OverwriteWithNull = overwriteWithNULL,
                CompositeKey = compositeKey,
                Source = source
            };
            return CommunicatorMethodsHelper.GetDataTableEmailGroups(readAndFillParams, ProcedureEmailGroupImportEmailsWithDupes);
        }

        public static DataTable ImportEmailsToCS(int userID, int baseChannelID, string xml)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_ImportEmailsToCS";

            cmd.Parameters.Add("@BaseChannelID", SqlDbType.Int);
            cmd.Parameters["@BaseChannelID"].Value = baseChannelID;

            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters["@UserID"].Value = userID;

            cmd.Parameters.Add("@xml", SqlDbType.Text);
            cmd.Parameters["@xml"].Value = xml;
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable ImportEmailsToGlobalMS(int userID, int baseChannelID, string xml)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_ImportEmailsToGlobalMS";

            //cmd.Parameters.Add("@BaseChannelID", SqlDbType.Int);
            //cmd.Parameters["@BaseChannelID"].Value = baseChannelID;

            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters["@UserID"].Value = userID;

            cmd.Parameters.Add("@xml", SqlDbType.Text);
            cmd.Parameters["@xml"].Value = xml;
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable ImportEmailsToNoThreshold(int userID, int baseChannelID, string xml)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_ImportEmailsToNoThreshold";

            cmd.Parameters.Add("@BaseChannelID", SqlDbType.Int);
            cmd.Parameters["@BaseChannelID"].Value = baseChannelID;

            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters["@UserID"].Value = userID;

            cmd.Parameters.Add("@xml", SqlDbType.Text);
            cmd.Parameters["@xml"].Value = xml;
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetBestProfileForEmailAddress(int groupID, int customerID, string emailAddress)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_GetBestProfileForEmailAddress";

            cmd.Parameters.Add("@GroupID", SqlDbType.Int);
            cmd.Parameters["@GroupID"].Value = groupID;

            cmd.Parameters.Add("@CustomerID", SqlDbType.Int);
            cmd.Parameters["@CustomerID"].Value = customerID;

            cmd.Parameters.Add("@Filter", SqlDbType.VarChar);
            cmd.Parameters["@Filter"].Value = String.Format(" and emails.emailaddress = '{0}' ", emailAddress.Replace("'", "''"));

            cmd.Parameters.Add("@SubscribeType", SqlDbType.VarChar);
            cmd.Parameters["@SubscribeType"].Value = "'S','P','U','M'";
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetGroupEmailProfilesWithUDF(int groupID, int customerID, string filter, string subscribeType, string profFilter)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_GetGroupEmailProfilesWithUDF";

            cmd.Parameters.Add("@GroupID", SqlDbType.Int);
            cmd.Parameters["@GroupID"].Value = groupID;

            cmd.Parameters.Add("@CustomerID", SqlDbType.Int);
            cmd.Parameters["@CustomerID"].Value = customerID;

            cmd.Parameters.Add("@Filter", SqlDbType.VarChar);
            cmd.Parameters["@Filter"].Value = filter;

            if (profFilter.Length > 0)
            {
                cmd.Parameters.Add("@ProfileFilter", SqlDbType.VarChar);
                cmd.Parameters["@ProfileFilter"].Value = profFilter;
            }

            cmd.Parameters.Add("@SubscribeType", SqlDbType.VarChar);
            cmd.Parameters["@SubscribeType"].Value = subscribeType;
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetGroupEmailProfilesWithUDF(int groupID, int customerID, string subscribeType,DateTime fromDate,DateTime toDate,bool recent, string filter, string profFilter)
        {
            if (fromDate == DateTime.MinValue) { fromDate = new DateTime(1753, 1, 1); }
            if (toDate == DateTime.MinValue) { toDate = DateTime.Now; }

            int i = recent ? 1 : 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_GetGroupEmailProfilesWithUDF_By_Date";

            cmd.Parameters.Add("@GroupID", SqlDbType.Int);
            cmd.Parameters["@GroupID"].Value = groupID;

            cmd.Parameters.Add("@CustomerID", SqlDbType.Int);
            cmd.Parameters["@CustomerID"].Value = customerID;

            if (profFilter.Length > 0)
            {
                cmd.Parameters.Add("@ProfileFilter", SqlDbType.VarChar);
                cmd.Parameters["@ProfileFilter"].Value = profFilter;
            }

            cmd.Parameters.Add("@SubscribeType", SqlDbType.VarChar);
            cmd.Parameters["@SubscribeType"].Value = subscribeType;

            cmd.Parameters.Add("@FromDate", SqlDbType.DateTime);
            cmd.Parameters["@FromDate"].Value = fromDate;

            cmd.Parameters.Add("@ToDate", SqlDbType.DateTime);
            cmd.Parameters["@ToDate"].Value = toDate;

            cmd.Parameters.Add("@Recent", SqlDbType.Int);
            cmd.Parameters["@Recent"].Value = i;

            cmd.Parameters.Add("@Filter", SqlDbType.VarChar);
            cmd.Parameters["@Filter"].Value = filter;


            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }


        public static DataTable GetColumnNames()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_GetColumnNames";
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable PreviewFilteredEmails(int groupID, int customerID, string filter)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_PreviewFilteredEmails";

            cmd.Parameters.Add("@GroupID", SqlDbType.Int);
            cmd.Parameters["@GroupID"].Value = groupID;

            cmd.Parameters.Add("@CustomerID", SqlDbType.Int);
            cmd.Parameters["@CustomerID"].Value = customerID;

            cmd.Parameters.Add("@Filter", SqlDbType.VarChar);
            cmd.Parameters["@Filter"].Value = filter;
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable PreviewFilteredEmails_Paging(int groupID, int customerID, string filter, string sortColumn, string sortDirection, int pageNumber, int pageSize)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_PreviewFilteredEmails_Paging";

            cmd.Parameters.Add("@GroupID", SqlDbType.Int);
            cmd.Parameters["@GroupID"].Value = groupID;

            cmd.Parameters.Add("@CustomerID", SqlDbType.Int);
            cmd.Parameters["@CustomerID"].Value = customerID;

            cmd.Parameters.Add("@Filter", SqlDbType.VarChar);
            cmd.Parameters["@Filter"].Value = filter;

            cmd.Parameters.AddWithValue("@SortColumn", sortColumn);
            cmd.Parameters.AddWithValue("@SortDirection", sortDirection);
            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);

            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetByUserID(int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_EmailGroup_Get_UserID";
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetByBounceScore(int customerID, int? groupID, int bounceScore, string bounceCondition)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_EmailGroup_Get_BounceScore";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BounceScore", bounceScore);
            cmd.Parameters.AddWithValue("@BounceCondition", bounceCondition);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataSet GetBySearchStringPaging(int customerID, int groupID, int pageNo, int pageSize, DateTime dateFrom, DateTime dateTo, bool recent, string filter, string sortColumn = "EmailID", string sortDirection = "ASC")
        {
            try
            {
                if (dateFrom.ToShortDateString() == DateTime.MinValue.ToShortDateString())
                {
                    dateFrom = new DateTime(1753, 1, 1);  //SQL min date
                }
                if (dateTo.ToShortDateString() == DateTime.MinValue.ToShortDateString())
                {
                    dateTo = DateTime.Now;
                }

                int i = recent ? 1 : 0;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "v_EmailGroup_Get_Paging_By_Date";
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters.AddWithValue("@GroupID", groupID);
                cmd.Parameters.AddWithValue("@PageNo", pageNo);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                cmd.Parameters.AddWithValue("@FromDate", dateFrom.ToShortDateString());
                cmd.Parameters.AddWithValue("@ToDate", dateTo.ToShortDateString());
                cmd.Parameters.AddWithValue("@Recent", i);
                cmd.Parameters.AddWithValue("@Filter", filter);
                cmd.Parameters.AddWithValue("@SortColumn", sortColumn);
                cmd.Parameters.AddWithValue("@SortDirection", sortDirection);
                return DataFunctions.GetDataSet(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            }
            catch
            {
                return new DataSet();
            }
            
        }

        public static DataSet GetBySearchStringPaging(int customerID, int groupID, int pageNo, int pageSize, string searchString)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_EmailGroup_Get_Paging";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@PageNo", pageNo);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@SearchString", searchString);
            return DataFunctions.GetDataSet(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static int ValidateEmails(int groupID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_MarkBadEmails";
            cmd.Parameters.Add(new SqlParameter("@GroupID", groupID));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static int DeleteBadEmails(int groupID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_DeleteBadEmails";
            cmd.Parameters.Add(new SqlParameter("@GroupID", groupID));
            cmd.Parameters.Add(new SqlParameter("@userID", userID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static bool Update_Spam_Feedback(string XMLEmailList)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_update_Spam_Feedback_XML";
            cmd.Parameters.Add(new SqlParameter("@xmlDocument", SqlDbType.Text));
            cmd.Parameters["@xmlDocument"].Value = "<ROOT>" + XMLEmailList + "</ROOT>"; 
            cmd.Parameters.Add(new SqlParameter("@ActionTypeCode", SqlDbType.VarChar, 100));
            cmd.Parameters["@ActionTypeCode"].Value = "FEEDBACK_UNSUB";
            return DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void SetSTC(string newCode, int emailID, int groupID)
        {
            using (var cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@NewCode", SqlDbType.Int).Value = newCode;
                cmd.Parameters.Add("@EmailID", SqlDbType.Int).Value = emailID;
                cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = groupID;
                cmd.CommandText =
                    "update EmailGroups set SubscribeTypeCode = @NewCode, LastChanged = GetDate() where EmailGroupID in ( " +
                    "select eg.EmailGroupID from Emails e with (nolock) join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID join Groups g with (nolock) on eg.GroupID = g.GroupID " +
                    "where e.EmailID in (" + emailID + ") and eg.GroupID = @GroupID )";
                DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            }
        }

        public static void UnsubscribeSubscribers(int groupID, string emailAddresses)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            
            cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = groupID;
            cmd.CommandText = "update EmailGroups set SubscribeTypeCode = 'U', LastChanged = GetDate() where EmailGroupID in (" +
                                " select eg.EmailGroupID from Emails e with (nolock) join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID join Groups g with (nolock) on eg.GroupID = g.GroupID" +
                                " where e.EmailAddress in (" + emailAddresses + ") and eg.GroupID = @GroupID and eg.SubscribeTypeCode in ('S', 'P') and isnull(g.MasterSupression,0) <> 1)";
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void AddToMasterSuppression(int customerID, int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@EmailID", SqlDbType.Int).Value = emailID;
            cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerID;
            cmd.CommandText = "e_EmailGroup_AddToMasterSuppression";
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static int UnsubscribeSubscribersInFolder(int folderID, string emailIDs)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = " UPDATE EmailGroups SET SubscribeTypeCode = 'U', LastChanged = GETDATE()" +
                                                " FROM Emails e JOIN EmailGroups eg ON e.EmailID = eg.EmailID JOIN Groups g on eg.GroupID = g.groupID " +
                                                " WHERE g.FolderID = " + folderID + " AND eg.SubscribeTypeCode <> 'U' AND eg.EmailID IN ( " + emailIDs + ")";
            return DataFunctions.Execute(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void UnsubscribeBounces(int blastID, string isp)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_UnsubscribeBounces";
            cmd.Parameters.Add(new SqlParameter("@blastID", blastID));
            cmd.Parameters.Add(new SqlParameter("@bounceType", "hard,hardbounce"));
            cmd.Parameters.Add(new SqlParameter("@ISP", isp));
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static int GetEmailIDFromComposite(int groupID, int customerID, string emailAddress, string compositeKey, string compositeKeyValue)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_EmailGroup_Select_CompositeKey";
            cmd.Parameters.Add(new SqlParameter("@GroupID", groupID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            cmd.Parameters.Add(new SqlParameter("@EmailAddress", emailAddress));
            cmd.Parameters.Add(new SqlParameter("@CompositeKey", compositeKey));
            cmd.Parameters.Add(new SqlParameter("@CompositeKeyValue", compositeKeyValue));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static int GetEmailIDFromWhatEmail(int groupID, int customerID, string emailAddress)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_EmailGroup_Select_WhatEmail";
            cmd.Parameters.Add(new SqlParameter("@GroupID", groupID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            cmd.Parameters.Add(new SqlParameter("@EmailAddress", emailAddress));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static int GetSubscriberCount(int groupID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_EmailGroup_GetSubscriberCount";
            cmd.Parameters.Add(new SqlParameter("@GroupID", groupID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static DataTable GetSubscriberStatus(int customerID, string emailAddress)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_EmailGroup_GetSubscriberStatus";
            cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetUnsubscribesForDay(int daysBack, int folderID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_EmailGroup_GetUnsubscribesForDay";
            cmd.Parameters.AddWithValue("@FolderID", folderID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@DaysBack", daysBack);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetUnsubscribesForCurrentBackToDay(int daysBack, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_EmailGroup_GetUnsubscribesForCurrentBackToDay";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@DaysBack", daysBack);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetEmailsFromOtherGroupsToUnsubscribe(string emailIDs, int folderID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_EmailGroup_GetEmailsFromOtherGroupsToUnsubscribe";
            cmd.Parameters.AddWithValue("@FolderID", folderID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@EmailIDs", emailIDs);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void CopyProfileFromGroup(int sourceGroupID, int destinationGroupID, int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_CopyProfileFromGroup";
            cmd.Parameters.Add(new SqlParameter("@sourcegroupID", sourceGroupID));
            cmd.Parameters.Add(new SqlParameter("@destinationgroupID", destinationGroupID));
            cmd.Parameters.Add(new SqlParameter("@EmailID", emailID));

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void ImportEmailsToGroups(string xml, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_Update_SubscriptionManagement";
            cmd.Parameters.AddWithValue("@xmlProfiles", xml);
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static bool EmailExistsInCustomerSeedList(int EmailID, int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_Exists_SeedList";
            cmd.Parameters.AddWithValue("@EmailID", EmailID);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString()) > 0 ? true : false;
        }

        public static DataTable GetGroupStats(int groupID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_EmailGroup_GetGroupStats";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);

            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static int FDSubscriberLogin(int groupID, string emailAddress, int UDFID, string UDFValue, string Password, string User1, string User2, string User3, string User4, string User5, string User6)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_FDSubscriberLogin";
            cmd.Parameters.Add(new SqlParameter("@GroupID", groupID));
            if (!string.IsNullOrEmpty(emailAddress)) cmd.Parameters.Add(new SqlParameter("@EmailAddress", emailAddress));
            if (UDFID > 0) cmd.Parameters.Add(new SqlParameter("@UDFID", UDFID));
            if (!string.IsNullOrEmpty(UDFValue)) cmd.Parameters.Add(new SqlParameter("@UDFValue", UDFValue));
            if (!string.IsNullOrEmpty(Password)) cmd.Parameters.Add(new SqlParameter("@Password", Password));
            if (!string.IsNullOrEmpty(User1)) cmd.Parameters.Add(new SqlParameter("@User1", User1));
            if (!string.IsNullOrEmpty(User2)) cmd.Parameters.Add(new SqlParameter("@User2", User2));
            if (!string.IsNullOrEmpty(User3)) cmd.Parameters.Add(new SqlParameter("@User3", User3));
            if (!string.IsNullOrEmpty(User4)) cmd.Parameters.Add(new SqlParameter("@User4", User4));
            if (!string.IsNullOrEmpty(User5)) cmd.Parameters.Add(new SqlParameter("@User5", User5));
            if (!string.IsNullOrEmpty(User6)) cmd.Parameters.Add(new SqlParameter("@User6", User6));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }
    }
}
