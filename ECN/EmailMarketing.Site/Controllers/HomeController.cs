using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EmailMarketing.Site.Infrastructure.Abstract;
using EmailMarketing.Site.Infrastructure.Abstract.Settings;

using EcnNotification = ECN_Framework_Entities.Accounts.Notification;

namespace EmailMarketing.Site.Controllers
{
    [Authorize]
    public class HomeController : ControllerBase
    {
        IAccountProvider AccountProvider;
        public HomeController(IUserSessionProvider userSessionProvider, IPathProvider pathProvider, IBaseChannelProvider baseChannelProvider, IAccountProvider accountProvider)
            : base(userSessionProvider, pathProvider, baseChannelProvider)
        {
            AccountProvider = accountProvider;
        }
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult Notification()
        {
            EcnNotification notification = AccountProvider.Notification_GetByCurrentDateTime();
            return PartialView( notification );
        }
    }
}