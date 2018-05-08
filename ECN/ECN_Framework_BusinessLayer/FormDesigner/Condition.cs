using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using KMPlatform.Entity;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.FormDesigner
{
    [Serializable]
    public class Condition
    {
        public static List<ECN_Framework_Entities.FormDesigner.Condition> GetByCondGroupID(int CondGroupID)
        {
            List<ECN_Framework_Entities.FormDesigner.Condition> retItem = new List<ECN_Framework_Entities.FormDesigner.Condition>();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retItem = ECN_Framework_DataLayer.FormDesigner.Condition.GetByConditionGroup_ID(CondGroupID);
                scope.Complete();
            }
            return retItem;
        }

        public static ECN_Framework_Entities.FormDesigner.Condition GetByCondID(int CondID)
        {
           ECN_Framework_Entities.FormDesigner.Condition retItem = new ECN_Framework_Entities.FormDesigner.Condition();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retItem = ECN_Framework_DataLayer.FormDesigner.Condition.GetByConditionID(CondID);
                scope.Complete();
            }
            return retItem;
        }

        public static void Delete(int condID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.FormDesigner.Condition.DeleteByID(condID);
                scope.Complete();
            }

        }

        public static void DeleteByGroupID(int condGroupID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.FormDesigner.Condition.DeleteByConditionGroupID(condGroupID);
                scope.Complete();
            }
        }

        public static int Save(ECN_Framework_Entities.FormDesigner.Condition cg)
        {
            int retId = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                retId = ECN_Framework_DataLayer.FormDesigner.Condition.Save(cg);
                scope.Complete();
            }
            return retId;
        }
    }
}
