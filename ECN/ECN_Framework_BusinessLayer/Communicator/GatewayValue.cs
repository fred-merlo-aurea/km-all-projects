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
    public class GatewayValue
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.GatewayValue;

        public static List<ECN_Framework_Entities.Communicator.GatewayValue> GetByGatewayID(int GatewayID)
        {
            List<ECN_Framework_Entities.Communicator.GatewayValue> retList = new List<ECN_Framework_Entities.Communicator.GatewayValue>();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.GatewayValue.GetByGatewayID(GatewayID);
                scope.Complete();
            }
            return retList;
        }

        public static void WipeOutValues(bool LoginValues, int GatewayID)
        {
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ECN_Framework_DataLayer.Communicator.GatewayValue.WipeOutValues(LoginValues, GatewayID);
                scope.Complete();
            }
        }

        public static void DeleteByID(int GatewayValueID)
        {
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ECN_Framework_DataLayer.Communicator.GatewayValue.Delete(GatewayValueID);
                scope.Complete();
            }
        }

        public static int Save(ECN_Framework_Entities.Communicator.GatewayValue gv, KMPlatform.Entity.User user)
        {
            Validate(gv, user);

            int retID = -1;
            using(TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.Communicator.GatewayValue.Save(gv);
                scope.Complete();
            }
            return retID;
        }

        private static void Validate(ECN_Framework_Entities.Communicator.GatewayValue gv, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (gv.IsCaptureValue && gv.IsLoginValidator)
                errorList.Add(new ECNError() { Entity = Enums.Entity.GatewayValue, ErrorMessage = "Cannot be a capture value and login validator", Method = Method });
            else if(!gv.IsCaptureValue && !gv.IsLoginValidator)
                errorList.Add(new ECNError() { Entity = Enums.Entity.GatewayValue, ErrorMessage = "Must be either a capture value or login validator", Method = Method });

            if (!ECN_Framework_BusinessLayer.Communicator.Gateway.Exists(gv.GatewayID, user.CustomerID))
                errorList.Add(new ECNError() { Entity = Enums.Entity.GatewayValue, ErrorMessage = "Gateway doesn't exist", Method = Method });

            if(errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }
    }
}
