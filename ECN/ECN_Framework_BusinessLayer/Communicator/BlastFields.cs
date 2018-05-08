using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class BlastFields
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Blast;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.BlastFields;

        public static bool Exists(int blastID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.BlastFields.Exists(blastID, customerID);
                scope.Complete();
            }
            return exists;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static ECN_Framework_Entities.Communicator.BlastFields GetByBlastID_NoAccessCheck(int blastID)
        {
            ECN_Framework_Entities.Communicator.BlastFields blastFields = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                blastFields = ECN_Framework_DataLayer.Communicator.BlastFields.GetByBlastID(blastID);
                scope.Complete();
            }

            return blastFields;
        }

        public static ECN_Framework_Entities.Communicator.BlastFields GetByBlastID_NoAccessCheck_UseAmbientTransaction(int blastID)
        {
            ECN_Framework_Entities.Communicator.BlastFields blastFields = null;
            using (TransactionScope scope = new TransactionScope())
            {
                blastFields = ECN_Framework_DataLayer.Communicator.BlastFields.GetByBlastID(blastID);
                scope.Complete();
            }

            return blastFields;
        }

        public static ECN_Framework_Entities.Communicator.BlastFields GetByBlastID(int blastID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.BlastFields blastFields = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                blastFields = ECN_Framework_DataLayer.Communicator.BlastFields.GetByBlastID(blastID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(blastFields, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();
            return blastFields;
        }

        public static ECN_Framework_Entities.Communicator.BlastFields GetByBlastID_UseAmbientTransaction(int blastID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.BlastFields blastFields = null;
            using (TransactionScope scope = new TransactionScope())
            {
                blastFields = ECN_Framework_DataLayer.Communicator.BlastFields.GetByBlastID(blastID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(blastFields, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();
            return blastFields;
        }

        public static void Save(ECN_Framework_Entities.Communicator.BlastFields blastFields, KMPlatform.Entity.User user)
        {
            List<ECNError> errorList = new List<ECNError>();
            Validate(blastFields);

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(blastFields, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.BlastFields.Save(blastFields);
                scope.Complete();
            }
        }

        public static void Save_NoAccessCheck(ECN_Framework_Entities.Communicator.BlastFields blastFields, KMPlatform.Entity.User user)
        {
            List<ECNError> errorList = new List<ECNError>();
            Validate_NoAccessCheck(blastFields);

            
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.BlastFields.Save(blastFields);
                scope.Complete();
            }
        }

        public static void Validate(ECN_Framework_Entities.Communicator.BlastFields blastFields)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (blastFields.CustomerID == null)
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));              
            }
            else
            {   
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(blastFields.CustomerID.Value))
                    {
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
                    }
                    if (blastFields.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(blastFields.CreatedUserID.Value, blastFields.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                    scope.Complete();
                }
            }

            if (blastFields.BlastID <= 0)
            {
                errorList.Add(new ECNError(Entity, Method, "BlastID is invalid"));
            }
            else if (blastFields.CustomerID != null)
            {
                if (blastFields.BlastID <= 0 || (!ECN_Framework_BusinessLayer.Communicator.Blast.Exists(blastFields.BlastID, blastFields.CustomerID.Value)))
                    errorList.Add(new ECNError(Entity, Method, "BlastID is invalid"));
            }

            if (blastFields.Field1.Trim().Length == 0 && blastFields.Field2.Trim().Length == 0 && blastFields.Field3.Trim().Length == 0 && blastFields.Field4.Trim().Length == 0 && blastFields.Field5.Trim().Length == 0)
                errorList.Add(new ECNError(Entity, Method, "Must contain at least one BlastField"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Validate_NoAccessCheck(ECN_Framework_Entities.Communicator.BlastFields blastFields)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (blastFields.CustomerID == null)
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            }
            else
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(blastFields.CustomerID.Value))
                    {
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
                    }
                    
                    scope.Complete();
                }
            }

            if (blastFields.BlastID <= 0)
            {
                errorList.Add(new ECNError(Entity, Method, "BlastID is invalid"));
            }
            else if (blastFields.CustomerID != null)
            {
                if (blastFields.BlastID <= 0 || (!ECN_Framework_BusinessLayer.Communicator.Blast.Exists(blastFields.BlastID, blastFields.CustomerID.Value)))
                    errorList.Add(new ECNError(Entity, Method, "BlastID is invalid"));
            }

            if (blastFields.Field1.Trim().Length == 0 && blastFields.Field2.Trim().Length == 0 && blastFields.Field3.Trim().Length == 0 && blastFields.Field4.Trim().Length == 0 && blastFields.Field5.Trim().Length == 0)
                errorList.Add(new ECNError(Entity, Method, "Must contain at least one BlastField"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Delete(int blastID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            ECN_Framework_Entities.Communicator.BlastFields blastFields = GetByBlastID(blastID, user);
            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(blastFields, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Delete, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (ECN_Framework_BusinessLayer.Communicator.Blast.ActiveOrSent(blastID, user.CustomerID))
            {
                errorList.Add(new ECNError(Entity, Method, "Cannot delete BlastFields as it is active or sent"));
                throw new ECNException(errorList);
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ECN_Framework_DataLayer.Communicator.BlastFields.Delete(blastID, user.CustomerID, user.UserID);
                    scope.Complete();
                }
            }
        }
    }
}
