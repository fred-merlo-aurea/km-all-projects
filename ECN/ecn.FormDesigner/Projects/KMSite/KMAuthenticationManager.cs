using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using ECN_Framework_BusinessLayer.Application;

namespace KMSite
{
    public class KMAuthenticationManager : IKMAuthenticationManager
    {
        private const int DaysTillAuthenticationTicketExpires = 30;

        protected KMPlatform.Entity.User CurrentUser
        {
            get { return ECNSession.CurrentSession().CurrentUser; }
        }


        /// <summary>
        ///  Adds encrypted <see cref="FormsAuthentication"/> cookie 
        /// </summary>
        /// <param name="clientId">Client id</param>
        /// <param name="clientGroupId">Client group id </param>
        /// <param name="cookieCollection">Response cookie collection where new <see cref="FormsAuthenticationTicket"/> is being added </param>
        /// <returns>True if AuthenticationCookie was added</returns>
        public bool AddFormsAuthenticationCookie(int clientId, int clientGroupId, HttpCookieCollection cookieCollection)
        {
            return AddFormsAuthenticationCookieInternal(clientId, clientGroupId, cookieCollection, null);
        }

        /// <summary>
        ///  Adds encrypted <see cref="FormsAuthentication"/> cookie
        /// </summary>
        /// <param name="clientId">Client id</param>
        /// <param name="clientGroupId">Client group id </param>
        /// <param name="cookieCollection">Response cookie collection where new <see cref="FormsAuthenticationTicket"/> is being added </param>
        /// <param name="productId">Product id </param>
        /// <returns>True if AuthenticationCookie was added</returns>
        public bool AddFormsAuthenticationCookie(int clientId, int clientGroupId, int productId, HttpCookieCollection cookieCollection)
        {
            return AddFormsAuthenticationCookieInternal(clientId, clientGroupId, cookieCollection, productId);
        }

        public bool HasAuthorized(int clientId)
        {
            if (CurrentUser.UserClientSecurityGroupMaps.Find(x => x.ClientID == clientId) != null)
            {
                return true;
            }

            return false;
        }

        private bool AddFormsAuthenticationCookieInternal(int clientId, int clientGroupId, HttpCookieCollection cookieCollection, int? productId)
        {
            var userID = CurrentUser.UserID;

            if (clientId > 0 &&
                (IsSystemAdministrator()
                || HasAuthorized(clientId)))
            {
                FormsAuthentication.SignOut();
                FormsAuthentication.SetAuthCookie(userID.ToString(), false);

                // Create a new ticket used for authentication
                var ticket = new FormsAuthenticationTicket(
                    1, // Ticket version
                    userID.ToString(), // UserID associated with ticket
                    DateTime.Now, // Date/time issued
                    DateTime.Now.AddDays(DaysTillAuthenticationTicketExpires), // Date/time to expire
                    true, // "true" for a persistent user cookie
                    CreateAuthenticationTicketUserData(CurrentUser, clientGroupId, clientId, productId), // User-data, in this case the roles
                    FormsAuthentication.FormsCookiePath); // Path cookie valid for

                // Hash the cookie for transport
                var hash = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(
                    FormsAuthentication.FormsCookieName, // Name of auth cookie
                    hash); // Hashed ticket

                // Add the cookie to the list for outgoing response
                cookieCollection.Add(cookie);

                //Wiping out local UserSession so it will get what we just updated
                ECNSession.CurrentSession().ClearSession();
                return true;
            }

            return false;
        }

        protected virtual bool IsSystemAdministrator()
        {
            return KMPlatform.BusinessLogic.User.IsSystemAdministrator(CurrentUser);
        }

        private static string CreateAuthenticationTicketUserData(
            KMPlatform.Entity.User user,
            int clientgroupID,
            int clientID,
            int? productId)
        {
            var ecnCustomer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(clientID, false);
            var parameters = new List<object> {
                ecnCustomer.CustomerID,
                ecnCustomer.BaseChannelID,
                clientgroupID,
                clientID,
                user.AccessKey};

            if (productId.HasValue)
            {
                parameters.Add(productId.Value);
            }

            return string.Join(",",  parameters);
        }

    }
}