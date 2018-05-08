using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KMSite;

namespace ecn.domaintracking.Controllers
{
    public class DomainTrackingKMAuthenticationManager : KMAuthenticationManager
    {
        protected override bool IsSystemAdministrator()
        {
            return KM.Platform.User.IsSystemAdministrator(CurrentUser);
        }
    }
}