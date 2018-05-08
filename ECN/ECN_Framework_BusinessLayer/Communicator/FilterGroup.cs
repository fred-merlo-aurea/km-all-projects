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
    public class FilterGroup
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.GroupFilter;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.FilterGroup;

        public static bool Exists(int filterGroupID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.FilterGroup.Exists(filterGroupID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int filterGroupID, string name, int filterID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.FilterGroup.Exists(filterGroupID, name, filterID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static ECN_Framework_Entities.Communicator.FilterGroup GetByFilterGroupID(int filterGroupID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.FilterGroup filterGroup = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                filterGroup = ECN_Framework_DataLayer.Communicator.FilterGroup.GetByFilterGroupID(filterGroupID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(filterGroup, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            filterGroup.FilterConditionList = FilterCondition.GetByFilterGroupID(filterGroup.FilterGroupID, user);
                
            return filterGroup;
        }

        public static List<ECN_Framework_Entities.Communicator.FilterGroup> GetByFilterID(int filterID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.FilterGroup> filterGroupList = new List<ECN_Framework_Entities.Communicator.FilterGroup>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                filterGroupList = ECN_Framework_DataLayer.Communicator.FilterGroup.GetByFilterID(filterID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(filterGroupList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            foreach (ECN_Framework_Entities.Communicator.FilterGroup filterGroup in filterGroupList)
            {
                filterGroup.FilterConditionList = FilterCondition.GetByFilterGroupID(filterGroup.FilterGroupID, user);
            }
                
            return filterGroupList;
        }

        public static List<ECN_Framework_Entities.Communicator.FilterGroup> GetByFilterID_NoAccessCheck(int filterID)
        {
            List<ECN_Framework_Entities.Communicator.FilterGroup> filterGroupList = new List<ECN_Framework_Entities.Communicator.FilterGroup>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                filterGroupList = ECN_Framework_DataLayer.Communicator.FilterGroup.GetByFilterID(filterID);
                scope.Complete();
            }

            foreach (ECN_Framework_Entities.Communicator.FilterGroup filterGroup in filterGroupList)
            {
                filterGroup.FilterConditionList = FilterCondition.GetByFilterGroupID_NoAccessCheck(filterGroup.FilterGroupID);
            }

            return filterGroupList;
        }

        //private static bool SecurityCheck(List<ECN_Framework_Entities.Communicator.FilterGroup> filterGroupList, KMPlatform.Entity.User user)
        //{
        //    if (filterGroupList != null)
        //    {
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var securityCheck = from f in filterGroupList
        //                                join c in custList on f.CustomerID equals c.CustomerID
        //                                select new { f.FilterGroupID };

        //            if (securityCheck.Count() != filterGroupList.Count)
        //                return false;
        //        }
        //        else
        //        {
        //            var securityCheck = from f in filterGroupList
        //                                where f.CustomerID != user.CustomerID
        //                                select new { f.FilterGroupID };

        //            if (securityCheck.Any())
        //                return false;
        //        }
        //    }
        //    return true;
        //}

        //private static bool SecurityCheck(ECN_Framework_Entities.Communicator.FilterGroup filterGroup, KMPlatform.Entity.User user)
        //{
        //    if (filterGroup != null)
        //    {
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

        //            if (!custExists.Any())
        //                return false;
        //        }
        //        else if (filterGroup.CustomerID.Value != user.CustomerID)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        public static void Delete(int filterID, KMPlatform.Entity.User user)
        {
            Delete(filterID, user, true);
        }

        internal static void Delete(int filterID, KMPlatform.Entity.User user, bool updateWhere)
        {
            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            GetByFilterID(filterID, user);

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                List<ECN_Framework_Entities.Communicator.FilterGroup> filterGroupList = ECN_Framework_BusinessLayer.Communicator.FilterGroup.GetByFilterID(filterID, user);
                foreach (ECN_Framework_Entities.Communicator.FilterGroup filterGroup in filterGroupList)
	            {
                    ECN_Framework_BusinessLayer.Communicator.FilterCondition.Delete(filterGroup.FilterGroupID, user, false);
	            }
                ECN_Framework_DataLayer.Communicator.FilterGroup.Delete(filterID, user.CustomerID, user.UserID);
                if (updateWhere)
                {
                    ECN_Framework_Entities.Communicator.Filter filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(filterID, user);
                    ECN_Framework_BusinessLayer.Communicator.Filter.UpdateWhereClause(filterID, ECN_Framework_BusinessLayer.Communicator.Filter.CreateWhereClause(filter), user);
                    ECN_Framework_BusinessLayer.Communicator.Filter.UpdateDynamicWhere(filterID, ECN_Framework_BusinessLayer.Communicator.Filter.CreateDynamicWhere(filter), user);
                }
                scope.Complete();
            }

        }

        public static void Delete(int filterID, int filterGroupID, KMPlatform.Entity.User user)
        {
            Delete(filterID, filterGroupID, user, true);
        }

        internal static void Delete(int filterID, int filterGroupID, KMPlatform.Entity.User user, bool updateWhere)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(filterGroupID, user.CustomerID))
            {
                //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
                GetByFilterGroupID(filterGroupID, user);

                if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                    throw new ECN_Framework_Common.Objects.SecurityException();

                using (TransactionScope scope = new TransactionScope())
                {
                    ECN_Framework_BusinessLayer.Communicator.FilterCondition.Delete(filterGroupID, user, false);
                    ECN_Framework_DataLayer.Communicator.FilterGroup.Delete(filterID, filterGroupID, user.CustomerID, user.UserID);
                    if (updateWhere)
                    {
                        ECN_Framework_Entities.Communicator.Filter filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(filterID, user);
                        ECN_Framework_BusinessLayer.Communicator.Filter.UpdateWhereClause(filterID, ECN_Framework_BusinessLayer.Communicator.Filter.CreateWhereClause(filter), user);
                        ECN_Framework_BusinessLayer.Communicator.Filter.UpdateDynamicWhere(filterID, ECN_Framework_BusinessLayer.Communicator.Filter.CreateDynamicWhere(filter), user);
                    }
                    ReSort(filterID, user);
                    scope.Complete();
                }
            }
            else
            {
                errorList.Add(new ECNError(Entity, Method, "FilterGroup does not exist"));
                throw new ECNException(errorList);
            }
        }

        public static int Save(ECN_Framework_Entities.Communicator.FilterGroup filterGroup, KMPlatform.Entity.User user)
        {
            return Save(filterGroup, user, true);
        }

        internal static int Save(ECN_Framework_Entities.Communicator.FilterGroup filterGroup, KMPlatform.Entity.User user, bool updateWhere)
        {
            Validate(filterGroup);

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(filterGroup, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                filterGroup.FilterGroupID = ECN_Framework_DataLayer.Communicator.FilterGroup.Save(filterGroup);
                if (filterGroup.FilterConditionList != null)
                {
                    foreach (ECN_Framework_Entities.Communicator.FilterCondition filterCondition in filterGroup.FilterConditionList)
                    {
                        filterCondition.FilterGroupID = filterGroup.FilterGroupID;
                        FilterCondition.Save(filterCondition, user, updateWhere);
                    }
                }
                if (updateWhere)
                {
                    ECN_Framework_Entities.Communicator.Filter filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(filterGroup.FilterID, user);            
                    ECN_Framework_BusinessLayer.Communicator.Filter.UpdateWhereClause(filterGroup.FilterID, ECN_Framework_BusinessLayer.Communicator.Filter.CreateWhereClause(filter), user);
                    ECN_Framework_BusinessLayer.Communicator.Filter.UpdateDynamicWhere(filterGroup.FilterID, ECN_Framework_BusinessLayer.Communicator.Filter.CreateDynamicWhere(filter), user);
                }
                ReSort(filterGroup.FilterID, user);
                scope.Complete();
                
            }
            return filterGroup.FilterGroupID;
        }

        public static void Validate(ECN_Framework_Entities.Communicator.FilterGroup filterGroup)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (filterGroup.ConditionCompareType.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "ConditionCompareType cannot be empty"));
            if (filterGroup.SortOrder < 0)
                errorList.Add(new ECNError(Entity, Method, "SortOrder is invalid"));

            if (filterGroup.CustomerID == null)
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            else
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(filterGroup.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));

                    if (filterGroup.CreatedUserID == null || (filterGroup.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(filterGroup.CreatedUserID.Value, false))))
                    {
                        if (filterGroup.FilterGroupID <= 0 && (filterGroup.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(filterGroup.CreatedUserID.Value, filterGroup.CustomerID.Value)))
                            errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                    }
                    if (filterGroup.FilterID > 0 && (filterGroup.UpdatedUserID == null || (filterGroup.UpdatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(filterGroup.UpdatedUserID.Value, false)))))
                    {
                        if (filterGroup.FilterGroupID > 0 && (filterGroup.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(filterGroup.UpdatedUserID.Value, filterGroup.CustomerID.Value)))
                            errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                    }
                    scope.Complete();
                }

                if (filterGroup.FilterID <= 0)
                    errorList.Add(new ECNError(Entity, Method, "FilterID is invalid"));
                else
                {
                    if (!ECN_Framework_BusinessLayer.Communicator.Filter.Exists(filterGroup.FilterID, filterGroup.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "FilterID is invalid"));
                    else
                        if (filterGroup.Name.Trim() == string.Empty)
                            errorList.Add(new ECNError(Entity, Method, "Name cannot be empty"));
                        else if (Exists(filterGroup.FilterGroupID, filterGroup.Name, filterGroup.FilterID, filterGroup.CustomerID.Value))
                            errorList.Add(new ECNError(Entity, Method, "Name already exists in this filter"));
                        else if (!ECN_Framework_Common.Functions.RegexUtilities.IsValidObjectName(filterGroup.Name))
                            errorList.Add(new ECNError(Entity, Method, "Name has invalid characters"));
                }
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static int GetSortOrder(int filterID)
        {
            int count = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                count = ECN_Framework_DataLayer.Communicator.FilterGroup.GetSortOrder(filterID);
                scope.Complete();
            }
            return count;
        }

        private static void ReSort(int filterID, KMPlatform.Entity.User user)
        {           
            List<ECN_Framework_Entities.Communicator.FilterGroup> filterGroupList = ECN_Framework_BusinessLayer.Communicator.FilterGroup.GetByFilterID(filterID, user);

            if (filterGroupList != null)
            {
                int i = 0;
                using (TransactionScope scope = new TransactionScope())
                {  
                    foreach (ECN_Framework_Entities.Communicator.FilterGroup filterGroup in filterGroupList)
                    {
                        ECN_Framework_DataLayer.Communicator.FilterGroup.UpdateSortOrder(filterGroup.FilterGroupID, ++i, user.UserID);
                    }
                    scope.Complete();
                }
                   
            }
        }
    }
}
