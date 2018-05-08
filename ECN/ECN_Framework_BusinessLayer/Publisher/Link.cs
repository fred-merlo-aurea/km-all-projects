using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Publisher
{
    [Serializable]
    public class Link
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Link;

        public static bool Exists(int linkID, int customerID)
        {
            return ECN_Framework_DataLayer.Publisher.Link.Exists(linkID, customerID);
        }

        public static List<ECN_Framework_Entities.Publisher.Link> GetByPageID(int pageID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Publisher.Link> linkList = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                linkList = ECN_Framework_DataLayer.Publisher.Link.GetByPageID(pageID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && !SecurityCheck(linkList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return linkList;
        }

        public static ECN_Framework_Entities.Publisher.Link GetByLinkID(int LinkID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Publisher.Link link = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                link = ECN_Framework_DataLayer.Publisher.Link.GetByLinkID(LinkID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && user.CustomerID != link.CustomerID && !SecurityCheck(link, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return link;
        }

        private static bool SecurityCheck(ECN_Framework_Entities.Publisher.Link link, KMPlatform.Entity.User user)
        {
            if (link != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

                    if (!custExists.Any())
                        return false;
                }
                else if (link.CustomerID != user.CustomerID)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool SecurityCheck(List<ECN_Framework_Entities.Publisher.Link> linkList, KMPlatform.Entity.User user)
        {
            if (linkList != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var securityCheck = from l in linkList
                                        join c in custList on l.CustomerID equals c.CustomerID
                                        select new { l.LinkID };

                    if (securityCheck.Count() != linkList.Count)
                        return false;
                }
                else
                {
                    var securityCheck = from l in linkList
                                        where l.CustomerID != user.CustomerID
                                        select new { l.LinkID };

                    if (securityCheck.Any())
                        return false;
                }
            }
            return true;
        }

        public static void Delete(int linkID, KMPlatform.Entity.User user)
        {
            ECN_Framework_DataLayer.Publisher.Link.Delete(linkID, user.UserID);
        }

        public static void DeleteByEditionID(int editionID, KMPlatform.Entity.User user)
        {
            ECN_Framework_DataLayer.Publisher.Link.DeleteByEditionID(editionID, user.UserID);
        }

        public static void Validate(ECN_Framework_Entities.Publisher.Link link, bool validate)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (validate)
                {
                    if (!ECN_Framework_BusinessLayer.Publisher.Page.Exists(link.PageID, link.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "PageID is invalid"));

                    if (link.LinkID <= 0 && (link.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(link.CreatedUserID.Value, link.CustomerID.Value)))
                        errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

                    if (link.LinkID > 0 && (link.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(link.UpdatedUserID.Value, link.CustomerID.Value)))
                        errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                }

                if (string.IsNullOrWhiteSpace(link.LinkURL))
                    errorList.Add(new ECNError(Entity, Method, "LinkURL is missing"));
                scope.Complete();
            }
            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Save(ECN_Framework_Entities.Publisher.Link link, KMPlatform.Entity.User user, bool validate)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;

            if (link.LinkID > 0)
            {
                if (!Exists(link.LinkID, link.CustomerID.Value))
                {
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(new ECNError(Entity, Method, "LinkID is invalid"));
                    throw new ECNException(errorList);
                }
            }

            Validate(link, validate);

            ECN_Framework_DataLayer.Publisher.Link.Save(link);

            return;
        }

    }
}
