using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;

using EmailMarketing.Site.Infrastructure.Abstract.Settings;
using EmailMarketing.Site.Models;
using EmailMarketing.Site.Infrastructure.Abstract;
using EmailMarketing.Site.Infrastructure.Authorization;

using PlatformUser = KMPlatform.Entity.User;
using System.Web;

namespace EmailMarketing.Site.Controllers
{
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        IAuthenticationProvider AuthenticationProvider { get; set; }
        IAccountProvider AccountProvider { get; set; }

        /// <summary>
        /// Authentication delegate for adding a cookie to the current request's response
        /// </summary>
        /// <param name="cookie">the HttpCookie to add to the response</param>
        private void AddCookie(HttpCookie cookie)
        {
            Response.SetCookie(cookie);
        }

        public LoginController(
            IUserSessionProvider userSessionProvider,
            IPathProvider pathProvider,
            IAuthenticationProvider authenticationProvider,
            IBaseChannelProvider baseChannelProvider,
            IAccountProvider accountProvider
            )
            : base(userSessionProvider, pathProvider, baseChannelProvider)
        {
            AuthenticationProvider = authenticationProvider;
            AccountProvider = accountProvider;
        }

        public ActionResult SecurityAcccessError()
        {
            //throw new UnauthorizedAccessException();
            return View();
        }

        public ActionResult Logout()
        {
            try
            {
                ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClearSession();
            }
            catch { }
            AuthenticationProvider.Deauthenticate(AddCookie);

            Session.Abandon();
            return Redirect("~/Login");
        }

        public ActionResult Index()
        {
            if (!string.IsNullOrEmpty(Request.Url.Query))
            {
                try
                {
                    string userNameValue = string.Empty;
                    string passwordValue = string.Empty;
                    string redirectApp = string.Empty;

                    int applicationID = int.Parse(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"]);
                    KM.Common.QueryString.ParseEncryptedSSOQuerystring(Request.Url.Query, applicationID, out userNameValue, out passwordValue, out redirectApp);

                    if (false == string.IsNullOrWhiteSpace(userNameValue)
                        && false == string.IsNullOrWhiteSpace(passwordValue))
                    {
                        redirectApp = redirectApp.ToLower();
                        string redirectUrl = "~/";

                        switch (redirectApp)
                        {
                            case "communicator": redirectUrl = "/ecn.communicator/main/default.aspx"; break;
                            case "surveys": redirectUrl = "/ecn.collector/main/survey/"; break;
                            case "digitaleditions": redirectUrl = "/ecn.publisher/main/edition/default.aspx"; break;
                            case "domaintracking": redirectUrl = "/ecn.domaintracking/Main/Index/"; break;
                        }

                        LoginUserViewModel loginUser = new LoginUserViewModel
                        {
                            User = userNameValue,
                            Password = passwordValue,
                            Persist = false
                        };
                        return Index(loginUser, redirectUrl);
                    }

                }
                catch (Exception e)
                {
                    // TODO: logging?
                    System.Diagnostics.Trace.TraceWarning("Login: caught exception processing encrypted credentials. {0}", e);
                }
            }
            //ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClearSession();
            AuthenticationProvider.Deauthenticate(AddCookie);
            return View(new LoginUserViewModel());
        }

        [HttpPost]
        public ActionResult Index(LoginUserViewModel loginUser, string returnUrl = null)
        {
            // prevent Login/Logout looping
            if (false == String.IsNullOrEmpty(returnUrl))
            {
                if (returnUrl.ToLower().EndsWith("logout"))
                {
                    returnUrl = null;
                }
                else if(returnUrl.ToLower().Contains("emailmarketing.site"))
                    returnUrl = null;
            }


            if (ModelState.IsValid)
            {
                try
                {
                    KMPlatform.Entity.User authenticationSuccess = AuthenticationProvider.Authenticate(
                        AddCookie, // action to add auth cookies
                        loginUser.User,
                        loginUser.Password,
                        loginUser.Persist); // not displayed on the form but defaulted true in the model
                    if (authenticationSuccess == null)
                    {
                        return View(loginUser);
                    }
                    else
                    {
                        if(authenticationSuccess.RequirePasswordReset)
                        {
                            Models.ResetPasswordViewModel rpvm = new ResetPasswordViewModel();
                            rpvm.UserName = authenticationSuccess.UserName;
                            rpvm.UserID = authenticationSuccess.UserID;
                            return RedirectToAction("Index","Reset", new { id = rpvm.UserID });
                        }

                        //return Redirect(returnUrl ?? "http://localhost/ecn.accounts/main/default.aspx");
                        return Redirect(returnUrl ?? System.Configuration.ConfigurationManager.AppSettings["Accounts_VirtualPath"] + "main/default.aspx");
                    }
                }
                catch (KMPlatform.Object.UserLoginException ule)
                {
                    if (ule.UserStatus == KMPlatform.Enums.UserLoginStatus.LockedUser)
                    {
                        loginUser.UserIsLocked = true;
                        return View(loginUser);
                    }
                    else if (ule.UserStatus == KMPlatform.Enums.UserLoginStatus.DisabledUser)
                    {
                        loginUser.UserIsDisabled = true;
                        return View(loginUser);
                    }
                    else if (ule.UserStatus == KMPlatform.Enums.UserLoginStatus.NoRoles)
                    {
                        loginUser.NoActiveRoles = true;
                        return View(loginUser);
                    }
                    else if(ule.UserStatus == KMPlatform.Enums.UserLoginStatus.InvalidPassword)
                    {
                        loginUser.InvalidUsername_Password = true;
                        return View(loginUser);
                    }
                    else if(ule.UserStatus == KMPlatform.Enums.UserLoginStatus.NoActiveClients)
                    {
                        loginUser.NoActive_Clients = true;
                        return View(loginUser);
                    }
                    else if(ule.UserStatus == KMPlatform.Enums.UserLoginStatus.ShowProfileChangeMessage)
                    {
                        return View("Message");
                    }
                }
            }

            return View(loginUser);
        }

        
        [HttpGet]
        public ActionResult Forgot()
        {
            return View("Forgot",new Models.ForgotPasswordViewModel());
        }

        [HttpPost]
        public ActionResult Forgot(ForgotPasswordViewModel fpvm)
        {
            KMPlatform.Entity.User currentUser = new KMPlatform.BusinessLogic.User().SearchUserName(fpvm.UserName);
            if(currentUser != null && currentUser.UserID > 0)
            {
                
                //do password reset and send email
                string html = GetResestHTMLandReplace(currentUser);
                ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();

                
                //Send the email to the user

                ed.CreatedDate = DateTime.Now;
                ed.CreatedUserID = 501;
                ed.EmailAddress = currentUser.EmailAddress;
                ed.EmailSubject = "KM Platform Password Reset";
                ed.FromName = "KM Platform";
                ed.Content = html;
                ed.SendTime = DateTime.Now;
                ed.ReplyEmailAddress = "info@knowledgemarketing.com";
                ed.CustomerID = 1;
                ed.Process = "Reset My Password Email";
                ed.Source = "ECN Accounts";

                ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);

                fpvm.Exists = true;
                fpvm.EmailSent = true;
            }
            else
            {
                //no user with that username
                fpvm.Exists = false;
                fpvm.EmailSent = false;
                
            }

            return View("Forgot",fpvm);

        }

