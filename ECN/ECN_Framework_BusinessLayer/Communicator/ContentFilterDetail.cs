using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class ContentFilterDetail
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Content;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.ContentFilterDetail;

        public static bool Exists(int filterID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.ContentFilterDetail.Exists(filterID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int filterID, int fdid, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.ContentFilterDetail.Exists(filterID, fdid, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static ECN_Framework_Entities.Communicator.ContentFilterDetail GetByFDID(int fdid, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.ContentFilterDetail detail = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                detail = ECN_Framework_DataLayer.Communicator.ContentFilterDetail.GetByFDID(fdid);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(detail, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();
            return detail;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static List<ECN_Framework_Entities.Communicator.ContentFilterDetail> GetByFilterID_NoAccessCheck(int filterID)
        {
            List<ECN_Framework_Entities.Communicator.ContentFilterDetail> detailList = new List<ECN_Framework_Entities.Communicator.ContentFilterDetail>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                detailList = ECN_Framework_DataLayer.Communicator.ContentFilterDetail.GetByFilterID(filterID);
                scope.Complete();
            }

            return detailList;
        }

        public static List<ECN_Framework_Entities.Communicator.ContentFilterDetail> GetByFilterID_NoAccessCheck_UseAmbientTransaction(int filterID)
        {
            List<ECN_Framework_Entities.Communicator.ContentFilterDetail> detailList = new List<ECN_Framework_Entities.Communicator.ContentFilterDetail>();
            using (TransactionScope scope = new TransactionScope())
            {
                detailList = ECN_Framework_DataLayer.Communicator.ContentFilterDetail.GetByFilterID(filterID);
                scope.Complete();
            }

            return detailList;
        }

        public static List<ECN_Framework_Entities.Communicator.ContentFilterDetail> GetByFilterID(int filterID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.ContentFilterDetail> detailList = new List<ECN_Framework_Entities.Communicator.ContentFilterDetail>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                detailList = ECN_Framework_DataLayer.Communicator.ContentFilterDetail.GetByFilterID(filterID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(detailList, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();
            return detailList;
        }

        public static List<ECN_Framework_Entities.Communicator.ContentFilterDetail> GetByFilterID_UseAmbientTransaction(int filterID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.ContentFilterDetail> detailList = new List<ECN_Framework_Entities.Communicator.ContentFilterDetail>();
            using (TransactionScope scope = new TransactionScope())
            {
                detailList = ECN_Framework_DataLayer.Communicator.ContentFilterDetail.GetByFilterID(filterID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(detailList, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();
            return detailList;
        }

        //private static bool SecurityCheck(ECN_Framework_Entities.Communicator.ContentFilterDetail detail, KMPlatform.Entity.User user)
        //{
        //    if (detail != null)
        //    {
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

        //            if (!custExists.Any())
        //                return false;
        //        }
        //        else if (detail.CustomerID.Value != user.CustomerID)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        //private static bool SecurityCheck(List<ECN_Framework_Entities.Communicator.ContentFilterDetail> detailList, KMPlatform.Entity.User user)
        //{
        //    if (detailList != null)
        //    {
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var securityCheck = from ct in detailList
        //                                join c in custList on ct.CustomerID.Value equals c.CustomerID
        //                                select new { ct.FDID };

        //            if (securityCheck.Count() != detailList.Count)
        //                return false;
        //        }
        //        else
        //        {
        //            var securityCheck = from ct in detailList
        //                                where ct.CustomerID.Value != user.CustomerID
        //                                select new { ct.FDID };

        //            if (securityCheck.Any())
        //                return false;
        //        }
        //    }
        //    return true;
        //}

        public static void Delete(int filterID, int fdid, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(filterID, fdid, user.CustomerID))
            {
                ECN_Framework_Entities.Communicator.ContentFilter filter = ECN_Framework_BusinessLayer.Communicator.ContentFilter.GetByFilterID(filterID, user, false);
                if (!ECN_Framework_BusinessLayer.Communicator.Layout.ContentUsedInLayout(filter.ContentID.Value))
                {
                    //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
                    ECN_Framework_Entities.Communicator.ContentFilterDetail filterDetail = GetByFDID(fdid, user);

                    using (TransactionScope scope = new TransactionScope())
                    {
                        ECN_Framework_DataLayer.Communicator.ContentFilterDetail.Delete(filterID, fdid, user.CustomerID, user.UserID);
                        scope.Complete();
                    }
                }
                else
                {
                    errorList.Add(new ECNError(Entity, Method, "Content is used in layout(s)"));
                    throw new ECNException(errorList);
                }
            }
            else
            {
                errorList.Add(new ECNError(Entity, Method, "Item does not exist"));
                throw new ECNException(errorList);
            }
        }

        public static void Delete(int filterID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(filterID, user.CustomerID))
            {
                ECN_Framework_Entities.Communicator.ContentFilter filter = ECN_Framework_BusinessLayer.Communicator.ContentFilter.GetByFilterID(filterID, user, false);
                //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
                List<ECN_Framework_Entities.Communicator.ContentFilterDetail> filterDetailList = GetByFilterID(filterID, user);

                using (TransactionScope scope = new TransactionScope())
                {
                    ECN_Framework_DataLayer.Communicator.CampaignItem.Delete(filterID, user.CustomerID, user.UserID);
                    scope.Complete();
                }
            }
        }

        static System.Text.RegularExpressions.Regex isValidComparatorRegex = new System.Text.RegularExpressions.Regex
            (@"  between  
               | contains
               | (?:start|end)  (?:ing|s) \s+ with
               | equals
               | (?:greater|less) \s+ than"
            , System.Text.RegularExpressions.RegexOptions.Compiled 
            | System.Text.RegularExpressions.RegexOptions.IgnoreCase
            | System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace);

        public static void Validate(ECN_Framework_Entities.Communicator.ContentFilterDetail detail)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (detail.CustomerID == null)
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            }
            else
            {
                if (detail.FilterID == null)
                {
                    errorList.Add(new ECNError(Entity, Method, "FilterID is invalid"));
                }
                if (detail.FieldType.Trim() == string.Empty)
                    errorList.Add(new ECNError(Entity, Method, "FieldType cannot be empty"));
                if (detail.CompareType.Trim() == string.Empty)
                    errorList.Add(new ECNError(Entity, Method, "CompareType cannot be empty"));
                if (String.IsNullOrWhiteSpace(detail.FieldName))
                {
                    errorList.Add(new ECNError(Entity, Method, "FieldName cannot be empty"));
                }
                else if(detail.FieldName.Contains('/'))
                {
                    errorList.Add(new ECNError(Entity, Method, "FieldName contains invalid characters"));
                }
                if (String.IsNullOrWhiteSpace(detail.Comparator))
                {
                    errorList.Add(new ECNError(Entity, Method, "Comparator cannot be empty"));
                }
                else if(false == isValidComparatorRegex.IsMatch(detail.Comparator))
                {
                    errorList.Add(new ECNError(Entity, Method, "Comparator is invalid"));
                }
                
                if (detail.CompareValue.Trim() == string.Empty)
                    errorList.Add(new ECNError(Entity, Method, "CompareValue cannot be empty"));

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(detail.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));

                    if (detail.CreatedUserID == null || (detail.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(detail.CreatedUserID.Value, false))))
                    {
                        if (detail.CreatedUserID == null || (!KMPlatform.BusinessLogic.User.Exists(detail.CreatedUserID.Value, detail.CustomerID.Value)))
                            errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                    }

                    if (detail.FDID > 0 && detail.UpdatedUserID != null)
                    {
                        if (!KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(detail.UpdatedUserID.Value, false)) && !KMPlatform.BusinessLogic.User.Exists(detail.UpdatedUserID.Value, detail.CustomerID.Value))
                            errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                    }
                    else if (detail.FDID > 0 && detail.UpdatedUserID == null)
                        errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

                    scope.Complete();
                }
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Save(ECN_Framework_Entities.Communicator.ContentFilterDetail detail, KMPlatform.Entity.User user)
        {
            List<ECNError> errorList = new List<ECNError>();
            Validate(detail);

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(detail, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                detail.FDID = ECN_Framework_DataLayer.Communicator.ContentFilterDetail.Save(detail);
                scope.Complete();
            }
        }

        public static DataTable GetByContentIDFilterID(int filterID, KMPlatform.Entity.User user)
        {
            DataTable dtContentFilterDetail = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtContentFilterDetail = ECN_Framework_DataLayer.Communicator.ContentFilterDetail.GetByContentIDFilterID(filterID, user.CustomerID);
                scope.Complete();
            }

            return dtContentFilterDetail;
        }
    }
}
