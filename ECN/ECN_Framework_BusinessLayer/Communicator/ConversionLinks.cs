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
    public class ConversionLinks
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.ConversionLinks;

        private static bool HasFeature(KMPlatform.Entity.User user)
        {

            if (KM.Platform.User.HasServiceFeature(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ConversionTracking))
                return true;
            return false;
        }

        public static bool Exists(int layoutID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.ConversionLinks.Exists(layoutID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int layoutID, int linkID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.ConversionLinks.Exists(layoutID, linkID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int linkID, string linkName, int layoutID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.ConversionLinks.Exists(linkID, linkName, layoutID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static List<ECN_Framework_Entities.Communicator.ConversionLinks> GetByLayoutID(int layoutID, KMPlatform.Entity.User user)
        {
         

            List<ECN_Framework_Entities.Communicator.ConversionLinks> linkList = new List<ECN_Framework_Entities.Communicator.ConversionLinks>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                linkList = ECN_Framework_DataLayer.Communicator.ConversionLinks.GetByLayoutID(layoutID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(linkList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();
            if (linkList.Count > 0)
            {
                if (!HasFeature(user))
                    throw new ECN_Framework_Common.Objects.SecurityException();
            }

            return linkList;
        }

        public static List<ECN_Framework_Entities.Communicator.ConversionLinks> GetByLayoutID_UseAmbientTransaction(int layoutID, KMPlatform.Entity.User user)
        {


            List<ECN_Framework_Entities.Communicator.ConversionLinks> linkList = new List<ECN_Framework_Entities.Communicator.ConversionLinks>();
            using (TransactionScope scope = new TransactionScope())
            {
                linkList = ECN_Framework_DataLayer.Communicator.ConversionLinks.GetByLayoutID(layoutID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(linkList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();
            if (linkList.Count > 0)
            {
                if (!HasFeature(user))
                    throw new ECN_Framework_Common.Objects.SecurityException();
            }

            return linkList;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static List<ECN_Framework_Entities.Communicator.ConversionLinks> GetByLayoutID_NoAccessCheck(int layoutID)
        {
            List<ECN_Framework_Entities.Communicator.ConversionLinks> linkList = new List<ECN_Framework_Entities.Communicator.ConversionLinks>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                linkList = ECN_Framework_DataLayer.Communicator.ConversionLinks.GetByLayoutID(layoutID);
                scope.Complete();
            }

            return linkList;
        }

        public static List<ECN_Framework_Entities.Communicator.ConversionLinks> GetByLayoutID_NoAccessCheck_UseAmbientTransaction(int layoutID)
        {
            List<ECN_Framework_Entities.Communicator.ConversionLinks> linkList = new List<ECN_Framework_Entities.Communicator.ConversionLinks>();
            using (TransactionScope scope = new TransactionScope())
            {
                linkList = ECN_Framework_DataLayer.Communicator.ConversionLinks.GetByLayoutID(layoutID);
                scope.Complete();
            }

            return linkList;
        }

        public static List<ECN_Framework_Entities.Communicator.ConversionLinks> GetByBlastID(int blastID, KMPlatform.Entity.User user)
        {
            if (!HasFeature(user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            List<ECN_Framework_Entities.Communicator.ConversionLinks> linkList = new List<ECN_Framework_Entities.Communicator.ConversionLinks>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                linkList = ECN_Framework_DataLayer.Communicator.ConversionLinks.GetByBlastID(blastID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(linkList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return linkList;
        }

        public static ECN_Framework_Entities.Communicator.ConversionLinks GetByLinkID(int linkID, KMPlatform.Entity.User user, bool getChildren)
        {
            if (!HasFeature(user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.ConversionLinks link = null; 
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                link = ECN_Framework_DataLayer.Communicator.ConversionLinks.GetByLinkID(linkID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(link, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return link;
        }

        //private static bool SecurityCheck(ECN_Framework_Entities.Communicator.ConversionLinks link, KMPlatform.Entity.User user)
        //{
        //    if (link != null)
        //    {
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

        //            if (!custExists.Any())
        //                return false;
        //        }
        //        else if (link.CustomerID.Value != user.CustomerID)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        //private static bool SecurityCheck(List<ECN_Framework_Entities.Communicator.ConversionLinks> linkList, KMPlatform.Entity.User user)
        //{
        //    if (linkList != null)
        //    {
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var securityCheck = from ct in linkList
        //                                join c in custList on ct.CustomerID.Value equals c.CustomerID
        //                                select new { ct.LinkID };

        //            if (securityCheck.Count() != linkList.Count)
        //                return false;
        //        }
        //        else
        //        {
        //            var securityCheck = from ct in linkList
        //                                where ct.CustomerID.Value != user.CustomerID
        //                                select new { ct.LinkID };

        //            if (securityCheck.Any())
        //                return false;
        //        }
        //    }
        //    return true;
        //}

        public static void Delete(int layoutID, int linkID, KMPlatform.Entity.User user)
        {
            if (!HasFeature(user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (!ECN_Framework_BusinessLayer.Communicator.Blast.ActivePendingOrSentByLayout(layoutID, user.CustomerID))
            {
                //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
                GetByLinkID(linkID, user, false);

                using (TransactionScope scope = new TransactionScope())
                {
                    ECN_Framework_DataLayer.Communicator.ConversionLinks.Delete(layoutID, linkID, user.CustomerID, user.UserID);
                    scope.Complete();
                }
            }

            else
            {
                errorList.Add(new ECNError(Entity, Method, "Layout is used in Blast(s)"));
                throw new ECNException(errorList);
            }
        }

        public static void Delete(int layoutID, KMPlatform.Entity.User user)
        {
            if (!KMPlatform.BusinessLogic.User.HasAccess(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ConversionTracking, KMPlatform.Enums.Access.Delete))
                throw new ECN_Framework_Common.Objects.SecurityException();
            
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(layoutID, user.CustomerID))
            {
                if (!ECN_Framework_BusinessLayer.Communicator.Blast.ActivePendingOrSentByLayout(layoutID, user.CustomerID))
                {
                    //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
                    GetByLayoutID(layoutID, user);

                    using (TransactionScope scope = new TransactionScope())
                    {
                        ECN_Framework_DataLayer.Communicator.ConversionLinks.Delete(layoutID, user.CustomerID, user.UserID);
                        scope.Complete();
                    }
                }
                else
                {
                    errorList.Add(new ECNError(Entity, Method, "Layout is used in Blast(s)"));
                    throw new ECNException(errorList);
                }
            }
        }

        public static void Validate(ECN_Framework_Entities.Communicator.ConversionLinks link)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (link.CustomerID == null)
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            }
            else
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(link.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
                    if (link.CreatedUserID == null || (link.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(link.CreatedUserID.Value, false))))
                    {
                        if (link.CreatedUserID == null || (!KMPlatform.BusinessLogic.User.Exists(link.CreatedUserID.Value, link.CustomerID.Value)))
                            errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                    }

                    if (link.LinkID > 0 && link.UpdatedUserID!=null)
                    {
                         if(!KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(link.UpdatedUserID.Value, false)) && !KMPlatform.BusinessLogic.User.Exists(link.UpdatedUserID.Value, link.CustomerID.Value))
                             errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                    }
                    else if (link.LinkID > 0 && link.UpdatedUserID != null)
                        errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

                    scope.Complete();
                }

                if (!ECN_Framework_Common.Functions.RegexUtilities.IsValidObjectName(link.LinkName))
                    errorList.Add(new ECNError(Entity, Method, "Link name has invalid characters"));

                if (link.LayoutID == null || !ECN_Framework_BusinessLayer.Communicator.Layout.Exists(link.LayoutID.Value, link.CustomerID.Value))
                    errorList.Add(new ECNError(Entity, Method, "LayoutID is invalid"));

                if (link.LinkURL.Trim() == string.Empty || link.LinkURL.Trim().Length > 1792)
                    errorList.Add(new ECNError(Entity, Method, "LinkURL cannot be empty"));

                if(link.IsActive.Trim().Length == 0 || (link.IsActive.ToUpper() != "Y" && link.IsActive.ToUpper() != "N"))
                    errorList.Add(new ECNError(Entity, Method, "IsActive is invalid"));
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Save(ECN_Framework_Entities.Communicator.ConversionLinks link, KMPlatform.Entity.User user)
        {
            if (!KMPlatform.BusinessLogic.User.HasAccess(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ConversionTracking, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            List<ECNError> errorList = new List<ECNError>();
            Validate(link);

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(link, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                link.LinkID = ECN_Framework_DataLayer.Communicator.ConversionLinks.Save(link);
                scope.Complete();
            }
        }
    }
}
