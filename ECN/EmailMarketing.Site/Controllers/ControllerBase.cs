using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EmailMarketing.Site.Infrastructure.Abstract;
using EmailMarketing.Site.Infrastructure.Abstract.Settings;

namespace EmailMarketing.Site.Controllers
{
    /// <summary>
    /// Abstract base-class for MVC controller classes.
    /// Add properties here that should be available to all (or most) derived classes.
    /// For cases where several (but not most) controllers will share a requirement
    /// consider creating a derived abstract base (e.g. 
    ///   abstract public class MyGroupControllerBase : ControllerBase { ...
    /// )
    /// </summary>
    abstract public class ControllerBase : Controller
    {
        protected IUserSessionProvider UserSessionProvider { get; set; }
        protected IPathProvider PathProvider { get; set; }
        protected IBaseChannelProvider BaseChannelProvider { get; set; }

        protected ECN_Framework_Entities.Accounts.BaseChannel BaseChannel
        {
            get
            {
                return BaseChannelProvider.GetBaseChannelForDomainWithDefault(Request.Url.Host, 12);
            }
        }

        protected ControllerBase(IUserSessionProvider userSessionProvider, IPathProvider pathProvider, IBaseChannelProvider baseChannelProvider)
        {
            UserSessionProvider = userSessionProvider;
            PathProvider = pathProvider;
            BaseChannelProvider = baseChannelProvider;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SetupDefaultViewBagContents();
        }

        protected virtual void SetupDefaultViewBagContents()
        {
            ViewBag.CurrentAccount = null != CurrentCustomer ? CurrentCustomer.CustomerName : String.Empty;
            ViewBag.CurrentUser = null != CurrentUser ? CurrentUser.UserName : String.Empty;

            ViewBag.BrandingImageSrc = PathProvider.Images() + "/Channels/" + BaseChannel.BaseChannelID + "/" + BaseChannel.BrandLogo;
        }

        protected ECN_Framework_BusinessLayer.Application.ECNSession UserSession
        {
            get
            {
                return UserSessionProvider.GetUserSession();
            }
        }

        protected KMPlatform.Entity.User CurrentUser
        {
            get
            {
                if(null != UserSession)
                {
                    return UserSession.CurrentUser;
                }
                return null;
            }
        }

        protected ECN_Framework_Entities.Accounts.Customer CurrentCustomer
        {
            get
            {
                if (null != UserSession)
                {
                    return UserSession.CurrentCustomer;
                }
                return null;
            }
        }

        private ECN_Framework_Entities.Application.AuthenticationTicket _authenticationTicket;
        protected ECN_Framework_Entities.Application.AuthenticationTicket AuthenticationTicket
        {
            get
            {
                if(null == _authenticationTicket)
                {
                    _authenticationTicket = UserSessionProvider.GetAuthenticationTicket();
                }
                return _authenticationTicket;
            }
        }
    }
}