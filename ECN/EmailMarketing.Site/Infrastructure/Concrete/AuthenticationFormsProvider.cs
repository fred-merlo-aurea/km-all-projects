using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

using PlatformUser = KMPlatform.Entity.User;
using PlatformClient = KMPlatform.Entity.Client;
using EcnAuthenticationTicket = ECN_Framework_Entities.Application.AuthenticationTicket;

using EmailMarketing.Site.Infrastructure.Abstract;
using System.Threading.Tasks;
using System.Web.Mvc;
using EmailMarketing.Site.Infrastructure.Authorization;

namespace EmailMarketing.Site.Infrastructure.Concrete
{
    public sealed class AuthenticationFormsProvider : IAuthenticationProvider
    {
        IAccountProvider AccountProvider { get; set; }
        IWebAuthenticationWrapper FormsAuthentication { get; set; }
        public AuthenticationFormsProvider(IWebAuthenticationWrapper formsAuthenticationWrapper, IAccountProvider accountProvider)
        {
            FormsAuthentication = formsAuthenticationWrapper;
            AccountProvider = accountProvider;
        }

        public void Deauthenticate(Action<HttpCookie> addCookie)
        {
            FormsAuthentication.SignOut();
        }


        public KMPlatform.Entity.User Authenticate(Action<HttpCookie> addCookie, string username, string password, bool persist)
        {
            Boolean authenticationSuccessful = false;
            KMPlatform.Entity.User u = null;
            try
            {
                u = AccountProvider.User_Login(username, password);
                if (u != null && u.UserID > 0 && u.CustomerID > 0)
                {

                    //List<KMPlatform.Entity.Client> lc = AccountProvider.Client_GetByUserID(u.UserID, false);
                    //if (lc != null)
                    //{
                    //    KMPlatform.Entity.Client c = lc.SingleOrDefault(x => x.ClientID == u.DefaultClientID);

                    //    authenticationSuccessful = Authenticate(addCookie, u, c, persist, null);
                    //}

                    if (KM.Platform.User.IsSystemAdministrator(u))
                    {
                        KMPlatform.Entity.Client c = (new KMPlatform.BusinessLogic.Client()).Select(u.DefaultClientID, false);

                        if (c != null && c.IsActive)
                        {
                            authenticationSuccessful = Authenticate(addCookie, u, c, persist);
                        }
                        else 
                        {
                            List<KMPlatform.Entity.Client> lc = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroup(u.DefaultClientGroupID);
                            authenticationSuccessful = Authenticate(addCookie, u, lc.First(), persist);
                            throw new KMPlatform.Object.UserLoginException() { CurrentUser = u, UserStatus = KMPlatform.Enums.UserLoginStatus.ShowProfileChangeMessage };
                        }

                    }
                    else
                    {
                        List<KMPlatform.Entity.Client> lc = AccountProvider.Client_GetByUserID(u.UserID, false);
                        //KMPlatform.Entity.Client lc = new KMPlatform.BusinessLogic.Client().Select(u.DefaultClientID);
                        if (lc != null)
                        {
                            KMPlatform.Entity.Client c = lc.SingleOrDefault(x => x.ClientID == u.DefaultClientID);
                            if(c != null)
                                authenticationSuccessful = Authenticate(addCookie, u, c, persist);
                            else if(c == null && lc.Count > 0)
                            {
                                authenticationSuccessful = Authenticate(addCookie, u, lc.First(), persist);
                                throw new KMPlatform.Object.UserLoginException() { CurrentUser = u, UserStatus = KMPlatform.Enums.UserLoginStatus.ShowProfileChangeMessage };
                            }
                            else
                            {
                                //User doesn't have an active default client
                                throw new KMPlatform.Object.UserLoginException() { CurrentUser = u, UserStatus = KMPlatform.Enums.UserLoginStatus.NoActiveClients };
                            }
                        }
                    }


                }
            }
            catch (KMPlatform.Object.UserLoginException ule)
            {
                throw ule;
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError("Authentication: caught exception. Exception:{0}", e);
            }

            return u;
        }


        public bool Authenticate(Action<HttpCookie> addCookie, KMPlatform.Entity.User u, KMPlatform.Entity.Client c, bool persist)
        {
            try
            {
                FormsAuthentication.Initialize();

                string userIdString = u.UserID.ToString();

                string userData = CreateAuthenticationTicketUserData(userIdString, u, c);
                HttpCookie cookie = CreateAuthenticationCookies(userIdString, userData, persist);

                addCookie(cookie);
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError("Authentication: caught exception. Exception:{0}", e);
            }

            Deauthenticate(addCookie);
            return false;
        }


        private HttpCookie CreateAuthenticationCookies(string userIdString, string userData, bool persist)
        {
            FormsAuthentication.SetAuthCookie(userIdString, persist);

            // Create a new ticket used for authentication
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1, // Ticket version
                userIdString, // UserID associated with ticket
                DateTime.Now, // Date/time issued
                DateTime.Now.AddDays(30), // Date/time to expire
                true, // "true" for a persistent user cookie
                userData, // User-data, in this case the roles
                FormsAuthentication.FormsCookiePath); // Path cookie valid for

