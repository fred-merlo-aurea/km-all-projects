using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator
{

    [Serializable]
    public class Rule
    {
        private const string DataTypeString = "String";
        private const string DataTypeNumber = "Number";
        private const string DataTypeDate = "Date";
        private const string FieldFormat = "CONVERT(VARCHAR({0}), ISNULL([{1}], ''))";
        private const string FieldIsNumericFormat = "(ISNUMERIC([{0}]) = 1 AND CONVERT(INT, [{0}])";
        private const string FieldIsDateFormat = "(ISDATE([{0}]) = 1 AND ";
        private const string ComparatorEquals = "Equals";
        private const string ComparatorNotEquals = "Not Equals";
        private const string ComparatorContains = "Contains";
        private const string ComparatorStartsWith = "Starts With";
        private const string ComparatorEndsWith = "Ends With";
        private const string ComparatorIsEmpty = "Is Empty";
        private const string ComparatorIsNotEmpty = "Is Not Empty";
        private const string ComparatorEmpty = "Empty";
        private const string ComparatorGreaterThan = "Greater than";
        private const string ComparatorLessThan = "Less than";
        private const string DateEqualsFormat = "{0}CONVERT(VARCHAR(10),CONVERT(DATETIME, [{1}]), 101) = CONVERT(VARCHAR(10), '{2}', 101))";
        private const string DateNotEqualsFormat = "{0}CONVERT(DATETIME, [{1}]) <> CONVERT(DATETIME, '{2}'))";
        private const string DateGreaterThanFormat = "{0}CONVERT(DATETIME, [{1}]) > CONVERT(DATETIME, '{2}'))";
        private const string DateLessThanFormat = "{0}CONVERT(DATETIME, [{1}]) < CONVERT(DATETIME, '{2}'))";

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Rule;
        public static void Validate(ECN_Framework_Entities.Communicator.Rule Rule)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();
            if (Rule.RuleName.Trim().Equals(string.Empty))
                errorList.Add(new ECNError(Entity, Method, "Rule Name cannot be empty"));
            else if (Rule.RuleName.Trim().Length > 50)
                errorList.Add(new ECNError(Entity, Method, "Rule Name cannot be over 50 chars."));
            if (Exists(Rule.RuleName, Rule.CustomerID.Value, Rule.RuleID))
                errorList.Add(new ECNError(Entity, Method, "Rule Name is already in use"));
            if (Rule.RuleConditionsList.Count<1)
                errorList.Add(new ECNError(Entity, Method, "A Rule must have at least one condition"));
            if (!ECN_Framework_Common.Functions.RegexUtilities.IsValidObjectName(Rule.RuleName))
                errorList.Add(new ECNError(Entity, Method, "Rule Name has invalid characters"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static int Save(ECN_Framework_Entities.Communicator.Rule Rule, KMPlatform.Entity.User user)
        {
            Validate(Rule);
            using (TransactionScope scope = new TransactionScope())
            {
                Rule.RuleID = ECN_Framework_DataLayer.Communicator.Rule.Save(Rule);
                if (Rule.RuleConditionsList != null)
                {
                    foreach (ECN_Framework_Entities.Communicator.RuleCondition RuleCondition in Rule.RuleConditionsList)
                    {
                        RuleCondition.RuleID = Rule.RuleID;
                        ECN_Framework_BusinessLayer.Communicator.RuleCondition.Save(RuleCondition, user);
                        if (RuleCondition.IsDeleted.Value)
                            ECN_Framework_BusinessLayer.Communicator.RuleCondition.Delete(RuleCondition.RuleConditionID, user);
                    }
                }
                Rule.WhereClause = GenerateWhereClause(Rule, user);
                ECN_Framework_DataLayer.Communicator.Rule.Save(Rule);
                scope.Complete();
            }
            return Rule.RuleID;
        }

        public static bool IsUsedInDynamicTag(int RuleID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Rule.IsUsedInDynamicTag(RuleID);
                scope.Complete();
            }
            return exists;
        }

        private static string GenerateWhereClause(ECN_Framework_Entities.Communicator.Rule Rule, User user)
        {
            var whereClause = new StringBuilder();
            var conditionConnector = $" {Rule.ConditionConnector} ";
            if (Rule.RuleID > 0)
            {
                Rule = GetByRuleID(Rule.RuleID, user, true);
            }

            var ruleConditionList = Rule.RuleConditionsList
                .Where(src => src.IsDeleted == false)
                .ToList();

            if (ruleConditionList.Count > 0)
            {
                foreach (var ruleCondition in ruleConditionList)
                {
                    if (ruleCondition.DataType.Equals(DataTypeString))
                    {
                        GenerateWhereConditionForString(whereClause, ruleCondition);
                    }
                    else if (ruleCondition.DataType.Equals(DataTypeNumber))
                    {
                        GenerateWhereConditionForNumber(whereClause, ruleCondition);
                    }
                    else if (ruleCondition.DataType.Equals(DataTypeDate))
                    {
                        GenerateWhereConditionForDate(whereClause, ruleCondition);
                    }

                    whereClause.Append(conditionConnector);
                }

                whereClause = whereClause.Remove(whereClause.Length - conditionConnector.Length, conditionConnector.Length);
            }

            return whereClause.ToString();
        }

        private static void GenerateWhereConditionForString(StringBuilder whereClause, ECN_Framework_Entities.Communicator.RuleCondition ruleCondition)
        {
            var field = string.Format(FieldFormat, 1000, ruleCondition.Field);
            if (ruleCondition.Comparator.Equals(ComparatorEquals))
            {
                whereClause.Append(field + " = '" + ruleCondition.Value + "'");
            }
            else if (ruleCondition.Comparator.Equals(ComparatorNotEquals))
            {
                whereClause.Append(field + " <> '" + ruleCondition.Value + "'");
            }
            else if (ruleCondition.Comparator.Equals(ComparatorContains))
            {
                whereClause.Append(field + " like '%" + ruleCondition.Value + "%'");
            }
            else if (ruleCondition.Comparator.Equals(ComparatorStartsWith))
            {
                whereClause.Append(field + " like '" + ruleCondition.Value + "%'");
            }
            else if (ruleCondition.Comparator.Equals(ComparatorEndsWith))
            {
                whereClause.Append(field + " like '%" + ruleCondition.Value + "'");
            }

            else if (ruleCondition.Comparator.Equals(ComparatorIsEmpty))
            {
                whereClause.Append(field + " = '' ");
            }
            else if (ruleCondition.Comparator.Equals(ComparatorIsNotEmpty))
            {
                whereClause.Append(field + " <> '' ");
            }
        }

        private static void GenerateWhereConditionForNumber(StringBuilder whereClause, ECN_Framework_Entities.Communicator.RuleCondition ruleCondition)
        {
            var field = string.Empty;
            if (!ruleCondition.Comparator.Contains(ComparatorEmpty))
            {
                field = string.Format(FieldIsNumericFormat, ruleCondition.Field);
            }
            else
            {
                field = string.Format(FieldFormat, 30, ruleCondition.Field);
            }

            if (ruleCondition.Comparator.Equals(ComparatorIsEmpty))
            {
                whereClause.Append(field + " = '' ");
            }

            if (ruleCondition.Comparator.Equals(ComparatorIsNotEmpty))
            {
                whereClause.Append(field + " <> '' ");
            }

            if (ruleCondition.Comparator.Equals(ComparatorEquals))
            {
                whereClause.Append(field + " = " + ruleCondition.Value + ")");
            }
            else if (ruleCondition.Comparator.Equals(ComparatorNotEquals))
            {
                whereClause.Append(field + " <> " + ruleCondition.Value + ")");
            }
            else if (ruleCondition.Comparator.Equals(ComparatorGreaterThan))
            {
                whereClause.Append(field + " > " + ruleCondition.Value + ")");
            }
            else if (ruleCondition.Comparator.Equals(ComparatorLessThan))
            {
                whereClause.Append(field + " < " + ruleCondition.Value + ")");
            }
        }

        private static void GenerateWhereConditionForDate(StringBuilder whereClause, ECN_Framework_Entities.Communicator.RuleCondition ruleCondition)
        {
            var field = string.Empty;
            if (!ruleCondition.Comparator.Contains(ComparatorEmpty))
            {
                field = string.Format(FieldIsDateFormat, ruleCondition.Field);
            }
            else
            {
                field = string.Format(FieldFormat, 30, ruleCondition.Field);
            }

            if (ruleCondition.Comparator.Equals(ComparatorIsEmpty))
            {
                whereClause.Append(field + " = '' ");
            }

            if (ruleCondition.Comparator.Equals(ComparatorIsNotEmpty))
            {
                whereClause.Append(field + " <> '' ");
            }

            if (ruleCondition.Comparator.Equals(ComparatorEquals))
            {
                whereClause.AppendFormat(DateEqualsFormat, field, ruleCondition.Field, ruleCondition.Value);
            }
            else if (ruleCondition.Comparator.Equals(ComparatorNotEquals))
            {
                whereClause.AppendFormat(DateNotEqualsFormat, field, ruleCondition.Field, ruleCondition.Value);
            }
            else if (ruleCondition.Comparator.Equals(ComparatorGreaterThan))
            {
                whereClause.AppendFormat(DateGreaterThanFormat, field, ruleCondition.Field, ruleCondition.Value);
            }
            else if (ruleCondition.Comparator.Equals(ComparatorLessThan))
            {
                whereClause.AppendFormat(DateLessThanFormat, field, ruleCondition.Field, ruleCondition.Value);
            }
        }

        public static void Delete(int RuleID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (IsUsedInDynamicTag(RuleID))
            {            
                errorList.Add(new ECNError(Entity, Method, "This Rule is being used in a Dynamic Tag"));
                throw new ECNException(errorList);
            }

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_BusinessLayer.Communicator.RuleCondition.DeleteByRuleID(RuleID, user);
                ECN_Framework_DataLayer.Communicator.Rule.Delete(RuleID, user.UserID);
                scope.Complete();
            }
        }

        public static List<ECN_Framework_Entities.Communicator.Rule> GetByCustomerID(int customerID, KMPlatform.Entity.User user, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.Rule> RuleList = new List<ECN_Framework_Entities.Communicator.Rule>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                RuleList = ECN_Framework_DataLayer.Communicator.Rule.GetByCustomerID(customerID);
                if (RuleList != null && RuleList.Count > 0 && getChildren)
                {
                    foreach (ECN_Framework_Entities.Communicator.Rule Rule in RuleList)
                    {
                        Rule.RuleConditionsList = ECN_Framework_BusinessLayer.Communicator.RuleCondition.GetByRuleID(Rule.RuleID, user);
                    }
                }
                scope.Complete();
            }
            return RuleList;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static ECN_Framework_Entities.Communicator.Rule GetByRuleID_NoAccessCheck(int RuleID, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.Rule Rule = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                Rule = ECN_Framework_DataLayer.Communicator.Rule.GetByRuleID(RuleID);
                if (Rule != null && getChildren)
                {
                    Rule.RuleConditionsList = ECN_Framework_BusinessLayer.Communicator.RuleCondition.GetByRuleID_NoAccessCheck(RuleID);
                }
                scope.Complete();
            }
            return Rule;
        }

        public static ECN_Framework_Entities.Communicator.Rule GetByRuleID_NoAccessCheck_UseAmbientTransaction(int RuleID, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.Rule Rule = null;
            using (TransactionScope scope = new TransactionScope())
            {
                Rule = ECN_Framework_DataLayer.Communicator.Rule.GetByRuleID(RuleID);
                if (Rule != null && getChildren)
                {
                    Rule.RuleConditionsList = ECN_Framework_BusinessLayer.Communicator.RuleCondition.GetByRuleID_NoAccessCheck_UseAmbientTransaction(RuleID);
                }
                scope.Complete();
            }
            return Rule;
        }

        public static ECN_Framework_Entities.Communicator.Rule GetByRuleID(int RuleID, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.Rule Rule = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                Rule = ECN_Framework_DataLayer.Communicator.Rule.GetByRuleID(RuleID);
                if (Rule != null && getChildren)
                {
                    Rule.RuleConditionsList = ECN_Framework_BusinessLayer.Communicator.RuleCondition.GetByRuleID(RuleID, user);
                }
                scope.Complete();
            }
            return Rule;
        }

        public static bool Exists(string RuleName, int CustomerID, int RuleID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Rule.Exists(RuleName, CustomerID, RuleID);
                scope.Complete();
            }
            return exists;
        }

        public static bool IsApplicable(int RuleID, int EmailID, int GroupID)
        {
            bool IsApplicable = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                IsApplicable = ECN_Framework_DataLayer.Communicator.Rule.IsApplicable(RuleID, EmailID, GroupID);
                scope.Complete();
            }
            return IsApplicable;
        }
    }
}
