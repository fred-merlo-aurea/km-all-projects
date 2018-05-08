using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Text;
using ecn.webservice.classes;
using System.Configuration;
using ecn.webservice.IssueAccess;
using System.Web.Services.Protocols;
using System.ComponentModel;


namespace ecn.webservice.CustomAPI
{
    /// <summary>
    /// Summary description for WATTAPI
    /// </summary>
    [WebService(Namespace = "http://webservices.ecn5.com/")]

    public class WATTAPI : System.Web.Services.WebService
    {
        private int? LogID = null;
        private ECN_Framework_Entities.Communicator.APILogging Log = null;
        private string ServiceName = "ecn.webservice.WATTAPI.";
        private string MethodName = string.Empty;

        public WATTAPI()
        {
            InitializeComponent();
        }
        [WebMethod(Description = "Will return a token for the specified IssueID for the specified Email Address within the specified group")]
        public string GetTokenForSubscriber(string accessKey, int GroupID, string EmailAddress, int issueID)
        {
            try
            {
                KMPlatform.Entity.User myUser = KMPlatform.BusinessLogic.User.ECN_GetByAccessKey(accessKey, true);
                myUser.CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(myUser.DefaultClientID, false).CustomerID;
                MethodName = "GetTokenForSubscriber";
                Log = new ECN_Framework_Entities.Communicator.APILogging();
                Log.AccessKey = accessKey;
                Log.APIMethod = ServiceName + MethodName;
                Log.Input = "<ROOT><GROUPID>" + GroupID.ToString() + "</GROUPID><EMAILADDRESS>" + EmailAddress + "</EMAILADDRESS><ISSUEID>" + issueID.ToString() + "</ISSUEID></ROOT>";
                Log.APILogID = ECN_Framework_BusinessLayer.Communicator.APILogging.Insert(Log);

                if (myUser != null && myUser.UserID > 0)
                {
                    string retToken = string.Empty;
                    retToken = ECN_Framework_BusinessLayer.Communicator.EmailDataValues.WATT_GetTokenForSubscriber(EmailAddress, "Mozaic_Issue_" + issueID.ToString(), GroupID, myUser.CustomerID);

                    if (!string.IsNullOrEmpty(retToken))
                    {
                        ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                        return SendResponse.response("GetTokenForSubscriber", SendResponse.ResponseCode.Success, 0, retToken);
                    }
                    else
                    {
                        ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                        return SendResponse.response("GetTokenForSubscriber", SendResponse.ResponseCode.Fail, 0, "No token for Email Address");
                    }
                    
                }
                else
                {
                    ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                    return SendResponse.response("GetTokenForSubscriber", SendResponse.ResponseCode.Fail, 0, "Invalid Access Key: " + accessKey);
                }
            }
            catch (ECN_Framework_Common.Objects.ECNException ecnEX)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, ECN_Framework_Common.Objects.ECNException.CreateErrorMessage(ecnEX));
            }
            catch (ECN_Framework_Common.Objects.SecurityException)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "SECURITY VIOLATION");
            }
            catch (Exception ex)
            {
                LogID = LogUnspecifiedException(ex, ServiceName + MethodName);
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, LogID);
                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, ex.ToString());
            }

        }

        [WebMethod(Description = "Will return the URL for the specified Issue")]
        public string GetIssueURL(string accesskey, string IssueID)
        {
            MethodName = "GetIssueURL";
            Log = new ECN_Framework_Entities.Communicator.APILogging();
            Log.AccessKey = accesskey;
            Log.APIMethod = ServiceName + MethodName;
            Log.Input = "<ROOT><ISSUEID>" + IssueID.ToString() + "</ISSUEID></ROOT>";
            Log.APILogID = ECN_Framework_BusinessLayer.Communicator.APILogging.Insert(Log);

            string thirdPartyID = ConfigurationManager.AppSettings["WATT_FSID"].ToString();
            IssueAccess.IssueAccessClient iac = new IssueAccessClient();
            IssueAccess.ThirdPartyEditionUrlRequest tpeur = new ThirdPartyEditionUrlRequest();
            tpeur.ThirdPartyID = thirdPartyID;
            tpeur.ThirdPartyEditionID = IssueID;
            tpeur.TokenPlaceholder = "{{TokenHere}}";
            IssueAccess.EditionUrl url = new EditionUrl();
            try
            {
                try
                {
                    url = iac.GetEditionUrl(tpeur);
                }
                catch (Exception ex)
                {
                    KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, ServiceName + MethodName, Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                    ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                    return SendResponse.response("GetIssueURL", SendResponse.ResponseCode.Fail, 0, "Could not get URL");
                }
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                return SendResponse.response("GetIssueURL", SendResponse.ResponseCode.Success, 0, url.Url);
            }
            catch (Exception ex)
            {
                LogID = LogUnspecifiedException(ex, ServiceName + MethodName);
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, LogID);
                return SendResponse.response("GetIssueURL", SendResponse.ResponseCode.Fail, 0, "Could not get URL");
            }


        }

        [WebMethod(Description = "Will return the token for an issue the subscriber is trying to navigate to")]
        public string GetNextTokenForSubscriber(string accessKey, string Token, int IssueID)
        {
            MethodName = "GetNextTokenForSubscriber";
            Log = new ECN_Framework_Entities.Communicator.APILogging();
            Log.AccessKey = accessKey;
            Log.APIMethod = ServiceName + MethodName;
            Log.Input = "<ROOT><TOKEN>" + Token.ToString() + "</TOKEN><ISSUEID>" + IssueID.ToString() + "</ISSUEID></ROOT>";
            Log.APILogID = ECN_Framework_BusinessLayer.Communicator.APILogging.Insert(Log);

            string retToken = "";
            try
            {
                retToken = ECN_Framework_BusinessLayer.Communicator.EmailDataValues.WATT_GetNextTokenForSubscriber(Token, IssueID);
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                if (retToken.Length > 0)
                {
                    return SendResponse.response("GetNextTokenForSubscriber", SendResponse.ResponseCode.Success, 0, retToken);
                }
                else
                {
                    return SendResponse.response("GetNextTokenForSubscriber", SendResponse.ResponseCode.Fail, 0, "Could not find next Token");
                }
            }
            catch (Exception ex)
            {
                LogID = LogUnspecifiedException(ex, ServiceName + MethodName);
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, LogID);
                return SendResponse.response("GetNextTokenForSubscriber", SendResponse.ResponseCode.Fail, 0, "Could not return a token");
            }

        }

        [WebMethod(Description = "Will return true/false based on whether a token exists for a specified Issue")]
        public string SubscriberExists(string accessKey, string Token, int IssueID)
        {
            MethodName = "SubscriberExists";
            Log = new ECN_Framework_Entities.Communicator.APILogging();
            Log.AccessKey = accessKey;
            Log.APIMethod = ServiceName + MethodName;
            Log.Input = "<ROOT><TOKEN>" + Token.ToString() + "</TOKEN><ISSUEID>" + IssueID.ToString() + "</ISSUEID></ROOT>";
            Log.APILogID = ECN_Framework_BusinessLayer.Communicator.APILogging.Insert(Log);

            bool exists = false;
            try
            {
                exists = ECN_Framework_BusinessLayer.Communicator.EmailDataValues.WATT_SubscriberExists(Token, IssueID);
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);

                return SendResponse.response("SubscriberExists", SendResponse.ResponseCode.Success, 0, exists.ToString());
            }
            catch (Exception ex)
            {
                LogID = LogUnspecifiedException(ex, ServiceName + MethodName);
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, LogID);
                return SendResponse.response("SubscriberExists", SendResponse.ResponseCode.Fail, 0, "An Error Occurred");
            }

        }

        #region Methods added 09.04.14 from KmWattService from JointForms
        /// <summary>
        /// Summary description for Service1
        /// </summary>
        [System.ComponentModel.ToolboxItem(false)]
        // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
        // [System.Web.Script.Services.ScriptService]
        [WebMethod]
        public string AddProfile(string accessKey, CustomerData Profile, object[][] hUDF)
        {
            MethodName = "AddProfile";
            Log = new ECN_Framework_Entities.Communicator.APILogging();
            Log.AccessKey = accessKey;
            Log.APIMethod = ServiceName + MethodName;

            Log.Input = "<ROOT><accessKey>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(accessKey) + "</accessKey> <CustomerData>" +
                "<EktronUserName>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.EktronUserName) + "</EktronUserName>" +
                "<PubCode>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.PubCode) + "</PubCode>" +
                "<Birthday>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.BirthDay != null ? Profile.BirthDay.ToString() : "") + "</Birthday>" +
                "<FirstName>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.FirstName) + "</FirstName>" +
                "<LastName>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.LastName) + "</LastName>" +
                "<AddressLine1>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.AddressLine1) + "</AddressLine1>" +
                "<AddressLine2>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.AddressLine2) + "</AddressLine2>" +
                "<City>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.City) + "</City>" +
                "<State>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.State) + "</State>" +
                "<Country>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.Country) + "</Country>" +
                "<PostalCode>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.PostalCode) + "</PostalCode>" +
                "<CompanyName>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.CompanyName) + "</CompanyName>" +
                "<Title>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.Title) + "</Title>" +
                "<FullName>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.FullName) + "</FullName>" +
                "<Occupation>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.Occupation) + "</Occupation>" +
                "<Phone>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.Phone) + "</Phone>" +
                "<Mobile>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.Mobile) + "</Mobile>" +
                "<Fax>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.Fax) + "</Fax>" +
                "<Website>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.Website) + "</Website>" +
                "<Age>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.Age != null ? Profile.Age.ToString() : "") + "</Age>" +
                "<Income>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.Income != null ? Profile.Income.ToString() : "") + "</Income>" +
                "<Gender>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.Gender) + "</Gender>" +
                "<Password>" + ECN_Framework_Common.Functions.StringFunctions.GetTrimmed(Profile.Password) + "</Password></CustomerData>" +
               "</ROOT>";
            Log.APILogID = ECN_Framework_BusinessLayer.Communicator.APILogging.Insert(Log);

            CustomerData cd = new CustomerData(Profile);
            WattLogic wl = new WattLogic();

            try
            {
                if (cd.EktronUserName != string.Empty && ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(cd.EktronUserName))
                {
                    string response = wl.AddProfile(cd, HelperFunctions.ToHashtable(hUDF), accessKey);
                    ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);

                    return response;
                }
                else
                {
                    ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                    return "Incorrect Email Address (EktronUserName)";

                }
            }
            catch (ECN_Framework_Common.Objects.SecurityException)
            {
                return "ECN_Framework_Common.Objects.SecurityException: ECN Security Issue - Enums.ErrorMessage.SecurityError";
            }
            catch (Exception ex)
            {
                LogID = LogUnspecifiedException(ex, ServiceName + MethodName);
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, LogID);
                return "An error occurred";
            }
        }

        [WebMethod]
        public CustomerData GetProfile(string accessKey, string Emailaddress, string pubCode)
        {
            MethodName = "GetProfile";
            Log = new ECN_Framework_Entities.Communicator.APILogging();
            Log.AccessKey = accessKey;
            Log.APIMethod = ServiceName + MethodName;
            Log.Input = "<ROOT><accessKey>" + accessKey + "</accessKey><EmailAddress>" + Emailaddress + "</EmailAddress><PubCode>" + pubCode.ToString() + "</PubCode></ROOT>";
            Log.APILogID = ECN_Framework_BusinessLayer.Communicator.APILogging.Insert(Log);
            WattLogic wl = new WattLogic();

            try
            {
                if (ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(Emailaddress))
                {
                    CustomerData customer = wl.GetProfile(pubCode, Emailaddress, accessKey);
                    ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);

                    return customer;
                }
                else
                {
                    ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                    throw new SoapException("Incorrect Email Address (EktronUserName)", SoapException.ClientFaultCode);
                }

            }
            catch (Exception ex)
            {
                LogID = LogUnspecifiedException(ex, ServiceName + MethodName);
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, LogID);
                return null;
            }
        }



        [WebMethod]
        public string CreateUDF(string accessKey, string pubcode, string newUDF)
        {
            MethodName = "CreateUDF";
            Log = new ECN_Framework_Entities.Communicator.APILogging();
            Log.AccessKey = accessKey;
            Log.APIMethod = ServiceName + MethodName;
            Log.Input = "<ROOT><accessKey>" + accessKey + "</accessKey> <pubCode>" + pubcode + "</pubCode><newUDF>" + newUDF + "</newUDF></ROOT>";
            Log.APILogID = ECN_Framework_BusinessLayer.Communicator.APILogging.Insert(Log);


            WattLogic wl = new WattLogic();
            if (newUDF.Trim() != string.Empty)
            {

                newUDF = newUDF.Replace(" ", "_").ToUpper();

                int v = wl.CreateUDF(pubcode, newUDF, accessKey);
                if (v > 0)
                    return "UDF Created Successfully";
                else if (v == -2)
                    return "UDF Name already exists";
                else
                    return "UDF NOT Created - try again";
            }
            else
            {
                return "UDF NOT Created - Cannot be blank";
            }
        }

        //[WebMethod]
        //public int GetGroupID(string accessKey, string pubCode)
        //{
        //    if (System.Configuration.ConfigurationManager.AppSettings["AccessKey"].Equals(accessKey))
        //    {
        //    System.Xml.Linq.XDocument xd = System.Xml.Linq.XDocument.Load(HttpContext.Current.Server.MapPath("~/App_Data/PubCode.xml"));
        //    var gid = from x in xd.Descendants("Customer")
        //              where (string)x.Attribute("pubCode") == pubCode
        //              select (int)x.Element("GroupID");

        //    int groupID = gid.First();
        //        return groupID;
        //    }
        //    else
        //    {
        //        //access key was incorrect so return false
        //        throw new SoapException("Incorrect AccessKey", SoapException.ClientFaultCode);
        //    }
        //}
        [WebMethod]
        public List<GroupDataFieldExportClass> GetUDFList(string accessKey, string pubCode)
        {
            MethodName = "GetUDFList";
            Log = new ECN_Framework_Entities.Communicator.APILogging();
            Log.AccessKey = accessKey;
            Log.APIMethod = ServiceName + MethodName;
            Log.Input = "<ROOT><accessKey>" + accessKey + "</accessKey> <pubCode>" + pubCode + "</pubCode></ROOT>";
            Log.APILogID = ECN_Framework_BusinessLayer.Communicator.APILogging.Insert(Log);

            System.Xml.Linq.XDocument xd = System.Xml.Linq.XDocument.Load(HttpContext.Current.Server.MapPath("~/App_Data/PubCode.xml"));
            var gid = from x in xd.Descendants("Customer")
                      where (string)x.Attribute("pubCode") == pubCode
                      select (int)x.Element("GroupID");
            var cid = from x in xd.Descendants("Customer")
                      where (string)x.Attribute("pubCode") == pubCode
                      select (int)x.Element("CustomerID");

            int groupID = gid.First();
            int customerID = cid.First();

            try
            {
                List<KMPlatform.Entity.User> user = KMPlatform.BusinessLogic.User.GetByCustomerID(customerID);
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);

                return GroupDataFieldExportClass.convertGroupDataFieldForExport(ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupID, user[0]));

            }
            catch (Exception ex)
            {
                LogID = LogUnspecifiedException(ex, ServiceName + MethodName);
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, LogID);
                return null;
            }
        }

        #endregion


        private int LogUnspecifiedException(Exception ex, string sourceMethod, string note = "")
        {
            return KM.Common.Entity.ApplicationLog.LogCriticalError(ex, sourceMethod, Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), note);
            //KM.Common.Entity.ApplicationLog log = new KM.Common.Entity.ApplicationLog();
            //log.ApplicationID = Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]);
            //log.SeverityID = 1;
            //log.Exception = KM.Common.Entity.ApplicationLog.FormatException(ex);
            //log.NotificationSent = false;
            //log.SourceMethod = sourceMethod;
            //KM.Common.Entity.ApplicationLog.Save(ref log);

        }

        private void InitializeComponent()
        {
        }


    }
    public class GroupDataFieldExportClass
    {
        public int GroupDataFieldsID;
        public string ShortName;
        public string LongName;

        public static List<GroupDataFieldExportClass> convertGroupDataFieldForExport(List<ECN_Framework_Entities.Communicator.GroupDataFields> gdf)
        {
            List<GroupDataFieldExportClass> gdfsForExportList = new List<GroupDataFieldExportClass>();
            foreach (ECN_Framework_Entities.Communicator.GroupDataFields gdfToBeExported in gdf)
            {
                GroupDataFieldExportClass gdfForExport = new GroupDataFieldExportClass();
                gdfForExport.GroupDataFieldsID = gdfToBeExported.GroupDataFieldsID;
                gdfForExport.ShortName = gdfToBeExported.ShortName;
                gdfForExport.LongName = gdfToBeExported.LongName;
                gdfsForExportList.Add(gdfForExport);
            }


            return gdfsForExportList;
        }
    }


}
