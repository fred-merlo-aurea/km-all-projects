using System;
using System.Web;

namespace EmailMarketing.Site.Infrastructure.Authorization
{
    public class RequireSystemAdministratorAttribute : RequireRoleAttributeBase
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (base.AuthorizeCore(httpContext))
            {
                var ecnSession = UserSessionProvider.GetUserSession();
                if (null != ecnSession && HasRole.IsSystemAdministrator(ecnSession.CurrentUser))
                {
                    return true;
                }
            }

            return false;
        }
    }
}