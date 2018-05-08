using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class QuickTestBlastConfig
    {
        //SUNIL - TODO - support for content  / group folder.
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.QuickTestBlast;

        public static ECN_Framework_Entities.Communicator.QuickTestBlastConfig GetByBaseChannelID(int baseChannelID)
        {
            ECN_Framework_Entities.Communicator.QuickTestBlastConfig item = new ECN_Framework_Entities.Communicator.QuickTestBlastConfig();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Communicator.QuickTestBlastConfig.GetByBaseChannelID(baseChannelID);
                scope.Complete();
            }

            return item;
        }

        public static ECN_Framework_Entities.Communicator.QuickTestBlastConfig GetByCustomerID(int customerID)
        {
            ECN_Framework_Entities.Communicator.QuickTestBlastConfig item = new ECN_Framework_Entities.Communicator.QuickTestBlastConfig();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Communicator.QuickTestBlastConfig.GetByCustomerID(customerID);
                scope.Complete();
            }

            return item;
        }

        public static ECN_Framework_Entities.Communicator.QuickTestBlastConfig GetKMDefaultConfig()
        {
            ECN_Framework_Entities.Communicator.QuickTestBlastConfig item = new ECN_Framework_Entities.Communicator.QuickTestBlastConfig();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Communicator.QuickTestBlastConfig.GetKMDefaultConfig();
                scope.Complete();
            }

            return item;
        }

        public static void Save(ECN_Framework_Entities.Communicator.QuickTestBlastConfig qtb, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                qtb.QTBCID = ECN_Framework_DataLayer.Communicator.QuickTestBlastConfig.Save(qtb, user.UserID);
                if (qtb.BaseChannelID != null && qtb.CustomerCanOverride == false)
                {
                    RemoveBaseChannelOverrideForCustomer(qtb.BaseChannelID.Value);
                }
                scope.Complete();
            }
        }

        public static void RemoveBaseChannelOverrideForCustomer(int BaseChannelID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.QuickTestBlastConfig.RemoveBaseChannelOverrideForCustomer(BaseChannelID);
                scope.Complete();
            }
        }
    }
}
