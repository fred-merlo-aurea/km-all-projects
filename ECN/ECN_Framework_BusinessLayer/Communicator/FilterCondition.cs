using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class FilterCondition
    {
        private const string EqualsComparator = "equals";
        private const string IsInComparator = "is in";
        private const string IsEmptyComparator = "is empty";
        private const string GreaterThanComparator = "greater than";
        private const string LessThanComparator = "less than";
        private const string TypeString = "String";
        private const string TypeDate = "Date";
        private const string TypeNumber = "Number";
        private const string TypeMoney = "Money";
        private const string EXP = "EXP:";
        private const string Full = "full";
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.GroupFilter;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.FilterCondition;

        public static bool Exists(int filterConditionID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.FilterCondition.Exists(filterConditionID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static ECN_Framework_Entities.Communicator.FilterCondition GetByFilterConditionID(int filterConditionID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.FilterCondition filterCondition = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                filterCondition = ECN_Framework_DataLayer.Communicator.FilterCondition.GetByFilterConditionID(filterConditionID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(filterCondition, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return filterCondition;
        }

        public static List<ECN_Framework_Entities.Communicator.FilterCondition> GetByFilterGroupID(int filterGroupID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.FilterCondition> filterConditionList = new List<ECN_Framework_Entities.Communicator.FilterCondition>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                filterConditionList = ECN_Framework_DataLayer.Communicator.FilterCondition.GetByFilterGroupID(filterGroupID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(filterConditionList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

                
            return filterConditionList;
        }

        public static List<ECN_Framework_Entities.Communicator.FilterCondition> GetByFilterGroupID_NoAccessCheck(int filterGroupID)
        {
            List<ECN_Framework_Entities.Communicator.FilterCondition> filterConditionList = new List<ECN_Framework_Entities.Communicator.FilterCondition>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                filterConditionList = ECN_Framework_DataLayer.Communicator.FilterCondition.GetByFilterGroupID(filterGroupID);
                scope.Complete();
            }
            return filterConditionList;
        }

        public static void Delete(int filterGroupID, int filterConditionID, KMPlatform.Entity.User user)
        {
            Delete(filterGroupID, filterConditionID, user, true);
        }

        internal static void Delete(int filterGroupID, int filterConditionID, KMPlatform.Entity.User user, bool updateWhere)
        {
            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            GetByFilterConditionID(filterConditionID, user);

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.FilterCondition.Delete(filterGroupID, filterConditionID, user.CustomerID, user.UserID);
                if (updateWhere)
                {
                    ECN_Framework_Entities.Communicator.FilterGroup filterGroup = ECN_Framework_BusinessLayer.Communicator.FilterGroup.GetByFilterGroupID(filterGroupID, user);
                    ECN_Framework_Entities.Communicator.Filter filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(filterGroup.FilterID, user);
                    ECN_Framework_BusinessLayer.Communicator.Filter.UpdateWhereClause(filterGroup.FilterID, ECN_Framework_BusinessLayer.Communicator.Filter.CreateWhereClause(filter), user);
                    ECN_Framework_BusinessLayer.Communicator.Filter.UpdateDynamicWhere(filterGroup.FilterID, ECN_Framework_BusinessLayer.Communicator.Filter.CreateDynamicWhere(filter), user);
                }
                ReSort(filterGroupID, user);
                scope.Complete();
            }  
        }

        public static void Delete(int filterGroupID, KMPlatform.Entity.User user)
        {
            Delete(filterGroupID, user, true);
        }

        internal static void Delete(int filterGroupID, KMPlatform.Entity.User user, bool updateWhere)
        {
            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            GetByFilterGroupID(filterGroupID, user);

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.FilterCondition.Delete(filterGroupID, user.CustomerID, user.UserID);
                if (updateWhere)
                {
                    ECN_Framework_Entities.Communicator.FilterGroup filterGroup = ECN_Framework_BusinessLayer.Communicator.FilterGroup.GetByFilterGroupID(filterGroupID, user);
                    ECN_Framework_Entities.Communicator.Filter filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(filterGroup.FilterID, user);
                    ECN_Framework_BusinessLayer.Communicator.Filter.UpdateWhereClause(filterGroup.FilterID, ECN_Framework_BusinessLayer.Communicator.Filter.CreateWhereClause(filter), user);
                    ECN_Framework_BusinessLayer.Communicator.Filter.UpdateDynamicWhere(filterGroup.FilterID, ECN_Framework_BusinessLayer.Communicator.Filter.CreateDynamicWhere(filter), user);
                }
                ReSort(filterGroupID, user);
                scope.Complete();
            }
        }

        private static void ReSort(int filterGroupID, KMPlatform.Entity.User user)
        {
           
                List<ECN_Framework_Entities.Communicator.FilterCondition> filterConditionList = ECN_Framework_BusinessLayer.Communicator.FilterCondition.GetByFilterGroupID(filterGroupID, user);

                if (filterConditionList != null)
                {
                    int i = 0;
                    using (TransactionScope scope = new TransactionScope())
                    {                
                        foreach (ECN_Framework_Entities.Communicator.FilterCondition filterCondition in filterConditionList)
                        {
                            ECN_Framework_DataLayer.Communicator.FilterCondition.UpdateSortOrder(filterCondition.FilterConditionID, ++i, user.UserID);
                        }
                        scope.Complete();
                    }
                }
        }

        public static int GetSortOrder(int filterGroupID)
        {
            int count = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                count = ECN_Framework_DataLayer.Communicator.FilterCondition.GetSortOrder(filterGroupID);
                scope.Complete();
            }
            return count;
        }

        public static int Save(ECN_Framework_Entities.Communicator.FilterCondition filterCondition, KMPlatform.Entity.User user)
        {
            return Save(filterCondition, user, true);
        }

        internal static int Save(ECN_Framework_Entities.Communicator.FilterCondition filterCondition, KMPlatform.Entity.User user, bool updateWhere)
        {
            Validate(filterCondition);

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(filterCondition, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                filterCondition.FilterConditionID = ECN_Framework_DataLayer.Communicator.FilterCondition.Save(filterCondition);
                if (updateWhere)
                {
                    ECN_Framework_Entities.Communicator.FilterGroup filterGroup = ECN_Framework_BusinessLayer.Communicator.FilterGroup.GetByFilterGroupID(filterCondition.FilterGroupID, user);
                    ECN_Framework_Entities.Communicator.Filter filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(filterGroup.FilterID, user);
                    ECN_Framework_BusinessLayer.Communicator.Filter.UpdateWhereClause(filterGroup.FilterID, ECN_Framework_BusinessLayer.Communicator.Filter.CreateWhereClause(filter), user);
                    ECN_Framework_BusinessLayer.Communicator.Filter.UpdateDynamicWhere(filterGroup.FilterID, ECN_Framework_BusinessLayer.Communicator.Filter.CreateDynamicWhere(filter), user);
                }
                ReSort(filterCondition.FilterGroupID, user);
                scope.Complete();
            }
            return filterCondition.FilterConditionID;
        }

        public static void Validate(ECN_Framework_Entities.Communicator.FilterCondition filterCondition)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (filterCondition.Field.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "Field cannot be empty"));
            if (filterCondition.SortOrder < 0)
                errorList.Add(new ECNError(Entity, Method, "SortOrder is invalid"));
            if (filterCondition.FieldType.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "FieldType cannot be empty"));
            else if (!CheckValues(filterCondition))
                errorList.Add(new ECNError(Entity, Method, "CompareValue is invalid"));
            if (filterCondition.Comparator.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "Comparator is invalid"));
            if (filterCondition.CustomerID == null)
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            else
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(filterCondition.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));

                    if (filterCondition.CreatedUserID == null || (filterCondition.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(filterCondition.CreatedUserID.Value, false))))
                    {
                        if (filterCondition.FilterConditionID <= 0 && (filterCondition.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(filterCondition.CreatedUserID.Value, filterCondition.CustomerID.Value)))
                            errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                    }
                    if (filterCondition.FilterConditionID > 0 && (filterCondition.UpdatedUserID == null || (filterCondition.UpdatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(filterCondition.UpdatedUserID.Value, false)))))
                    {
                        if (filterCondition.FilterConditionID > 0 && (filterCondition.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(filterCondition.UpdatedUserID.Value, filterCondition.CustomerID.Value)))
                            errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                    }
                    scope.Complete();
                }

                if (filterCondition.FilterGroupID <= 0)
                    errorList.Add(new ECNError(Entity, Method, "FilterGroupID is invalid"));
                else
                    if (!ECN_Framework_BusinessLayer.Communicator.FilterGroup.Exists(filterCondition.FilterGroupID, filterCondition.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "FilterGroupID is invalid"));
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        private static bool CheckValues(ECN_Framework_Entities.Communicator.FilterCondition filterCondition)
        {
            if (filterCondition.Comparator != IsEmptyComparator)
            {
                switch (filterCondition.FieldType)
                {
                    case TypeString:
                        return CheckStringValue(filterCondition);
                    case TypeDate:
                        return CheckDateValue(filterCondition);
                    case TypeNumber:
                        return CheckNumberValue(filterCondition);
                    case TypeMoney:
                        return CheckMoneyValue(filterCondition);
                }
            }

            return true;
        }

        private static bool CheckStringValue(ECN_Framework_Entities.Communicator.FilterCondition filterCondition)
        {
            if (string.IsNullOrWhiteSpace(filterCondition.CompareValue))
            {
                return false;
            }

            return true;
        }

        private static bool CheckDateValue(ECN_Framework_Entities.Communicator.FilterCondition filterCondition)
        {
            if (!filterCondition.CompareValue.Contains(EXP))
            {
                if (filterCondition.DatePart.Equals(Full))
                {
                    // Unneeded variable just used as the out of TryParse
                    DateTime dummyDate;
                    switch (filterCondition.Comparator)
                    {
                        case EqualsComparator:
                        case GreaterThanComparator:
                        case LessThanComparator:
                            return DateTime.TryParse(filterCondition.CompareValue, out dummyDate);
                        case IsInComparator:
                            foreach (var value in filterCondition.CompareValue.Split(','))
                            {
                                if (!DateTime.TryParse(value, out dummyDate))
                                {
                                    return false;
                                }
                            }
                            break;
                    }
                }
                else
                {
                    // Unneeded variable just used as the out of TryParse
                    int dummyNumber;
                    switch (filterCondition.Comparator)
                    {
                        case EqualsComparator:
                        case GreaterThanComparator:
                        case LessThanComparator:
                            return int.TryParse(filterCondition.CompareValue, out dummyNumber);
                    }
                }
            }

            return true;
        }

        private static bool CheckNumberValue(ECN_Framework_Entities.Communicator.FilterCondition filterCondition)
        {
            // Unneeded variable just used as the out of TryParse
            int dummyNumber;
            switch (filterCondition.Comparator)
            {
                case EqualsComparator:
                case GreaterThanComparator:
                case LessThanComparator:
                    return int.TryParse(filterCondition.CompareValue, out dummyNumber);
                case IsInComparator:
                    foreach (var value in filterCondition.CompareValue.Split(','))
                    {
                        if (!int.TryParse(value, out dummyNumber))
                        {
                            return false;
                        }
                    }
                    break;
            }

            return true;
        }

        private static bool CheckMoneyValue(ECN_Framework_Entities.Communicator.FilterCondition filterCondition)
        {
            // Unneeded variable just used as the out of TryParse
            decimal dummyDecimal;
            switch (filterCondition.Comparator)
            {
                case EqualsComparator:
                case GreaterThanComparator:
                case LessThanComparator:
                    return decimal.TryParse(filterCondition.CompareValue, out dummyDecimal);
                case IsInComparator:
                    foreach (var value in filterCondition.CompareValue.Split(','))
                    {
                        if (!decimal.TryParse(value, out dummyDecimal))
                        {
                            return false;
                        }
                    }
                    break;
            }

            return true;
        }
    }
}
