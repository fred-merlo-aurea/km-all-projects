using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using KM.Common;

namespace WQT_ECN_Sync
{
    public class Program
    {
        private static string daysToProcess = ConfigurationManager.AppSettings["DaysToProcess"];
        private static string groupID = ConfigurationManager.AppSettings["GroupID"];
        private static string customerID = ConfigurationManager.AppSettings["CustomerID"];
        private static string magazineID = ConfigurationManager.AppSettings["MagazineID"];
        private static DataTable responses_DT = getResponses();
        private static DataTable responseGroups_DT = getResponseGroups();
        private static DataTable subscribers_DT = getSubscribers();

        static void Main(string[] args)
        {
            FileFunctions.LogConsoleActivity("initialize ECN to WQT Sync");
            initialize_ECN2WQT();
            FileFunctions.LogConsoleActivity("Completed ECN to WQT Sync");
            FileFunctions.LogConsoleActivity("=========================");
            FileFunctions.LogConsoleActivity("initialize WQT to ECN Sync");
            initialize_WQT2ECN();
            FileFunctions.LogConsoleActivity("Completed WQT to ECN Sync");
        }

        #region WQT to ECN
        private static void updateEmail(DataRow dr)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            string FirstName = dr["Fname"] == DBNull.Value ? "" : dr["Fname"].ToString();
            string LastName = dr["Lname"] == DBNull.Value ? "" : dr["Lname"].ToString();
            string State = dr["State"] == DBNull.Value ? "" : dr["State"].ToString();

            string emailAddress = string.Empty;
            if (dr["EmailAddress"] == null)
            {
                cmd.CommandText = " update Emails set CustomerID= @CustomerID, Title=@Title, FirstName=@FirstName, LastName=@LastName, Company= @Company, Address=@Address, "
                            + " City=@City, State=@State, Zip=@Zip, Country=@Country, Voice=@Voice, Fax=@Fax, DateAdded=@DateAdded, User1=@User1 where EmailID=@EmailID ";
            }
            else
            {
                if (dr["EmailAddress"].ToString().Equals(string.Empty))
                {
                    cmd.CommandText = " update Emails set CustomerID= @CustomerID, Title=@Title, FirstName=@FirstName, LastName=@LastName, Company= @Company, Address=@Address, "
                             + " City=@City, State=@State, Zip=@Zip, Country=@Country, Voice=@Voice, Fax=@Fax, DateAdded=@DateAdded, User1=@User1 where EmailID=@EmailID ";
                }
                else
                {
                    cmd.CommandText = " update Emails set EmailAddress=@EmailAddress, CustomerID= @CustomerID, Title=@Title, FirstName=@FirstName, LastName=@LastName, Company= @Company, Address=@Address, "
                            + " City=@City, State=@State, Zip=@Zip, Country=@Country, Voice=@Voice, Fax=@Fax, DateAdded=@DateAdded, User1=@User1 where EmailID=@EmailID ";

                    cmd.Parameters.AddWithValue("@EmailAddress", dr["EmailAddress"].ToString());
                }
            }
            cmd.Parameters.AddWithValue("@EmailID", dr["SubscriberID"].ToString());
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@Title", dr["title"] == DBNull.Value ? "" : dr["title"].ToString());
            cmd.Parameters.AddWithValue("@FirstName", FirstName);
            cmd.Parameters.AddWithValue("@LastName", LastName);
            cmd.Parameters.AddWithValue("@Company", dr["Company"] == DBNull.Value ? "" : dr["Company"].ToString());
            cmd.Parameters.AddWithValue("@Address", dr["Address"] == DBNull.Value ? "" : dr["Address"].ToString());
            cmd.Parameters.AddWithValue("@City", dr["City"] == DBNull.Value ? "" : dr["City"].ToString());
            cmd.Parameters.AddWithValue("@State", State);
            cmd.Parameters.AddWithValue("@Zip", dr["Zip"] == DBNull.Value ? "" : dr["Zip"].ToString());
            cmd.Parameters.AddWithValue("@Country", dr["Country"] == DBNull.Value ? "" : dr["Country"].ToString());
            cmd.Parameters.AddWithValue("@Voice", dr["Phone"] == DBNull.Value ? "" : dr["Phone"].ToString());
            cmd.Parameters.AddWithValue("@Fax", dr["Fax"] == DBNull.Value ? "" : dr["Fax"].ToString());
            cmd.Parameters.AddWithValue("@dateAdded", dr["LastModified"].ToString());
            cmd.Parameters.AddWithValue("@user1", dr["Fname"].ToString() + " " + dr["Lname"].ToString() + " " + dr["state"].ToString());
            ecn.common.classes.DataFunctions.ExecuteScalar("communicator", cmd);

            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = " update EmailGroups set LastChanged=@LastChanged where EmailID=@EmailID and GroupID=@GroupID";
            cmd.Parameters.AddWithValue("@EmailID", dr["SubscriberID"].ToString());
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@LastChanged", dr["LastModified"].ToString());
            ecn.common.classes.DataFunctions.ExecuteScalar("communicator", cmd);
        }

