using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class BlastEnvelope
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Blast;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.BlastEnvelope;

        public static bool HasPermission(KMPlatform.Enums.Access AccessCode, KMPlatform.Entity.User user)
        {
            if (AccessCode == KMPlatform.Enums.Access.View)
            {
                //if (KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Campaigns.ToString()) ||
                //    KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Create_Regular_Blast.ToString()))
                if (KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                    return true;
            }
            else if (AccessCode == KMPlatform.Enums.Access.Edit)
            {
                //if (KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Create_Regular_Blast.ToString()))
                if (KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                    return true;
            }
            return false;
        }

        public static bool Exists(int blastEnvelopeID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.BlastEnvelope.Exists(blastEnvelopeID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static List<ECN_Framework_Entities.Communicator.BlastEnvelope> GetByCustomerID(int customerID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.BlastEnvelope> envelopeList = new List<ECN_Framework_Entities.Communicator.BlastEnvelope>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                envelopeList = ECN_Framework_DataLayer.Communicator.BlastEnvelope.GetByCustomerID(customerID);
                scope.Complete();
            }

            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(envelopeList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return envelopeList;
        }

        public static List<ECN_Framework_Entities.Communicator.BlastEnvelope> GetByCustomerID_NoAccessCheck(int customerID)
        {
            List<ECN_Framework_Entities.Communicator.BlastEnvelope> envelopeList = new List<ECN_Framework_Entities.Communicator.BlastEnvelope>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                envelopeList = ECN_Framework_DataLayer.Communicator.BlastEnvelope.GetByCustomerID(customerID);
                scope.Complete();
            }

            return envelopeList;
        }


        public static ECN_Framework_Entities.Communicator.BlastEnvelope GetByBlastEnvelopeID(int blastEnvelopeID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.BlastEnvelope blastEnvelope = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                blastEnvelope = ECN_Framework_DataLayer.Communicator.BlastEnvelope.GetByBlastEnvelopeID(blastEnvelopeID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(blastEnvelope, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return blastEnvelope;
        }

        public static void Delete(int BlastEnvelopeID, int customerID, KMPlatform.Entity.User loggingUser)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.BlastEnvelope.Delete(BlastEnvelopeID, customerID, loggingUser.UserID);
                scope.Complete();
            }
        }

        public static void Validate(ECN_Framework_Entities.Communicator.BlastEnvelope blastEnvelope, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (blastEnvelope.BlastEnvelopeID > 0)
            {
                if (!Exists(blastEnvelope.BlastEnvelopeID, blastEnvelope.CustomerID.Value))
                {
                    errorList.Add(new ECNError(Entity, Method, "BlastEnvelopeID is invalid"));
                }
            }

            if (blastEnvelope.CustomerID == null)
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            }
            else
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(blastEnvelope.CustomerID.Value))
                    {
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
                    }
                    if (blastEnvelope.BlastEnvelopeID <= 0 && (blastEnvelope.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(blastEnvelope.CreatedUserID.Value, blastEnvelope.CustomerID.Value)))
                        errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                    scope.Complete();
                }
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static int Save(ECN_Framework_Entities.Communicator.BlastEnvelope blastEnvelope, KMPlatform.Entity.User user)
        {
            Validate(blastEnvelope, user);

            using (TransactionScope scope = new TransactionScope())
            {
                blastEnvelope.BlastEnvelopeID = ECN_Framework_DataLayer.Communicator.BlastEnvelope.Save(blastEnvelope);
                scope.Complete();
            }
            return blastEnvelope.BlastEnvelopeID;
        }
    }
}
