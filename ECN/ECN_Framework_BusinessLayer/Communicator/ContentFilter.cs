using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class ContentFilter
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Content;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.ContentFilter;

        public static bool HasPermission(KMPlatform.Enums.Access AccessCode, KMPlatform.Entity.User user)
        {
            if (AccessCode == KMPlatform.Enums.Access.View)
            {
                //if (KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Content.ToString()) ||
                //    KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Create_Content.ToString()))
                if (KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                    return true;
            }
            else if (AccessCode == KMPlatform.Enums.Access.Edit)
            {
                //if (KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Create_Content.ToString()))
                if (KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                    return true;
            }
            return false;
        }

        public static bool Exists(int contentID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.ContentFilter.Exists(contentID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int contentID, int filterID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.ContentFilter.Exists(contentID, filterID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int filterID, string filterName, int LayoutID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.ContentFilter.Exists(filterID, filterName, LayoutID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool HasDynamicContent(int layoutID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.ContentFilter.HasDynamicContent(layoutID);
                scope.Complete();
            }
            return exists;
        }

        public static List<ECN_Framework_Entities.Communicator.ContentFilter> GetByLayoutIDSlotNumber(int layoutID, int slotNumber, KMPlatform.Entity.User user, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.ContentFilter> contentFilterList = new List<ECN_Framework_Entities.Communicator.ContentFilter>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                contentFilterList = ECN_Framework_DataLayer.Communicator.ContentFilter.GetByLayoutIDSlotNumber(layoutID, slotNumber);
                scope.Complete();
            }

            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(contentFilterList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (contentFilterList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.ContentFilter contentFilter in contentFilterList)
                {
                    contentFilter.DetailList = ContentFilterDetail.GetByFilterID(contentFilter.FilterID, user);
                }                
            }
            return contentFilterList;
        }

        public static ECN_Framework_Entities.Communicator.ContentFilter GetByFilterID(int filterID, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.ContentFilter contentFilter = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                contentFilter = ECN_Framework_DataLayer.Communicator.ContentFilter.GetByFilterID(filterID);
                scope.Complete();
            }

            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(contentFilter, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (contentFilter != null && getChildren)
            {
                contentFilter.DetailList = ContentFilterDetail.GetByFilterID(filterID, user);
            }
            return contentFilter;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static List<ECN_Framework_Entities.Communicator.ContentFilter> GetByContentID_NoAccessCheck(int contentID, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.ContentFilter> contentFilterList = new List<ECN_Framework_Entities.Communicator.ContentFilter>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                contentFilterList = ECN_Framework_DataLayer.Communicator.ContentFilter.GetByContentID(contentID);
                scope.Complete();
            }

            if (contentFilterList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.ContentFilter contentFilter in contentFilterList)
                {
                    contentFilter.DetailList = ContentFilterDetail.GetByFilterID_NoAccessCheck(contentFilter.FilterID);
                }
            }
            return contentFilterList;
        }

        public static List<ECN_Framework_Entities.Communicator.ContentFilter> GetByContentID_NoAccessCheck_UseAmbientTransaction(int contentID, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.ContentFilter> contentFilterList = new List<ECN_Framework_Entities.Communicator.ContentFilter>();
            using (TransactionScope scope = new TransactionScope())
            {
                contentFilterList = ECN_Framework_DataLayer.Communicator.ContentFilter.GetByContentID(contentID);
                scope.Complete();
            }

            if (contentFilterList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.ContentFilter contentFilter in contentFilterList)
                {
                    contentFilter.DetailList = ContentFilterDetail.GetByFilterID_NoAccessCheck_UseAmbientTransaction(contentFilter.FilterID);
                }
            }
            return contentFilterList;
        }

        public static List<ECN_Framework_Entities.Communicator.ContentFilter> GetByContentID(int contentID, KMPlatform.Entity.User user, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.ContentFilter> contentFilterList = new List<ECN_Framework_Entities.Communicator.ContentFilter>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                contentFilterList = ECN_Framework_DataLayer.Communicator.ContentFilter.GetByContentID(contentID);
                scope.Complete();
            }

            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(contentFilterList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (contentFilterList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.ContentFilter contentFilter in contentFilterList)
                {
                    contentFilter.DetailList = ContentFilterDetail.GetByFilterID(contentFilter.FilterID, user);
                }
            }
            return contentFilterList;
        }

        public static List<ECN_Framework_Entities.Communicator.ContentFilter> GetByContentID_UseAmbientTransaction(int contentID, KMPlatform.Entity.User user, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.ContentFilter> contentFilterList = new List<ECN_Framework_Entities.Communicator.ContentFilter>();
            using (TransactionScope scope = new TransactionScope())
            {
                contentFilterList = ECN_Framework_DataLayer.Communicator.ContentFilter.GetByContentID(contentID);
                scope.Complete();
            }

            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(contentFilterList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (contentFilterList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.ContentFilter contentFilter in contentFilterList)
                {
                    contentFilter.DetailList = ContentFilterDetail.GetByFilterID_UseAmbientTransaction(contentFilter.FilterID, user);
                }
            }
            return contentFilterList;
        }

        //private static bool SecurityCheck(ECN_Framework_Entities.Communicator.ContentFilter filter, KMPlatform.Entity.User user)
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

        //private static bool SecurityCheck(List<ECN_Framework_Entities.Communicator.ContentFilter> filterList, KMPlatform.Entity.User user)
        //{
        //    if (filterList != null)
        //    {
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var securityCheck = from ct in filterList
        //                                join c in custList on ct.CustomerID.Value equals c.CustomerID
        //                                select new { ct.FilterID };

        //            if (securityCheck.Count() != filterList.Count)
        //                return false;
        //        }
        //        else
        //        {
        //            var securityCheck = from ct in filterList
        //                                where ct.CustomerID.Value != user.CustomerID
        //                                select new { ct.FilterID };

        //            if (securityCheck.Any())
        //                return false;
        //        }
        //    }
        //    return true;
        //}

        //public static void Delete(int contentID, int filterID, KMPlatform.Entity.User user)
        //{
        //    ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
        //    List<ECNError> errorList = new List<ECNError>();

        //    if (Exists(contentID, filterID, user.CustomerID))
        //    {
        //        if (!ECN_Framework_BusinessLayer.Communicator.Layout.ContentUsedInLayout(contentID))
        //        {
        //            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
        //            ECN_Framework_Entities.Communicator.ContentFilter filter = GetByFilterID(filterID, user, false);

        //            if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
        //                throw new ECN_Framework_Common.Objects.SecurityException();

        //            using (TransactionScope scope = new TransactionScope())
        //            {
        //                ECN_Framework_BusinessLayer.Communicator.ContentFilterDetail.Delete(filterID, user);
        //                ECN_Framework_DataLayer.Communicator.ContentFilter.Delete(contentID, filterID, user.CustomerID, user.UserID);
        //                scope.Complete();
        //            }
        //        }

        //        else
        //        {
        //            errorList.Add(new ECNError(Entity, Method, "Content is used in layout(s)"));
        //            throw new ECNException(errorList);
        //        }
        //    }
        //    else
        //    {
        //        errorList.Add(new ECNError(Entity, Method, "Item does not exist"));
        //        throw new ECNException(errorList);
        //    }
        //}

        public static void Delete(int contentID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(contentID, user.CustomerID))
            {
                if (!ECN_Framework_BusinessLayer.Communicator.Layout.ContentUsedInLayout(contentID))
                {
                    //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
                    List<ECN_Framework_Entities.Communicator.ContentFilter> filterList = ECN_Framework_BusinessLayer.Communicator.ContentFilter.GetByContentID(contentID, user, false);

                    if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
                        throw new ECN_Framework_Common.Objects.SecurityException();

                    using (TransactionScope scope = new TransactionScope())
                    {                     
                        foreach (ECN_Framework_Entities.Communicator.ContentFilter filter in filterList)
                        {
                            ECN_Framework_BusinessLayer.Communicator.ContentFilterDetail.Delete(filter.FilterID, user);
                        }
                        ECN_Framework_DataLayer.Communicator.ContentFilter.Delete(contentID, user.CustomerID, user.UserID);

                        scope.Complete();
                    }
                }
                else
                {
                    errorList.Add(new ECNError(Entity, Method, "Content is used in layout(s)"));
                    throw new ECNException(errorList);
                }
            }
        }

        public static void Delete(int contentID, int filterID, KMPlatform.Entity.User user)
        {
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(contentID, user.CustomerID))
            {
                
                    if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
                        throw new ECN_Framework_Common.Objects.SecurityException();

                    using (TransactionScope scope = new TransactionScope())
                    {
                        ECN_Framework_DataLayer.Communicator.ContentFilter.Delete(contentID, filterID, user.CustomerID, user.UserID);

                        scope.Complete();
                    }
                
            }
        }

        public static void Validate(ECN_Framework_Entities.Communicator.ContentFilter filter)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (filter.CustomerID == null)
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            }
            else
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(filter.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));

                    if (filter.CreatedUserID == null || (filter.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(filter.CreatedUserID.Value, false))))
                    {
                        if (filter.CreatedUserID == null || (!KMPlatform.BusinessLogic.User.Exists(filter.CreatedUserID.Value, filter.CustomerID.Value)))
                            errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                    }

                    if (filter.FilterID > 0 && filter.UpdatedUserID != null)
                    {
                        if(!KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(filter.UpdatedUserID.Value, false)) && !KMPlatform.BusinessLogic.User.Exists(filter.UpdatedUserID.Value, filter.CustomerID.Value))
                             errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                    }
                    else if(filter.FilterID > 0 && filter.UpdatedUserID == null)
                        errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

                    scope.Complete();
                }

                if (filter.LayoutID == null || !ECN_Framework_BusinessLayer.Communicator.Layout.Exists(filter.LayoutID.Value, filter.CustomerID.Value))
                    errorList.Add(new ECNError(Entity, Method, "LayoutID is invalid"));
                if (filter.SlotNumber == null || filter.SlotNumber <= 0 || filter.SlotNumber > 9)
                    errorList.Add(new ECNError(Entity, Method, "SlotNumber is invalid"));
                if (filter.ContentID == null || !ECN_Framework_BusinessLayer.Communicator.Content.Exists(filter.ContentID.Value, filter.CustomerID.Value))
                    errorList.Add(new ECNError(Entity, Method, "ContentID is invalid"));
                if (filter.GroupID == null || !ECN_Framework_BusinessLayer.Communicator.Group.Exists(filter.GroupID.Value, filter.CustomerID.Value))
                    errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));
                //if (filter.WhereClause.Trim() == string.Empty)
                //    errorList.Add(new ECNError(Entity, Method, "WhereClause cannot be empty"));

                if (filter.FilterName.Trim() == string.Empty)
                    errorList.Add(new ECNError(Entity, Method, "FilterName cannot be empty"));
                //else if (Exists(filter.FilterID, filter.FilterName, filter.ContentID.Value, filter.CustomerID.Value))
                //    errorList.Add(new ECNError(Entity, Method, "FilterName already exists for this Content"));
                else if(Exists(filter.FilterID, filter.FilterName, filter.LayoutID.Value, filter.CustomerID.Value))
                    errorList.Add(new ECNError(Entity, Method, "FilterName already exists for this Layout"));
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Save(ECN_Framework_Entities.Communicator.ContentFilter filter, KMPlatform.Entity.User user)
        {
            List<ECNError> errorList = new List<ECNError>();
            Validate(filter);

            if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(filter,   user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                filter.FilterID = ECN_Framework_DataLayer.Communicator.ContentFilter.Save(filter);
                if (filter.DetailList != null)
                {
                    foreach (ECN_Framework_Entities.Communicator.ContentFilterDetail detail in filter.DetailList)
                    {
                        detail.FilterID = filter.FilterID;
                        ContentFilterDetail.Save(detail, user);
                    }
                }
                scope.Complete();
            }
        }
    }
}
