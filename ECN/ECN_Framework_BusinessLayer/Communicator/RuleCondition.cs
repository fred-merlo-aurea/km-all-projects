using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class RuleCondition
    {
        public static int Save(ECN_Framework_Entities.Communicator.RuleCondition RuleCondition, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                RuleCondition.RuleConditionID = ECN_Framework_DataLayer.Communicator.RuleCondition.Save(RuleCondition);
                scope.Complete();
            }
            return RuleCondition.RuleConditionID;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static List<ECN_Framework_Entities.Communicator.RuleCondition> GetByRuleID_NoAccessCheck(int RuleID)
        {
            List<ECN_Framework_Entities.Communicator.RuleCondition> RuleConditionsList = new List<ECN_Framework_Entities.Communicator.RuleCondition>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                RuleConditionsList = ECN_Framework_DataLayer.Communicator.RuleCondition.GetByRuleID(RuleID);
                scope.Complete();
            }
            return RuleConditionsList;
        }

        public static List<ECN_Framework_Entities.Communicator.RuleCondition> GetByRuleID_NoAccessCheck_UseAmbientTransaction(int RuleID)
        {
            List<ECN_Framework_Entities.Communicator.RuleCondition> RuleConditionsList = new List<ECN_Framework_Entities.Communicator.RuleCondition>();
            using (TransactionScope scope = new TransactionScope())
            {
                RuleConditionsList = ECN_Framework_DataLayer.Communicator.RuleCondition.GetByRuleID(RuleID);
                scope.Complete();
            }
            return RuleConditionsList;
        }

        public static List<ECN_Framework_Entities.Communicator.RuleCondition> GetByRuleID(int RuleID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.RuleCondition> RuleConditionsList = new List<ECN_Framework_Entities.Communicator.RuleCondition>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                RuleConditionsList = ECN_Framework_DataLayer.Communicator.RuleCondition.GetByRuleID(RuleID);
                scope.Complete();
            }
            return RuleConditionsList;
        }

        public static void DeleteByRuleID(int RuleID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.RuleCondition.DeleteByRuleID(RuleID, user.UserID);
                scope.Complete();
            }
        }

        public static void Delete(int RuleConditionsID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.RuleCondition.Delete(RuleConditionsID, user.UserID);
                scope.Complete();
            }
        }
    }
}
