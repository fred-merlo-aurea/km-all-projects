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
    public class CampaignItemOptOutGroup
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.CampaignItemOptOutGroup;

        public static List<ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup> GetByCampaignItemID(int CampaignItemID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup> CampaignItemOptOutGroupList = new List<ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                CampaignItemOptOutGroupList = ECN_Framework_DataLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID(CampaignItemID);
                scope.Complete();
            }
            return CampaignItemOptOutGroupList;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup> GetByCampaignItemID_UseAmbientTransaction(int CampaignItemID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup> CampaignItemOptOutGroupList = new List<ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup>();
            using (TransactionScope scope = new TransactionScope())
            {
                CampaignItemOptOutGroupList = ECN_Framework_DataLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID(CampaignItemID);
                scope.Complete();
            }
            return CampaignItemOptOutGroupList;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static List<ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup> GetByCampaignItemID_NoAccessCheck(int CampaignItemID)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup> CampaignItemOptOutGroupList = new List<ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                CampaignItemOptOutGroupList = ECN_Framework_DataLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID(CampaignItemID);
                scope.Complete();
            }
            return CampaignItemOptOutGroupList;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup> GetByCampaignItemID_NoAccessCheck_UseAmbientTransaction(int CampaignItemID)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup> CampaignItemOptOutGroupList = new List<ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup>();
            using (TransactionScope scope = new TransactionScope())
            {
                CampaignItemOptOutGroupList = ECN_Framework_DataLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID(CampaignItemID);
                scope.Complete();
            }
            return CampaignItemOptOutGroupList;
        }

        public static void Save(ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup CampaignItemOptOutGroup, KMPlatform.Entity.User user)
        {
            List<ECNError> errorList = new List<ECNError>();
            Validate(CampaignItemOptOutGroup);

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItemOptOutGroup.Save(CampaignItemOptOutGroup);
                scope.Complete();
            }
        }

        public static void Validate(ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup CampaignItemOptOutGroup)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (CampaignItemOptOutGroup.CustomerID == null)
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            }
            else
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(CampaignItemOptOutGroup.CustomerID.Value))
                    {
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
                    }
                    if (CampaignItemOptOutGroup.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(CampaignItemOptOutGroup.CreatedUserID.Value, CampaignItemOptOutGroup.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                    scope.Complete();
                }
            }

            if (CampaignItemOptOutGroup.CampaignItemID == null)
            {
                errorList.Add(new ECNError(Entity, Method, "CampaignItemID is invalid"));
            }

            if (CampaignItemOptOutGroup.GroupID == null)
            {
                errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));
            }
            else
            {
                if (CampaignItemOptOutGroup.GroupID <= 0 || (!ECN_Framework_BusinessLayer.Communicator.Group.Exists(CampaignItemOptOutGroup.GroupID.Value, CampaignItemOptOutGroup.CustomerID.Value)))
                    errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Delete(int CampaignItemID, int CustomerID, KMPlatform.Entity.User user)
        {
            List<ECNError> errorList = new List<ECNError>();
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItemOptOutGroup.Delete(CampaignItemID, user.CustomerID, user.UserID);
                scope.Complete();
            }
        }

        public static void Delete(int CampaingItemOptOutID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItemOptOutGroup.Delete(CampaingItemOptOutID, user.UserID);
                scope.Complete();
            }
        }
    }
}
