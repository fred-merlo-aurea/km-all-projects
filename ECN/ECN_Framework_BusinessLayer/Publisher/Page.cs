using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECN_Framework_Common.Objects;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Publisher
{
    [Serializable]
    public class Page
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Page;

        public static bool Exists(int pageID, int customerID)
        {
            return ECN_Framework_DataLayer.Publisher.Page.Exists(pageID, customerID);
        }

        public static List<ECN_Framework_Entities.Publisher.Page> GetLinks(int editionID, string pageNo, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Publisher.Page> lPage = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                lPage = ECN_Framework_DataLayer.Publisher.Page.GetLinks(editionID, pageNo);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && !SecurityCheck(lPage, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return lPage;
        }

        public static List<ECN_Framework_Entities.Publisher.Page> GetByEditionID(int editionID, string pageNo, KMPlatform.Entity.User user, bool getChildren)
        {
            List<ECN_Framework_Entities.Publisher.Page> lPage = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                lPage = ECN_Framework_DataLayer.Publisher.Page.GetByEditionIDPageNo(editionID, pageNo);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && !SecurityCheck(lPage, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (lPage.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Publisher.Page page in lPage)
                {
                    page.LinkList = ECN_Framework_BusinessLayer.Publisher.Link.GetByPageID(page.PageID, user);
                }
            }

            return lPage;
        }

        public static List<ECN_Framework_Entities.Publisher.Page> GetByEditionID(int editionID,KMPlatform.Entity.User user, bool getChildren)
        {
            List<ECN_Framework_Entities.Publisher.Page>  lPage = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                lPage = ECN_Framework_DataLayer.Publisher.Page.GetByEditionID(editionID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && !SecurityCheck(lPage, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (lPage.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Publisher.Page page in lPage)
                {
                    page.LinkList = ECN_Framework_BusinessLayer.Publisher.Link.GetByPageID(page.PageID, user);
                }
            }

            return lPage;
        }

        private static bool SecurityCheck(ECN_Framework_Entities.Publisher.Page page, KMPlatform.Entity.User user)
        {
            if (page != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

                    if (!custExists.Any())
                        return false;
                }
                else if (page.CustomerID != user.CustomerID)
                {
                    return false;
                }
            }
            return true;
        }


        private static bool SecurityCheck(List<ECN_Framework_Entities.Publisher.Page> lPage, KMPlatform.Entity.User user)
        {
            if (lPage != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var securityCheck = from p in lPage
                                        join c in custList on p.CustomerID equals c.CustomerID
                                        select new { p.PageID };

                    if (securityCheck.Count() != lPage.Count)
                        return false;
                }
                else
                {
                    var securityCheck = from p in lPage
                                        where p.CustomerID != user.CustomerID
                                        select new { p.PageID };

                    if (securityCheck.Any())
                        return false;
                }
            }
            return true;
        }

        public static void DeleteByEditionID(int editionID, KMPlatform.Entity.User user)
        {
            ECN_Framework_DataLayer.Publisher.Page.DeleteByEditionID(editionID, user.UserID);
        }

        public static void Validate(ECN_Framework_Entities.Publisher.Page page)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                    if (!ECN_Framework_BusinessLayer.Publisher.Edition.Exists(page.EditionID, page.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "EditionID is invalid"));

                if (page.PageID <= 0 && (page.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(page.CreatedUserID.Value, page.CustomerID.Value)))
                    errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

                if (page.PageID > 0 && (page.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(page.UpdatedUserID.Value, page.CustomerID.Value)))
                    errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

                scope.Complete();
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Save(ECN_Framework_Entities.Publisher.Page page, KMPlatform.Entity.User user, bool validate)
        {
            try
            {
                ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
                if (page.PageID > 0)
                {
                    if (!Exists(page.PageID, page.CustomerID.Value))
                    {
                        List<ECNError> errorList = new List<ECNError>();
                        errorList.Add(new ECNError(Entity, Method, "PageID is invalid"));
                        throw new ECNException(errorList);
                    }
                }
                if (validate)
                    Validate(page);

                using (TransactionScope scope = new TransactionScope())
                {
                    ECN_Framework_DataLayer.Publisher.Page.Save(page);

                    if (page.LinkList != null)
                    {
                        foreach (ECN_Framework_Entities.Publisher.Link link in page.LinkList)
                        {
                            link.PageID = page.PageID;
                            Link.Save(link, user, false);
                        }
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return;

        }

    }
}
