using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Publisher
{
    [Serializable]
    public class Edition
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Edition;

        public static bool Exists(int editionID, int customerID)
        {
            return ECN_Framework_DataLayer.Publisher.Edition.Exists(editionID, customerID);
        }

        public static bool ExistsByName(int publicationID, int editionID, string editionName,  int customerID)
        {
            return ECN_Framework_DataLayer.Publisher.Edition.ExistsByName(publicationID, editionID, editionName, customerID);
        }

        public static List<ECN_Framework_Entities.Publisher.Edition> GetByCustomerID(int customerID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Publisher.Edition> editionList = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                editionList = ECN_Framework_DataLayer.Publisher.Edition.GetByCustomerID(customerID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && !SecurityCheck(editionList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return editionList;
        }

        public static ECN_Framework_Entities.Publisher.Edition GetByPublicationCode(string publicationcode, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Publisher.Edition edition = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                edition = ECN_Framework_DataLayer.Publisher.Edition.GetByPublicationCode(publicationcode);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && !SecurityCheck(edition, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return edition;
        }

        public static ECN_Framework_Entities.Publisher.Edition GetByEditionID(int editionID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Publisher.Edition edition = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                edition = ECN_Framework_DataLayer.Publisher.Edition.GetByEditionID(editionID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && !SecurityCheck(edition, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return edition;
        }

        public static List<ECN_Framework_Entities.Publisher.Edition> GetByPublicationID(int publicationID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Publisher.Edition> editionList = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                editionList = ECN_Framework_DataLayer.Publisher.Edition.GetByPublicationID(publicationID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && !SecurityCheck(editionList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return editionList;
        }

        private static bool SecurityCheck(ECN_Framework_Entities.Publisher.Edition edition, KMPlatform.Entity.User user)
        {
            if (edition != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

                    if (!custExists.Any())
                        return false;
                }
                else if (edition.CustomerID != user.CustomerID)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool SecurityCheck(List<ECN_Framework_Entities.Publisher.Edition> editionList, KMPlatform.Entity.User user)
        {
            if (editionList != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var securityCheck = from e in editionList
                                        join c in custList on e.CustomerID equals c.CustomerID
                                        select new { e.EditionID };

                    if (securityCheck.Count() != editionList.Count)
                        return false;
                }
                else
                {
                    var securityCheck = from e in editionList
                                        where e.CustomerID != user.CustomerID
                                        select new { e.EditionID };

                    if (securityCheck.Any())
                        return false;
                }
            }
            return true;
        }

        public static void Delete(int editionID, KMPlatform.Entity.User user)
        {
            ECN_Framework_DataLayer.Publisher.Edition.Delete(editionID, user.UserID);
        }

        public static void Validate(ECN_Framework_Entities.Publisher.Edition edition)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (!ECN_Framework_BusinessLayer.Publisher.Publication.Exists(edition.PublicationID, edition.CustomerID.Value))
                errorList.Add(new ECNError(Entity, Method, "PublicationID is invalid"));

            if (edition.EditionName.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "Edition Name cannot be empty"));

            if (ExistsByName(edition.PublicationID, edition.EditionID, edition.EditionName, edition.CustomerID.Value))
               errorList.Add(new ECNError(Entity, Method, "Edition Name already exists"));

            if (edition.EditionID == 0)
            {
                if (edition.FileName == string.Empty)
                    errorList.Add(new ECNError(Entity, Method, "Please upload the PDF file."));

                if (!edition.FileName.ToLower().EndsWith(".pdf"))
                    errorList.Add(new ECNError(Entity, Method, "Invalid File Format. Please upload PDF file."));
            }

            if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(edition.CustomerID.Value))
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
          
            if (edition.EditionID <= 0 && (edition.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(edition.CreatedUserID.Value, edition.CustomerID.Value)))
                errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

            if (edition.EditionID > 0 && (edition.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(edition.UpdatedUserID.Value, edition.CustomerID.Value)))
                errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static int Save(ECN_Framework_Entities.Publisher.Edition edition, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
            if (edition.EditionID > 0)
            {
                if (!Exists(edition.EditionID, edition.CustomerID.Value))
                {
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(new ECNError(Entity, Method, "EditionID is invalid"));
                    throw new ECNException(errorList);
                }
            }

            Validate(edition);

            bool update = false;

            if (edition.EditionID > 0)
            {
                update = true;
            }

                string currentstatus = "";

                if (update == true)
                {
                    currentstatus = ECN_Framework_BusinessLayer.Publisher.Edition.GetByEditionID(edition.EditionID, user).Status;
                }

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                edition.EditionID =  ECN_Framework_DataLayer.Publisher.Edition.Save(edition);

                if (update == true)
                {
                    ECN_Framework_Entities.Publisher.EditionHistory editionHistory = null;

                    if (currentstatus != edition.Status)
                    {
                        if (edition.Status == ECN_Framework_Common.Objects.Publisher.Enums.Status.Active.ToString() || edition.Status == ECN_Framework_Common.Objects.Publisher.Enums.Status.Archieve.ToString() || edition.Status == ECN_Framework_Common.Objects.Publisher.Enums.Status.Inactive.ToString())
                        {
                            editionHistory = new ECN_Framework_Entities.Publisher.EditionHistory();

                            editionHistory = ECN_Framework_BusinessLayer.Publisher.EditionHistory.GetByEditionID(edition.EditionID, user);
                            if (editionHistory != null)
                            {
                                editionHistory.EditionHistoryID = editionHistory.EditionHistoryID;
                                editionHistory.DeActivatedDate = System.DateTime.Now;
                                editionHistory.UpdatedUserID = edition.UpdatedUserID;
                                editionHistory.CustomerID = edition.CustomerID;
                                editionHistory.EditionID = edition.EditionID;
                                ECN_Framework_BusinessLayer.Publisher.EditionHistory.Save(editionHistory, user);
                            }
                        }

                        if (edition.Status == ECN_Framework_Common.Objects.Publisher.Enums.Status.Active.ToString() || edition.Status == ECN_Framework_Common.Objects.Publisher.Enums.Status.Archieve.ToString())
                        {
                            editionHistory = new ECN_Framework_Entities.Publisher.EditionHistory();

                            if (edition.Status == ECN_Framework_Common.Objects.Publisher.Enums.Status.Active.ToString())
                            {
                                editionHistory.ActivatedDate = System.DateTime.Now;
                            }
                            else
                            {
                                editionHistory.ArchievedDate = System.DateTime.Now;
                            }

                            editionHistory.CreatedUserID = edition.UpdatedUserID;
                            editionHistory.CustomerID = edition.CustomerID;
                            editionHistory.EditionID = edition.EditionID;
                            ECN_Framework_BusinessLayer.Publisher.EditionHistory.Save(editionHistory, user);
                        }

                    }
                }
                else
                {
                    if (edition.Status == ECN_Framework_Common.Objects.Publisher.Enums.Status.Active.ToString() || edition.Status == ECN_Framework_Common.Objects.Publisher.Enums.Status.Archieve.ToString())
                    {
                        ECN_Framework_Entities.Publisher.EditionHistory editionHistory = new ECN_Framework_Entities.Publisher.EditionHistory();

                        if (edition.Status == ECN_Framework_Common.Objects.Publisher.Enums.Status.Active.ToString())
                        {
                            editionHistory.ActivatedDate = System.DateTime.Now;
                       }
                        else
                        {
                            editionHistory.ArchievedDate = System.DateTime.Now;
                        }

                        editionHistory.CreatedUserID = edition.CreatedUserID;
                        editionHistory.CustomerID = edition.CustomerID;
                        editionHistory.EditionID = edition.EditionID;
                        ECN_Framework_BusinessLayer.Publisher.EditionHistory.Save(editionHistory, user);
                    }
                }

                if (edition.PageList != null)
                {
                    ECN_Framework_BusinessLayer.Publisher.EditionActivityLog.DeleteByEditionID(edition.EditionID, user);

                    ECN_Framework_BusinessLayer.Publisher.Link.DeleteByEditionID(edition.EditionID, user);

                    ECN_Framework_BusinessLayer.Publisher.Page.DeleteByEditionID(edition.EditionID, user);

                    foreach (ECN_Framework_Entities.Publisher.Page page in edition.PageList)
                    {
                        page.EditionID = edition.EditionID;
                        Page.Save(page, user, false);
                    }
                }

                scope.Complete();
            }
            return edition.EditionID;
        }
    }
}
