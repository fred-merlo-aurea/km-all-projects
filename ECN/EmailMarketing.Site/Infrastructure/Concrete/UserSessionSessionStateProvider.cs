using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using EmailMarketing.Site.Infrastructure.Abstract;

namespace EmailMarketing.Site.Infrastructure.Concrete
{
    public class UserSessionSessionStateProvider : IUserSessionProvider 
    {
        public ECN_Framework_BusinessLayer.Application.ECNSession GetUserSession()
        {
            return UserSession;
        }

        public ECN_Framework_Entities.Application.AuthenticationTicket GetAuthenticationTicket()
        {
            return AuthenticationTicket;
        }


        public static ECN_Framework_Entities.Application.AuthenticationTicket AuthenticationTicket
        {
            get
            {
                return ECN_Framework_BusinessLayer.Application.AuthenticationTicket.getTicket();
            }
        }

        public static ECN_Framework_BusinessLayer.Application.ECNSession UserSession
        {
            get
            {
                return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            }
        }
    }
}