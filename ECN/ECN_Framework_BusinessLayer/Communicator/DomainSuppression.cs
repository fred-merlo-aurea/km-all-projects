using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class DomainSuppression
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.DomainSuppression;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.DomainSuppression;

        public static bool Exists(int? customerID, int? baseChannelID, int domainSuppressionID, string domain)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.DomainSuppression.Exists(customerID, baseChannelID, domainSuppressionID, domain);
                scope.Complete();
            }
            return exists;
        }

        public static void Delete(int domainSuppressionID, KMPlatform.Entity.User user)
        {
            //this checks security
            GetByDomainSuppressionID(domainSuppressionID, user);
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.DomainSuppression.Delete(domainSuppressionID, user.UserID);
                scope.Complete();
            }
        }

        public static List<ECN_Framework_Entities.Communicator.DomainSuppression> GetByDomain(string searchString, int? customerID, int? baseChannelID, KMPlatform.Entity.User user)
        {
            if (!KM.Platform.User.IsSystemAdministrator(user) && !KM.Platform.User.IsChannelAdministrator(user))
            {
                baseChannelID = null;

                if (customerID.Value != user.CustomerID)
                    throw new ECN_Framework_Common.Objects.SecurityException();

            }
            else if (!KM.Platform.User.IsSystemAdministrator(user) || KM.Platform.User.IsChannelAdministrator(user))
            {
                int userbasechannelID = 0;
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    userbasechannelID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false).BaseChannelID.Value;
                    scope.Complete();
                }

                if (baseChannelID != userbasechannelID)
                    throw new SecurityException();
            }

            List<ECN_Framework_Entities.Communicator.DomainSuppression> suppressionList = new List<ECN_Framework_Entities.Communicator.DomainSuppression>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                suppressionList = ECN_Framework_DataLayer.Communicator.DomainSuppression.GetByDomain(customerID, baseChannelID, searchString);

                scope.Complete();
            }

            return suppressionList;
        }

        public static ECN_Framework_Entities.Communicator.DomainSuppression GetByDomainSuppressionID(int domainSuppressionID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.DomainSuppression suppression = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                suppression = ECN_Framework_DataLayer.Communicator.DomainSuppression.GetByDomainSuppressionID(domainSuppressionID);
                scope.Complete();
            }

            return suppression;
        }

        public static void Validate(ECN_Framework_Entities.Communicator.DomainSuppression suppression)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (suppression.Domain.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "Domain is invalid"));

            if (suppression.CustomerID == null && suppression.BaseChannelID == null)
                errorList.Add(new ECNError(Entity, Method, "Must have CustomerID or BaseChannelID"));
            else
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (suppression.CustomerID != null)
                    {
                        if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(suppression.CustomerID.Value))
                            errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
                        if (suppression.CreatedUserID == null || (suppression.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(suppression.CreatedUserID.Value, false))))
                        {
                            if (suppression.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(suppression.CreatedUserID.Value, suppression.CustomerID.Value))
                                errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                        }
                        if (suppression.DomainSuppressionID > 0 && (suppression.UpdatedUserID == null || (suppression.UpdatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(suppression.UpdatedUserID.Value, false)))))
                        {
                            if (suppression.DomainSuppressionID > 0 && (suppression.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(suppression.UpdatedUserID.Value, suppression.CustomerID.Value)))
                                errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                        }
                    }

                    if (suppression.BaseChannelID != null)
                    {
                        if (!ECN_Framework_BusinessLayer.Accounts.BaseChannel.Exists(suppression.BaseChannelID.Value))
                            errorList.Add(new ECNError(Entity, Method, "BaseChannelID is invalid"));
                        if (suppression.CreatedUserID == null || (suppression.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(suppression.CreatedUserID.Value, false))))
                        {
                            if (suppression.CreatedUserID == null || !KMPlatform.BusinessLogic.User.ExistsByBaseChannelID(suppression.CreatedUserID.Value, suppression.BaseChannelID.Value))
                                errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                        }
                        if (suppression.DomainSuppressionID > 0 && (suppression.UpdatedUserID == null || (suppression.UpdatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(suppression.UpdatedUserID.Value, false)))))
                        {
                            if (suppression.DomainSuppressionID > 0 && (suppression.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.ExistsByBaseChannelID(suppression.UpdatedUserID.Value, suppression.BaseChannelID.Value)))
                                errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                        }
                    }
                    scope.Complete();
                }
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Save(ECN_Framework_Entities.Communicator.DomainSuppression suppression, KMPlatform.Entity.User user)
        {
            if (suppression.UpdatedUserID != null && suppression.CreatedUserID == null)
                suppression.CreatedUserID = suppression.UpdatedUserID;

            Validate(suppression);
            if (suppression.CustomerID != null)
            {
                if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(suppression, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit, user))
                    throw new ECN_Framework_Common.Objects.SecurityException();
            }
            else
            {
                if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByBaseChannel(suppression, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit, user))
                    throw new ECN_Framework_Common.Objects.SecurityException();
            }
            using (TransactionScope scope = new TransactionScope())
            {
                suppression.DomainSuppressionID = ECN_Framework_DataLayer.Communicator.DomainSuppression.Save(suppression);
                scope.Complete();
            }
        }
    }
}
