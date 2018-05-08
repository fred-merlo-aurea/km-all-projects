using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;

namespace ecn.accounts.customersmanager
{
    public partial class customerlogin : ECN_Framework_BusinessLayer.Application.WebPageHelper
    {
        protected void Page_Load(object sender, System.EventArgs e) 
        {
            //bool isauthorizedUser = false;

            //int masterUserID = ECN_Framework_BusinessLayer.Application.AuthenticationTicket.getTicket().MasterUserID;
            //int masterUserBaseChannelID = 0;
            //bool isOrWasSystemAdministrator = false;

            ////Check masteruserID exists.
            //if (masterUserID > 0)
            //{
            //    KMPlatform.Entity.User masteruser = KMPlatform.BusinessLogic.User.GetByUserID(masterUserID, false);
            //    ECN_Framework_Entities.Accounts.Customer masterusercustomer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByUserID(masterUserID, masteruser.CustomerID.Value, false);
            //    masterUserBaseChannelID = masterusercustomer.BaseChannelID.Value;                
            //    isOrWasSystemAdministrator = masterKM.Platform.User.IsSystemAdministrator(user);

            //    if (masteruser.ActiveFlag.ToUpper() == "Y" && (masterKM.Platform.User.IsSystemAdministrator(user) || (masterKM.Platform.User.IsChannelAdministrator(user) && Master.UserSession.CurrentBaseChannel.BaseChannelID == masterusercustomer.BaseChannelID.Value)))
            //        isauthorizedUser = true;
            //}
            //else if (KM.Platform.User.IsChannelAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
            //{  // pretty sure this is unreachable b/c MasterUserId always added to auth cookie when System or Channel admin does login
            //    isauthorizedUser = true;
            //    masterUserID = Master.UserSession.CurrentUser.UserID;
            //    masterUserBaseChannelID = Master.UserSession.CurrentCustomer.BaseChannelID.Value;
            //    isOrWasSystemAdministrator = KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser);
            //}

            //if (isauthorizedUser)
            //{
            //    IEnumerable<KMPlatform.Entity.User> luser = 
            //        from x in KMPlatform.BusinessLogic.User.GetByCustomerID(int.Parse(Request["CustomerID"].ToString()))
            //       where KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser)
            //             || ((false == x.IsSysAdmin)
            //                 && (Master.UserSession.CurrentKM.Platform.User.IsChannelAdministrator(user)
            //                 || (false == x.IsChannelAdmin)))
            //      select x;

            //    KMPlatform.Entity.User u = (KMPlatform.Entity.User) luser.FirstOrDefault(x => x.IsChannelAdmin && x.ActiveFlag.ToUpper() == "Y");

            //    if (u == null)
            //    {
            //        u = (KMPlatform.Entity.User)luser.FirstOrDefault(x => x.IsAdmin && x.ActiveFlag.ToUpper() == "Y");
            //    }

            //    if (u == null)
            //    {
            //        u = (KMPlatform.Entity.User)luser.FirstOrDefault(x => x.ActiveFlag.ToUpper() == "Y");
            //    }

            //    if (u != null)
            //    {
            //        ECN_Framework_Entities.Accounts.Customer c = ECN_Framework_BusinessLayer.Accounts.Customer.GetByUserID(u.UserID, u.CustomerID.Value, false);

            //        if (c != null && (isOrWasSystemAdministrator || c.BaseChannelID == masterUserBaseChannelID))
            //        {
            //            //-- TODO sunil -- get rid of storing the user details in UserData & use User or Customer objects from ECNSession 
            //            // -- USE this store User Role - redefine UserRoles in ECN database.

            //            string UD = c.CustomerID + "," +
            //                c.BaseChannelID + "," +
            //                c.CommunicatorChannelID.Value.ToString() + c.CollectorChannelID.Value.ToString() + c.CreatorChannelID.Value.ToString() + c.PublisherChannelID.Value.ToString() + c.CharityChannelID.Value.ToString() + "," +
            //                u.AccountsOptions + u.CommunicatorOptions + u.CollectorOptions + u.CreatorOptions + "," +
            //                c.CommunicatorLevel + c.CollectorLevel + c.CreatorLevel + c.AccountsLevel + c.PublisherLevel + c.CharityLevel;

            //            if (isauthorizedUser)
            //                UD = UD + "," + masterUserID.ToString();

            //            FormsAuthentication.SetAuthCookie(u.UserID.ToString(), false);

            //            // Create a new ticket used for authentication
            //            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
            //                1, // Ticket version
            //                u.UserID.ToString(), // UserID associated with ticket
            //                DateTime.Now, // Date/time issued
            //                DateTime.Now.AddDays(30), // Date/time to expire
            //                true, // "true" for a persistent user cookie
            //                UD, // User-data, in this case the roles
            //                FormsAuthentication.FormsCookiePath); // Path cookie valid for

            //            // Hash the cookie for transport
            //            string hash = FormsAuthentication.Encrypt(ticket);
            //            HttpCookie cookie = new HttpCookie(
            //                FormsAuthentication.FormsCookieName, // Name of auth cookie
            //                hash); // Hashed ticket

            //            // Add the cookie to the list for outgoing response
            //            Response.Cookies.Add(cookie);

            //            string redirectPage = string.Empty;
            //            try
            //            {
            //                redirectPage = ConfigurationManager.AppSettings[c.BaseChannelID.Value + "_" + c.CustomerID + "_Home_Redirect"].ToString();
            //            }
            //            catch
            //            {
            //                try
            //                {
            //                    redirectPage = ConfigurationManager.AppSettings[c.BaseChannelID.Value + "_Home_Redirect"].ToString();
            //                }
            //                catch
            //                {
            //                    redirectPage = ConfigurationManager.AppSettings["ECN_Home_Redirect"].ToString();
            //                }
            //            }

            //            Response.Redirect(redirectPage, true);
            //        }
            //    }

            //    #region OLD Code
            //    //if (dt != null && dt.Rows.Count > 0)
            //    //{
            //    //    DataRow dr = dt.Rows[0];
            //    //    String UserID = "";
            //    //    String BaseChannelID = "";
            //    //    String UD = "";

            //    //    if (!KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser) && Master.UserSession.CurrentKM.Platform.User.IsChannelAdministrator(user) && Convert.ToInt32(dr["BaseChannelID"].ToString()) != Master.UserSession.CurrentBaseChannel.BaseChannelID)
            //    //        isauthorizedUser = false;

            //    //    if (isauthorizedUser)
            //    //    {
            //    //        UserID = dr["UserID"].ToString();
            //    //        BaseChannelID = dr["BaseChannelID"].ToString();
            //    //        UD = dr["CustomerID"].ToString() + "," +
            //    //            dr["BaseChannelID"].ToString() + "," +
            //    //            dr["CommunicatorChannelID"].ToString() + dr["CollectorChannelID"].ToString() + dr["CreatorChannelID"].ToString() + dr["PublisherChannelID"].ToString() + dr["CharityChannelID"].ToString() + "," +
            //    //            dr["AccountsOptions"].ToString() + dr["CommunicatorOptions"].ToString() + dr["CollectorOptions"].ToString() + dr["CreatorOptions"].ToString() + "," +
            //    //            dr["CommunicatorLevel"].ToString() + dr["CollectorLevel"].ToString() + dr["CreatorLevel"].ToString() + dr["AccountsLevel"].ToString() + dr["PublisherLevel"].ToString() + dr["CharityLevel"].ToString();

            //    //        if (isauthorizedUser)
            //    //            UD = UD + "," + masterUserID.ToString();

            //    //        FormsAuthentication.SetAuthCookie(UserID, true);

            //    //        // Create a new ticket used for authentication
            //    //        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
            //    //            1, // Ticket version
            //    //            UserID, // UserID associated with ticket
            //    //            DateTime.Now, // Date/time issued
            //    //            DateTime.Now.AddMinutes(900), // Date/time to expire
            //    //            true, // "true" for a persistent user cookie
            //    //            UD, // User-data, in this case the roles
            //    //            FormsAuthentication.FormsCookiePath); // Path cookie valid for

            //    //        // Hash the cookie for transport
            //    //        string hash = FormsAuthentication.Encrypt(ticket);
            //    //        HttpCookie cookie = new HttpCookie(
            //    //            FormsAuthentication.FormsCookieName, // Name of auth cookie
            //    //            hash); // Hashed ticket

            //    //        // Add the cookie to the list for outgoing response
            //    //        Response.Cookies.Add(cookie);
            //    //        es.RefreshSession();
            //    //        Response.Redirect("../../main/default.aspx", true);
            //    //    }
            //    //}
            //    #endregion
            //}

            Response.Redirect("~/main/securityAccessError.aspx", true);
        }

    }
}
