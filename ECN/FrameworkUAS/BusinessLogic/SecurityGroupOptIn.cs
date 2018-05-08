using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    [Serializable]
    public class SecurityGroupOptIn
    {
        public int Save(Entity.SecurityGroupOptIn sgoi)
        {
            int retID = -1;
            using(TransactionScope scope = new TransactionScope())
            {
                retID = DataAccess.SecurityGroupOptIn.Save(sgoi);
                scope.Complete();
            }
            return retID;
        }

        public void Delete(int securityGroupOptInID)
        {
            using(TransactionScope scope = new TransactionScope())
            {
                DataAccess.SecurityGroupOptIn.Delete(securityGroupOptInID);
                scope.Complete();
            }
        }

        public void Delete(int SecurityGroupID, int UserID)
        {
            using(TransactionScope scope = new TransactionScope())
            {
                DataAccess.SecurityGroupOptIn.Delete(SecurityGroupID, UserID);
                scope.Complete();
            }
        }

        public List<Entity.SecurityGroupOptIn> GetBySetID(Guid setID)
        {
            List<Entity.SecurityGroupOptIn> retList = new List<Entity.SecurityGroupOptIn>();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = DataAccess.SecurityGroupOptIn.GetBySetID(setID);
                scope.Complete();
            }
            return retList;
        }

        public void MarkAsAccepted(int securityGroupOptInID)
        {
            using(TransactionScope scope = new TransactionScope())
            {
                DataAccess.SecurityGroupOptIn.MarkAsAccepted(securityGroupOptInID);
                scope.Complete();
            }
        }

        public List<Entity.SecurityGroupOptIn> SelectPendingForUser(int UserID)
        {
            List<Entity.SecurityGroupOptIn> retList = new List<Entity.SecurityGroupOptIn>();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = DataAccess.SecurityGroupOptIn.SelectPendingForUser(UserID);
                scope.Complete();
            }
            return retList;
        }

        public List<Entity.SecurityGroupOptIn> SelectBySecurityGroup_UserID(int securityGroup, int userID)
        {
            List<Entity.SecurityGroupOptIn> retList = new List<Entity.SecurityGroupOptIn>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = DataAccess.SecurityGroupOptIn.SelectForSecurityGroup_UserID(securityGroup,userID);
                scope.Complete();
            }
            return retList;
        }
    }
}
