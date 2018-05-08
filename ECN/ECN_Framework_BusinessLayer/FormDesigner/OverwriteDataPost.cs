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
    public class OverwriteDataPost
    {
        public static int Save(ECN_Framework_Entities.FormDesigner.OverwriteDataPost odp)
        {
            int retID = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.FormDesigner.OverwriteDataPost.Save(odp);
                scope.Complete();
            }
            return retID;
        }

        public static void DeleteByRuleID(int ruleID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.FormDesigner.OverwriteDataPost.DeleteByRuleID(ruleID);
                scope.Complete();
            }
        }

        public static List<ECN_Framework_Entities.FormDesigner.OverwriteDataPost> GetByRuleID(int ruleID)
        {
            List<ECN_Framework_Entities.FormDesigner.OverwriteDataPost> retList = new List<ECN_Framework_Entities.FormDesigner.OverwriteDataPost>();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.FormDesigner.OverwriteDataPost.GetByRuleID(ruleID);
                scope.Complete();
            }
            return retList;

        }
    }
}
