using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace KMPS.MD.Objects
{
    public class AuthenticationTicket
    {
        public int UserID { get; set; }
        public int CustomerID { get; set; }
        public int BaseChannelID { get; set; }
        public int ClientGroupID { get; set; }
        public int ClientID { get; set; }
        public Guid AccessKey { get; set; }
        public DateTime IssueDate { get; set; }

        public AuthenticationTicket()
        {
        }

        //Get the Ticket info
        public static AuthenticationTicket getTicket()
        {
            AuthenticationTicket at = null;

            FormsAuthenticationTicket ticket = null;

            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        System.Web.Security.FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;

                        try
                        {
                            ticket = id.Ticket;
                            string AuthData = ticket.UserData;
                            string AuthTime = ticket.IssueDate.ToString();

                            string[] myIDs = AuthData.Split(',');


                            at = new AuthenticationTicket();

                            at.UserID = Convert.ToInt32(ticket.Name);
                            at.CustomerID = Convert.ToInt32(myIDs[0]); //NOT REQUIRED - REMOVE - 
                            at.BaseChannelID = Convert.ToInt32(myIDs[1]); //NOT REQUIRED - REMOVE - 
                            at.ClientGroupID = Convert.ToInt32(myIDs[2]);
                            at.ClientID = Convert.ToInt32(myIDs[3]);
                            at.AccessKey = new Guid(myIDs[4]);
                            at.IssueDate = ticket.IssueDate;
                        }
                        catch
                        {
                            at = null;
                        }
                    }
                }
            }

            return at;
        }

    }
}
