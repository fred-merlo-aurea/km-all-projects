using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class BlastPlans
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Blast;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.BlastPlans;

        public static bool Exists(int blastPlanID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.BlastPlans.Exists(blastPlanID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static void Validate(ECN_Framework_Entities.Communicator.BlastPlans blastPlans)
        {
            //to do
        }

        public static int Save(ECN_Framework_Entities.Communicator.BlastPlans blastPlans, KMPlatform.Entity.User user)
        {
            Validate(blastPlans);

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(blastPlans, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                blastPlans.BlastPlanID = ECN_Framework_DataLayer.Communicator.BlastPlans.Save(blastPlans);
                scope.Complete();
            }
            return blastPlans.BlastPlanID;
        }

        //public static bool Exists(int campaignID, int customerID)
        //{
        //    return ECN_Framework_DataLayer.Communicator.Campaign.Exists(campaignID, customerID);
        //}

        //public static bool Exists(string campaignName, int customerID)
        //{
        //    return ECN_Framework_DataLayer.Communicator.Campaign.Exists(campaignName, customerID);
        //}

        public static ECN_Framework_Entities.Communicator.BlastPlans GetByBlastPlanID(int blastPlanID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.BlastPlans blastPlans = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                blastPlans = ECN_Framework_DataLayer.Communicator.BlastPlans.GetByBlastPlanID(blastPlanID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(blastPlans, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return blastPlans;
        }

        public static List<ECN_Framework_Entities.Communicator.BlastPlans> GetByCustomerID(int customerID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.BlastPlans> blastPlanList = new List<ECN_Framework_Entities.Communicator.BlastPlans>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                blastPlanList = ECN_Framework_DataLayer.Communicator.BlastPlans.GetByCustomerID(customerID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(blastPlanList, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return blastPlanList;
        }

        public static List<ECN_Framework_Entities.Communicator.BlastPlans> GetByBlastID(int blastID, string eventType, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.BlastPlans> blastPlanList = new List<ECN_Framework_Entities.Communicator.BlastPlans>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                blastPlanList = ECN_Framework_DataLayer.Communicator.BlastPlans.GetByBlastID(blastID, eventType);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(blastPlanList, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return blastPlanList;
        }

        public static void Delete(int blastPlanID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(blastPlanID, user.CustomerID))
            {
                
                //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
                GetByBlastPlanID(blastPlanID, user);

                using (TransactionScope scope = new TransactionScope())
                {
                    ECN_Framework_DataLayer.Communicator.BlastPlans.Delete(blastPlanID, user.CustomerID, user.UserID);
                    scope.Complete();
                }                
            }
            else
            {
                errorList.Add(new ECNError(Entity, Method, "BlastPlans does not exist"));
                throw new ECNException(errorList);
            }
        }
    }
}