            // Hash the cookie for transport
            string hash = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(
                FormsAuthentication.FormsCookieName, // Name of auth cookie
                hash); // Hashed ticket

            return cookie;
        }

        private string CreateAuthenticationTicketUserData(string userIdString, KMPlatform.Entity.User u, KMPlatform.Entity.Client c)
        {
            ECN_Framework_Entities.Accounts.Customer ecnCustomer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(c.ClientID, false);
            ECN_Framework_Entities.Accounts.BaseChannel bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(ecnCustomer.BaseChannelID.Value);
            return String.Join(",",
                ecnCustomer.CustomerID,
                ecnCustomer.BaseChannelID,
                bc.PlatformClientGroupID,
                ecnCustomer.PlatformClientID,
                u.AccessKey
                );
            /*string userData =
                c.CustomerID + "," +
                c.BaseChannelID + "," +
                c.CommunicatorChannelID.Value.ToString() + c.CollectorChannelID.Value.ToString() + c.CreatorChannelID.Value.ToString()
                    + c.PublisherChannelID.Value.ToString() + c.CharityChannelID.Value.ToString() + "," +
                u.AccountsOptions + u.CommunicatorOptions + u.CollectorOptions + u.CreatorOptions + "," +
                c.CommunicatorLevel + c.CollectorLevel + c.CreatorLevel + c.AccountsLevel + c.PublisherLevel + c.CharityLevel;

            if(overrideMasterId.HasValue)
            {
                userData += "," + overrideMasterId.Value;
            }
            else if (u.IsSysAdmin || u.IsChannelAdmin)
            {
                userData += "," + userIdString;
            }

            return userData;*/
        }

        #region cruft - this version does manual validation of security rules, now (better) expressed by HasRole.CanImpersonate()

        /*
        /// <summary>
        /// Authenticate via impersonation, requires 
        /// <ol>
        ///   <li>An Authentication Ticket reflecting an active master user with the Channel Administrator (or greater) privilege, and</li>
        ///   <ul>
        ///     <li>Master user is a System Administrator and user is active and customer is active, or </li>
        ///     <li>Master user is a Channel Administrator and user is not a system administrator 
        ///     and user is active and customer is active and user customer ID equals customer ID 
        ///     and customer and customer of master user are in the same base channel</li>
        ///   </ul>
        /// </ol>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="authenticationTicket"></param>
        /// <param name="user">the customer to impersonate</param>
        /// <param name="customer">the customer to impersonate</param>
        /// <param name="persist">whether authentication token cookie should span browser sessions</param>
        /// <returns>true if new authentication token cookie was added to response, otherwise false</returns>
        public bool Authenticate(HttpResponseBase response, EcnAuthenticationTicket authenticationTicket, EcnUser user, EcnCustomer customer, bool persist) {

            bool canAuthenticate = false;
            bool didAuthenticate = true;
            int? masterUserId = null;
            string authenticationFailureReason = "<unknown>";
            

            if(null == user)
            {
                throw new ArgumentNullException("user");
            }
            if(null == customer)
            {
                throw new ArgumentNullException("customer");
            }

            if (null != authenticationTicket)
            {
                masterUserId = authenticationTicket.MasterUserID;
                if (HasRole.IsChannelMaster(authenticationTicket))
                {
                    EcnUser masteruser = AccountProvider.User_GetByUserID(masterUserId.Value, false);
                    EcnCustomer masterusercustomer = AccountProvider.Customer_GetByUserID(masterUserId.Value, masteruser.CustomerID.Value, false);
                    if (HasRole.IsSystemAdministrator(masteruser))
                    {
                        if (HasRole.IsActive(user))
                        {
                            if (HasRole.IsActive(customer))
                            {
                                canAuthenticate = true;
                            }
                            else authenticationFailureReason = "inactive customer";
                        }
                        else authenticationFailureReason = "inactive user";
                    }
                    else if (HasRole.IsSameChannel(masterusercustomer, customer))
                    {
                        if (HasRole.IsChannelAdministrator(masteruser))
                        {
                            if (false == HasRole.IsSystemAdministrator(user))
                            {
                                canAuthenticate = true;
                            }
                            else authenticationFailureReason = "target user is system administrator but master user is not";
                        }
                        else authenticationFailureReason = "attempt to change user but master user is not system or channel admin";
                    }
                    else authenticationFailureReason = "attempt to change base-channel but master is not a system admin";

                }
                // ERGO: false == HasRole.IsChannelMaster(authenticationTicket)
                else authenticationFailureReason = "no master-id record";
            }
            // ERGO: null == authenticationTicket
            else authenticationFailureReason = "no auth ticket";

            if(canAuthenticate)
            {
                didAuthenticate = Authenticate(response, user, customer, persist, masterUserId); 
            }

            if(false == didAuthenticate)
            {
                // TODO: log authentication failure?
                string message = String.Format("Authentication: impersonate failure. Reason: {0}, MasterUserId:{1}, UserId:{2}, CustomerId:{3}",
                    authenticationFailureReason,
                    (authenticationTicket != null ? authenticationTicket.MasterUserID.ToString() : "<NONE>"),
                    user.UserID,
                    customer.CustomerID);

                System.Diagnostics.Trace.TraceWarning(message);
            }

            return didAuthenticate;
        }*/

        #endregion cruft
    }
}