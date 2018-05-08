using ecn.communicator.classes;
using KM.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;

namespace KMPS_JF_Objects.Objects
{
    public class ECNUtils
    {
        public static string GetSubscriberID(int groupID, int custID, string email)
        {
            string subscriberID = string.Empty;

            try
            {
                if (custID > 0 && groupID > 0)
                {
                    SqlCommand cmdGetSubscriberID = new SqlCommand("sp_GetSubscriberIDByEmailAndGroup");
                    cmdGetSubscriberID.CommandType = CommandType.StoredProcedure;
                    cmdGetSubscriberID.Parameters.Add(new SqlParameter("@emailAddress", SqlDbType.VarChar)).Value = email;
                    cmdGetSubscriberID.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;
                    cmdGetSubscriberID.Parameters.Add(new SqlParameter("@CustID", SqlDbType.Int)).Value = custID;
                    subscriberID = DataFunctions.ExecuteScalar("communicator", cmdGetSubscriberID).ToString();
                    return subscriberID;
                }
            }
            catch 
            {
                return "0";
            }

            return "0";
        }

        private static void TrackHttpPost(string emailaddress, string pubcode, string postparams, string browserInfo)
        {
            try
            {
                SqlCommand cmdHttpPost = new SqlCommand("insert into TrackHttpPost (EmailAddress, Pubcode, PostData, BrowserInfo) values ( @EmailAddress, @PubCode, @PostParams, @BrowserInfo)");
                cmdHttpPost.CommandType = CommandType.Text;
                cmdHttpPost.Parameters.AddWithValue("@EmailAddress", emailaddress);
                cmdHttpPost.Parameters.AddWithValue("@PubCode", pubcode);
                cmdHttpPost.Parameters.AddWithValue("@PostParams", postparams);
                cmdHttpPost.Parameters.AddWithValue("@BrowserInfo", browserInfo);
                DataFunctions.Execute(cmdHttpPost);
            }
            catch
            { }

        }

        public static bool ECNHttpPost(string emailaddress, string pubcode, string postparams, string browserInfo)
        {
            TrackHttpPost(emailaddress, pubcode, postparams, browserInfo);


            postparams = postparams.Replace("#", "");
            string requestURIString = string.Empty;
            requestURIString = string.Format(ConfigurationManager.AppSettings["ECNSubscribeURL"].ToString(), "?" + postparams);

            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(requestURIString);
                webRequest.Method = "GET";
                HttpWebResponse WebResp = (HttpWebResponse)webRequest.GetResponse();

                if (WebResp.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    string emailMsg = "Error in Posting the Data to ECN.<br /><br />";
                    emailMsg += "<b>URL:</b>" + requestURIString + "<br /><br />";
                    Utilities.SendMail(emailMsg);
                    return false;
                }
            }
            catch (Exception ex)
            {
                string emailMsg = "Error in Posting the Data to ECN.<br /><br />";
                emailMsg += "<b>URL:</b>" + requestURIString + "<br /><br />";
                emailMsg += "<b>Exception Details:</b>" + ex.Message;
                Utilities.SendMail(emailMsg);
                return false;
            }
        }

        private static string CleanXMLString(string text)
        {
            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            return text.Trim();
        }

