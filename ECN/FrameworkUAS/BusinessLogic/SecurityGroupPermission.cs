using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace KMPlatform.BusinessLogic
{
    public class SecurityGroupPermission
    {
        public static List<Entity.SecurityGroupPermission> Select(int securityGroupID)
        {
            return DataAccess.SecurityGroupPermission.Select(securityGroupID);
        }

        public static List<Entity.SecurityGroupPermission.Permission> GetPermissions(int securityGroupID)
        {
            return DataAccess.SecurityGroupPermission.GetPermissions(securityGroupID);
        }

        public int Save(Entity.SecurityGroupPermission x)
        {
            return DataAccess.SecurityGroupPermission.Save(x);
        }
    }
}
