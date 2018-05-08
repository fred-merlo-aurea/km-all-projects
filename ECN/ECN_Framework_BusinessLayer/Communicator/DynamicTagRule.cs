using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class DynamicTagRule
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.DynamicTagRule;
        public static void DeleteByDynamicTagID(int DynamicTagID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.DynamicTagRule.DeleteByDynamicTagID(DynamicTagID, user.UserID);
                scope.Complete();
            }
        }

        public static void Delete(int DynamicTagRulesID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.DynamicTagRule.Delete(DynamicTagRulesID, user.UserID);
                scope.Complete();
            }
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static List<ECN_Framework_Entities.Communicator.DynamicTagRule> GetByDynamicTagID_NoAccessCheck(int DynamicTagID, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.DynamicTagRule> DynamicTagRulesList = new List<ECN_Framework_Entities.Communicator.DynamicTagRule>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                DynamicTagRulesList = ECN_Framework_DataLayer.Communicator.DynamicTagRule.GetByDynamicTagID(DynamicTagID);
                if (DynamicTagRulesList.Count > 0 && getChildren)
                {
                    foreach (ECN_Framework_Entities.Communicator.DynamicTagRule DynamicTagRules in DynamicTagRulesList)
                    {
                        DynamicTagRules.Rule = ECN_Framework_BusinessLayer.Communicator.Rule.GetByRuleID_NoAccessCheck(DynamicTagRules.RuleID.Value, getChildren);
                    }
                }
                scope.Complete();
            }
            return DynamicTagRulesList;
        }

        public static List<ECN_Framework_Entities.Communicator.DynamicTagRule> GetByDynamicTagID_NoAccessCheck_UseAmbientTransaction(int DynamicTagID, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.DynamicTagRule> DynamicTagRulesList = new List<ECN_Framework_Entities.Communicator.DynamicTagRule>();
            using (TransactionScope scope = new TransactionScope())
            {
                DynamicTagRulesList = ECN_Framework_DataLayer.Communicator.DynamicTagRule.GetByDynamicTagID(DynamicTagID);
                if (DynamicTagRulesList.Count > 0 && getChildren)
                {
                    foreach (ECN_Framework_Entities.Communicator.DynamicTagRule DynamicTagRules in DynamicTagRulesList)
                    {
                        DynamicTagRules.Rule = ECN_Framework_BusinessLayer.Communicator.Rule.GetByRuleID_NoAccessCheck_UseAmbientTransaction(DynamicTagRules.RuleID.Value, getChildren);
                    }
                }
                scope.Complete();
            }
            return DynamicTagRulesList;
        }

        public static List<ECN_Framework_Entities.Communicator.DynamicTagRule> GetByDynamicTagID(int DynamicTagID, KMPlatform.Entity.User user, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.DynamicTagRule> DynamicTagRulesList = new List<ECN_Framework_Entities.Communicator.DynamicTagRule>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                DynamicTagRulesList = ECN_Framework_DataLayer.Communicator.DynamicTagRule.GetByDynamicTagID(DynamicTagID);
                if (DynamicTagRulesList.Count > 0 && getChildren)
                {
                    foreach (ECN_Framework_Entities.Communicator.DynamicTagRule DynamicTagRules in DynamicTagRulesList)
                    {
                        DynamicTagRules.Rule = ECN_Framework_BusinessLayer.Communicator.Rule.GetByRuleID(DynamicTagRules.RuleID.Value, user, getChildren);
                    }
                }
                scope.Complete();
            }
            return DynamicTagRulesList;
        }

        public static int Save(ECN_Framework_Entities.Communicator.DynamicTagRule DynamicTagRule, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                DynamicTagRule.DynamicTagRuleID = ECN_Framework_DataLayer.Communicator.DynamicTagRule.Save(DynamicTagRule);
                scope.Complete();
            }
            return DynamicTagRule.DynamicTagRuleID;
        }
    }
}
