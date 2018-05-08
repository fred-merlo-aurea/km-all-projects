using System;
//using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace KMPlatform.Object
{
     public class UserLoginException : System.Exception
    {
         public UserLoginException()
         {
             UserStatus = Enums.UserLoginStatus.DisabledUser;
             CurrentUser = new Entity.User();
         }

         public KMPlatform.Enums.UserLoginStatus UserStatus { get; set; }

         public KMPlatform.Entity.User CurrentUser { get; set; }

    }
}
