using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Web;
using ECN_Framework_Entities.Application;

namespace ECN_Framework_BusinessLayer.Application
{
    public class AuthenticationTicket
    {
       
        public AuthenticationTicket()
        {
        }

        //Get the Ticket info
        public static ECN_Framework_Entities.Application.AuthenticationTicket getTicket()
        {
            ECN_Framework_Entities.Application.AuthenticationTicket at = null;

            FormsAuthenticationTicket ticket = null;

            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        System.Web.Security.FormsIdentity id = (FormsIdentity) HttpContext.Current.User.Identity;

                        try
                        {
                            ticket = id.Ticket;
                            string AuthData = ticket.UserData;
                            string AuthTime = ticket.IssueDate.ToString();

                            string[] myIDs = AuthData.Split(',');


                            at = new ECN_Framework_Entities.Application.AuthenticationTicket();

                            at.UserID = Convert.ToInt32(ticket.Name);
                            at.CustomerID = Convert.ToInt32(myIDs[0]); //NOT REQUIRED - REMOVE - 
                            at.BaseChannelID = Convert.ToInt32(myIDs[1]); //NOT REQUIRED - REMOVE - 
                            at.ClientGroupID = Convert.ToInt32(myIDs[2]);
                            at.ClientID = Convert.ToInt32(myIDs[3]);
                            at.AccessKey = new Guid(myIDs[4]);
                            at.IssueDate = ticket.IssueDate;
                            if (myIDs.Length > 5)
                                at.ProductID = string.IsNullOrEmpty(myIDs[5]) ? 0 : Convert.ToInt32(myIDs[5]);
                            else
                                at.ProductID = 0;
                        }
                        catch(Exception ex) {
                            at = null;
                        }

                        //try
                        //{
                        //    at.MasterUserID = Convert.ToInt32(myIDs[5]);
                        //}
                        //catch
                        //{
                        //    at.MasterUserID = 0 ;
                        //}
                        
                    }
                }
            }

            return at;
        }

    }
}