        private static void initialize_WQT2ECN()
        {
            DataTable SubscriberDetailsCSV_DT = getSubscriptionData();
            Hashtable gdf = GetGroupDataFields();
            FileFunctions.LogConsoleActivity("Records Retrieved form WQT: " + SubscriberDetailsCSV_DT.Rows.Count.ToString());
            foreach (DataRow dr in SubscriberDetailsCSV_DT.AsEnumerable())
            {
                if (dr["SubscriberID"].ToString().Equals("0"))
                {
                    //Insert Profile
                    int emailID = insertEmail(dr);
                    //Insert UDF's
                    insertUDF(gdf, dr, emailID);
                    //update WQT with SubscriberID
                    updateSubscriberID(emailID, Convert.ToInt32(dr["subscriptionID"].ToString()));
                }
                else
                {
                    DataTable dt = getEmailAddress(Convert.ToInt32(dr["SubscriberID"].ToString()));
                    if (Convert.ToDateTime(dt.Rows[0][1].ToString()) < Convert.ToDateTime(dr["LastModified"].ToString()))
                    {
                        //Update Profile
                        updateEmail(dr);
                        //Delete UDF's
                        deleteUDF(Convert.ToInt32(dr["SubscriberID"].ToString()));
                        //Insert UDF's
                        insertUDF(gdf, dr, Convert.ToInt32(dr["SubscriberID"].ToString()));
                    }
                }
            }
        }

        private static void insertUDF(Hashtable gdf, DataRow dr, int emailID)
        {
            IDictionaryEnumerator en1 = gdf.GetEnumerator();
            SqlCommand cmd;
            while (en1.MoveNext())
            {
                try
                {
                    if (dr[en1.Value.ToString()] != null && dr[en1.Value.ToString()].ToString() != "")
                    {

                        cmd = new SqlCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = " insert into EmailDataValues (EmailID, GroupDataFieldsID, DataValue, ModifiedDate) values(@emailID, @groupDataFieldsID, @dateValue, getDate())";
                        cmd.Parameters.AddWithValue("@emailID", emailID);
                        cmd.Parameters.AddWithValue("@groupDataFieldsID", en1.Key.ToString());
                        cmd.Parameters.AddWithValue("@dateValue", CleanXMLString(dr[en1.Value.ToString()].ToString()));
                        ecn.common.classes.DataFunctions.ExecuteScalar("communicator", cmd);
                    }
                }
                catch
                { }
            }
        }

        private static void deleteUDF(int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = " delete from EmailDataValues where EmailID= @emailID and GroupDataFieldsID in (select GroupDataFieldsID from GroupDataFields gdf where gdf.GroupID=@groupID)";
            cmd.Parameters.AddWithValue("@emailID", emailID);
            cmd.Parameters.AddWithValue("@groupID", groupID);
            ecn.common.classes.DataFunctions.ExecuteScalar("communicator", cmd);
        }

        private static void updateSubscriberID(int subscriberID, int subscriptionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update Subscriptions set subscriberID= @subscriberID where subscriptionID= @subscriptionID";
            cmd.Parameters.AddWithValue("@subscriberID", subscriberID);
            cmd.Parameters.AddWithValue("@subscriptionID", subscriptionID);
            ecn.common.classes.DataFunctions.ExecuteScalar("connString", cmd);
        }

