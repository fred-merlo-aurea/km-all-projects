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
    public class SFSettings
    {
        public static ECN_Framework_Entities.Accounts.SFSettings GetOneToUse(int customerID)
        {
            ECN_Framework_Entities.Accounts.SFSettings item = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Accounts.SFSettings.GetOneToUse(customerID);
                scope.Complete();
            }
            return item;
        }

        public static void RemoveBaseChannelOverrideForCustomer(int BaseChannelID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Accounts.SFSettings.RemoveBaseChannelOverrideForCustomer(BaseChannelID);
                scope.Complete();
            }
        }

        public static void Save(ECN_Framework_Entities.Accounts.SFSettings sfs, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                sfs.SFSettingsID = ECN_Framework_DataLayer.Accounts.SFSettings.Save(sfs, user.UserID);
                if (sfs.BaseChannelID != null && sfs.CustomerCanOverride == false)
                {
                    RemoveBaseChannelOverrideForCustomer(sfs.BaseChannelID.Value);
                }
                scope.Complete();
            }
        }

        public static List<ECN_Framework_Entities.Accounts.SFSettings> GetCMSBaseChannels()
        {
            List<ECN_Framework_Entities.Accounts.SFSettings> itemList = new List<ECN_Framework_Entities.Accounts.SFSettings>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.Accounts.SFSettings.GetCMSBaseChannels();
                scope.Complete();
            }
            return itemList;
        }
    }
}
