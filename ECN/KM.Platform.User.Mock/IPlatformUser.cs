using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KM.Platform.User.Mock
{
    public interface IPlatformUser
    {
        string PlatformUserName { get; }
        string PlatformUserPassword { get; } 
        Guid PlatformUserAccessKey { get; } 

        int PlatformUserID { get; } 
        int PlatformUserDefaultClientID { get; }
    }
}
