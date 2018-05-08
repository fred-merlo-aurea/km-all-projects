using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class MarketingAutomation
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.MARKETINGAUTOMATION;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.MarketingAutomation;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.MarketingAutomation;

        public static bool HasPermission(KMPlatform.Enums.Access AccessCode, KMPlatform.Entity.User user)
        {
            if (AccessCode == KMPlatform.Enums.Access.View)
            {
                //if (KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Content.ToString()) ||
                //    KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Create_Content.ToString()))
                if (KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                    return true;
            }
            else if (AccessCode == KMPlatform.Enums.Access.Edit)
            {
                //if (KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Create_Content.ToString()))
                if (KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                    return true;
            }
            else if (AccessCode == KMPlatform.Enums.Access.Delete)
            {
                if (KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                    return true;
            }
            else if(AccessCode == KMPlatform.Enums.Access.FullAccess)
            {
                if (KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.FullAccess))
                    return true;
            }
            return false;
        }

        public static bool Exists(int baseChannelID, string Name, int MAID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.MarketingAutomation.Exists(baseChannelID, Name, MAID);
                scope.Complete();
            }

            return exists;
        }


        public static int Save(ECN_Framework_Entities.Communicator.MarketingAutomation MA, KMPlatform.Entity.User user)
        {
            if (!HasPermission(KMPlatform.Enums.Access.FullAccess, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            int retID = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.Communicator.MarketingAutomation.Save(MA);
                scope.Complete();
            }

            return retID;
        }

        public static List<ECN_Framework_Entities.Communicator.MarketingAutomation> SelectByCustomerID(int CustomerID, KMPlatform.Entity.User user)
        {
            if (!HasPermission(KMPlatform.Enums.Access.FullAccess, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            List<ECN_Framework_Entities.Communicator.MarketingAutomation> retList = new List<ECN_Framework_Entities.Communicator.MarketingAutomation>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.MarketingAutomation.SelectByCustomerID(CustomerID);
                scope.Complete();
            }

            return retList;
        }

        public static List<ECN_Framework_Entities.Communicator.MarketingAutomation> SelectByBaseChannelID(int BaseChannelID, KMPlatform.Entity.User user)
        {
            if (!HasPermission(KMPlatform.Enums.Access.FullAccess, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            List<ECN_Framework_Entities.Communicator.MarketingAutomation> retList = new List<ECN_Framework_Entities.Communicator.MarketingAutomation>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.MarketingAutomation.SelectByBaseChannelID(BaseChannelID);
                scope.Complete();
            }

            return retList;
        }
        public static DataSet GetAllMarketingAutomationsbySearch(int BaseChannelID, KMPlatform.Entity.User user, string AutomationName,string State,string SearchCriteria,int currentPage, int pageSize, string sortDirection, string sortColumn)
        {
            if (!HasPermission(KMPlatform.Enums.Access.FullAccess, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            DataSet dsMA = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dsMA = ECN_Framework_DataLayer.Communicator.MarketingAutomation.GetAllMarketingAutomationsbySearch(BaseChannelID,AutomationName,State, SearchCriteria, currentPage, pageSize, sortDirection, sortColumn);
                scope.Complete();
            }

            return dsMA;
        }

        public static ECN_Framework_Entities.Communicator.MarketingAutomation GetByMarketingAutomationID(int MAID, bool getChildren, KMPlatform.Entity.User user)
        {
            if (!HasPermission(KMPlatform.Enums.Access.FullAccess, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.MarketingAutomation retItem = new ECN_Framework_Entities.Communicator.MarketingAutomation();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retItem = ECN_Framework_DataLayer.Communicator.MarketingAutomation.GetByMarketingAutomationID(MAID);
                scope.Complete();
            }

            if (getChildren && retItem != null && retItem.MarketingAutomationID > 0)
            {
                retItem.Controls = ECN_Framework_BusinessLayer.Communicator.MAControl.GetByMarketingAutomationID(MAID);
                retItem.Connectors = ECN_Framework_BusinessLayer.Communicator.MAConnector.GetByMarketingAutomationID(MAID);
            }

            return retItem;
        }

        public static void Delete(ECN_Framework_Entities.Communicator.MarketingAutomation ma, KMPlatform.Entity.User user)
        {
            if (!HasPermission(KMPlatform.Enums.Access.FullAccess, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (ValidateDelete(ma))
            {
                //Do the delete

                using (TransactionScope scope = new TransactionScope())
                {
                    ma.IsDeleted = true;
                    ma.UpdatedUserID = user.UserID;
                    ECN_Framework_DataLayer.Communicator.MarketingAutomation.Save(ma);

                    //Delete all controls


                    //Delete all connectors

                    scope.Complete();
                }

            }
            else
            {

            }


        }

        private static bool ValidateDelete(ECN_Framework_Entities.Communicator.MarketingAutomation ma)
        {
            //Need to put validation code here
            return true;
        }

        public static List<ECN_Framework_Entities.Communicator.MarketingAutomation> CheckIfControlExists(int ECNID, ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType controlType)
        {
            List<ECN_Framework_Entities.Communicator.MarketingAutomation> retBool = new List<ECN_Framework_Entities.Communicator.MarketingAutomation>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retBool = ECN_Framework_DataLayer.Communicator.MarketingAutomation.CheckIfControlExists(ECNID, controlType);
                scope.Complete();
            }
            return retBool;
        }

    }
}
