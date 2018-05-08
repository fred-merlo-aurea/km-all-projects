using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class SecurityGroupTemplate
    {
        public static List<KMPlatform.Entity.SecurityGroupTemplate> GetNonAdminTemplates()
        {
            List<KMPlatform.Entity.SecurityGroupTemplate> retList = new List<Entity.SecurityGroupTemplate>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = DataAccess.SecurityGroupTemplate.GetNonAdminTemplates();
                scope.Complete();
            }

            return retList;
        }
    }
}
