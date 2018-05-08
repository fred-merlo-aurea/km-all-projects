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
    public class Sample
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Samples;

        public static bool Exists(int sampleID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Sample.Exists(sampleID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists_UseAmbientTransaction(int sampleID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope())
            {
                exists = ECN_Framework_DataLayer.Communicator.Sample.Exists(sampleID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static ECN_Framework_Entities.Communicator.Sample GetBySampleID(int sampleID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.Sample sample = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                sample = ECN_Framework_DataLayer.Communicator.Sample.GetBySampleID(sampleID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(sample, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return sample;
        }

        public static ECN_Framework_Entities.Communicator.Sample GetBySampleID_NoAccessCheck(int sampleID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.Sample sample = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                sample = ECN_Framework_DataLayer.Communicator.Sample.GetBySampleID(sampleID);
                scope.Complete();
            }            

            return sample;
        }

        public static ECN_Framework_Entities.Communicator.Sample GetBySampleID_UseAmbientTransaction(int sampleID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.Sample sample = null;
            using (TransactionScope scope = new TransactionScope())
            {
                sample = ECN_Framework_DataLayer.Communicator.Sample.GetBySampleID(sampleID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(sample, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return sample;
        }

        public static ECN_Framework_Entities.Communicator.Sample GetByWinningBlastID(int blastID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.Sample sample = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                sample = ECN_Framework_DataLayer.Communicator.Sample.GetByWinningBlastID(blastID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.View) )
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(sample, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return sample;
        }

        //private static bool SecurityCheck(ECN_Framework_Entities.Communicator.Sample sample, KMPlatform.Entity.User user)
        //{
        //    if (sample != null)
        //    {
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

        //            if (!custExists.Any())
        //                return false;
        //        }
        //        else if (sample.CustomerID.Value != user.CustomerID)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        public static void Validate(ECN_Framework_Entities.Communicator.Sample sample)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(sample.CustomerID.Value))
                    errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
                else
                {
                    if (sample.WinningBlastID != null && (!ECN_Framework_BusinessLayer.Communicator.Blast.Exists(sample.WinningBlastID.Value, sample.CustomerID.Value)))
                        errorList.Add(new ECNError(Entity, Method, "WinningBlastID is invalid"));

                    if (sample.CreatedUserID == null || (sample.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(sample.CreatedUserID.Value, false))))
                    {
                        if (sample.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(sample.CreatedUserID.Value, sample.CustomerID.Value))
                            errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                    }

                    if (sample.SampleID > 0 && (sample.UpdatedUserID == null || (sample.UpdatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(sample.UpdatedUserID.Value, false)))))
                    {
                        if (sample.SampleID > 0 && (sample.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(sample.UpdatedUserID.Value, sample.CustomerID.Value)))
                            errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                    }
                }
                scope.Complete();
            }
            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Validate_NoAccessCheck(ECN_Framework_Entities.Communicator.Sample sample)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(sample.CustomerID.Value))
                    errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
                else
                {
                    if (sample.WinningBlastID != null && (!ECN_Framework_BusinessLayer.Communicator.Blast.Exists(sample.WinningBlastID.Value, sample.CustomerID.Value)))
                        errorList.Add(new ECNError(Entity, Method, "WinningBlastID is invalid"));

                   
                }
                scope.Complete();
            }
            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Save(ECN_Framework_Entities.Communicator.Sample sample, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
            if (sample.SampleID > 0)
            {
                if (!Exists(sample.SampleID, sample.CustomerID.Value))
                {
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(new ECNError(Entity, Method, "SampleID is invalid"));
                    throw new ECNException(errorList);
                }
            }
            Validate(sample);

            if (!KM.Platform.User.HasAccess(user, ServiceCode, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.Edit) )
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(sample, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                sample.SampleID = ECN_Framework_DataLayer.Communicator.Sample.Save(sample);
                scope.Complete();
            }
        }

        public static void Save_NoAccessCheck(ECN_Framework_Entities.Communicator.Sample sample, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
            if (sample.SampleID > 0)
            {
                if (!Exists(sample.SampleID, sample.CustomerID.Value))
                {
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(new ECNError(Entity, Method, "SampleID is invalid"));
                    throw new ECNException(errorList);
                }
            }
            Validate_NoAccessCheck(sample);

           
            using (TransactionScope scope = new TransactionScope())
            {
                sample.SampleID = ECN_Framework_DataLayer.Communicator.Sample.Save(sample);
                scope.Complete();
            }
        }

        public static void Delete(int sampleID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(sampleID, user.CustomerID))
            {
                if (!ECN_Framework_BusinessLayer.Communicator.Blast.ActivePendingOrSentBySample(sampleID, user.CustomerID))
                {
                    //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
                    GetBySampleID(sampleID, user);

                    if (!KM.Platform.User.HasAccess(user, ServiceCode, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.Delete) )
                        throw new ECN_Framework_Common.Objects.SecurityException();

                    using (TransactionScope scope = new TransactionScope())
                    {
                        ECN_Framework_DataLayer.Communicator.Sample.Delete(sampleID, user.CustomerID, user.UserID);
                        scope.Complete();
                    }
                }
                else
                {
                    errorList.Add(new ECNError(Entity, Method, "Sample is used in Blasts"));
                    throw new ECNException(errorList);
                }
            }
            else
            {
                errorList.Add(new ECNError(Entity, Method, "Sample does not exist"));
                throw new ECNException(errorList);
            }

        }

        public static DataTable GetAvailableSamples(KMPlatform.Entity.User user, int CampaignItemID)
        {
            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);
            DataTable dtSample = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtSample = ECN_Framework_DataLayer.Communicator.Sample.GetAvailableSamples(user.CustomerID, CampaignItemID);
                scope.Complete();
            }

            return dtSample;
        }
    }
}
