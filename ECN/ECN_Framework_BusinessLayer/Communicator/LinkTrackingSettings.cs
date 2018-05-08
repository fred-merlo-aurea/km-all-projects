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
    public class LinkTrackingSettings
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.LinkTrackingSettings;
        public static ECN_Framework_Entities.Communicator.LinkTrackingSettings GetByCustomerID_LTID(int CustomerID, int LTID)
        {
            ECN_Framework_Entities.Communicator.LinkTrackingSettings lts = new ECN_Framework_Entities.Communicator.LinkTrackingSettings();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                lts = ECN_Framework_DataLayer.Communicator.LinkTrackingSettings.Get_CustomerID_LTID(CustomerID, LTID);
                scope.Complete();
            }
            return lts;
        }

        public static ECN_Framework_Entities.Communicator.LinkTrackingSettings GetByBaseChannelID_LTID(int BaseChannelID, int LTID)
        {
            ECN_Framework_Entities.Communicator.LinkTrackingSettings lts = new ECN_Framework_Entities.Communicator.LinkTrackingSettings();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                lts = ECN_Framework_DataLayer.Communicator.LinkTrackingSettings.Get_BaseChannelID_LTID(BaseChannelID, LTID);
                scope.Complete();
            }
            return lts;
        }

        public static int Insert(ECN_Framework_Entities.Communicator.LinkTrackingSettings lts)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                lts.LTSID = ECN_Framework_DataLayer.Communicator.LinkTrackingSettings.Insert(lts);
                scope.Complete();
            }
            return lts.LTSID;
        }

        public static void Update(ECN_Framework_Entities.Communicator.LinkTrackingSettings lts)
        {
            Validate(lts);
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.LinkTrackingSettings.Update(lts);
                scope.Complete();
            }
            
        }

        public static void UpdateCustomerOmnitureOverride(int baseChannelID, bool isOverride, int UserID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.LinkTrackingSettings.UpdateCustomerOmnitureOverride(baseChannelID, isOverride, UserID);
                scope.Complete();
            }
        }

        private static void Validate(ECN_Framework_Entities.Communicator.LinkTrackingSettings lts)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if ((lts.CustomerID == null && lts.BaseChannelID == null) || (lts.CustomerID != null && lts.BaseChannelID != null))
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID or BaseChannelID invalid"));
            }
            else
            {
                if (lts.LTID == null)
                    errorList.Add(new ECNError(Entity, Method, "LinkTrackingID is invalid"));
                if (lts.LTSID <= 0 && lts.CreatedUserID == null)
                    errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                if (lts.LTSID != null && lts.LTSID > 0)
                {
                    if (lts.UpdatedUserID == null)
                        errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                }
            }
            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }
    }
}
