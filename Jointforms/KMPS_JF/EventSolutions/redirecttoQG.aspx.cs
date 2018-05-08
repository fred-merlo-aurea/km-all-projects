using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Collections;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Xml;
using System.Xml.XPath;

namespace KMPS_JF.EventSolutions
{
    public partial class redirecttoQG : System.Web.UI.Page
    {
        //private string _PubCode
        //{
        //    get
        //    {
        //        try { return Request.QueryString["pub_code"].ToString(); }
        //        catch { return string.Empty; }
        //    }
        //}
        private bool _Add
        {
            get
            {
                try { return Convert.ToBoolean(Request.QueryString["add"].ToString()); }
                catch { return true; }
            }
        }

        private int _PublisherID
        {
            get
            {
                try { return Convert.ToInt32(Request.QueryString["btx_pub_id"].ToString()); }
                catch { return 0; }
            }
        }

        private int _PublicationID
        {
            get
            {
                try { return Convert.ToInt32(Request.QueryString["btx_m"].ToString()); }
                catch { return 0; }
            }
        }
        private int _IssueID
        {
            get
            {
                int tempIssueID;
                try 
                {
                    tempIssueID = Convert.ToInt32(Request.QueryString["btx_i"].ToString());                    
                }
                catch 
                { 
                    tempIssueID = 0; 
                }
                if (tempIssueID == 0)
                {
                    tempIssueID = GetLatestIssue();
                }
                return tempIssueID;
            }
        }
        private int _EmailID
        {
            get
            {
                try { return Convert.ToInt32(Request.QueryString["email_id"].ToString()); }
                catch { return 0; }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ValidateParameters())
            {
                try
                {
                    DataTable dt = GetSubscriberInfo();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string loginParams = string.Empty;
                        if (_Add)
                        {
                            SaveAutoLoginParams(dt);
                        }
                        loginParams = GetAutoLoginParams();

                        //email existed but we didn't yet have params stored
                        if (loginParams == string.Empty)
                        {
                            SaveAutoLoginParams(dt);
                            loginParams = GetAutoLoginParams();
                        }
                        
                        Response.Redirect(ConfigurationManager.AppSettings["LoginCheck"].ToString() + loginParams);
                    }
                }
                catch 
                {                       
                }
            }
        }

        private bool ValidateParameters()
        {
            if (_EmailID <= 0)
            {
                return false;
            }
            if (_IssueID <= 0)
            {
                return false;
            }
            if (_PublicationID <= 0)
            {
                return false;
            }
            if (_PublisherID <= 0)
            {
                return false;
            }
            //if (_PubCode == string.Empty)
            //{
            //    return false;
            //}

            return true;
        }

        private string GetAutoLoginParams()
        {
            string loginParams = string.Empty;
            string sqlSelect = string.Empty;
            sqlSelect = "SELECT AutoLoginParams " +
                        "FROM ecn_misc..QGLoginParams " +
                        "WHERE EmailID = @EmailID AND Publisher = @Publisher AND Magazine = @Magazine AND Issue = @Issue";
            SqlCommand cmd = new SqlCommand(sqlSelect);
            cmd.Parameters.Add(new SqlParameter("@EmailID", _EmailID));
            cmd.Parameters.Add(new SqlParameter("@Publisher", _PublisherID.ToString()));   
            cmd.Parameters.Add(new SqlParameter("@Magazine", _PublicationID.ToString()));   
            cmd.Parameters.Add(new SqlParameter("@Issue", _IssueID.ToString()));
            try
            {
                loginParams = DataFunctions.ExecuteScalar(cmd).ToString();
            }
            catch (Exception)
            {
            }
            return loginParams;
        }

        private void SaveAutoLoginParams(DataTable dt)
        {
            string xmlString = CreateImportXML(dt);
            string loginParams = GetAutoLoginParamsFromXML(xmlString).Trim();                                      
            SqlCommand cmd = new SqlCommand("ecn_misc..spQGLoginParam");
            cmd.CommandType = CommandType.StoredProcedure; 
            cmd.Parameters.Add(new SqlParameter("@EmailID", _EmailID));
            cmd.Parameters.Add(new SqlParameter("@AutoLoginParams", loginParams));
            cmd.Parameters.Add(new SqlParameter("@Publisher", _PublisherID.ToString()));
            cmd.Parameters.Add(new SqlParameter("@Magazine", _PublicationID.ToString()));
            cmd.Parameters.Add(new SqlParameter("@Issue", _IssueID.ToString()));
            DataFunctions.Execute(cmd);   
        }

