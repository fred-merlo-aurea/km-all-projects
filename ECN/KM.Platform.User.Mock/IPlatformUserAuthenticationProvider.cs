using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace KM.Platform.User.Mock
{
    public interface IPlatformUserAuthenticationProvider
    {
        IPlatformUser Authenticate(string username, string password);
        IPlatformUser Authenticate(Guid accessKey);
    }
}
