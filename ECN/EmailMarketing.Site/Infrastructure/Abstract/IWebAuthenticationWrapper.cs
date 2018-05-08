using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace EmailMarketing.Site.Infrastructure.Abstract
{
    /// <summary>
    /// testability wrapper for System.Web.Security.FormsAuthentication
    /// </summary>
    public interface IWebAuthenticationWrapper
    {
        void Initialize();
        void SetAuthCookie(string userName, bool createPersistentCookie);
        string Encrypt(FormsAuthenticationTicket ticket);
        void SignOut();
        string FormsCookieName { get; }
        string FormsCookiePath { get; }
    }
}
