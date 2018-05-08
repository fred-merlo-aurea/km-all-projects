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
    public class RequestQueryValue
    {
        public static int Save(ECN_Framework_Entities.FormDesigner.RequestQueryValue rqv)
        {
            int retID = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.FormDesigner.RequestQueryValue.Save(rqv);
                scope.Complete();
            }
            return retID;
        }

        public static void DeleteByRuleID(int ruleID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.FormDesigner.RequestQueryValue.DeleteByRuleID(ruleID);
                scope.Complete();
            }
        }

        public static List<ECN_Framework_Entities.FormDesigner.RequestQueryValue> GetByRuleID(int ruleID)
        {
            List<ECN_Framework_Entities.FormDesigner.RequestQueryValue> retList = new List<ECN_Framework_Entities.FormDesigner.RequestQueryValue>();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.FormDesigner.RequestQueryValue.GetByRuleID(ruleID);
                scope.Complete();
            }
            return retList;

        }

    }
}
