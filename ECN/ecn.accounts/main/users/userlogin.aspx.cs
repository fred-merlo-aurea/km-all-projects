using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;

namespace ecn.accounts.usersmanager
{
    public partial class userlogin : ECN_Framework_BusinessLayer.Application.WebPageHelper
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {            
            //String sqlQuery = string.Empty;

            //bool isauthorizedUser = false;
            //int masterUserID = ECN_Framework_BusinessLayer.Application.AuthenticationTicket.getTicket().MasterUserID;
            //int masterBaseChannelID = 0;
            //bool hasSysAdmin = false;
            //bool hasChannelAdmin = false;
            //int targetUserId = int.Parse(Request["UserID"].ToString());

            ////Check masteruserID exists.
            //if (masterUserID > 0)
            //{
            //    KMPlatform.Entity.User masteruser = KMPlatform.BusinessLogic.User.GetByUserID(masterUserID, false);
            //    ECN_Framework_Entities.Accounts.Customer masterusercustomer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByUserID(masterUserID, masteruser.CustomerID.Value, false);

            //    masterBaseChannelID = masterusercustomer.BaseChannelID.Value;
            //    hasSysAdmin = masterKM.Platform.User.IsSystemAdministrator(user);
            //    hasChannelAdmin = masterKM.Platform.User.IsChannelAdministrator(user);

            //    if (masteruser.ActiveFlag.ToUpper() == "Y" 
            //        && (masterKM.Platform.User.IsSystemAdministrator(user) 
            //            || (masterKM.Platform.User.IsChannelAdministrator(user) && Master.UserSession.CurrentBaseChannel.BaseChannelID == masterusercustomer.BaseChannelID.Value)
            //            || (masterUserID == targetUserId)
            //            || (masteruser.IsAdmin && masteruser.HasUserAccess)))
            //        isauthorizedUser = true;
            //}
            //else if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser) || ((Master.UserSession.CurrentKM.Platform.User.IsChannelAdministrator(user) || Master.UserSession.CurrentUser.IsAdmin) && Master.UserSession.CurrentUser.HasUserAccess))
            //{
            //    isauthorizedUser = true;
            //    masterUserID = Master.UserSession.CurrentUser.UserID;
            //    masterBaseChannelID = Master.UserSession.CurrentCustomer.BaseChannelID.Value;
            //    hasSysAdmin = KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser);
            //    hasChannelAdmin = Master.UserSession.CurrentKM.Platform.User.IsChannelAdministrator(user);
            //}

            //if (isauthorizedUser)
            //{
            //     KMPlatform.Entity.User u = KMPlatform.BusinessLogic.User.GetByUserID(targetUserId, true);

            //     if (u != null && u.ActiveFlag.ToUpper() == "Y" && (hasSysAdmin || (false == u.IsSysAdmin && (hasChannelAdmin || false == u.IsChannelAdmin))))
            //     {
            //         ECN_Framework_Entities.Accounts.Customer c = ECN_Framework_BusinessLayer.Accounts.Customer.GetByUserID(u.UserID, u.CustomerID.Value, false);

            //         if (c != null && (hasSysAdmin || masterBaseChannelID == c.BaseChannelID))
            //         {
            //             //-- TODO sunil -- get rid of storing the user details in UserData & use User or Customer objects from ECNSession 
            //             // -- USE this store User Role - redefine UserRoles in ECN database.

            //             string UD = c.CustomerID + "," +
            //                 c.BaseChannelID + "," +
            //                 c.CommunicatorChannelID.Value.ToString() + c.CollectorChannelID.Value.ToString() + c.CreatorChannelID.Value.ToString() + c.PublisherChannelID.Value.ToString() + c.CharityChannelID.Value.ToString() + "," +
            //                 u.AccountsOptions + u.CommunicatorOptions + u.CollectorOptions + u.CreatorOptions + "," +
            //                 c.CommunicatorLevel + c.CollectorLevel + c.CreatorLevel + c.AccountsLevel + c.PublisherLevel + c.CharityLevel;

            //             if (isauthorizedUser)
            //                 UD = UD + "," + masterUserID.ToString();

            //             FormsAuthentication.SetAuthCookie(u.UserID.ToString(), false);

            //             // Create a new ticket used for authentication
            //             FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
            //                 1, // Ticket version
            //                 u.UserID.ToString(), // UserID associated with ticket
            //                 DateTime.Now, // Date/time issued
            //                 DateTime.Now.AddDays(30), // Date/time to expire
            //                 true, // "true" for a persistent user cookie
            //                 UD, // User-data, in this case the roles
            //                 FormsAuthentication.FormsCookiePath); // Path cookie valid for

            //             // Hash the cookie for transport
            //             string hash = FormsAuthentication.Encrypt(ticket);
            //             HttpCookie cookie = new HttpCookie(
            //                 FormsAuthentication.FormsCookieName, // Name of auth cookie
            //                 hash); // Hashed ticket

            //             // Add the cookie to the list for outgoing response
            //             Response.Cookies.Add(cookie);

            //             string redirectPage = string.Empty;
            //             try
            //             {
            //                 redirectPage = ConfigurationManager.AppSettings[c.BaseChannelID.Value + "_" + c.CustomerID + "_Home_Redirect"].ToString();
            //             }
            //             catch
            //             {
            //                 try
            //                 {
            //                     redirectPage = ConfigurationManager.AppSettings[c.BaseChannelID.Value + "_Home_Redirect"].ToString();
            //                 }
            //                 catch
            //                 {
            //                     redirectPage = ConfigurationManager.AppSettings["ECN_Home_Redirect"].ToString();
            //                 }
            //             }

            //             Response.Redirect(redirectPage, true);
            //         }
            //     }
            //}

            Response.Redirect("../../main/securityAccessError.aspx", true);

        }
    }
}
