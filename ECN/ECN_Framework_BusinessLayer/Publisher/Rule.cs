using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECN_Framework_Common.Objects;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Publisher
{
    [Serializable]
    public class Rule
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Rule;

        public static bool Exists(int ruleID, int customerID)
        {
            return ECN_Framework_DataLayer.Publisher.Rule.Exists(ruleID, customerID);
        }

        public static List<ECN_Framework_Entities.Publisher.Rule> GetByEditionID(int publicationID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Publisher.Rule> lRule = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                lRule = ECN_Framework_DataLayer.Publisher.Rule.GetByPublicationID(publicationID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && !SecurityCheck(lRule, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return lRule;
        }


        private static bool SecurityCheck(List<ECN_Framework_Entities.Publisher.Rule> lRule, KMPlatform.Entity.User user)
        {
            if (lRule != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var securityCheck = from r in lRule
                                        join c in custList on r.CustomerID equals c.CustomerID
                                        select new {r.RuleID };

                    if (securityCheck.Count() != lRule.Count)
                        return false;
                }
                else
                {
                    var securityCheck = from r in lRule
                                        where r.CustomerID != user.CustomerID
                                        select new { r.RuleID };

                    if (securityCheck.Any())
                        return false;
                }
            }
            return true;
        }

        public static void Delete(int ruleID, KMPlatform.Entity.User user)
        {
            ECN_Framework_DataLayer.Publisher.Rule.Delete(ruleID, user.UserID);
        }

        public static void Validate(ECN_Framework_Entities.Publisher.Rule rule)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (!ECN_Framework_BusinessLayer.Publisher.Edition.Exists(rule.EditionID, rule.CustomerID.Value))
                    errorList.Add(new ECNError(Entity, Method, "EditionID is invalid"));

                if (rule.RuleID <= 0 && (rule.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(rule.CreatedUserID.Value, rule.CustomerID.Value)))
                    errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

                if (rule.RuleID > 0 && (rule.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(rule.UpdatedUserID.Value, rule.CustomerID.Value)))
                    errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

                scope.Complete();
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Save(ECN_Framework_Entities.Publisher.Rule rule, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
            if (rule.RuleID > 0)
            {
                if (!Exists(rule.RuleID, rule.CustomerID.Value))
                {
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(new ECNError(Entity, Method, "RuleID is invalid"));
                    throw new ECNException(errorList);
                }
            }
            Validate(rule);

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Publisher.Rule.Save(rule);
                scope.Complete();
            }

            return;
        }
    }
}