        private int GetLatestIssue()
        {
            int issueID = 0;
            try
            {
                ASCIIEncoding encoder = new ASCIIEncoding();    
                XmlDocument xmlReply = null;

                // Post the <messageRequest> element to the conference center;
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["LatestIssue"].ToString() + _PublicationID.ToString());
                myReq.Method = "POST";
                myReq.ContentType = "application/x-www-form-urlencoded";   

                HttpWebResponse myResp = null;
                myResp = (HttpWebResponse)myReq.GetResponse();
                Stream respStream = myResp.GetResponseStream();
                if (myResp.ContentType == "text/xml; charset=UTF-8")
                {
                    xmlReply = new XmlDocument();
                    xmlReply.Load(respStream);
                }
                if (myResp != null)
                    myResp.Close();
                //// Create the XmlDocument.                
                XmlElement rootNode = xmlReply.DocumentElement;
                XmlNodeList issueList = rootNode.SelectNodes("issue");
                foreach (XmlNode issueNode in issueList)
                {
                    XmlAttributeCollection attCol = issueNode.Attributes;
                    XmlAttribute issueAtrib = attCol["issueid"];
                    issueID = Convert.ToInt32(issueAtrib.Value.ToString());

                    break;
                }
            }
            catch (Exception)
            {
            }
            return issueID;
        }
        
        private string GetAutoLoginParamsFromXML(string xmlString)
        {
            string autoLoginParams = string.Empty;

            try
            {
                //try original
                ASCIIEncoding encoder = new ASCIIEncoding();    
                XmlDocument xmlReply = null;

                // Post the <messageRequest> element to the conference center;
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["SubscriptionImport"].ToString());
                myReq.Method = "POST";
                myReq.ContentType = "application/x-www-form-urlencoded";   

                HttpWebResponse myResp = null;
                // Write the XML message to the request stream and
                // send it to the conference center
                byte[] byteArray = encoder.GetBytes(xmlString);
                myReq.ContentLength = byteArray.Length;
                Stream reqStream = myReq.GetRequestStream();
                reqStream.Write(byteArray, 0, byteArray.Length);
                reqStream.Close();

                // Get the response from the conference center
                myResp = (HttpWebResponse)myReq.GetResponse();
                Stream respStream = myResp.GetResponseStream();
                if (myResp.ContentType == "text/xml; charset=UTF-8")
                {
                    xmlReply = new XmlDocument();
                    xmlReply.Load(respStream);
                }
                if (myResp != null)
                    myResp.Close();
                //// Create the XmlDocument.                
                XmlElement rootNode = xmlReply.DocumentElement;
                XmlNode loginNode = rootNode.SelectSingleNode("subscriber/auto_login_post_string");    
                autoLoginParams = loginNode.InnerText;  
            }
            catch (Exception)
            {
                Response.Write("Error using login, please contact Customer Service");
            }

            return autoLoginParams;
        }

        private DataTable GetSubscriberInfo()
        {
            string xmlString = string.Empty;
            string sqlSelect = string.Empty;
            sqlSelect = "SELECT top 1 e.EmailID, e.EmailAddress, e.FirstName, e.LastName, e.Address, e.Address2, e.City, e.State, e.Country, e.Zip, e.Voice, e.Fax, e.Company " +
                        "FROM ecn5_communicator..Emails e " +
                        "WHERE e.EmailID = @EmailID";
            SqlCommand cmd = new SqlCommand(sqlSelect);
            cmd.Parameters.Add(new SqlParameter("@EmailID", _EmailID));
            DataTable dt = null;
            try
            {
                dt = DataFunctions.GetDataTable(cmd);
            }
            catch (Exception)
            {
            }

            return dt;
        }

        private string CreateImportXML(DataTable dt)
        {
            string xmlString = string.Empty;
            string user = ConfigurationManager.AppSettings["User_" + _PublicationID.ToString()].ToString();
            string pass = ConfigurationManager.AppSettings["Pass_" + _PublicationID.ToString()].ToString();
            string subscriptionID = ConfigurationManager.AppSettings["SubscriptionID_" + _PublicationID.ToString()].ToString();

            try
            {
                xmlString = @"<subscription_import user=""";
                xmlString += user;
                xmlString += @""" pass=""";
                xmlString += pass;
                xmlString += @""" method=""insert"" subscription_id=""";
                xmlString += subscriptionID;
                xmlString += @""">";
                xmlString += "<subscriber>" +
                                "<email>" + dt.Rows[0]["EmailAddress"].ToString().Trim() + "</email>" +
                                "<issue_id>" + _IssueID.ToString() + "</issue_id>" +
                                "<host>onlinedigitalpubs.com</host>";//what is host?
                if (dt.Rows[0]["FirstName"] != null)
                {
                    xmlString += "<fname>" + dt.Rows[0]["FirstName"].ToString().Trim() + "</fname>";
                }
                else
                {
                    xmlString += "<fname></fname>";
                }
                if (dt.Rows[0]["LastName"] != null)
                {
                    xmlString += "<lname>" + dt.Rows[0]["LastName"].ToString().Trim() + "</lname>";
                }
                else
                {
                    xmlString += "<lname></lname>";
                }
                if (dt.Rows[0]["City"] != null)
                {
                    xmlString += "<city>" + dt.Rows[0]["City"].ToString().Trim() + "</city>";
                }
                else
                {
                    xmlString += "<city></city>";
                }
                if (dt.Rows[0]["State"] != null)
                {
                    xmlString += "<state>" + dt.Rows[0]["State"].ToString().Trim() + "</state>";
                }
                else
                {
                    xmlString += "<state></state>";
                }
                if (dt.Rows[0]["Country"] != null)
                {
                    xmlString += "<country>" + dt.Rows[0]["Country"].ToString().Trim() + "</country>";
                }
                else
                {
                    xmlString += "<country></country>";
                }
                if (dt.Rows[0]["Zip"] != null)
                {
                    xmlString += "<zip>" + dt.Rows[0]["Zip"].ToString().Trim() + "</zip>";
                }
                else
                {
                    xmlString += "<zip></zip>";
                }
                if (dt.Rows[0]["Address"] != null)
                {
                    xmlString += "<address>" + dt.Rows[0]["Address"].ToString().Trim() + "</address>";
                }
                else
                {
                    xmlString += "<address></address>";
                }
                if (dt.Rows[0]["Address2"] != null)
                {
                    xmlString += "<address2>" + dt.Rows[0]["Address2"].ToString().Trim() + "</address2>";
                }
                else
                {
                    xmlString += "<address2></address2>";
                }
                if (dt.Rows[0]["Voice"] != null)
                {
                    xmlString += "<phone>" + dt.Rows[0]["Voice"].ToString().Trim() + "</phone>";
                }
                else
                {
                    xmlString += "<phone></phone>";
                }
                if (dt.Rows[0]["Fax"] != null)
                {
                    xmlString += "<fax>" + dt.Rows[0]["Fax"].ToString().Trim() + "</fax>";
                }
                else
                {
                    xmlString += "<fax></fax>";
                }
                DateTime today = DateTime.Today;
                xmlString += "<begindate>" + today.Year.ToString() + "-" + today.Month.ToString() + "-" + today.Day.ToString() + "</begindate>";
                xmlString += "<expiration>0000-00-00</expiration>";//are we expiring?
                if (dt.Rows[0]["Company"] != null)
                {
                    xmlString += "<company>" + dt.Rows[0]["Company"].ToString().Trim() + "</company>";
                }
                else
                {
                    xmlString += "<company></company>";
                }
                //what do we put for hearfrom?
                xmlString += "<hearfrom>KM</hearfrom></subscriber></subscription_import>";
            }
            catch (Exception)
            {
            }
            xmlString = xmlString.Replace("&", "&amp;");
            return xmlString;
        }
    }


}
