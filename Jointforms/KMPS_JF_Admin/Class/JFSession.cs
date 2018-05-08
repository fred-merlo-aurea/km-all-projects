using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Configuration;
using System.Web.Security;

namespace KMPS_JF_Setup
{
    public class JFSession
    {
        public JFSession()
        {
		}

        public string UserID()
        {
            try
            {
                return getCookie().Name;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string UserName()
        {
            try
            {
                return getCookie().UserData.Split('|')[0].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public string AllowedCustoemerIDs()
        {
            try
            {
                return getCookie().UserData.Split('|')[1].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public FormsAuthenticationTicket getCookie()
        {
            FormsAuthenticationTicket ticket = null;
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        System.Web.Security.FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                        ticket = id.Ticket;
                        string AuthData = ticket.UserData;
                        string AuthName = ticket.Name;
                        string AuthTime = ticket.IssueDate.ToString();
                    }
                }
            }
            return ticket;
        }
    }
}
