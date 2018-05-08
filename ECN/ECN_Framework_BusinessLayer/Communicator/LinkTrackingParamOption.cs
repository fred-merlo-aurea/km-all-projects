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
    public class LinkTrackingParamOption
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.LinkTrackingParamOption;

        public static List<ECN_Framework_Entities.Communicator.LinkTrackingParamOption> GetAll()
        {
            List<ECN_Framework_Entities.Communicator.LinkTrackingParamOption> linkTrackingParamOption = new List<ECN_Framework_Entities.Communicator.LinkTrackingParamOption>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                linkTrackingParamOption = ECN_Framework_DataLayer.Communicator.LinkTrackingParamOption.Getall();
                scope.Complete();
            }
            return linkTrackingParamOption;

        }

        public static ECN_Framework_Entities.Communicator.LinkTrackingParamOption GetByLTPOID(int LTPOID)
        {
            ECN_Framework_Entities.Communicator.LinkTrackingParamOption ltpo = new ECN_Framework_Entities.Communicator.LinkTrackingParamOption();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ltpo = ECN_Framework_DataLayer.Communicator.LinkTrackingParamOption.GetByLTPOID(LTPOID);
                scope.Complete();
            }
            return ltpo;
        }
        public static ECN_Framework_Entities.Communicator.LinkTrackingParamOption GetLTPOIDByCustomerID(int LTPID,string Value,int CustomerID)
        {
            ECN_Framework_Entities.Communicator.LinkTrackingParamOption ltpo = new ECN_Framework_Entities.Communicator.LinkTrackingParamOption();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ltpo = ECN_Framework_DataLayer.Communicator.LinkTrackingParamOption.GetLTPOIDByCustomerID(LTPID,Value, CustomerID);
                scope.Complete();
            }
            return ltpo;
        }
        public static ECN_Framework_Entities.Communicator.LinkTrackingParamOption GetLTPOIDByBaseChannelID(int LTPID, string Value, int BaseChannelID)
        {
            ECN_Framework_Entities.Communicator.LinkTrackingParamOption ltpo = new ECN_Framework_Entities.Communicator.LinkTrackingParamOption();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ltpo = ECN_Framework_DataLayer.Communicator.LinkTrackingParamOption.GetLTPOIDByBaseChannelID(LTPID,Value,BaseChannelID);
                scope.Complete();
            }
            return ltpo;
        }
        public static List<ECN_Framework_Entities.Communicator.LinkTrackingParamOption> GetByLTPID(int LTPID)
        {
            List<ECN_Framework_Entities.Communicator.LinkTrackingParamOption> ltpo = new List<ECN_Framework_Entities.Communicator.LinkTrackingParamOption>();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ltpo = ECN_Framework_DataLayer.Communicator.LinkTrackingParamOption.GetByLTPID(LTPID);
                scope.Complete();
            }
            return ltpo;
        }

        public static List<ECN_Framework_Entities.Communicator.LinkTrackingParamOption> Get_LTPID_CustomerID(int LTPID, int CustomerID)
        {
            List<ECN_Framework_Entities.Communicator.LinkTrackingParamOption> linkTrackingParamOption = new List<ECN_Framework_Entities.Communicator.LinkTrackingParamOption>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                linkTrackingParamOption = ECN_Framework_DataLayer.Communicator.LinkTrackingParamOption.Get_CustomerID_LTPID(CustomerID, LTPID);
                scope.Complete();
            }
            return linkTrackingParamOption;
        }

        public static DataTable GetDT_LTPID_CustomerID(int LTPID, int CustomerID)
        {
            DataTable retTable = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retTable = ECN_Framework_DataLayer.Communicator.LinkTrackingParamOption.GetDT_CustomerID_LTPID(CustomerID, LTPID);
                scope.Complete();
            }
            return retTable;
        }

        public static List<ECN_Framework_Entities.Communicator.LinkTrackingParamOption> Get_LTPID_BaseChannelID(int LTPID, int BaseChannelID)
        {
            List<ECN_Framework_Entities.Communicator.LinkTrackingParamOption> linkTrackingParamOption = new List<ECN_Framework_Entities.Communicator.LinkTrackingParamOption>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                linkTrackingParamOption = ECN_Framework_DataLayer.Communicator.LinkTrackingParamOption.Get_BaseChannelID_LTPID(BaseChannelID, LTPID);
                scope.Complete();
            }
            return linkTrackingParamOption;
        }

        public static DataTable GetDT_LTPID_BaseChannelID(int LTPID, int BaseChannelID)
        {
            DataTable retTable = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retTable = ECN_Framework_DataLayer.Communicator.LinkTrackingParamOption.GetDT_BaseChannelID_LTPID(BaseChannelID, LTPID);
                scope.Complete();
            }
            return retTable;
        }

        public static int Insert(ECN_Framework_Entities.Communicator.LinkTrackingParamOption ltpo)
        {
            Validate(ltpo);
            int returnID = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                returnID = ECN_Framework_DataLayer.Communicator.LinkTrackingParamOption.Insert(ltpo);
                scope.Complete();
            }
            return returnID;
        }

        public static void Update(ECN_Framework_Entities.Communicator.LinkTrackingParamOption ltpo)
        {
            Validate(ltpo);
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.LinkTrackingParamOption.Update(ltpo);
                scope.Complete();
            }
        }

        public static void ResetCustDefault(int LTPID, int CustomerID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.LinkTrackingParamOption.ResetCustDefault(LTPID, CustomerID);
                scope.Complete();
            }
        }

        public static void ResetBaseDefault(int LTPID, int BaseChannelID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.LinkTrackingParamOption.ResetBaseDefault(LTPID, BaseChannelID);
                scope.Complete();
            }
        }

        public static void Delete(ECN_Framework_Entities.Communicator.LinkTrackingParamOption ltpo)
        {
            List<ECNError> errorList = new List<ECNError>();
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECN_Framework_Entities.Communicator.CampaignItemLinkTracking> lstCILT = ECN_Framework_BusinessLayer.Communicator.CampaignItemLinkTracking.GetByLinkTrackingParamOptionID(ltpo.LTPOID);
            if (lstCILT.Count > 0)
            {
                errorList.Add(new ECNError(Entity, Method, "Param Option is used in an active blast, cannot delete."));
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ECN_Framework_DataLayer.Communicator.LinkTrackingParamOption.Update(ltpo);
                    scope.Complete();
                }
            }
        }

        private static void Validate(ECN_Framework_Entities.Communicator.LinkTrackingParamOption ltpo)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (ltpo.DisplayName == null || string.IsNullOrEmpty(ltpo.DisplayName))
                errorList.Add(new ECNError(Entity, Method, "DisplayName cannot be empty"));

            if (ltpo.Value == null || string.IsNullOrEmpty(ltpo.Value))
                errorList.Add(new ECNError(Entity, Method, "Value cannot be empty"));

            if ((ltpo.CustomerID == null && ltpo.BaseChannelID == null) || (ltpo.CustomerID > 0 && ltpo.BaseChannelID > 0))
                errorList.Add(new ECNError(Entity, Method, "CustomerID or BaseChannelID is invalid"));

            if (ltpo.LTPOID <= 0 && ltpo.CreatedDate == null)
                errorList.Add(new ECNError(Entity, Method, "CreatedDate is invalid"));

            if (ltpo.LTPOID <= 0 && (ltpo.CreatedUserID == null || ltpo.CreatedUserID < 0))
                errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

            if (ltpo.LTPOID > 0)
            {
                if (ltpo.UpdatedDate == null)
                    errorList.Add(new ECNError(Entity, Method, "UpdatedDate is invalid"));
                if (ltpo.UpdatedUserID == null || ltpo.UpdatedUserID < 0)
                    errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
            }

           

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }

        }


    }
}
