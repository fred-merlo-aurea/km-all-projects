using System;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using ecn.common.classes;
using System.Data.SqlClient;
using ecn.webservice.classes;

namespace ecn.webservice.CustomAPI
{
    /// <summary>
    /// Summary description for PharmaliveServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class PharmaliveServices : System.Web.Services.WebService
    {

        private readonly string AccessKey = "2E3435A3-D2FA-4DF3-9A06-F464DCDBAE3F"; //hardcoded for pharmalive account

        [WebMethod(Description = "Provides Authentication for PharmaLive Account. Parameters passed are username & password .<br>- Returns UserID or 0(failed login).")]
        public string Login(string ecnAccessKey, string username, string password)
        {
            int userID = 0;

            if (ecnAccessKey == AccessKey)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT emailID FROM ecn5_communicator..emails WHERE CustomerID = 2069 and emailaddress=@EmailAddress and isnull(password,'') = @Password";
                cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar).Value = username;
                cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = password;

                try
                {
                    userID = Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", cmd));

                    if (userID == 0)
                    {
                        return SendResponse.response("Login", SendResponse.ResponseCode.Fail, 0, "LOGIN FAILED");
                    }
                    else
                    {
                        return SendResponse.response("Login", SendResponse.ResponseCode.Success, userID, "UserID");
                    }
                }
                catch
                {
                    return SendResponse.response("Login", SendResponse.ResponseCode.Fail, 0, "UNKNOWN ERROR");
                }
            }
            else
            {
                return SendResponse.response("Login", SendResponse.ResponseCode.Fail, 0, "AUTHENTICATION FAILED");
            }

        }

        //[WebMethod(Description = "returns subscribed Premium newsletters - delimited by comma(,)")]
        //public string getPremiumNewsletters(int userID)
        //{
        //    string groupIDs = string.Empty;

        //    try
        //    {
        //        string Pharmalive_SubscriptionGroupsIDs = DataFunctions.ExecuteScalar("communicator", "select SubscriptionGroupIDs from SmartFormsHistory where SmartformID = " + ConfigurationManager.AppSettings["Pharmalive_SubscriptionGroups_SmartFormID"].ToString()).ToString();

        //        DataTable dtGroups = DataFunctions.GetDataTable("select g.groupID, datavalue from emaildatavalues edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID join groups g on g.groupID =gdf.groupID join emailgroups eg on eg.emailID = edv.emailID and eg.groupID = g.groupID where edv.emailID =  " + userID + " and shortname = 'PaidOrFree' and datavalue in ('PAID','COMP','TRIAL') and subscribetypecode='S' and g.groupID in (" + Pharmalive_SubscriptionGroupsIDs + ")", ConfigurationManager.AppSettings["com"].ToString());

        //        foreach (DataRow dr in dtGroups.Rows)
        //        {
        //            if (dr["datavalue"].ToString() == "COMP")
        //            {
        //                groupIDs += (groupIDs == string.Empty ? dr["groupID"].ToString() : "," + "</groupIDs>" + dr["groupID"].ToString() + "</groupIDs>");
        //            }
        //            else if (dr["datavalue"].ToString() == "TRIAL")
        //            {
        //                try
        //                {
        //                    DateTime dtEndDate = Convert.ToDateTime(DataFunctions.ExecuteScalar("communicator", "select max(datavalue) from emaildatavalues edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID where emailID = " + userID + " and groupID = " + dr["groupID"].ToString() + " and shortname = 'enddate'").ToString());

        //                    if (DateTime.Now <= dtEndDate)
        //                    {
        //                        groupIDs += (groupIDs == string.Empty ? dr["groupID"].ToString() : "," + dr["groupID"].ToString());
        //                    }
        //                }
        //                catch
        //                { }
        //            }
        //            else if (dr["datavalue"].ToString() == "PAID")
        //            {
        //                try
        //                {
        //                    DataTable dtDates = DataFunctions.GetDataTable("select  entryID, max(case when shortname = 'startdate' then datavalue end) as startdate, max(case when shortname = 'enddate' then datavalue end)  as enddate from emaildatavalues edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID where emailID = " + userID + " and entryID is not null and groupID = " + dr["groupID"].ToString() + " and shortname in ('startdate', 'enddate') group by entryID", ConfigurationManager.AppSettings["com"].ToString());

        //                    foreach (DataRow r in dtDates.Rows)
        //                    {
        //                        DateTime StartDate = Convert.ToDateTime(r["startdate"].ToString());
        //                        DateTime EndDate = Convert.ToDateTime(r["enddate"].ToString());

        //                        if (DateTime.Now >= StartDate && DateTime.Now <= EndDate)
        //                        {
        //                            groupIDs += (groupIDs == string.Empty ? dr["groupID"].ToString() : "," + dr["groupID"].ToString());
        //                            break;
        //                        }
        //                    }
        //                }
        //                catch
        //                { }
        //            }
        //        }
        //    }
        //    catch
        //    { }

        //    return groupIDs;
        //}

        [WebMethod(Description = "returns subscribed newsletters groupIDs- delimited by comma(,)")]
        public string getSubcribedNewsletters(string ecnAccessKey, string emailaddress)
        {
            string groupIDs = string.Empty;

            if (ecnAccessKey == AccessKey) //Hardcoded for PHARMALIVE.
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT emailID FROM ecn5_communicator..emails WHERE CustomerID = 2069 and emailaddress=@Emailaddress";
                    cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar).Value = emailaddress;

                    int EmailID = Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", cmd));

                    string Pharmalive_SubscriptionGroupsIDs = DataFunctions.ExecuteScalar("communicator", "select SubscriptionGroupIDs from SmartFormsHistory where SmartformID = " + ConfigurationManager.AppSettings["Pharmalive_SubscriptionGroups_SmartFormID"].ToString()).ToString();

                    DataTable dtGroups = DataFunctions.GetDataTable("select g.groupID, datavalue from emaildatavalues edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID join groups g on g.groupID =gdf.groupID join emailgroups eg on eg.emailID = edv.emailID and eg.groupID = g.groupID where edv.emailID =  " + EmailID + " and shortname = 'PaidOrFree' and subscribetypecode='S' and g.groupID in (" + Pharmalive_SubscriptionGroupsIDs + ")", ConfigurationManager.AppSettings["com"].ToString());

                    foreach (DataRow dr in dtGroups.Rows)
                        groupIDs += (groupIDs == string.Empty ? dr["groupID"].ToString() : "," + dr["groupID"].ToString());

                    return SendResponse.response("getSubcribedNewsletters", SendResponse.ResponseCode.Success, 0, groupIDs);
                }
                catch
                {
                    return SendResponse.response("getSubcribedNewsletters", SendResponse.ResponseCode.Fail, 0, "UNKNOWN ERROR.");
                }
            }
            else
            {
                return SendResponse.response("getSubcribedNewsletters", SendResponse.ResponseCode.Fail, 0, "AUTHENTICATION FAILED");
            }
        }
    }
}
