using System;
using System.Web;

namespace EmailMarketing.Site.Infrastructure.Authorization
{
    public class RequireAdministratorAttribute : RequireRoleAttributeBase
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if(base.AuthorizeCore(httpContext))
            {
                var ecnSession = UserSessionProvider.GetUserSession();
                if(null != ecnSession && HasRole.IsAdministrator(ecnSession.CurrentUser))
                {
                    return true;
                }
            }

            return false;
        }
    }
}