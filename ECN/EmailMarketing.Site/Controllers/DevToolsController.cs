using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EmailMarketing.Site.Infrastructure.Abstract;
using EmailMarketing.Site.Infrastructure.Abstract.Settings;
using EmailMarketing.Site.Infrastructure.Authorization;

namespace EmailMarketing.Site.Controllers
{
    public class DevToolsController : ControllerBase
    {
        public DevToolsController(
            IUserSessionProvider userSessionProvider, 
            IPathProvider pathProvider,
            IBaseChannelProvider baseChannelProvider)
            : base(userSessionProvider, pathProvider, baseChannelProvider)
        {

        }

        
        /// put stuff like cache clear tool here
        /// link to each action from the index

        [RequireSystemAdministrator]
        public ActionResult Index()
        {
            return View();
        }
    }
}