        //subscribe email to the group
        public static void SubscribeToGroup(int customerID, int groupID, string pubcode, Dictionary<string, string> ProfileandUDFdata, string browserInfo)
        {
            string postparams = string.Empty;

            try
            {
                postparams = string.Join("&", ProfileandUDFdata.Select(x => string.Format("{0}={1}", x.Key, x.Value)).ToArray());

                TrackHttpPost(ProfileandUDFdata["emailaddress"], pubcode, postparams, browserInfo);
            }
            catch
            { }

            try
            {
                Dictionary<string, string> ECNFields = GetECNProfileFields();
                Dictionary<int, string> UDFFields = GetGroupDataFields(groupID);

                string ECNPostEmailAddress = ProfileandUDFdata["emailaddress"];

                StringBuilder xmlProfile = new StringBuilder("");
                StringBuilder xmlUDF = new StringBuilder("");

                xmlProfile.Append("<Emails>");
                xmlProfile.Append("<subscribetypecode>S</subscribetypecode>");

                foreach (KeyValuePair<string, string> data in ProfileandUDFdata)
                {
                    try
                    {
                        if (ECNFields.ContainsKey(data.Key))
                        {
                            string keyValue = Convert.ToString(ProfileandUDFdata[data.Key]);

                            if (data.Key.ToString().ToLower() == "voice")
                            {
                                keyValue.Replace("(", "");
                                keyValue.Replace(")", "");
                                keyValue.Replace("-", "");
                                keyValue.Replace(" ", "");
                            }

                            //if (data.Key.ToString().ToLower() == "notes")
                            //    xmlProfile.Append("<" + data.Key + ">" + "<![CDATA[ [" + ECNPostfromIP + "] [" + ECNPostfromURL + "] [" + DateTime.Now.ToString() + "] ]]> " + "</" + data.Key + ">");
                            //else
                                xmlProfile.Append("<" + data.Key + ">" + CleanXMLString(keyValue) + "</" + data.Key + ">");
                        }
                    }
                    catch
                    { }
                }

                xmlProfile.Append("</Emails>");

                if (UDFFields.Count > 0)
                {
                    xmlUDF.Append("<row>");
                    xmlUDF.Append("<ea>" + ECNPostEmailAddress + "</ea>");

                    foreach (KeyValuePair<string, string> data in ProfileandUDFdata)
                    {
                        try
                        {
                            if (UDFFields.ContainsValue(data.Key.ToLower()))
                            {
                                try
                                {
                                    xmlUDF.Append("<udf id=\"" + UDFFields.FirstOrDefault(x => x.Value == data.Key.ToLower()).Key  + "\"><v>" + CleanXMLString(data.Value) + "</v></udf>");
                                }
                                catch
                                { }
                            }
                        }
                        catch
                        { }
                    }
                    xmlUDF.Append("</row>");
                }

                UpdateToDB(customerID, groupID, xmlProfile.ToString(), xmlUDF.ToString());


            }
            catch (Exception ex)
            {
                SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"].ToString(), ConfigurationManager.AppSettings["Admin_FromEmail"].ToString(), "Paidforms - DB Update Failed in SubscribeToGroup method", "<br/>Exception Message<br/>" + ex.Message + "<br/>Stack Trace<br/>" + ex.StackTrace + "<br/>URL<br/>Data<br/>" + postparams); // + Request.RawUrl
                throw ex;
            }
        }

