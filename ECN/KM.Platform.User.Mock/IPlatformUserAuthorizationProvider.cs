using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KM.Platform.User.Mock
{
    public interface IPlatformUserAuthorizationProvider
    {
        bool HasMenu(IPlatformUser user, string menuName);
        IEnumerable<IPlatformUserMenuItem> GetUserMenu(IPlatformUser user, string menuName);
        
        bool HasPermission(IPlatformUser user, params string[] permissionNames);
        IEnumerable<IPlatformUserPermission> GetUserPermissions(IPlatformUser user);
    }
}
