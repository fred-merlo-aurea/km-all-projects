using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;
using Entities = ECN_Framework_Entities.Communicator;


namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class Filter : FilterBase
    {
        private const string ContainsComparator = "contains";
        private const string EndsWithComparator = "ends with";
        private const string StartsWithComparator = "starts with";
        private const string IsEmptyComparator = "is empty";
        private const string TrimFieldFormat = "rtrim(ltrim(ISNULL([{0}], ''))) ";
        private const string CommaString = ",";
        private const string StringFieldType = "String";
        private const string DateFieldType = "Date";
        private const string NumberFieldType = "Number";
        private const string MoneyFieldType = "Money";
        private const string SingleQuote = "'";
        private const string ExpToday = "EXP:Today";
        
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.GroupFilter;

        public static Enums.Entity Entity = Enums.Entity.Filter;

        public static bool Exists(int filterID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Filter.Exists(filterID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int filterID, string filterName, int groupID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Filter.Exists(filterID, filterName, groupID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static void ArchiveFilter(int filterID, bool Archived, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.Filter.ArchiveFilter(filterID, Archived, user);
                scope.Complete();
            }
        }


        public static ECN_Framework_Entities.Communicator.Filter GetByFilterID(int filterID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.Filter filter = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                filter = ECN_Framework_DataLayer.Communicator.Filter.GetByFilterID(filterID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(filter, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (filter != null)
                filter.FilterGroupList = FilterGroup.GetByFilterID(filter.FilterID, user);

            return filter;
        }

        public static ECN_Framework_Entities.Communicator.Filter GetByFilterID_NoAccessCheck(int filterID)
        {
            ECN_Framework_Entities.Communicator.Filter filter = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                filter = ECN_Framework_DataLayer.Communicator.Filter.GetByFilterID(filterID);
                scope.Complete();
            }

            if (filter != null)
                filter.FilterGroupList = FilterGroup.GetByFilterID_NoAccessCheck(filter.FilterID);

            return filter;
        }

        public static List<ECN_Framework_Entities.Communicator.Filter> GetByFilterSearch(KMPlatform.Entity.User user, int? groupID, int CustomerID, bool? archived = null)
        {
            List<ECN_Framework_Entities.Communicator.Filter> filterList = new List<ECN_Framework_Entities.Communicator.Filter>();
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new SecurityException();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                filterList = ECN_Framework_DataLayer.Communicator.Filter.GetByFilterSearch(groupID, CustomerID, archived);
                scope.Complete();
            }
            return filterList;
        }

        public static List<ECN_Framework_Entities.Communicator.Filter> GetByGroupID(int groupID, bool validWhereOnly, KMPlatform.Entity.User user, string archiveFilter = "all")
        {
            List<ECN_Framework_Entities.Communicator.Filter> filterList = new List<ECN_Framework_Entities.Communicator.Filter>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                filterList = ECN_Framework_DataLayer.Communicator.Filter.GetByGroupID(groupID, validWhereOnly, archiveFilter);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(filterList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            foreach (ECN_Framework_Entities.Communicator.Filter filter in filterList)
            {
                filter.FilterGroupList = FilterGroup.GetByFilterID(filter.FilterID, user);
            }

            return filterList;
        }

        public static List<ECN_Framework_Entities.Communicator.Filter> GetByGroupID_NoAccessCheck(int groupID, bool validWhereOnly, string archiveFilter = "all")
        {
            List<ECN_Framework_Entities.Communicator.Filter> filterList = new List<ECN_Framework_Entities.Communicator.Filter>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                filterList = ECN_Framework_DataLayer.Communicator.Filter.GetByGroupID(groupID, validWhereOnly, archiveFilter);
                scope.Complete();
            }


            foreach (ECN_Framework_Entities.Communicator.Filter filter in filterList)
            {
                filter.FilterGroupList = FilterGroup.GetByFilterID_NoAccessCheck(filter.FilterID);
            }

            return filterList;
        }

        //private static bool SecurityCheck(List<ECN_Framework_Entities.Communicator.Filter> filterList, KMPlatform.Entity.User user)
        //{
        //    if (filterList != null)
        //    {
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var securityCheck = from f in filterList
        //                                join c in custList on f.CustomerID equals c.CustomerID
        //                                select new { f.FilterID };

        //            if (securityCheck.Count() != filterList.Count)
        //                return false;
        //        }
        //        else
        //        {
        //            var securityCheck = from f in filterList
        //                                where f.CustomerID != user.CustomerID
        //                                select new { f.FilterID };

        //            if (securityCheck.Any())
        //                return false;
        //        }
        //    }
        //    return true;
        //}

        //private static bool SecurityCheck(ECN_Framework_Entities.Communicator.Filter filter, KMPlatform.Entity.User user)
        //{
        //    if (filter != null)
        //    {
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

        //            if (!custExists.Any())
        //                return false;
        //        }
        //        else if (filter.CustomerID.Value != user.CustomerID)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        public static void Delete(int filterID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(filterID, user.CustomerID))
            {
                if (ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.SelectByFilterIDCanDelete(filterID))
                {
                    //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
                    GetByFilterID(filterID, user);

                    if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                        throw new ECN_Framework_Common.Objects.SecurityException();

                    using (TransactionScope scope = new TransactionScope())
                    {
                        ECN_Framework_BusinessLayer.Communicator.FilterGroup.Delete(filterID, user, false);
                        ECN_Framework_DataLayer.Communicator.Filter.Delete(filterID, user.CustomerID, user.UserID);
                        scope.Complete();
                    }
                }
                else
                {
                    errorList.Add(new ECNError(Entity, Method, "Filter is used in blast(s). Deleting is not allowed."));
                    throw new ECNException(errorList);
                }
            }
            else
            {
                errorList.Add(new ECNError(Entity, Method, "Filter does not exist"));
                throw new ECNException(errorList);
            }

        }

        internal static void UpdateWhereClause(int filterID, string whereClause, KMPlatform.Entity.User user)
        {
            List<ECNError> errorList = new List<ECNError>();

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.Filter.UpdateWhereClause(filterID, whereClause.Trim(), user.CustomerID, user.UserID);
                scope.Complete();
            }

        }

        internal static void UpdateDynamicWhere(int filterID, string dynamicWhere, KMPlatform.Entity.User user)
        {
            List<ECNError> errorList = new List<ECNError>();

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.Filter.UpdateDynamicWhere(filterID, dynamicWhere, user.CustomerID, user.UserID);
                scope.Complete();
            }

        }

        public static int Save(ECN_Framework_Entities.Communicator.Filter filter, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
            if (filter.FilterID > 0)
            {
                if (!Exists(filter.FilterID, filter.CustomerID.Value))
                {
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(new ECNError(Entity, Method, "FilterID is invalid"));
                    throw new ECNException(errorList);
                }
            }
            Validate(filter);

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(filter, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                filter.FilterID = ECN_Framework_DataLayer.Communicator.Filter.Save(filter);
                if (filter.FilterGroupList != null)
                {
                    foreach (ECN_Framework_Entities.Communicator.FilterGroup filterGroup in filter.FilterGroupList)
                    {
                        filterGroup.FilterID = filter.FilterID;
                        FilterGroup.Save(filterGroup, user, false);
                    }
                }
                UpdateWhereClause(filter.FilterID, ECN_Framework_BusinessLayer.Communicator.Filter.CreateWhereClause(filter), user);
                UpdateDynamicWhere(filter.FilterID, ECN_Framework_BusinessLayer.Communicator.Filter.CreateDynamicWhere(filter), user);
                scope.Complete();
            }
            return filter.FilterID;
        }

        internal static string CreateWhereClause(ECN_Framework_Entities.Communicator.Filter filter)
        {
            var whereClause = new StringBuilder();
            
            if (filter.FilterGroupList != null)
            {
                var firstGroup = true;
                foreach (var group in filter.FilterGroupList)
                {
                    if (!firstGroup && group.FilterConditionList != null && group.FilterConditionList.Any())
                    {
                        whereClause.Append($" {filter.GroupCompareType} ");
                    }

                    var firstCondition = true;
                    if (group.FilterConditionList != null && group.FilterConditionList.Any())
                    {
                        whereClause.Append("(");
                        
                        foreach (var condition in group.FilterConditionList)
                        {
                            if (!firstCondition)
                            {
                                whereClause.Append($" {group.ConditionCompareType} ");
                            }

                            AppendCondition(whereClause, condition);

                            firstCondition = false;
                        }

                        whereClause.Append(") ");
                    }

                    firstGroup = false;
                }
            }

            return whereClause.ToString().Trim();
        }

        private static void AppendCondition(StringBuilder whereClause, Entities.FilterCondition condition)
        {
            if (whereClause == null)
            {
                throw new ArgumentNullException(nameof(whereClause));
            }
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            whereClause.Append("(");

            // skip all of the regular field manipulation if they are looking for null or empty string
            if (condition.Comparator == IsEmptyComparator)
            {
                whereClause.Append(TrimField(condition));

                if (condition.NotComparator == 1)
                {
                    whereClause.Append("<> ''");
                }
                else
                {
                    whereClause.Append("= ''");
                }
            }
            else
            {
                whereClause.AppendFormat("{0} ", ConvertField(condition));
                AppendComparatorAndValue(whereClause, condition);
            }

            whereClause.Append(") ");
        }

        private static void AppendComparatorAndValue(StringBuilder whereClause, Entities.FilterCondition condition)
        {
            if (whereClause == null)
            {
                throw new ArgumentNullException(nameof(whereClause));
            }
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            switch (condition.Comparator)
            {
                case StartsWithComparator:
                case EndsWithComparator:
                case ContainsComparator:
                    if (condition.NotComparator == 1)
                    {
                        whereClause.Append("NOT ");
                    }
                    whereClause.Append("LIKE ");
                    whereClause.Append(ConvertValue(condition));
                    break;
                case IsInComparator:
                    if (condition.NotComparator == 1)
                    {
                        whereClause.Append("NOT ");
                    }
                    whereClause.Append("IN (");
                    whereClause.Append(ConvertValue(condition));
                    whereClause.Append(")");
                    break;
                case EqualsComparator:
                    if (condition.NotComparator == 1)
                    {
                        whereClause.Append("!");
                    }
                    whereClause.Append("= ");
                    whereClause.Append(ConvertValue(condition));
                    break;
                case GreaterThanComparator:
                    if (condition.NotComparator == 1)
                    {
                        whereClause.Append("<= ");
                    }
                    else
                    {
                        whereClause.Append("> ");
                    }
                    whereClause.Append(ConvertValue(condition));
                    break;
                case LessThanComparator:
                    if (condition.NotComparator == 1)
                    {
                        whereClause.Append(">= ");
                    }
                    else
                    {
                        whereClause.Append("< ");
                    }
                    whereClause.Append(ConvertValue(condition));
                    break;
                default:
                    break;
            }
        }

        private static string TrimField(Entities.FilterCondition condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            return string.Format(TrimFieldFormat, condition.Field);
        }

        internal static string CreateDynamicWhere(ECN_Framework_Entities.Communicator.Filter filter)
        {
            string where = "";
            //needs to be written

            return where;
        }

        public static void Validate(ECN_Framework_Entities.Communicator.Filter filter)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (filter.FilterName.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "FilterName cannot be empty"));
            else if (!ECN_Framework_Common.Functions.RegexUtilities.IsValidObjectName(filter.FilterName))
                errorList.Add(new ECNError(Entity, Method, "FilterName has invalid characters"));
            //if (filter.WhereClause.Trim() == string.Empty)
            //    errorList.Add(new ECNError(Entity, Method, "WhereClause cannot be empty"));
            //if (filter.DynamicWhere.Trim() == string.Empty)
            //    errorList.Add(new ECNError(Entity, Method, "DynamicWhere cannot be empty"));


            if (filter.GroupCompareType.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "GroupCompareType cannot be empty"));

            if (filter.CustomerID == null)
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            else
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(filter.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));

                    if (filter.FilterID <= 0 && (filter.CreatedUserID == null || (filter.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(filter.CreatedUserID.Value, false)))))
                    {
                        if (filter.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(filter.CreatedUserID.Value, filter.CustomerID.Value))
                            errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                    }
                    if (filter.FilterID > 0 && (filter.UpdatedUserID == null || (filter.UpdatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(filter.UpdatedUserID.Value, false)))))
                    {
                        if (filter.FilterID > 0 && (filter.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(filter.UpdatedUserID.Value, filter.CustomerID.Value)))
                            errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                    }
                    scope.Complete();
                }

                if (filter.GroupID == null)
                    errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));
                else
                {
                    if (!ECN_Framework_BusinessLayer.Communicator.Group.Exists(filter.GroupID.Value, filter.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));
                    else
                        if (Exists(filter.FilterID, filter.FilterName, filter.GroupID.Value, filter.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "FilterName already exists for this Group"));
                }
            }


            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        private static string ConvertField(ECN_Framework_Entities.Communicator.FilterCondition condition)
        {
            var newField = string.Empty;

            if (CommonFiltersFields.Any(x => string.CompareOrdinal(x, condition.Field) == 0))
            {
                return string.Format("ISNULL({0}, '')", condition.Field);
            }
            else if (NonCommonFiltersFields.Any(x => string.CompareOrdinal(x, condition.Field) == 0))
            {
                var nonEqualAndFullDateField = string.Format("ISDATE({0}) = 1 AND {1}", condition.Field, condition.Field);
                var nonEqualAndNonFullDateField = string.Format("ISDATE({0}) = 1 AND {1}({0})", condition.Field, condition.DatePart);

                var equalAndFullDateField = string.Format("ISDATE({0}) = 1 AND CONVERT(VARCHAR(10), {0}, 101)", condition.Field);
                var equalAndNonFullDateField = string.Format("ISDATE({0}) = 1 AND {1}({0})", condition.Field, condition.DatePart);

                return newField = GetDateField(condition.DatePart, condition.Comparator, nonEqualAndFullDateField, nonEqualAndNonFullDateField, equalAndFullDateField, equalAndNonFullDateField);
            }
            else
            {
                switch (condition.FieldType)
                {
                    case "String":
                        return newField = "CONVERT(VARCHAR(500), ISNULL([" + condition.Field + "], ''))";
                    case "Date":
                        var nonEqualAndFullDateField = string.Format("CASE WHEN ISDATE([{0}]) = 1 THEN CONVERT(DATETIME, [{0}]) end", condition.Field);
                        var nonEqualAndNonFullDateField = string.Format("CASE WHEN ISDATE([{0}]) = 1 THEN {1}(CONVERT(DATETIME, [{0}])) end", condition.Field, condition.DatePart);

                        var equalAndFullDateField = string.Format("CASE WHEN ISDATE([{0}]) = 1 then CONVERT(VARCHAR(10),CONVERT(DATETIME, [{0}]), 101) end", condition.Field);
                        var equalAndNonFullDateField = string.Format("CASE WHEN ISDATE([{0}]) = 1 then {1}({0}) end", condition.Field, condition.DatePart);

                        return newField = GetDateField(condition.DatePart, condition.Comparator, nonEqualAndFullDateField, nonEqualAndNonFullDateField, equalAndFullDateField, equalAndNonFullDateField);
                    case "Number":
                        return newField = "ISNUMERIC([" + condition.Field + "]) = 1 AND CONVERT(INT, [" + condition.Field + "])";
                    case "Money":
                        return newField = "ISNUMERIC([" + condition.Field + "]) = 1 AND CONVERT(MONEY, [" + condition.Field + "])";
                    default:
                        return newField = condition.Field;
                }
            }
        }

        private static string ConvertValue(Entities.FilterCondition condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            var newValue = new StringBuilder();

            if (CommonFiltersFields.Contains(condition.Field))
            {
                var equalsCompareValue = $"'{ condition.CompareValue }'";
                var containsCompareValue = $"'%{ condition.CompareValue }%'";
                var startsWithCompareValue = $"'{ condition.CompareValue }%'";
                var endsWithCompareValue = $"'%{ condition.CompareValue }'";
                return GetDateFieldBasedOnComparatorCondition(
                            condition.Comparator,
                            equalsCompareValue,
                            GetIsInCompareValue(condition.CompareValue, "'{0}'"),
                            containsCompareValue,
                            startsWithCompareValue,
                            endsWithCompareValue);
            }
            else
            {
                switch (condition.FieldType)
                {
                    case StringFieldType:
                        var equalsCompareValue = $"CONVERT(VARCHAR(500), '{ condition.CompareValue }')";
                        var containsCompareValue = $"CONVERT(VARCHAR(500), '%{ condition.CompareValue }%')";
                        var startsWithCompareValue = $"CONVERT(VARCHAR(500), '{ condition.CompareValue }%')";
                        var endsWithCompareValue = $"CONVERT(VARCHAR(500), '%{ condition.CompareValue }')";
                        return GetDateFieldBasedOnComparatorCondition(
                                    condition.Comparator,
                                    equalsCompareValue,
                                    GetIsInCompareValue(condition.CompareValue, "CONVERT(VARCHAR(500), '{0}')"),
                                    containsCompareValue,
                                    startsWithCompareValue,
                                    endsWithCompareValue);
                    case DateFieldType:
                        return GetDateConvertValue(condition);
                    case NumberFieldType:
                        return GetNumberAndMoneyConvertValue(
                                            condition.Comparator, 
                                            condition.CompareValue, 
                                            "CONVERT(INT, {0})");
                    case MoneyFieldType:
                        return GetNumberAndMoneyConvertValue(
                                            condition.Comparator,
                                            condition.CompareValue,
                                            "CONVERT(MONEY, {0})");
                }
            }

            return newValue.ToString();
        }

        private static string GetDateConvertValue(Entities.FilterCondition condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (string.CompareOrdinal(condition.Comparator, GreaterThanComparator) == 0
                || string.CompareOrdinal(condition.Comparator, LessThanComparator) == 0)
            {
                var fullDateField = string.Empty;
                var nonFullDateField = string.Empty;

                if (condition.CompareValue.Contains($"{ExpToday}["))
                {
                    var days = GetDays(condition.CompareValue);
                    fullDateField = string.Format("DATEADD(dd, {0}, GETDATE())", days);
                    nonFullDateField = string.Format("{0}(DATEADD(dd, {1}, GETDATE()))", condition.DatePart, days);
                }
                else if (condition.CompareValue.Contains(ExpToday))
                {
                    fullDateField = "GETDATE()";
                    nonFullDateField = string.Format("{0}(GETDATE())", condition.DatePart);
                }
                else
                {
                    fullDateField = string.Format("CONVERT(DATETIME, '{0}')", condition.CompareValue);
                    nonFullDateField = condition.CompareValue;
                }

                return GetDateFieldBasedOnDatePart(condition.DatePart, fullDateField, nonFullDateField);
            }
            else if (string.CompareOrdinal(condition.Comparator, EqualsComparator) == 0)
            {
                var fullDateField = string.Empty;
                var nonFullDateField = string.Empty;

                if (condition.CompareValue.Contains($"{ExpToday}["))
                {
                    var days = GetDays(condition.CompareValue);
                    fullDateField = string.Format("CONVERT(VARCHAR(10),DATEADD(dd, {0}, GETDATE()),101)", days);
                    nonFullDateField = string.Format("CONVERT(VARCHAR,{0}(DATEADD(dd, {1}, GETDATE())))", condition.DatePart, days);
                }
                else if (condition.CompareValue.Contains(ExpToday))
                {
                    fullDateField = "CONVERT(VARCHAR(10),GETDATE(),101)";
                    nonFullDateField = string.Format("CONVERT(VARCHAR,{0}(GETDATE()))", condition.DatePart);
                }
                else
                {
                    fullDateField = string.Format("CONVERT(VARCHAR(10),CONVERT(DATETIME, '{0}'),101)", condition.CompareValue);
                    nonFullDateField = condition.CompareValue;
                }

                return GetDateFieldBasedOnDatePart(condition.DatePart, fullDateField, nonFullDateField);
            }

            return string.Empty;
        }

        private static string GetDateFieldBasedOnComparatorCondition(string comparator, string equalsCompareValue, string isInCompareValue, string containsCompareValue, string startsWithCompareValue, string endsWithCompareValue)
        {
            switch (comparator)
            {
                case EqualsComparator:
                    return equalsCompareValue;
                case IsInComparator:
                    return isInCompareValue;
                case ContainsComparator:
                    return containsCompareValue;
                case StartsWithComparator:
                    return startsWithCompareValue;
                case EndsWithComparator:
                    return endsWithCompareValue;
                default:
                    return string.Empty;
            }
        }
    }
}
