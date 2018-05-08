using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailMarketing.Site.Infrastructure.Abstract
{
    public interface IUserSessionProvider
    {
        ECN_Framework_BusinessLayer.Application.ECNSession GetUserSession();
        ECN_Framework_Entities.Application.AuthenticationTicket GetAuthenticationTicket();
    }
}
