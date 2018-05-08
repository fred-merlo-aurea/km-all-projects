using KMWeb.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using KMSite;
using System.Configuration;

namespace KMWeb.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (!string.IsNullOrEmpty(Request.QueryString.ToString()))
            {
                if (ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession() == null)
                {
                    try
                    {
                        //string.Format("{0}={1}&{2}={3}&{4}={5}&{6}={7}", KM.Common.ECNParameterTypes.UserName, UserSession.CurrentUser.UserName, KM.Common.ECNParameterTypes.Password, UserSession.CurrentUser.Password, "ClientGroupID", UserSession.ClientGroupID, "ClientID", UserSession.CurrentUser.CurrentClient.ClientID);
                        KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(int.Parse(ConfigurationManager.AppSettings["KMCommon_Application"]));


                        string unencrypted = KM.Common.Encryption.Base64Decrypt(System.Web.HttpUtility.UrlDecode(Request.QueryString.ToString()), ec);

                        string[] valuePairs = unencrypted.Split('&');
                        string userName = ""; string password = ""; string clientGroupID = ""; string ClientID = "";

                        foreach (string s in valuePairs)
                        {
                            string[] nameValue = s.Split('=');
                            switch (nameValue[0].TrimStart('|'))
                            {
                                case "UserName":
                                    userName = nameValue[1].TrimEnd('|');
                                    break;
                                case "Password":
                                    password = nameValue[1].TrimEnd('|');
                                    break;
                                case "ClientGroupID":
                                    clientGroupID = nameValue[1].TrimEnd('|');
                                    break;
                                case "ClientID":
                                    ClientID = nameValue[1].TrimEnd('|');
                                    break;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
                        {
                            bool success = ProcessLogin(userName, password, Convert.ToInt32(clientGroupID), Convert.ToInt32(ClientID), false);
                            if (success)
                                return new RedirectResult("/KMWeb/Forms");
                            else
                                return new RedirectResult("/ecn.accounts/main/default.aspx");
                        }
                        else
                        {
                            return new RedirectResult("/ecn.accounts/main/default.aspx");
                        }
                    }
                    catch(Exception ex)
                    {
                        return new RedirectResult("/ecn.accounts/main/default.aspx");
                    }
                }
                else
                {
                    if (KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.FORMSDESIGNER, KMPlatform.Enums.ServiceFeatures.FormsDesigner, KMPlatform.Enums.Access.FullAccess))
                    {
                        return new RedirectResult("/KMWeb/Forms");
                    }
                    else
                    {
                        return new RedirectResult("/ecn.accounts/main/default.aspx");
                    }
                }
            }
            else
            {
                try
                {
                    if (KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.FORMSDESIGNER, KMPlatform.Enums.ServiceFeatures.FormsDesigner, KMPlatform.Enums.Access.FullAccess))
                    {
                        return new RedirectResult("/KMWeb/Forms");
                    }
                    else
                    {
                        return new RedirectResult("/ecn.accounts/main/default.aspx");
                    }
                }
                catch
                {
                    return View();
                }
            }
        }

        private bool ProcessLogin(string username, string password, int clientgroupID, int clientID, bool persist)
        {
            bool authenticationSuccessful = false;

            try
            {

                KMPlatform.Entity.User u = new KMPlatform.BusinessLogic.User().SearchUserName(username);
                if (u != null && u.Password.Equals(password))
                {
                    if (u.IsActive)
                        u = new KMPlatform.BusinessLogic.User().SetAuthorizedUserObjects(u, clientgroupID, clientID);
                }
                else
                {
                    u = null;
                }

                if (u != null && u.UserID > 0 && u.CustomerID > 0)  //.HasValue
                {
                    //sunil - TODO - check if client exists in the client group
                    //sunil - TOOD - check if clientgroup UAD URL matches the current page URL -- need to add clientgroup UAD url in the database.

                    if (KMPlatform.BusinessLogic.User.IsSystemAdministrator(u))
                    {
                        KMPlatform.Entity.Client c = (new KMPlatform.BusinessLogic.Client()).Select(clientID, false);

                        authenticationSuccessful = Authenticate(u, c, clientgroupID, persist);
                    }
                    else
                    {
                        List<KMPlatform.Entity.Client> lc = new KMPlatform.BusinessLogic.Client().SelectbyUserID(u.UserID, false);
                        if (lc != null)
                        {
                            KMPlatform.Entity.Client c = lc.SingleOrDefault(x => x.ClientID == clientID);

                            authenticationSuccessful = Authenticate(u, c, clientgroupID, persist);
                        }
                    }
                }
                return authenticationSuccessful;
                
            }
            catch
            {
                return authenticationSuccessful;
            }
        }

        public bool Authenticate(KMPlatform.Entity.User u, KMPlatform.Entity.Client c, int clientgroupID, bool persist)
        {
            try
            {
                FormsAuthentication.SignOut();

                FormsAuthentication.SetAuthCookie(u.UserID.ToString(), false);

                // Create a new ticket used for authentication
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1, // Ticket version
                    u.UserID.ToString(), // UserID associated with ticket
                    DateTime.Now, // Date/time issued
                    DateTime.Now.AddDays(30), // Date/time to expire
                    true, // "true" for a persistent user cookie
                    CreateAuthenticationTicketUserData(u, clientgroupID, c.ClientID), // User-data, in this case the roles
                    FormsAuthentication.FormsCookiePath); // Path cookie valid for

                // Hash the cookie for transport
                string hash = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(
                    FormsAuthentication.FormsCookieName, // Name of auth cookie
                    hash); // Hashed ticket

                // Add the cookie to the list for outgoing response
                Response.Cookies.Add(cookie);

                return true;
            }
            catch (Exception e)
            {

            }

            return false;
        }

        private string CreateAuthenticationTicketUserData(KMPlatform.Entity.User u, int clientgroupID, int clientID)
        {
            ECN_Framework_Entities.Accounts.Customer ecnCustomer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(clientID, false);

            return String.Join(",",
                ecnCustomer.CustomerID,
                ecnCustomer.BaseChannelID,
                clientgroupID,
                clientID,
                u.AccessKey
                );
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var AccessKey = string.Empty;
                var UserId = string.Empty;
                if (model.Username == "DemoKM" && model.Password == "456")
                {
                    AccessKey = "6E24B8D4-AF0A-4396-983F-F4CE120A009B";
                    UserId = "1";
                }
                else if (model.Username == "BetaOne" && model.Password == "PennWell")
                {
                    AccessKey = "A649A159-A890-490B-8127-254C048F9EC6";
                    UserId = "2";
                }
                else if (model.Username == "BetaTwo" && model.Password == "Tabor")
                {
                    AccessKey = "6C4CFDCB-C4E8-4E5D-8B2E-EB4D0F490A9C";
                    UserId = "3";
                }
                else if (model.Username == "BetaThree" && model.Password == "Watt")
                {
                    AccessKey = "40781091-D599-4B47-A3CA-0E8F1CA5872F";
                    UserId = "4";
                }

                if (AccessKey != string.Empty && UserId != string.Empty)
                {
                    var ticket = new FormsAuthenticationTicket(1, UserId, DateTime.Now, DateTime.Now.AddHours(1), true, AccessKey);
                    var encrypted = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    Response.Cookies.Add(cookie);

                    return new RedirectResult(FormsAuthentication.GetRedirectUrl(UserId, false));
                }
                else
                {
                    ModelState.AddModelError("Error", "Invalid Username of Password");
                }
            }
            return View();
        }


        public ActionResult Logout() 
        {
            FormsAuthentication.SignOut();
            return Redirect("/EmailMarketing.Site/Login/Logout");
        }

        //private void RedirectFromLoginPage(LoginModel model)
        //{
        //    var AccessKey = string.Empty;
        //    if (model.Username == "DemoKM" && model.Password == "456")
        //        AccessKey = "6E24B8D4-AF0A-4396-983F-F4CE120A009B";
        //    else if (model.Username == "BetaOne" && model.Password == "PennWell")
        //        AccessKey = "A649A159-A890-490B-8127-254C048F9EC6";
        //    //tabor does not exist in special projects db but will in prod db for beta
        //    else if (model.Username == "BetaTwo" && model.Password == "Tabor")
        //        AccessKey = "6C4CFDCB-C4E8-4E5D-8B2E-EB4D0F490A9C";
        //    else if (model.Username == "BetaThree" && model.Password == "Watt")
        //        AccessKey = "40781091-D599-4B47-A3CA-0E8F1CA5872F";

        //    if (AccessKey != string.Empty)
        //    {
        //        var ticket = new FormsAuthenticationTicket(1, model.Username, DateTime.Now, DateTime.Now.AddHours(1), true, AccessKey);
        //        var encrypted = FormsAuthentication.Encrypt(ticket);
        //        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
        //        Response.Cookies.Add(cookie);

        //        Response.Redirect(FormsAuthentication.GetRedirectUrl(model.Username, false));
        //    }
        //    else
        //    {
        //        FormsAuthentication.SignOut();

        //        Response.Redirect(FormsAuthentication.LoginUrl);
        //    }
        //}
    }
}
