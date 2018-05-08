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
    public class Rule
    {
        public static List<ECN_Framework_Entities.FormDesigner.Rule> GetByFormID(int FormID, bool getChildren = false)
        {
            List<ECN_Framework_Entities.FormDesigner.Rule> rules = new List<ECN_Framework_Entities.FormDesigner.Rule>();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                rules = ECN_Framework_DataLayer.FormDesigner.Rule.GetByFormID(FormID);
                scope.Complete();
            }

            if (rules != null && getChildren)
            {
                //get children
                foreach(ECN_Framework_Entities.FormDesigner.Rule r in rules)
                {
                    r.ConditionGroup = ECN_Framework_BusinessLayer.FormDesigner.ConditionGroup.GetByCondGroupID(r.ConditionGroup_Seq_ID);
                    r.OverwritePostValue = ECN_Framework_BusinessLayer.FormDesigner.OverwriteDataPost.GetByRuleID(r.Rule_Seq_ID);
                    r.RequestQueryValue = ECN_Framework_BusinessLayer.FormDesigner.RequestQueryValue.GetByRuleID(r.Rule_Seq_ID);
                }
            }

            return rules;
        }

        public static ECN_Framework_Entities.FormDesigner.Rule GetByRuleID(int RuleID, bool getChildren = false)
        {
            ECN_Framework_Entities.FormDesigner.Rule rule = new ECN_Framework_Entities.FormDesigner.Rule();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                rule = ECN_Framework_DataLayer.FormDesigner.Rule.GetByRuleID(RuleID);
                scope.Complete();
            }

            if(rule != null && rule.Rule_Seq_ID > 0 && getChildren)
            {
                rule.ConditionGroup = ECN_Framework_BusinessLayer.FormDesigner.ConditionGroup.GetByCondGroupID(rule.ConditionGroup_Seq_ID);
                rule.OverwritePostValue = ECN_Framework_BusinessLayer.FormDesigner.OverwriteDataPost.GetByRuleID(rule.Rule_Seq_ID);
                rule.RequestQueryValue = ECN_Framework_BusinessLayer.FormDesigner.RequestQueryValue.GetByRuleID(rule.Rule_Seq_ID);
                
            }

            return rule;
        }

        public static int Save(ECN_Framework_Entities.FormDesigner.Rule rule)
        {
            int retID = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.FormDesigner.Rule.Save(rule);
                scope.Complete();
            }
            return retID;
        }

        public static void Delete(int RuleID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                
                ECN_Framework_DataLayer.FormDesigner.Rule.Delete(RuleID);
                scope.Complete();
            }
        }

        
    }
}
