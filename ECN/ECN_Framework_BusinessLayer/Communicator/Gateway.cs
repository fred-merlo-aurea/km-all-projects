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
    public class Gateway
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Gateway;

        public static bool Exists(int GatewayID, int CustomerID)
        {
            bool exists = false;
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Gateway.Exists(GatewayID, CustomerID);
                scope.Complete();
            }
            return exists;
        }

        private static bool Exists_PubCode_TypeCode(string PubCode, string TypeCode)
        {
            bool exists = false;
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Gateway.Exists_PubCode_TypeCode(PubCode, TypeCode);
                scope.Complete();
            }
            return exists;
        }

        private static void Validate(ECN_Framework_Entities.Communicator.Gateway gateway)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if(gateway.GatewayID > 0)
            {
                if(!Exists(gateway.GatewayID, gateway.CustomerID))
                    errorList.Add(new ECNError { Entity = Enums.Entity.Gateway, ErrorMessage = "Gateway doesn't exist", Method = Method });
            }
            else
            {
                if(Exists_PubCode_TypeCode(gateway.PubCode, gateway.TypeCode))
                    errorList.Add(new ECNError { Entity = Enums.Entity.Gateway, ErrorMessage = "A Gateway already exists with this PubCode and TypeCode", Method = Method });
            }

            if (errorList.Count > 0)
                throw new ECNException(errorList);

        }

        public static int Save(ECN_Framework_Entities.Communicator.Gateway gateway, KMPlatform.Entity.User user)
        {
            Validate(gateway);
            int retID = -1;
            if(gateway.GatewayID > 0)
                gateway.UpdatedUserID = user.UserID;
            else
                gateway.CreatedUserID = user.UserID;
           
            using(TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.Communicator.Gateway.Save(gateway);
                scope.Complete();
            }
            return retID;
        }

        public static List<ECN_Framework_Entities.Communicator.Gateway> GetByCustomerID(int CustomerID)
        {
            List<ECN_Framework_Entities.Communicator.Gateway> retList = new List<ECN_Framework_Entities.Communicator.Gateway>();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.Gateway.GetByCustomerID(CustomerID);
                scope.Complete();
            }
            return retList;
        }

        public static ECN_Framework_Entities.Communicator.Gateway GetByGatewayID(int GatewayID)
        {
            ECN_Framework_Entities.Communicator.Gateway retItem = new ECN_Framework_Entities.Communicator.Gateway();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retItem = ECN_Framework_DataLayer.Communicator.Gateway.GetByGatewayID(GatewayID);
                scope.Complete();
            }
            return retItem;
        }

        public static ECN_Framework_Entities.Communicator.Gateway GetByGatewayPubCode(string pubCode, string typeCode)
        {
            ECN_Framework_Entities.Communicator.Gateway retItem = new ECN_Framework_Entities.Communicator.Gateway();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retItem = ECN_Framework_DataLayer.Communicator.Gateway.GetByGatewayPubCode(pubCode, typeCode);
                scope.Complete();
            }
            return retItem;
        }
    }
}