        private static int insertEmail(DataRow dr)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = " insert into Emails (EmailAddress, CustomerID, Title, FirstName, LastName, Company, Address, City, State, Zip, Country, Voice, Fax, DateAdded, User1) "
                             + " values (@EmailAddress, @CustomerID, @Title, @FirstName, @LastName, @Company,@Address,@City, @State, @Zip, @Country, @Voice, @Fax, @dateAdded, @user1);Select @@IDENTITY";

            string emailAddress = string.Empty;
            if (dr["EmailAddress"] == null)
            {
                emailAddress = Guid.NewGuid().ToString() + "@kmpsgroup.com";
            }
            else
            {
                if (dr["EmailAddress"].ToString().Equals(string.Empty))
                {
                    emailAddress = Guid.NewGuid().ToString() + "@kmpsgroup.com";
                }
            }
            string FirstName = dr["Fname"] == DBNull.Value ? "" : dr["Fname"].ToString();
            string LastName = dr["Lname"] == DBNull.Value ? "" : dr["Lname"].ToString();
            string State = dr["State"] == DBNull.Value ? "" : dr["State"].ToString();

            string user1 = FirstName + " " + LastName + " " + State;
            cmd.Parameters.AddWithValue("@EmailAddress", emailAddress == string.Empty ? dr["EmailAddress"].ToString() : emailAddress);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@Title", dr["title"] == DBNull.Value ? "" : dr["title"].ToString());
            cmd.Parameters.AddWithValue("@FirstName", FirstName);
            cmd.Parameters.AddWithValue("@LastName", LastName);
            cmd.Parameters.AddWithValue("@Company", dr["Company"] == DBNull.Value ? "" : dr["Company"].ToString());
            cmd.Parameters.AddWithValue("@Address", dr["Address"] == DBNull.Value ? "" : dr["Address"].ToString());
            cmd.Parameters.AddWithValue("@City", dr["City"] == DBNull.Value ? "" : dr["City"].ToString());
            cmd.Parameters.AddWithValue("@State", State);
            cmd.Parameters.AddWithValue("@Zip", dr["Zip"] == DBNull.Value ? "" : dr["Zip"].ToString());
            cmd.Parameters.AddWithValue("@Country", dr["Country"] == DBNull.Value ? "" : dr["Country"].ToString());
            cmd.Parameters.AddWithValue("@Voice", dr["Phone"] == DBNull.Value ? "" : dr["Phone"].ToString());
            cmd.Parameters.AddWithValue("@Fax", dr["Fax"] == DBNull.Value ? "" : dr["Fax"].ToString());
            cmd.Parameters.AddWithValue("@dateAdded", dr["LastModified"].ToString());
            cmd.Parameters.AddWithValue("@user1", user1);
            int EmailID = Convert.ToInt32(ecn.common.classes.DataFunctions.ExecuteScalar("communicator", cmd));

            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = " insert into EmailGroups(EmailID, GroupID, FormatTypeCode, SubscribeTypeCode, CreatedOn) "
                             + " values (@EmailID, @GroupID, @FormatTypeCode, @SubscribeTypeCode,@dateAdded)";
            cmd.Parameters.AddWithValue("@EmailID", EmailID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@FormatTypeCode", "html");
            cmd.Parameters.AddWithValue("@SubscribeTypeCode", "S");
            cmd.Parameters.AddWithValue("@dateAdded", dr["LastModified"].ToString());
            ecn.common.classes.DataFunctions.ExecuteScalar("communicator", cmd);
            return EmailID;

        }

        private static DataTable getEmailAddress(int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select e.EmailAddress, CASE when eg.LastChanged is null then eg.CreatedOn else eg.LastChanged end as LastChanged from Emails e inner join EmailGroups eg on e.EmailID=eg.EmailID  where e.EmailID=@emailID and eg.groupID=@groupID";
            cmd.Parameters.Add("@groupID", SqlDbType.Int);
            cmd.Parameters["@groupID"].Value = groupID;
            cmd.Parameters.Add("@emailID", SqlDbType.Int);
            cmd.Parameters["@emailID"].Value = emailID;
            return ecn.common.classes.DataFunctions.GetDataTable("communicator", cmd);
        }

