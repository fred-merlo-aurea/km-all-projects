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
    public class LinkTrackingParamSettings
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.LinkTrackingParamSettings;
        public static ECN_Framework_Entities.Communicator.LinkTrackingParamSettings Get_LTPID_CustomerID(int LTPID, int CustomerID)
        {
            ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps = new ECN_Framework_Entities.Communicator.LinkTrackingParamSettings();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ltps = ECN_Framework_DataLayer.Communicator.LinkTrackingParamSettings.Get_CustomerID_LTPID(CustomerID, LTPID);
                scope.Complete();
            }
            return ltps;
        }

        public static ECN_Framework_Entities.Communicator.LinkTrackingParamSettings Get_LTPID_BaseChannelID(int LTPID, int BaseChannelID)
        {
            ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps = new ECN_Framework_Entities.Communicator.LinkTrackingParamSettings();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ltps = ECN_Framework_DataLayer.Communicator.LinkTrackingParamSettings.Get_BaseChannelID_LTPID(BaseChannelID, LTPID);
                scope.Complete();
            }
            return ltps;
        }

        public static int Insert(ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps)
        {
            Validate(ltps);
            int returnID = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                returnID = ECN_Framework_DataLayer.Communicator.LinkTrackingParamSettings.Insert(ltps);
                scope.Complete();
            }
            return returnID;
        }

        public static void Update(ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps)
        {
            Validate(ltps);

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.LinkTrackingParamSettings.Update(ltps);
                scope.Complete();
            }

        }

        private static void Validate(ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if((ltps.CustomerID == null && ltps.BaseChannelID == null) || (ltps.CustomerID != null && ltps.BaseChannelID != null))
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID or BaseChannelID invalid"));
            }
            else
            {
                if(ltps.DisplayName == null || string.IsNullOrEmpty(ltps.DisplayName))
                    errorList.Add(new ECNError(Entity, Method, "DisplayName cannot be empty"));
                if(ltps.LTPSID <= 0 && ltps.CreatedUserID == null)
                    errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                if(ltps.LTPSID <= 0 && ltps.CreatedDate == null)
                    errorList.Add(new ECNError(Entity, Method, "CreatedDate is invalid"));
                if(ltps.AllowCustom == null)
                    errorList.Add(new ECNError(Entity, Method, "AllowCustom is invalid"));
                if(ltps.IsRequired == null)
                    errorList.Add(new ECNError(Entity, Method, "IsRequired is invalid"));

                if(ltps.LTPSID > 0)
                {
                    if(ltps.UpdatedDate == null)
                        errorList.Add(new ECNError(Entity, Method, "UpdatedDate is invalid"));
                    if(ltps.UpdatedUserID == null)
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
