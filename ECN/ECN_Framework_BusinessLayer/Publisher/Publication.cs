using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Publisher
{
    [Serializable]
    public class Publication
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Publication;

        public static List<ECN_Framework_Entities.Publisher.Publication> GetByCustomerID(int customerID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Publisher.Publication> lPublications = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                lPublications = ECN_Framework_DataLayer.Publisher.Publication.GetByCustomerID(customerID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && user.CustomerID != customerID && !SecurityCheck(lPublications, user))
                throw new ECN_Framework_Common.Objects.SecurityException();            
            
            return lPublications;
        }

        public static ECN_Framework_Entities.Publisher.Publication GetByPublicationID(int PublicationID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Publisher.Publication publication = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                publication = ECN_Framework_DataLayer.Publisher.Publication.GetByPublicationID(PublicationID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && user.CustomerID != publication.CustomerID && !SecurityCheck(publication, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return publication;
        }

        public static bool Exists(int publicationID, string publicationName, int customerID)
        {
            return ECN_Framework_DataLayer.Publisher.Publication.Exists(publicationID, publicationName, customerID);
        }

        public static bool Exists(int publicationID, int customerID)
        {
            return ECN_Framework_DataLayer.Publisher.Publication.Exists(publicationID, customerID);
        }

        public static bool ExistsAlias(int publicationID, string publicationCode)
        {
            return ECN_Framework_DataLayer.Publisher.Publication.ExistsAlias(publicationID, publicationCode);
        }

        public static void Validate(ECN_Framework_Entities.Publisher.Publication publication)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(publication.PublicationID, publication.PublicationName, publication.CustomerID))
            {
                errorList.Add(new ECNError(Entity, Method, "Publication already exists"));
            }
            else if (publication.PublicationID == 0)
            {
                if (ExistsAlias(publication.PublicationID, publication.PublicationCode))
                {
                    errorList.Add(new ECNError(Entity, Method, "Publication Alias already exists"));
                }
            }
            //if (!publication.LogoURL.ToLower().EndsWith(".gif") && !publication.LogoURL.ToLower().EndsWith(".jpg"))
            //    errorList.Add(new ECNError(Entity, Method, "Invalid Image format. Select Only gif or jpg image."));

            if (publication.PublicationID <= 0 && (publication.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(publication.CreatedUserID.Value, publication.CustomerID)))
                errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

            if (publication.PublicationID > 0 && (publication.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(publication.UpdatedUserID.Value, publication.CustomerID)))
                errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Save(ref ECN_Framework_Entities.Publisher.Publication publication,  KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
            if (publication.PublicationID > 0)
            {
                if (!Exists(publication.PublicationID, publication.CustomerID))
                {
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(new ECNError(Entity, Method, "PublicationID is invalid"));
                    throw new ECNException(errorList);
                }
            }

            Validate(publication);

            //Add/update publication group in groups
            ECN_Framework_Entities.Communicator.Group pubgroup = new ECN_Framework_Entities.Communicator.Group();

            pubgroup.GroupID = publication.GroupID.Value;
            pubgroup.FolderID = 0;

            pubgroup.CustomerID = publication.CustomerID;
            pubgroup.FolderID = 0;
            pubgroup.GroupName = publication.PublicationName + " Subscribers";
            pubgroup.GroupDescription = publication.PublicationName + " Subscribers";
            pubgroup.OwnerTypeCode = "customer";
            pubgroup.MasterSupression = null;
            pubgroup.PublicFolder = 0;
            pubgroup.OptinHTML = "";
            pubgroup.OptinFields = "EmailAddress|SubscribeTypeCode|FormatTypeCode|GroupID|CustomerID|";
            pubgroup.AllowUDFHistory ="N";
            pubgroup.IsSeedList = false;
            if (pubgroup.GroupID > 0)
            {
                pubgroup.UpdatedUserID = user.UserID;

                ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(publication.GroupID.Value, user);
                if (group != null)
                    pubgroup.CreatedUserID = group.CreatedUserID;

                if (pubgroup.CreatedUserID == null)
                    pubgroup.CreatedUserID = user.UserID;
            }
            else
                pubgroup.CreatedUserID = user.UserID;

            ECN_Framework_BusinessLayer.Communicator.Group.Save(pubgroup, user);
        
            publication.GroupID = pubgroup.GroupID;
             
            publication.PublicationID = ECN_Framework_DataLayer.Publisher.Publication.Save(publication);
        }

        public static void Delete(int publicationID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (!ECN_Framework_BusinessLayer.Publisher.Edition.Exists(publicationID, user.CustomerID))
            {
                ECN_Framework_Entities.Publisher.Publication pubToDelete = ECN_Framework_BusinessLayer.Publisher.Publication.GetByPublicationID(publicationID, user);
                ECN_Framework_DataLayer.Publisher.Publication.Delete(publicationID, user.UserID);
                ECN_Framework_Entities.Communicator.Group groupToDelete = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(pubToDelete.GroupID.Value, user);
                if (groupToDelete != null && groupToDelete.GroupID > 0)
                {
                    string tempName = "_deleted(" + DateTime.Now.ToShortDateString() + ")";
                    if ((groupToDelete.GroupName + tempName).Length > 50)
                    {
                        //shortening the groupname so we can append the _deleted(Date) string, 49 - temp.length because of the 0 based index, groupname cant be over 50 chars
                        groupToDelete.GroupName = groupToDelete.GroupName.Remove(49 - tempName.Length);
                        groupToDelete.GroupName += tempName;
                    }
                    else
                        groupToDelete.GroupName += tempName;

                    ECN_Framework_BusinessLayer.Communicator.Group.Save(groupToDelete, user);
                }
            }
            else
            {
                errorList.Add(new ECNError(Entity, Method, "Cannot Delete Publication. Delete the Editions before deleting the Publication"));
                throw new ECNException(errorList);
            }
        }

        private static bool SecurityCheck(ECN_Framework_Entities.Publisher.Publication publication, KMPlatform.Entity.User user)
        {
            if (publication != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

                    if (!custExists.Any())
                        return false;
                }
                else if (publication.CustomerID != user.CustomerID)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool SecurityCheck(List<ECN_Framework_Entities.Publisher.Publication> lPublications, KMPlatform.Entity.User user)
        {
            if (lPublications != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var securityCheck = from e in lPublications
                                        join c in custList on e.CustomerID equals c.CustomerID
                                        select new { e.PublicationID };

                    if (securityCheck.Count() != lPublications.Count)
                        return false;
                }
                else
                {
                    var securityCheck = from e in lPublications
                                        where e.CustomerID != user.CustomerID
                                        select new { e.PublicationID };

                    if (securityCheck.Any())
                        return false;
                }
            }
            return true;
        }
    }
}