        private static string CleanXMLString(string text)
        {
            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            return text.Trim();
        }

        private static Hashtable GetGroupDataFields()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM GroupDatafields WHERE GroupID=@groupID and IsDeleted = 0";

            cmd.Parameters.Add("@groupID", SqlDbType.Int);
            cmd.Parameters["@groupID"].Value = groupID;

            DataTable emailstable = ecn.common.classes.DataFunctions.GetDataTable("communicator", cmd);

            Hashtable fields = new Hashtable();
            foreach (DataRow dr in emailstable.Rows)
                fields.Add(Convert.ToInt32(dr["GroupDataFieldsID"]), dr["ShortName"].ToString().ToLower());

            return fields;
        }

        private static DataTable getSubscriptionData()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_getSubscriberWithDetails_CSV";
            cmd.Parameters.Add("@magazineID", SqlDbType.Int);
            cmd.Parameters["@magazineID"].Value = magazineID;
            cmd.Parameters.Add("@filter", SqlDbType.VarChar);
            cmd.Parameters["@filter"].Value = "and (isnull(s.UpdatedDate,s.CreatedDate) > CONVERT(VARCHAR(10),DATEADD(Day," + daysToProcess + ", GETDATE()),111))";
            return ecn.common.classes.DataFunctions.GetDataTable("connString", cmd);
        }
        #endregion

        #region ECN to WQT
        private static void initialize_ECN2WQT()
        {
            DataTable emailDataValues_DT = getECNData();

            var distinctEmails = (from edv in emailDataValues_DT.AsEnumerable()
                                  select new
                                  {
                                      EmailID = edv["EmailID"].ToString(),
                                      EmailAddress = edv["EmailAddress"].ToString(),
                                      FirstName = edv["FirstName"].ToString(),
                                      LastName = edv["LastName"].ToString(),
                                      State = edv["State"].ToString(),
                                  }).ToList().Distinct();
            FileFunctions.LogConsoleActivity("Records Retrieved from ECN: " + distinctEmails.Count().ToString());
            foreach (var dr in distinctEmails)
            {
                bool exists = checkSubscriberExists(Convert.ToInt32(dr.EmailID.ToString()));
                if (exists)
                {
                    updateSubscriber(emailDataValues_DT, Convert.ToInt32(dr.EmailID.ToString()));
                }
                else
                {
                    insertSubscriber(emailDataValues_DT, dr.EmailAddress, dr.FirstName, dr.LastName, dr.State);
                }
            }
        }

        private static void updateSubscriber(DataTable emailDataValues_DT, int EmailID)
        {
            int SubscriptionID = getSubscriptionID(EmailID);

            var subscriberProfile = (from edv in emailDataValues_DT.AsEnumerable()
                                     where edv.Field<int>("EmailID") == EmailID
                                     select edv).ToList().Distinct();

            var modifiedDate = from src in subscribers_DT.AsEnumerable()
                               where src.Field<int>("SubscriptionID") == SubscriptionID
                               select new
                               {
                                   ModifiedDate = DateTime.Parse(src["UpdatedDate"] == DBNull.Value ? src["CreatedDate"].ToString() : src["UpdatedDate"].ToString())
                               };

            if (modifiedDate.First().ModifiedDate < DateTime.Parse((subscriberProfile.First())["LastChanged"] == DBNull.Value ? (subscriberProfile.First())["CreatedOn"].ToString() : (subscriberProfile.First())["LastChanged"].ToString()))
            {
                string emailAddress;
                if (subscriberProfile.First().Field<string>("EmailAddress").ToString().Contains("kmpsgroup.com"))
                    emailAddress = "";
                else
                    emailAddress = subscriberProfile.First().Field<string>("EmailAddress").ToString();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = " update Subscriptions  set FName=@FirstName,LName=@LastName, State=@state, EmailAddress=@EmailAddress, Company = @Company, Title = @Title, Address =  @Address, City =  @City, Zip = @Zip, Country = @Country, Phone = @Phone, Fax = @Fax, UpdatedDate = @LastModified "
                                + " where SubscriptionID=@SubscriptionID and MagazineID=@MagazineID";
                cmd.Parameters.AddWithValue("@MagazineID", magazineID);
                cmd.Parameters.AddWithValue("@SubscriptionID", SubscriptionID);
                cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
                cmd.Parameters.AddWithValue("@FirstName", ((subscriberProfile.First())["FirstName"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("FirstName")).ToString());
                cmd.Parameters.AddWithValue("@LastName", ((subscriberProfile.First())["lastName"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("lastName")).ToString());
                cmd.Parameters.AddWithValue("@State", ((subscriberProfile.First())["state"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("state")).ToString());
                cmd.Parameters.AddWithValue("@Title", ((subscriberProfile.First())["title"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("title")).ToString());
                cmd.Parameters.AddWithValue("@Address", ((subscriberProfile.First())["Address"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("Address")).ToString() + " " + ((subscriberProfile.First())["Address2"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("Address2")).ToString());
                cmd.Parameters.AddWithValue("@City", ((subscriberProfile.First())["City"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("City")).ToString());
                cmd.Parameters.AddWithValue("@Company", ((subscriberProfile.First())["Company"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("Company")).ToString());
                cmd.Parameters.AddWithValue("@Zip", ((subscriberProfile.First())["zip"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("zip")).ToString());
                cmd.Parameters.AddWithValue("@Country", ((subscriberProfile.First())["Country"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("Country")).ToString());
                cmd.Parameters.AddWithValue("@Phone", ((subscriberProfile.First())["Voice"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("Voice")).ToString());
                cmd.Parameters.AddWithValue("@Fax", ((subscriberProfile.First())["Fax"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("Fax")).ToString());
                cmd.Parameters.AddWithValue("@LastModified", ((subscriberProfile.First())["LastChanged"] == DBNull.Value ? subscriberProfile.First().Field<DateTime>("CreatedOn") : subscriberProfile.First().Field<DateTime>("LastChanged")).ToString());
                ecn.common.classes.DataFunctions.ExecuteScalar("connString", cmd);
                FileFunctions.LogConsoleActivity("Updated Subscriber with EmailAddress: " + subscriberProfile.First().Field<string>("EmailAddress").ToString());
                foreach (DataRow dr in responseGroups_DT.AsEnumerable())
                {
                    updateSubscriptionDetails(SubscriptionID, subscriberProfile, dr["ResponseGroupName"].ToString(), Convert.ToInt32(dr["ResponseGroupID"].ToString()));
                }
            }
        }

        private static void updateSubscriptionDetails(int SubscriptionID, IEnumerable<DataRow> subscriberProfile_DT, string responseGroupName, int responseGroupID)
        {
            var subscriptionDetails = (from sp in subscriberProfile_DT.AsEnumerable()
                                       select new
                                       {
                                           EmailDataValue = sp.Field<string>(responseGroupName),
                                           CreatedOn = sp.Field<DateTime?>("CreatedOn"),
                                           LastChanged = sp.Field<DateTime?>("LastChanged")
                                       }).ToList();

            if (subscriptionDetails.Any())
            {
                DateTime? createdOn_ECN = subscriptionDetails.First().CreatedOn;
                DateTime? lastChanged_ECN = subscriptionDetails.First().LastChanged;
                DateTime ecnLatestDate = lastChanged_ECN != null ? lastChanged_ECN.Value : createdOn_ECN.Value;
                DateTime? lastChanged_WQT = getSubscriptions_LastUpdateDate(SubscriptionID);
                if (lastChanged_WQT != null)
                {
                    if (ecnLatestDate > lastChanged_WQT)
                    {
                        deleteSubscriptionDetails(responseGroupID, SubscriptionID);
                        insertSubscriptionDetails(SubscriptionID, subscriberProfile_DT, responseGroupName, responseGroupID);
                    }
                }
                else
                {
                    insertSubscriptionDetails(SubscriptionID, subscriberProfile_DT, responseGroupName, responseGroupID);
                }

                FileFunctions.LogConsoleActivity("Updated SubscriptionDetails for SubscriptionID: " + SubscriptionID);
            }
            else
            {
                FileFunctions.LogConsoleActivity("No SubscriptionDetails for SubscriptionID: " + SubscriptionID);
            }
        }

        private static void deleteSubscriptionDetails(int responseGroupID, int subscriptionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = " Delete from SubscriptionDetails  where SubscriptionID= @SubscriptionID and "
                             + " ResponseID in (select ResponseID from Responses r join ResponseGroups rg on r.ResponseGroupID=rg.ResponseGroupID where rg.ResponseGroupID=@ResponseGroupID)";
            cmd.Parameters.AddWithValue("@SubscriptionID", subscriptionID);
            cmd.Parameters.AddWithValue("@ResponseGroupID", responseGroupID);
            ecn.common.classes.DataFunctions.ExecuteScalar("connString", cmd);
        }

        private static int getResponseID(string responseValue, int responseGroupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = " select  ResponseID from Responses where  Response_Value=@responseValue and ResponseGroupID=@responseGroupID and MagazineID=@magazineID ";
            cmd.Parameters.AddWithValue("@responseGroupID", responseGroupID);
            cmd.Parameters.AddWithValue("@magazineID", magazineID);
            cmd.Parameters.AddWithValue("@responseValue", responseValue);
            return Convert.ToInt32(ecn.common.classes.DataFunctions.ExecuteScalar("connString", cmd));
        }

        private static void insertSubscriptionDetails(int SubscriptionID, IEnumerable<DataRow> subscriberProfile_DT, string responseGroupName, int responseGroupID)
        {
            var subscriptionDetails = (from sp in subscriberProfile_DT.AsEnumerable()
                                       select new
                                       {
                                           EmailDataValue = sp.Field<string>(responseGroupName)
                                       }).ToList();

            if (subscriptionDetails.Any())
            {
                DataTable responses_DT = getResponses();
                if (subscriptionDetails.First().EmailDataValue != null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string edv in subscriptionDetails.First().EmailDataValue.Split(','))
                    {
                        if (checkValidResponse(edv, responseGroupID))
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = " insert into SubscriptionDetails(SubscriptionID, ResponseID) "
                                             + " values (@SubscriptionID, @ResponseID)";
                            cmd.Parameters.AddWithValue("@SubscriptionID", SubscriptionID.ToString());
                            cmd.Parameters.AddWithValue("@ResponseID", getResponseID(edv, responseGroupID));
                            ecn.common.classes.DataFunctions.ExecuteScalar("connString", cmd);
                            FileFunctions.LogConsoleActivity("Updated SubscriptionDetails for SubscriptionID: " + SubscriptionID.ToString());
                            sb.Append(edv + ",");
                        }
                        else
                        {
                            FileFunctions.LogConsoleActivity("Invalid SubscriptionDetails for SubscriptionID: " + SubscriptionID.ToString() + " and EmailDataValue: " + edv);
                        }
                    }
                    char[] charsToTrim = { ',' };
                    string response = sb.ToString().TrimEnd(charsToTrim);
                    updateSubscription(SubscriptionID, responseGroupName, response);
                }
                else
                {
                    updateSubscription(SubscriptionID, responseGroupName, null);
                }
            }
        }

        private static void updateSubscription(int SubscriptionID, string responseGroupName, string response)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            if (response==null)
            {
                cmd.CommandText = " update Subscriptions set [" + responseGroupName + "]=NULL where SubscriptionID=@SubscriptionID and MagazineID=@MagazineID";
            }
            else
            {
                cmd.CommandText = " update Subscriptions set [" + responseGroupName + "]='" + response + "' where SubscriptionID=@SubscriptionID and MagazineID=@MagazineID";
            }
            cmd.Parameters.AddWithValue("@MagazineID", magazineID);
            cmd.Parameters.AddWithValue("@SubscriptionID", SubscriptionID);
            ecn.common.classes.DataFunctions.ExecuteScalar("connString", cmd);
            FileFunctions.LogConsoleActivity("Updated SubscriptionDetails Subscriptions in for SubscriptionID: " + SubscriptionID.ToString());
        }

        private static void insertSubscriber(DataTable emailDataValues_DT, string emailAddress, string firstName, string lastName, string state)
        {
            var subscriberProfile = (from edv in emailDataValues_DT.AsEnumerable()
                                     where (edv["emailAddress"] == DBNull.Value ? "" : edv["emailAddress"]).Equals(emailAddress)
                                        && (edv["firstName"] == DBNull.Value ? "" : edv["firstName"]).Equals(firstName)
                                        && (edv["lastName"] == DBNull.Value ? "" : edv["lastName"]).Equals(lastName)
                                        && (edv["state"] == DBNull.Value ? "" : edv["state"]).Equals(state)
                                     select edv).ToList().Distinct();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = " insert into Subscriptions(SubscriberID, MagazineID, EmailAddress, Fname, Lname, Company, Title, Address, City, State, Zip, Country, Phone, Fax, CreatedDate,Category_ID, Transaction_ID, Transaction_date, QualificationDate, QSource_ID) "
                             + " values (@SubscriberID, @MagazineID, @EmailAddress, @Fname, @Lname, @Company, @Title, @Address, @City, @State, @Zip, @Country, @Phone, @Fax, @CreatedDate, @categoryID,@transactionID, @transactionDate, @qualificationDate, @qsourceID);Select @@IDENTITY";
            cmd.Parameters.AddWithValue("@SubscriberID", subscriberProfile.First().Field<int>("EmailID").ToString()); ;
            cmd.Parameters.AddWithValue("@MagazineID", magazineID);
            cmd.Parameters.AddWithValue("@EmailAddress", subscriberProfile.First().Field<string>("EmailAddress").ToString().Contains("kmpsgroup.com") ? "" : subscriberProfile.First().Field<string>("EmailAddress").ToString());
            cmd.Parameters.AddWithValue("@Fname", ((subscriberProfile.First())["firstName"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("firstName")).ToString());
            cmd.Parameters.AddWithValue("@Lname", ((subscriberProfile.First())["lastName"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("lastName")).ToString());
            cmd.Parameters.AddWithValue("@Company", ((subscriberProfile.First())["company"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("company")).ToString());
            cmd.Parameters.AddWithValue("@Title", ((subscriberProfile.First())["title"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("title")).ToString());
            cmd.Parameters.AddWithValue("@Address", ((subscriberProfile.First())["Address"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("Address")).ToString() + " " + ((subscriberProfile.First())["Address2"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("Address2")).ToString());
            cmd.Parameters.AddWithValue("@City", ((subscriberProfile.First())["City"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("City")).ToString());
            cmd.Parameters.AddWithValue("@State", ((subscriberProfile.First())["state"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("state")).ToString());
            cmd.Parameters.AddWithValue("@Zip", ((subscriberProfile.First())["zip"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("zip")).ToString());
            cmd.Parameters.AddWithValue("@Country", ((subscriberProfile.First())["Country"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("Country")).ToString());
            cmd.Parameters.AddWithValue("@Phone", ((subscriberProfile.First())["Voice"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("Voice")).ToString());
            cmd.Parameters.AddWithValue("@Fax", ((subscriberProfile.First())["Fax"] == DBNull.Value ? "" : subscriberProfile.First().Field<string>("Fax")).ToString());
            cmd.Parameters.AddWithValue("@CreatedDate", ((subscriberProfile.First())["LastChanged"] == DBNull.Value ? subscriberProfile.First().Field<DateTime>("CreatedOn") : subscriberProfile.First().Field<DateTime>("LastChanged")).ToString());
            cmd.Parameters.AddWithValue("@categoryID", "10");
            cmd.Parameters.AddWithValue("@transactionID", "10");
            cmd.Parameters.AddWithValue("@transactionDate", ((subscriberProfile.First())["LastChanged"] == DBNull.Value ? subscriberProfile.First().Field<DateTime>("CreatedOn") : subscriberProfile.First().Field<DateTime>("LastChanged")).ToString());
            cmd.Parameters.AddWithValue("@qualificationDate", ((subscriberProfile.First())["LastChanged"] == DBNull.Value ? subscriberProfile.First().Field<DateTime>("CreatedOn") : subscriberProfile.First().Field<DateTime>("LastChanged")).ToString());
            cmd.Parameters.AddWithValue("@qsourceID", "3");
            int SubscriptionID = Convert.ToInt32(ecn.common.classes.DataFunctions.ExecuteScalar("connString", cmd));
            foreach (DataRow dr in responseGroups_DT.AsEnumerable())
            {
                insertSubscriptionDetails(SubscriptionID, subscriberProfile, dr["ResponseGroupName"].ToString(), Convert.ToInt32(dr["ResponseGroupID"].ToString()));
            }
        }

        private static bool checkValidResponse(string response, int responseGroupID)
        {
            var responseCheck = (from src in responses_DT.AsEnumerable()
                                 where src.Field<int>("ResponseGroupID") == responseGroupID
                                        && src.Field<string>("Response_Value") == response
                                 select src).ToList().Distinct();
            return responseCheck.Any() == true ? true : false;
        }

        private static bool checkSubscriberExists(int EmailID)
        {
            return Convert.ToInt32(ecn.common.classes.DataFunctions.ExecuteScalar("connString", "if exists (select top 1 SubscriptionID from Subscriptions where SubscriberID=" + EmailID.ToString() + " and MagazineID=" + magazineID + ") select 1 else select 0")) > 0 ? true : false;
        }

        private static int getSubscriptionID(int EmailID)
        {
            return Convert.ToInt32(ecn.common.classes.DataFunctions.ExecuteScalar("connString", "select SubscriptionID  from Subscriptions where SubscriberID=" + EmailID.ToString() + " and MagazineID=" + magazineID));
        }

        private static DataTable getResponses()
        {
            DataTable responses_DT = ecn.common.classes.DataFunctions.GetDataTable("select r.* from Responses r join ResponseGroups rg on r.ResponseGroupID=rg.ResponseGroupID where rg.MagazineID=" + magazineID);
            return responses_DT;
        }

        private static DataTable getResponseGroups()
        {
            DataTable responseGroups_DT = ecn.common.classes.DataFunctions.GetDataTable("select * from ResponseGroups where MagazineID=" + magazineID);
            return responseGroups_DT;
        }

        private static DataTable getECNData()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_GetGroupEmailProfilesWithUDF";

            cmd.Parameters.Add("@GroupID", SqlDbType.Int);
            cmd.Parameters["@GroupID"].Value = groupID;

            cmd.Parameters.Add("@CustomerID", SqlDbType.Int);
            cmd.Parameters["@CustomerID"].Value = customerID;

            cmd.Parameters.Add("@Filter", SqlDbType.Text);
            cmd.Parameters["@Filter"].Value = " and (isnull(LastChanged,CreatedOn) > CONVERT(VARCHAR(10),DATEADD(Day," + daysToProcess + ", GETDATE()),111))";

            cmd.Parameters.Add("@SubscribeType", SqlDbType.Text);
            cmd.Parameters["@SubscribeType"].Value = "'S'";

            DataTable dt = ecn.common.classes.DataFunctions.GetDataTable("communicator", cmd);

            return dt;
        }

        private static DataTable getSubscribers()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Subscriptions where MagazineID=@magazineID";
            cmd.Parameters.Add("@magazineID", SqlDbType.Int);
            cmd.Parameters["@magazineID"].Value = magazineID;
            return ecn.common.classes.DataFunctions.GetDataTable("connString", cmd);
        }

        private static DateTime? getSubscriptions_LastUpdateDate(int SubscriptionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select CreatedDate, UpdatedDate from Subscriptions s where s.SubscriptionID= @SubscriptionID and s.MagazineID=@Magazineid";
            cmd.Parameters.Add("@SubscriptionID", SqlDbType.Int);
            cmd.Parameters["@SubscriptionID"].Value = SubscriptionID;
            cmd.Parameters.Add("@Magazineid", SqlDbType.Int);
            cmd.Parameters["@Magazineid"].Value = magazineID;
            DataTable dt = ecn.common.classes.DataFunctions.GetDataTable("connString", cmd);
            if (dt.Rows.Count > 0)
            {
                DateTime? createdDate = Convert.ToDateTime(dt.Rows[0][0].ToString());
                DateTime? updatedDate = null;
                if (dt.Rows[0][1] != null && dt.Rows[0][1].ToString() != string.Empty)
                    updatedDate = Convert.ToDateTime(dt.Rows[0][1].ToString());
                if (updatedDate != null)
                    return Convert.ToDateTime(dt.Rows[0][1].ToString());
                else
                    return Convert.ToDateTime(dt.Rows[0][0].ToString());
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}