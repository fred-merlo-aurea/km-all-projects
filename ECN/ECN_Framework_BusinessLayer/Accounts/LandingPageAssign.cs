using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class LandingPageAssign
    {
        public static List<ECN_Framework_Entities.Accounts.LandingPageAssign> GetByBaseChannelID(int baseChannelID)
        {
            List<ECN_Framework_Entities.Accounts.LandingPageAssign> itemList = new List<ECN_Framework_Entities.Accounts.LandingPageAssign>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.Accounts.LandingPageAssign.GetByBaseChannelID(baseChannelID);
                scope.Complete();
            }

            return itemList;
        }

        public static ECN_Framework_Entities.Accounts.LandingPageAssign GetByBaseChannelID(int baseChannelID, int LPID)
        {
            ECN_Framework_Entities.Accounts.LandingPageAssign item = new ECN_Framework_Entities.Accounts.LandingPageAssign();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Accounts.LandingPageAssign.GetByBaseChannelID(baseChannelID, LPID);
                scope.Complete();
            }

            return item;
        }


        public static List<ECN_Framework_Entities.Accounts.LandingPageAssign> GetByCustomerID(int customerID)
        {
            List<ECN_Framework_Entities.Accounts.LandingPageAssign> itemList = new List<ECN_Framework_Entities.Accounts.LandingPageAssign>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.Accounts.LandingPageAssign.GetByCustomerID(customerID);
                scope.Complete();
            }

            return itemList;
        }

        public static ECN_Framework_Entities.Accounts.LandingPageAssign GetByCustomerID(int customerID, int LPID)
        {
            ECN_Framework_Entities.Accounts.LandingPageAssign item = new ECN_Framework_Entities.Accounts.LandingPageAssign();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Accounts.LandingPageAssign.GetByCustomerID(customerID, LPID);
                scope.Complete();
            }

            return item;
        }

        public static List<ECN_Framework_Entities.Accounts.LandingPageAssign> GetDefault()
        {
            List<ECN_Framework_Entities.Accounts.LandingPageAssign> itemList = new List<ECN_Framework_Entities.Accounts.LandingPageAssign>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.Accounts.LandingPageAssign.GetDefault();
                scope.Complete();
            }

            return itemList;
        }

        public static List<ECN_Framework_Entities.Accounts.LandingPageAssign> GetByLPID(int LPID)
        {
            List<ECN_Framework_Entities.Accounts.LandingPageAssign> itemList = new List<ECN_Framework_Entities.Accounts.LandingPageAssign>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.Accounts.LandingPageAssign.GetByLPID(LPID);
                scope.Complete();
            }

            return itemList;
        }

        public static ECN_Framework_Entities.Accounts.LandingPageAssign GetOneToUse(int LPID, int customerID, bool getChildren)
        {
            ECN_Framework_Entities.Accounts.LandingPageAssign item = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Accounts.LandingPageAssign.GetOneToUse(customerID, LPID);
                scope.Complete();
            }

            if (item != null && getChildren)
            {
                item.AssignContentList = ECN_Framework_BusinessLayer.Accounts.LandingPageAssignContent.GetByLPAID(item.LPAID);
            }

            return item;
        }

        public static ECN_Framework_Entities.Accounts.LandingPageAssign GetByLPAID(int LPAID, bool getChildren = false)
        {
            ECN_Framework_Entities.Accounts.LandingPageAssign item = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Accounts.LandingPageAssign.GetByLPAID(LPAID);
                scope.Complete();
            }

            if (item != null && getChildren)
            {
                item.AssignContentList = ECN_Framework_BusinessLayer.Accounts.LandingPageAssignContent.GetByLPAID(item.LPAID);
            }

            return item;
        }

        public static void RemoveBaseChannelOverrideForCustomer(int BaseChannelID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Accounts.LandingPageAssign.RemoveBaseChannelOverrideForCustomer(BaseChannelID);
                scope.Complete();
            }
        }

        public static void Save(ECN_Framework_Entities.Accounts.LandingPageAssign lpa, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                lpa.LPAID = ECN_Framework_DataLayer.Accounts.LandingPageAssign.Save(lpa, user.UserID);
                if (lpa.BaseChannelID != null && lpa.CustomerCanOverride == false)
                {
                    RemoveBaseChannelOverrideForCustomer(lpa.BaseChannelID.Value);
                }
                scope.Complete();
            }
        }

        public static DataTable GetPreviewParameters(int LPAID, int customerID)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Accounts.LandingPageAssign.GetPreviewParameters(LPAID, customerID);
                scope.Complete();
            }

            return dt;
        }

        public static DataTable GetPreviewParameters_BaseChannel(int LPAID, int BaseChannelID)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Accounts.LandingPageAssign.GetPreviewParameters_BaseChannel(LPAID, BaseChannelID);
                scope.Complete();
            }

            return dt;
        }


    }
}
