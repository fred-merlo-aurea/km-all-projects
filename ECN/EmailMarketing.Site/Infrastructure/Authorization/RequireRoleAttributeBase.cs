using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Ninject;

using EmailMarketing.Site.Infrastructure.Abstract;


namespace EmailMarketing.Site.Infrastructure.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public abstract class RequireRoleAttributeBase : AuthorizeAttribute
    {
        [Inject]
        public IUserSessionProvider UserSessionProvider { get; set; }
    }
}