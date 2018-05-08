 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class LinkAlias
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.LinkAlias;

        public static bool Exists(int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.LinkAlias.Exists(customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int contentID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.LinkAlias.Exists(contentID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool ExistsByOwnerID(int ownerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.LinkAlias.ExistsByOwnerID(ownerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int contentID, int aliasID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.LinkAlias.Exists(contentID, aliasID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int contentID, string link, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.LinkAlias.Exists(contentID, link, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int layoutID, string link)
        {
            bool exists = false;            
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                
                exists = ECN_Framework_DataLayer.Communicator.LinkAlias.Exists(layoutID, link);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int aliasID, string alias, int contentID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.LinkAlias.Exists(aliasID, alias, contentID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool CodeUsedInLinkAlias(int codeID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.LinkAlias.CodeUsedInLinkAlias(codeID);
                scope.Complete();
            }
            return exists;
        }

        public static ECN_Framework_Entities.Communicator.LinkAlias GetByAliasID(int aliasID, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.LinkAlias alias = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                alias = ECN_Framework_DataLayer.Communicator.LinkAlias.GetByAliasID(aliasID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(alias, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (alias != null && getChildren)
            {
                alias.linkOwner = ECN_Framework_BusinessLayer.Communicator.LinkOwnerIndex.GetByOwnerID(alias.LinkOwnerID.Value, user);
            }

            return alias;
        }

        public static ECN_Framework_Entities.Communicator.LinkAlias GetByBlastLink(int blastID, string link, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.LinkAlias alias = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                alias = ECN_Framework_DataLayer.Communicator.LinkAlias.GetByBlastLink(blastID, link);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(alias, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (alias != null && getChildren)
            {
                alias.linkOwner = ECN_Framework_BusinessLayer.Communicator.LinkOwnerIndex.GetByOwnerID(alias.LinkOwnerID.Value, user);
            }

            return alias;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static List<ECN_Framework_Entities.Communicator.LinkAlias> GetByContentID_NoAccessCheck(int contentID, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.LinkAlias> aliasList = new List<ECN_Framework_Entities.Communicator.LinkAlias>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                aliasList = ECN_Framework_DataLayer.Communicator.LinkAlias.GetByContentID(contentID);
                scope.Complete();
            }

            if (aliasList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.LinkAlias alias in aliasList)
                {
                    if (alias.LinkOwnerID != null)
                    {
                        alias.linkOwner = ECN_Framework_BusinessLayer.Communicator.LinkOwnerIndex.GetByOwnerID_NoAccessCheck(alias.LinkOwnerID.Value);
                    }
                }
            }

            return aliasList;
        }

        public static List<ECN_Framework_Entities.Communicator.LinkAlias> GetByContentID_NoAccessCheck_UseAmbientTransaction(int contentID, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.LinkAlias> aliasList = new List<ECN_Framework_Entities.Communicator.LinkAlias>();
            using (TransactionScope scope = new TransactionScope())
            {
                aliasList = ECN_Framework_DataLayer.Communicator.LinkAlias.GetByContentID(contentID);
                scope.Complete();
            }

            if (aliasList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.LinkAlias alias in aliasList)
                {
                    if (alias.LinkOwnerID != null)
                    {
                        alias.linkOwner = ECN_Framework_BusinessLayer.Communicator.LinkOwnerIndex.GetByOwnerID_NoAccessCheck_UseAmbientTransaction(alias.LinkOwnerID.Value);
                    }
                }
            }

            return aliasList;
        }

        public static List<ECN_Framework_Entities.Communicator.LinkAlias> GetByContentID(int contentID, KMPlatform.Entity.User user, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.LinkAlias> aliasList = new List<ECN_Framework_Entities.Communicator.LinkAlias>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                aliasList = ECN_Framework_DataLayer.Communicator.LinkAlias.GetByContentID(contentID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(aliasList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (aliasList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.LinkAlias alias in aliasList)
                {
                    if (alias.LinkOwnerID != null)
                    {
                        alias.linkOwner = ECN_Framework_BusinessLayer.Communicator.LinkOwnerIndex.GetByOwnerID_NoAccessCheck(alias.LinkOwnerID.Value);
                    }
                }
            }

            return aliasList;
        }

        public static List<ECN_Framework_Entities.Communicator.LinkAlias> GetByContentID_UseAmbientTransaction(int contentID, KMPlatform.Entity.User user, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.LinkAlias> aliasList = new List<ECN_Framework_Entities.Communicator.LinkAlias>();
            using (TransactionScope scope = new TransactionScope())
            {
                aliasList = ECN_Framework_DataLayer.Communicator.LinkAlias.GetByContentID(contentID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(aliasList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (aliasList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.LinkAlias alias in aliasList)
                {
                    if (alias.LinkOwnerID != null)
                    {
                        alias.linkOwner = ECN_Framework_BusinessLayer.Communicator.LinkOwnerIndex.GetByOwnerID_NoAccessCheck_UseAmbientTransaction(alias.LinkOwnerID.Value);
                    }
                }
            }

            return aliasList;
        }

        public static List<ECN_Framework_Entities.Communicator.LinkAlias> GetByOwnerID(int ownerID, KMPlatform.Entity.User user, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.LinkAlias> aliasList = new List<ECN_Framework_Entities.Communicator.LinkAlias>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                aliasList = ECN_Framework_DataLayer.Communicator.LinkAlias.GetByOwnerID(ownerID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(aliasList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (aliasList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.LinkAlias alias in aliasList)
                {
                    alias.linkOwner = ECN_Framework_BusinessLayer.Communicator.LinkOwnerIndex.GetByOwnerID_NoAccessCheck(alias.LinkOwnerID.Value);
                }
            }

            return aliasList;
        }

        public static List<ECN_Framework_Entities.Communicator.LinkAlias> GetByLink(int contentID, string link, KMPlatform.Entity.User user, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.LinkAlias> aliasList = new List<ECN_Framework_Entities.Communicator.LinkAlias>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                aliasList = ECN_Framework_DataLayer.Communicator.LinkAlias.GetByLink(contentID, link);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(aliasList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (aliasList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.LinkAlias alias in aliasList)
                {
                    alias.linkOwner = ECN_Framework_BusinessLayer.Communicator.LinkOwnerIndex.GetByOwnerID_NoAccessCheck(alias.LinkOwnerID.Value);
                }
            }

            return aliasList;
        }

        //private static bool SecurityCheck(ECN_Framework_Entities.Communicator.LinkAlias alias, KMPlatform.Entity.User user)
        //{
        //    if (alias != null)
        //    {
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

        //            if (!custExists.Any())
        //                return false;
        //        }
        //        else if (alias.CustomerID.Value != user.CustomerID)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        //private static bool SecurityCheck(List<ECN_Framework_Entities.Communicator.LinkAlias> aliasList, KMPlatform.Entity.User user)
        //{
        //    if (aliasList != null)
        //    {
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var securityCheck = from ct in aliasList
        //                                join c in custList on ct.CustomerID.Value equals c.CustomerID
        //                                select new { ct.AliasID };

        //            if (securityCheck.Count() != aliasList.Count)
        //                return false;
        //        }
        //        else
        //        {
        //            var securityCheck = from ct in aliasList
        //                                where ct.CustomerID.Value != user.CustomerID
        //                                select new { ct.AliasID };

        //            if (securityCheck.Any())
        //                return false;
        //        }
        //    }
        //    return true;
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
                    GetByContentID(contentID, user, false);

                    using (TransactionScope scope = new TransactionScope())
                    {
                        ECN_Framework_DataLayer.Communicator.LinkAlias.Delete(contentID, user.CustomerID, user.UserID);
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

        public static void Delete(int contentID, int aliasID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(contentID, aliasID, user.CustomerID))
            {
                if (!ECN_Framework_BusinessLayer.Communicator.Layout.ContentUsedInLayout(contentID))
                {
                    //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
                    GetByAliasID(aliasID, user, false);

                    using (TransactionScope scope = new TransactionScope())
                    {
                        ECN_Framework_DataLayer.Communicator.LinkAlias.Delete(contentID, aliasID, user.CustomerID, user.UserID);
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
                errorList.Add(new ECNError(Entity, Method, "LinkAlias does not exist"));
                throw new ECNException(errorList);
            }
        }

        public static void Delete(int contentID, string link, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(contentID, link, user.CustomerID))
            {
                if (!ECN_Framework_BusinessLayer.Communicator.Layout.ContentUsedInLayout(contentID))
                {
                    //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
                    GetByLink(contentID, link, user, false);

                    using (TransactionScope scope = new TransactionScope())
                    {
                        ECN_Framework_DataLayer.Communicator.LinkAlias.Delete(contentID, link, user.CustomerID, user.UserID);
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
                errorList.Add(new ECNError(Entity, Method, "LinkAlias does not exist"));
                throw new ECNException(errorList);
            }
        }

        public static int Save(ECN_Framework_Entities.Communicator.LinkAlias alias, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
            if (alias.AliasID > 0)
            {
                if (!Exists(alias.ContentID.Value, alias.AliasID, alias.CustomerID.Value))
                {
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(new ECNError(Entity, Method, "AliasID is invalid"));
                    throw new ECNException(errorList);
                }
            }
            Validate(alias, user);

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(alias, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                alias.AliasID = ECN_Framework_DataLayer.Communicator.LinkAlias.Save(alias);
                scope.Complete();
            }
            return alias.AliasID;
        }

        public static void Validate(ECN_Framework_Entities.Communicator.LinkAlias alias, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (alias.CustomerID == -1)
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            }
            else
            {
                

                if (alias.Link.Trim() == string.Empty)
                    errorList.Add(new ECNError(Entity, Method, "Link cannot be empty"));

                if(alias.ContentID == null)
                    errorList.Add(new ECNError(Entity, Method, "ContentID is invalid"));
                else if (!ECN_Framework_BusinessLayer.Communicator.Content.Exists(alias.ContentID.Value, alias.CustomerID.Value))
                    errorList.Add(new ECNError(Entity, Method, "ContentID is invalid"));

                if (alias.Alias.Trim() == string.Empty)
                    errorList.Add(new ECNError(Entity, Method, "Alias cannot be empty"));

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(alias.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));

                    if (alias.CreatedUserID == null || (alias.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(alias.CreatedUserID.Value, false))))
                    {
                        if (alias.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(alias.CreatedUserID.Value, alias.CustomerID.Value))
                            errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                    }

                    if (alias.AliasID > 0 && (alias.UpdatedUserID == null || (alias.UpdatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(alias.UpdatedUserID.Value, false)))))
                    {
                        if (alias.AliasID > 0 && (alias.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(alias.UpdatedUserID.Value, alias.CustomerID.Value)))
                            errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                    }
                    //if (alias.LinkOwnerID != null)
                    //{
                    //    if (!ECN_Framework_BusinessLayer.Communicator.LinkOwnerIndex.Exists(alias.LinkOwnerID.Value, alias.CustomerID.Value))
                    //        errorList.Add(new ECNError(Entity, Method, "LinkOwnerID is invalid"));

                    //    if (!KMPlatform.Entity.Client.HasProductFeature(user.CustomerID,"ecn.communicator", "Link Owner"))
                    //        errorList.Add(new ECNError(Entity, Method, "LinkOwnerID is invalid"));
                    //}

                    scope.Complete();
                }
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static DataTable GetLinkAliasDR(int customerID, int layoutID, KMPlatform.Entity.User user)
        {
            DataTable dt = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.LinkAlias.GetLinkAliasDR(customerID, layoutID);
                scope.Complete();
            }

            return dt;
        }
    }
}
