using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Security;
using EmailMarketing.Site.Infrastructure.Abstract;

namespace EmailMarketing.Site.Infrastructure.Concrete
{
    public class WebAuthenticationWrapper : IWebAuthenticationWrapper
    {
        public void Initialize()
        {
            FormsAuthentication.Initialize();
        }

        public void SetAuthCookie(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public string Encrypt(FormsAuthenticationTicket ticket)
        {
            return FormsAuthentication.Encrypt(ticket);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        public string FormsCookieName
        {
            get { return FormsAuthentication.FormsCookieName; }
        }

        public string FormsCookiePath
        {
            get { return FormsAuthentication.FormsCookiePath; }
        }
    }
}