        //Save the profile and UDF for given XML
        private static void UpdateToDB(int customerID, int groupID, string xmlProfile, string xmlUDF)
        {
            int ECNUserID = GetECNUserIDFromAccessKey();

            //SqlCommand cmd = new SqlCommand("sp_ImportEmails");
            SqlCommand cmd = new SqlCommand("e_EmailGroup_ImportEmails");
            try
            {
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CustomerID", SqlDbType.VarChar);
                cmd.Parameters["@CustomerID"].Value = customerID;

                cmd.Parameters.Add("@GroupID", SqlDbType.VarChar);
                cmd.Parameters["@GroupID"].Value = groupID;

                cmd.Parameters.Add("@xmlProfile", SqlDbType.Text);
                cmd.Parameters["@xmlProfile"].Value = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile.ToString() + "</XML>";

                cmd.Parameters.Add("@xmlUDF", SqlDbType.Text);
                cmd.Parameters["@xmlUDF"].Value = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDF + "</XML>";

                cmd.Parameters.Add("@formattypecode", SqlDbType.VarChar);
                cmd.Parameters["@formattypecode"].Value = "html";

                cmd.Parameters.Add("@subscribetypecode", SqlDbType.VarChar);
                cmd.Parameters["@subscribetypecode"].Value = "s";

                cmd.Parameters.Add("@EmailAddressOnly", SqlDbType.Bit);
                cmd.Parameters["@EmailAddressOnly"].Value = 0;

                cmd.Parameters.Add("@UserID", SqlDbType.VarChar);
                cmd.Parameters["@UserID"].Value = ECNUserID;

                cmd.Parameters.Add("@source", SqlDbType.VarChar);
                cmd.Parameters["@source"].Value = "KMPS_JF.Subscriptions.UpdateToDB method";

                cmd.Parameters.Add("@insertMS", SqlDbType.Bit);
                cmd.Parameters["@insertMS"].Value = "true";

                DataFunctions.Execute("communicator", cmd);

            }
            catch (Exception ex)
            {
                SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"].ToString(), ConfigurationManager.AppSettings["Admin_FromEmail"].ToString(), "Paidforms - DB Update Failed in UpdateToDB method", ex.Message + "<br/>" + xmlProfile + "<br/>" + xmlUDF);
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        private static void SimpleSend(string toemail, string fromemail, string subject, string message)
        {
            ecn.communicator.classes.EmailFunctions emailFunctions = new ecn.communicator.classes.EmailFunctions();
            emailFunctions.SimpleSend(toemail, fromemail, subject, message);

        }

        //Load all UDF Fields for the group
        private static Dictionary<int, string> GetGroupDataFields(int groupID)
        {
            SqlCommand cmdsqlstmt = new SqlCommand("SELECT * FROM GroupDatafields  with (NOLOCK) WHERE GroupID = @groupID");
            cmdsqlstmt.CommandType = CommandType.Text;
            cmdsqlstmt.Parameters.Add(new SqlParameter("@groupID", SqlDbType.Int)).Value = groupID;

            //string sqlstmt = " SELECT * FROM GroupDatafields WHERE GroupID=" + groupID;
            DataTable emailstable = DataFunctions.GetDataTable("communicator", cmdsqlstmt);

            Dictionary<int, string> fields = new Dictionary<int, string>();

            foreach (DataRow dr in emailstable.Rows)
                fields.Add(Convert.ToInt32(dr["GroupDataFieldsID"]), dr["ShortName"].ToString().ToLower());

            return fields;
        }

        //Load profile fields
        public static Dictionary<string, string> GetECNProfileFields()
        {
            Dictionary<string, string> ECNFields = new Dictionary<string, string>();

            ECNFields.Add("emailaddress", "e");
            ECNFields.Add("title", "t");
            ECNFields.Add("firstname", "fn");
            ECNFields.Add("lastname", "ln");
            ECNFields.Add("fullname", "n");
            ECNFields.Add("company", "compname");
            ECNFields.Add("occupation", "t");
            ECNFields.Add("address", "adr");
            ECNFields.Add("address2", "adr2");
            ECNFields.Add("city", "city");
            ECNFields.Add("state", "state");
            ECNFields.Add("zip", "zc");
            ECNFields.Add("country", "ctry");
            ECNFields.Add("voice", "ph");
            ECNFields.Add("mobile", "mph");
            ECNFields.Add("fax", "fax");
            ECNFields.Add("website", "website");
            ECNFields.Add("age", "age");
            ECNFields.Add("income", "income");
            ECNFields.Add("gender", "gndr");
            ECNFields.Add("user1", "usr1");
            ECNFields.Add("user2", "usr2");
            ECNFields.Add("user3", "usr3");
            ECNFields.Add("user4", "usr4");
            ECNFields.Add("user5", "usr5");
            ECNFields.Add("user6", "usr6");
            ECNFields.Add("birthdate", "bdt");
            ECNFields.Add("userevent1", "usrevt1");
            ECNFields.Add("userevent1date", "usrevtdt1");
            ECNFields.Add("userevent2", "usrevt2");
            ECNFields.Add("userevent2date", "usrevtdt2");
            ECNFields.Add("notes", "notes");
            ECNFields.Add("password", "password");

            return ECNFields;
        }

        public static int GetECNUserIDFromAccessKey()
        {
            try
            {
                return GetUserFromECNAccessKey().UserID;
            }
            catch
            {
                return 0;
            }
        }

        public static KMPlatform.Entity.User GetUserFromECNAccessKey()
        {
            KMPlatform.Entity.User User = null;

            string ECNEngineAccessKey = ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString();

            try
            {
                if (CacheUtil.IsCacheEnabled())
                {
                    User = (KMPlatform.Entity.User)CacheUtil.GetFromCache("ECNUSER_" + ECNEngineAccessKey, "JOINTFORMS");

                    if (User == null)
                    {
                        User = KMPlatform.BusinessLogic.User.GetByAccessKey(ECNEngineAccessKey, false);

                        CacheUtil.AddToCache("ECNUSER_" + ECNEngineAccessKey, User, "JOINTFORMS");
                    }
                }
                else
                {
                    User = KMPlatform.BusinessLogic.User.GetByAccessKey(ECNEngineAccessKey, false);
                }
            }
            catch
            {
            }

            return User;
        }
    }

   

}
