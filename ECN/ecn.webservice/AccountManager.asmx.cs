using System;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Services.Description;
using ecn.webservice.classes;
using ecn.common.classes;
using ecn.communicator.classes;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace ecn.webservice
{
    /// <summary>
    /// Summary description for AccountManager
    /// </summary>
    [WebService(
         Namespace = "http://tempuri.org/",
         Description = "The ECN Application Programming Interface (API) is a web service that allows you to control your ECN account programatically via an HTTP POST, an HTTP GET, or an XML-based SOAP call. The following web service methods allow access to managing your ACCOUNTS in ECN. The supported methods are shown below. <u>IMPORTANT NOTE:</u> All methods need ECN ACCESS KEY to work properly.")
    ]

    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    [System.ComponentModel.ToolboxItem(false)]

    public class AccountManager : System.Web.Services.WebService
    {
        [WebMethod(Description = "Provides Access to add customer accounts in ECN.")]
        public string AddCustomer(string ecnAccessKey, string CustomerName, bool IsActive, string Address, string City, string State, string Country, string Zip, string WebAddress, string Phone, string Fax, string Email, string Salutation, string ContactFirstName, string ContactLastName, string ContactTitle, string TechContact, string TechEmail, string TechPhone, string userdefinedfield1, string userdefinedfield2, string userdefinedfield3, string userdefinedfield4, string userdefinedfield5)
        {
            return SaveCustomer(ecnAccessKey, 0, CustomerName, IsActive, Address, City, State, Country, Zip, WebAddress, Phone, Fax, Email, Salutation, ContactFirstName, ContactLastName, ContactTitle, TechContact, TechEmail, TechPhone, userdefinedfield1, userdefinedfield2, userdefinedfield3, userdefinedfield4, userdefinedfield5);
        }

        [WebMethod(Description = "Provides Access to update customer accounts in ECN.")]
        public string UpdateCustomer(string ecnAccessKey, int CustomerID, string CustomerName, bool IsActive, string Address, string City, string State, string Country, string Zip, string WebAddress, string Phone, string Fax, string Email, string Salutation, string ContactFirstName, string ContactLastName, string ContactTitle, string TechContact, string TechEmail, string TechPhone, string userdefinedfield1, string userdefinedfield2, string userdefinedfield3, string userdefinedfield4, string userdefinedfield5)
        {
            return SaveCustomer(ecnAccessKey, CustomerID, CustomerName, IsActive, Address, City, State, Country, Zip, WebAddress, Phone, Fax, Email, Salutation, ContactFirstName, ContactLastName, ContactTitle, TechContact, TechEmail, TechPhone, userdefinedfield1, userdefinedfield2, userdefinedfield3, userdefinedfield4, userdefinedfield5);
        }

        private string SaveCustomer(string ecnAccessKey, int CustomerID, string CustomerName, bool IsActive, string Address, string City, string State, string Country, string Zip, string WebAddress, string Phone, string Fax, string Email, string Salutation, string ContactFirstName, string ContactLastName, string ContactTitle, string TechContact, string TechEmail, string TechPhone, string userdefinedfield1, string userdefinedfield2, string userdefinedfield3, string userdefinedfield4, string userdefinedfield5)
        {

            if (CustomerID < 0)
                CustomerID = 0;

            AuthenticationHandler authHandler = new AuthenticationHandler(ecnAccessKey);
            KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.ECN_GetByAccessKey(ecnAccessKey, true);
            try
            {
                if (authHandler.authenticateUser() && user != null)
                {
                    // IF USER IS SYSADMIN OR CHANNELADMIN - ALLOW TO SETUP ACCOUNT
                    if (authHandler.isSysAdmin() || authHandler.isChannelAdmin())
                    {
                        if (CustomerName.Trim().Length == 0)
                        {
                            return SendResponse.response("SetupCustomer", SendResponse.ResponseCode.Fail, 0, "CUSTOMER NAME CANONOT BE NULL.");
                        }

                        Contact cc = new Contact(Salutation.Trim(), ContactFirstName.Trim(), ContactLastName.Trim(), ContactTitle.Trim(),
                                                Phone.Trim(), Fax.Trim(), Email.Trim(), Address.Trim(), City.Trim(), State.Trim(),
                                                Country.Trim(), Zip.Trim());

                        Customer cust;

                        if (CustomerID > 0) // UPDATE
                        {
                            if (!ECN_Framework.Common.SecurityAccess.hasAccess("Customers", CustomerID.ToString()))
                                return SendResponse.response("SetupCustomer", SendResponse.ResponseCode.Fail, 0, "USER DOES NOT HAVE PERMISSIONS.");

                            cust = Customer.GetCustomerByID(CustomerID);

                            if (cust == null)
                            {
                                return SendResponse.response("SetupCustomer", SendResponse.ResponseCode.Fail, 0, "CUSTOMER ACCOUNT NOT EXISTS");
                            }
                        }
                        else // NEW ACCOUNT
                        {
                            cust = new Customer();

                            cust.SubscriptionsEmail = authHandler.Customer.SubscriptionsEmail;

                            cust.CreatorLevel = authHandler.Customer.CreatorLevel;
                            cust.CreatorChannelID = authHandler.Customer.CreatorChannelID;

                            cust.CollectorLevel = authHandler.Customer.CollectorLevel;
                            cust.CollectorChannelID = authHandler.Customer.CollectorChannelID;

                            cust.CommunicatorLevel = authHandler.Customer.CommunicatorLevel;
                            cust.CommunicatorChannelID = authHandler.Customer.CommunicatorChannelID;

                            cust.PublisherLevel = authHandler.Customer.PublisherLevel;
                            cust.PublisherChannelID = authHandler.Customer.PublisherChannelID;

                            cust.CharityLevel = authHandler.Customer.CharityLevel;
                            cust.CharityChannelID = authHandler.Customer.CharityChannelID;

                            cust.AccountLevel = authHandler.Customer.AccountLevel;

                        }

                        if (IsActive)
                        {
                            cust.IsActive = "Y";
                        }
                        else
                        {
                            cust.IsActive = "N";
                        }

                        cust.BaseChannelID = authHandler.baseChannelID;
                        cust.Name = CustomerName;
                        cust.GeneralContact = cc;
                        cust.BillingContact = cc;
                        //cust.IsActive = IsActive;
                        cust.WebAddress = WebAddress.Trim();
                        cust.TechContact = TechContact.Trim();
                        cust.TechEmail = TechEmail.Trim();
                        cust.TechPhone = TechPhone.Trim();

                        cust.Save(user.UserID);

                        if (CustomerID <= 0)
                        {
                            //cust.CreateCustomerFolders(Server);
                            cust.CreateDefaultFeatures(user.UserID);
                            cust.CreateDefaulRole(user.UserID);
                            cust.CreateMasterSupressionGroup();

                            //Create Unlimited Licenses
                            SQLHelper.execute("INSERT INTO CustomerLicense (CustomerID , QuoteItemID, LicenseTypeCode, LicenseLevel, Quantity, Used, ExpirationDate, AddDate, IsActive) VALUES (" + cust.ID + ",'-1','emailblock10k','CUST','-1','0',convert(varchar(10),Dateadd(yy,1,getdate())-1 , 101) +' 23:59:59' ,convert(varchar(10),getdate(), 101),'1')", ConfigurationManager.AppSettings["act"]);

                            //Activate Default Features
                            SQLHelper.execute("update CustomerProduct set Active = 'y', ModifyDate = GETDATE() where CustomerID = " + cust.ID + " and  ProductDetailID in (101,109,110,111,112,113,120,121)", ConfigurationManager.AppSettings["act"]);


                        }

                        return SendResponse.response("SetupCustomer", SendResponse.ResponseCode.Success, cust.ID, CustomerID > 0 ? "CUSTOMER ACCOUNT UPDATED" : "CUSTOMER ACCOUNT CREATED");
                    }
                    else
                    {
                        return SendResponse.response("SetupCustomer", SendResponse.ResponseCode.Fail, 0, "USER DOES NOT HAVE PERMISSIONS TO CREATE AN ACCOUNT.");
                    }
                }
                else
                {
                    return SendResponse.response("SetupCustomer", SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
                }
            }
            catch (Exception ex)
            {
                return SendResponse.response("SetupCustomer", SendResponse.ResponseCode.Fail, 0, ex.Message);
            }
        }

        [WebMethod(Description = "List all customer accounts within the Channel.")]
        public string GetCustomers(string ecnAccessKey)
        {
            AuthenticationHandler authHandler = new AuthenticationHandler(ecnAccessKey);
            try
            {
                if (authHandler.authenticateUser())
                {
                    // IF USER IS SYSADMIN OR CHANNELADMIN - ALLOW TO SETUP ACCOUNT
                    if (authHandler.isSysAdmin() || authHandler.isChannelAdmin())
                    {

                        string CustomerXML = SQLHelper.executeXmlReader("select CustomerID,CustomerName,ActiveFlag,Address,City,State,Country,Zip,WebAddress,Phone,Fax,Email,Salutation,FirstName as ContactFirstName,LastName as ContactLastName,ContactTitle,TechContact,TechEmail,TechPhone,SubscriptionsEmail,customer_udf1 as userdefinedfield1,customer_udf2 as userdefinedfield2,customer_udf3 as userdefinedfield3,customer_udf4 as userdefinedfield4,customer_udf5 as userdefinedfield5,CreatedDate from Customer as Customer where BaseChannelID = " + authHandler.baseChannelID + " FOR XML PATH('Customer'), Root('Customers')", "Customers", ConfigurationManager.AppSettings["com"]);

                        return SendResponse.response("getCustomers", SendResponse.ResponseCode.Success, 0, CustomerXML);
                    }
                    else
                    {
                        return SendResponse.response("getCustomers", SendResponse.ResponseCode.Fail, 0, "USER DOES NOT HAVE PERMISSIONS.");
                    }
                }
                else
                {
                    return SendResponse.response("getCustomers", SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
                }
            }
            catch (Exception ex)
            {
                return SendResponse.response("getCustomers", SendResponse.ResponseCode.Fail, 0, ex.Message);
            }
        }

        [WebMethod(Description = "Get the information for the Customer.")]
        public string GetCustomerbyID(string ecnAccessKey, int CustomerID)
        {
            AuthenticationHandler authHandler = new AuthenticationHandler(ecnAccessKey);
            try
            {
                if (authHandler.authenticateUser())
                {
                    // IF USER IS SYSADMIN OR CHANNELADMIN - ALLOW TO SETUP ACCOUNT
                    if (authHandler.isSysAdmin() || authHandler.isChannelAdmin() || authHandler.isAdmin())
                    {
                        string CustomerXML = SQLHelper.executeXmlReader("select CustomerID,CustomerName,ActiveFlag,Address,City,State,Country,Zip,WebAddress,Phone,Fax,Email,Salutation,FirstName as ContactFirstName,LastName as ContactLastName,ContactTitle,TechContact,TechEmail,TechPhone,SubscriptionsEmail,customer_udf1 as userdefinedfield1,customer_udf2 as userdefinedfield2,customer_udf3 as userdefinedfield3,customer_udf4 as userdefinedfield4,customer_udf5 as userdefinedfield5,CreateDate from Customer where BaseChannelID = " + authHandler.baseChannelID + " and CustomerID = " + CustomerID + " for XML PATH('Customer'), Root('Customers')", "Customers", ConfigurationManager.AppSettings["act"]);
                        return SendResponse.response("getCustomerbyID", SendResponse.ResponseCode.Success, 0, CustomerXML == string.Empty ? "<Customers></Customers>" : CustomerXML);
                    }
                    else
                    {
                        return SendResponse.response("getCustomerbyID", SendResponse.ResponseCode.Fail, 0, "USER DOES NOT HAVE PERMISSIONS.");
                    }
                }
                else
                {
                    return SendResponse.response("getCustomerbyID", SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
                }
            }
            catch (Exception ex)
            {
                return SendResponse.response("getCustomerbyID", SendResponse.ResponseCode.Fail, 0, ex.Message);
            }
        }

        [WebMethod(Description = "Provides Access to add user account in ECN for a Customer.")]
        public string AddUser(string ecnAccessKey, int CustomerID, string Username, string Password, bool IsActive)
        {
            return SaveUser(ecnAccessKey, CustomerID, 0, Username, Password, IsActive);
        }

        [WebMethod(Description = "Provides Access to update user account in ECN for a Customer.")]
        public string UpdateUser(string ecnAccessKey, int CustomerID, int UserID, string Username, string Password, bool IsActive)
        {
            if (UserID == 0)
                return SendResponse.response("UpdateUser", SendResponse.ResponseCode.Fail, 0, "InValid UserID.");

            return SaveUser(ecnAccessKey, CustomerID, UserID, Username, Password, IsActive);
        }

        private string SaveUser(string ecnAccessKey, int CustomerID, int UserID, string Username, string Password, bool IsActive)
        {
            AuthenticationHandler authHandler = new AuthenticationHandler(ecnAccessKey);
            try
            {
                if (authHandler.authenticateUser())
                {
                    // IF USER IS SYSADMIN OR CHANNELADMIN - ALLOW TO SETUP ACCOUNT
                    if (authHandler.isSysAdmin() || authHandler.isChannelAdmin() || authHandler.isAdmin())
                    {
                        if (!ECN_Framework.Common.SecurityAccess.hasAccess("Customers", CustomerID.ToString()))
                            return SendResponse.response("SetupUser", SendResponse.ResponseCode.Fail, 0, "USER DOES NOT HAVE PERMISSIONS.");

                        if (UserID > 0 && !ECN_Framework.Common.SecurityAccess.hasAccess("Users", CustomerID.ToString()))
                            return SendResponse.response("SetupUser", SendResponse.ResponseCode.Fail, 0, "USER DOES NOT HAVE PERMISSIONS.");


                        if (Username.Trim().Length == 0)
                        {
                            return SendResponse.response("SetupUser", SendResponse.ResponseCode.Fail, 0, "USERNAME CANONOT BE NULL.");
                        }

                        if (Password.Trim().Length < 5)
                        {
                            return SendResponse.response("SetupUser", SendResponse.ResponseCode.Fail, 0, "PASSWORDS MUST BE AT LEAST 5 CHARACTERS LONG.");
                        }
                        User u = new User();

                        u.Customer = Customer.GetCustomerByID(CustomerID);
                        u.ID = UserID;
                        //u.fullname = Fullname;
                        u.UserName = Username;
                        u.Password = Password;
                        u.IsActive = IsActive;

                        u.Save();

                        return SendResponse.response("SetupUser", SendResponse.ResponseCode.Success, u.ID, UserID > 0 ? "USER ACCOUNT UPDATED" : "USER ACCOUNT CREATED");
                    }
                    else
                    {
                        return SendResponse.response("SetupUser", SendResponse.ResponseCode.Fail, 0, "USER DOES NOT HAVE PERMISSIONS.");
                    }
                }
                else
                {
                    return SendResponse.response("SetupUser", SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
                }
            }
            catch (Exception ex)
            {
                return SendResponse.response("SetupUser", SendResponse.ResponseCode.Fail, 0, ex.Message);
            }
        }

        [WebMethod(Description = "List all users with in the Customer account.")]
        public string GetUsers(string ecnAccessKey, int CustomerID)
        {
            AuthenticationHandler authHandler = new AuthenticationHandler(ecnAccessKey);
            try
            {
                if (authHandler.authenticateUser())
                {
                    // IF USER IS SYSADMIN OR CHANNELADMIN - ALLOW TO SETUP ACCOUNT
                    if (authHandler.isSysAdmin() || authHandler.isChannelAdmin() || authHandler.isAdmin())
                    {
                        if (!ECN_Framework.Common.SecurityAccess.hasAccess("Customers", CustomerID.ToString()))
                            return SendResponse.response("getUsers", SendResponse.ResponseCode.Fail, 0, "USER DOES NOT HAVE PERMISSIONS.");

                        string UserXML = SQLHelper.executeXmlReader("select UserID, FullName, username, u.ActiveFlag, AccessKey, u.CreateDate from Users u join Customer c on c.CustomerID = u.CustomerID  where c.CustomerID = " + CustomerID + " and c.basechannelID = " + authHandler.baseChannelID + " FOR XML PATH('User'), Root('Users')", "Users", ConfigurationManager.AppSettings["act"]);
                        return SendResponse.response("getUsers", SendResponse.ResponseCode.Success, 0, UserXML);
                    }
                    else
                    {
                        return SendResponse.response("getUsers", SendResponse.ResponseCode.Fail, 0, "USER DOES NOT HAVE PERMISSIONS.");
                    }
                }
                else
                {
                    return SendResponse.response("getUsers", SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
                }
            }
            catch (Exception ex)
            {
                return SendResponse.response("getUsers", SendResponse.ResponseCode.Fail, 0, ex.Message);
            }
        }

        [WebMethod(Description = "Get the information for the user.")]
        public string GetUserbyID(string ecnAccessKey, int CustomerID, int UserID)
        {
            AuthenticationHandler authHandler = new AuthenticationHandler(ecnAccessKey);
            try
            {
                if (authHandler.authenticateUser())
                {
                    // IF USER IS SYSADMIN OR CHANNELADMIN - ALLOW TO SETUP ACCOUNT
                    if (authHandler.isSysAdmin() || authHandler.isChannelAdmin() || authHandler.isAdmin())
                    {
                        if (!ECN_Framework.Common.SecurityAccess.hasAccess("Customers", CustomerID.ToString()))
                            return SendResponse.response("getUsersbyID", SendResponse.ResponseCode.Fail, 0, "USER DOES NOT HAVE PERMISSIONS.");

                        if (UserID > 0 && !ECN_Framework.Common.SecurityAccess.hasAccess("Users", CustomerID.ToString()))
                            return SendResponse.response("getUsersbyID", SendResponse.ResponseCode.Fail, 0, "USER DOES NOT HAVE PERMISSIONS.");


                        string UserXML = SQLHelper.executeXmlReader("select UserID, FullName, username, u.ActiveFlag, AccessKey, u.CreateDate from Users u join Customer c on c.CustomerID = u.CustomerID  where c.CustomerID = " + CustomerID + " and c.basechannelID = " + authHandler.baseChannelID + " and userID = " + UserID + " FOR XML PATH('User'), Root('Users')", "Users", ConfigurationManager.AppSettings["act"]);
                        return SendResponse.response("getUsersbyID", SendResponse.ResponseCode.Success, 0, UserXML);
                    }
                    else
                    {
                        return SendResponse.response("getUsersbyID", SendResponse.ResponseCode.Fail, 0, "USER DOES NOT HAVE PERMISSIONS.");
                    }
                }
                else
                {
                    return SendResponse.response("getUsersbyID", SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
                }
            }
            catch (Exception ex)
            {
                return SendResponse.response("getUsersbyID", SendResponse.ResponseCode.Fail, 0, ex.Message);
            }
        }

        [WebMethod(Description = "Generates encrypted query string for auto login")]
        public string GetLogin(string ecnAccessKey, string URL, int ecnUserID, string additionalParams)
        {
            AuthenticationHandler authHandler = new AuthenticationHandler(ecnAccessKey);
            if (authHandler.authenticateUser())
            {
                if(authHandler.isSysAdmin())
                {
                    if (ecnAccessKey.ToUpper().Equals(ConfigurationManager.AppSettings["UASMasterAccessKey"].ToString()))
                    {
                        KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByUserID(ecnUserID, false);
                        KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(int.Parse(ConfigurationManager.AppSettings["KMCommon_Application"]));
                        string postbackUrl = URL;
                        string queryString = string.Format("{0}={1}&{2}={3}", KM.Common.ECNParameterTypes.UserName, user.UserName, KM.Common.ECNParameterTypes.Password, user.Password);
                        if (!string.IsNullOrEmpty(additionalParams))
                        {
                            if (additionalParams.Substring(0, 1).Equals("&") || additionalParams.Substring(0, 1).Equals("?"))
                                additionalParams = additionalParams.Substring(1);
                            queryString += "&" + additionalParams;
                        }
                        string queryStringHash = System.Web.HttpUtility.UrlEncode(KM.Common.Encryption.Encrypt(queryString, ec));
                        string completePostbackUrl = string.Concat(postbackUrl, "?", queryStringHash);
                        return completePostbackUrl;
                    }
                    return SendResponse.response("GetLogin", SendResponse.ResponseCode.Fail, 0, "USER DOES NOT HAVE PERMISSIONS");
                }
                else
                {
                    return SendResponse.response("GetLogin", SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
                }
            }
            else
            {
                return SendResponse.response("GetLogin", SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
            }
        }

        [WebMethod(Description = "This is used by internal KM Applications")]
        public string GetBaseChanels_Internal(string ecnAccessKey)
        {
            AuthenticationHandler authHandler = new AuthenticationHandler(ecnAccessKey);
            if (authHandler.authenticateUser())
            {
                if (authHandler.isSysAdmin())
                {
                    if (ecnAccessKey.ToUpper().Equals(ConfigurationManager.AppSettings["UASMasterAccessKey"].ToString()))
                    {
                        List<ECN_Framework_Entities.Accounts.BaseChannel> bcList = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll();
                        StringBuilder sbBC = new StringBuilder();
                        sbBC.Append("<BaseChannels>");
                        foreach(ECN_Framework_Entities.Accounts.BaseChannel bc in bcList)
                        {
                            sbBC.Append("<BaseChannel>");
                            sbBC.Append("<ID>" + bc.BaseChannelID.ToString() + "</ID>");
                            sbBC.Append("<Name><![CDATA[" + bc.BaseChannelName + "]]></Name>");
                            sbBC.Append("</BaseChannel>");
                        }
                        sbBC.Append("</BaseChannels>");

                        return SendResponse.response("GetBaseChannels_Internal", SendResponse.ResponseCode.Success, 0, sbBC.ToString());
                    }
                    else
                    {
                        return SendResponse.response("GetBaseChannels_Internal", SendResponse.ResponseCode.Fail, 0, "USER DOES NOT HAVE PERMISSIONS");
                    }
                }
                else
                {
                    return SendResponse.response("GetBaseChannels_Internal", SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
                }
            }
            else
            {
                return SendResponse.response("GetBaseChannels_Internal", SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
            }
        }

        [WebMethod(Description = "This is used by internal KM Applications")]
        public string GetCustomers_Internal(string ecnAccessKey, int BaseChannelID)
        {
            AuthenticationHandler authHandler = new AuthenticationHandler(ecnAccessKey);
            if (authHandler.authenticateUser())
            {
                if (authHandler.isSysAdmin())
                {
                    if (ecnAccessKey.ToUpper().Equals(ConfigurationManager.AppSettings["UASMasterAccessKey"].ToString()))
                    {
                        List<ECN_Framework_Entities.Accounts.Customer> cuList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(BaseChannelID);
                        StringBuilder sbCU = new StringBuilder();
                        sbCU.Append("<Customers>");

                        foreach(ECN_Framework_Entities.Accounts.Customer c in cuList)
                        {
                            sbCU.Append("<Customer>");
                            sbCU.Append("<ID>" + c.CustomerID.ToString() + "</ID>");
                            sbCU.Append("<Name><![CDATA[" + c.CustomerName + "]]></Name>");
                            sbCU.Append("</Customer>");
                        }
                        sbCU.Append("</Customers>");

                        return SendResponse.response("GetCustomers_Internal", SendResponse.ResponseCode.Success, 0, sbCU.ToString());
                    }
                    else
                    {
                        return SendResponse.response("GetCustomers_Internal", SendResponse.ResponseCode.Fail, 0, "USER DOES NOT HAVE PERMISSIONS");
                    }
                }
                else
                {
                    return SendResponse.response("GetCustomers_Internal", SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
                }
            }
            else
            {
                return SendResponse.response("GetCustomers_Internal", SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
            }
        }
    }
}