        private string GetResestHTMLandReplace(KMPlatform.Entity.User currentUser)
        {
            KMPlatform.BusinessLogic.User uWorker = new KMPlatform.BusinessLogic.User();
            string html = uWorker.ResetPasswordHTML();
            html = html.Replace("%%FirstName%%", currentUser.FirstName);
            string tempPassword = uWorker.GenerateTempPassword();
            currentUser.Password = tempPassword;
            currentUser.RequirePasswordReset = true;
            html = html.Replace("%%TempPassword%%", tempPassword);
            string redirectUrl = ConfigurationManager.AppSettings["ResetPassword_URL"].ToString();
            redirectUrl += "?id=" + currentUser.UserID.ToString();
            html = html.Replace("%%RedirectLink%%", redirectUrl);
            
            uWorker.Save(currentUser);
            return html;
        }

        
        [ActionName("User")] // avoid conflict with ApiController.User()
        public ActionResult UserLogin(int id = 0)
        {
            //if(id < 1)
            //{
            //    id = AuthenticationTicket.MasterUserID;
            //}

            //int masterUserId = -1;
            //if(HasRole.IsChannelMaster(AuthenticationTicket))
            //{
            //    masterUserId = AuthenticationTicket.MasterUserID;
            //}
            //else if (HasRole.CanAdministrateUsers(CurrentUser))
            //{
            //    masterUserId = CurrentUser.UserID;
            //}

            //if (masterUserId > 0)
            //{
            //    EcnUser user = AccountProvider.User_GetByUserID(id, false);
            //    if (null != user)
            //    {
            //        EcnCustomer customer = AccountProvider.Customer_GetByCustomerID(user.CustomerID.Value, false);
            //        if (null != customer)
            //        {
            //            if (masterUserId == CurrentUser.UserID // special case: reverting to original user bypasses security checks in CanImpersonate
            //                || HasRole.CanImpersonate(CurrentUser, CurrentCustomer, user, customer))
            //            {
            //                if(Impersonate(user, customer, masterUserId))
            //                {
            //                    return Redirect("~/");
            //                }
            //            }
            //        }
            //    }
            //}

            return RedirectToAction("SecurityAcccessError");
        }

        public ActionResult Customer(int id)
        {
            //if(id > 0 && HasRole.IsChannelMaster(AuthenticationTicket))
            //{
            //    EcnCustomer customer = AccountProvider.Customer_GetByCustomerID(id, false);

            //    if(null != customer)
            //    {
            //        IEnumerable<EcnUser> users =
            //            from x in AccountProvider.User_GetByCustomerID(id)
            //            where HasRole.CanImpersonate(CurrentUser,CurrentCustomer,x,customer)
            //                  /* HasRole.IsActive(x)
            //                  && (HasRole.IsSystemAdministrator(CurrentUser)
            //                      || (false == HasRole.IsSystemAdministrator(x)
            //                          && (HasRole.IsChannelAdministrator(CurrentUser)
            //                              || false == HasRole.IsChannelAdministrator(x))))*/
            //            orderby x.IsSysAdmin     descending, 
            //                    x.IsChannelAdmin descending, 
            //                    x.IsAdmin        descending
            //            select x;
            //        /*EcnUser u = users.FirstOrDefault(x => HasRole.IsChannelAdministrator(x))
            //                 ?? users.FirstOrDefault(x => HasRole.IsAdministrator(x))
            //                 ?? users.FirstOrDefault(x => HasRole.IsActive(x));*/
            //        if(Impersonate(users.FirstOrDefault(), customer, AuthenticationTicket.MasterUserID))
            //        {
            //            return Redirect("~/");
            //        }
            //    }
            //}

            return RedirectToAction("SecurityAcccessError");
        }

    }
}
