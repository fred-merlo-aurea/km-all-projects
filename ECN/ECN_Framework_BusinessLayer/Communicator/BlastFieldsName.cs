using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;
using System.Configuration;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class BlastFieldsName
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.BlastFieldsName;

        public static ECN_Framework_Entities.Communicator.BlastFieldsName GetByBlastFieldID(int BlastFieldID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.BlastFieldsName BlastFieldsName;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                BlastFieldsName = ECN_Framework_DataLayer.Communicator.BlastFieldsName.GetByBlastFieldID(BlastFieldID, user.CustomerID);
                scope.Complete();
            }
            return BlastFieldsName;
        }
        public static ECN_Framework_Entities.Communicator.BlastFieldsName GetByBlastFieldIDCustomerID(int BlastFieldID,int CustomerID,KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.BlastFieldsName BlastFieldsName;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                BlastFieldsName = ECN_Framework_DataLayer.Communicator.BlastFieldsName.GetByBlastFieldID(BlastFieldID, CustomerID);
                scope.Complete();
            }
            return BlastFieldsName;
        }

        public static void Save(ECN_Framework_Entities.Communicator.BlastFieldsName blastFieldsName, KMPlatform.Entity.User user)
        {
            Validate(blastFieldsName);
            using (TransactionScope scope = new TransactionScope())
            {
                blastFieldsName.BlastFieldsNameID = ECN_Framework_DataLayer.Communicator.BlastFieldsName.Save(blastFieldsName, user.UserID );
                scope.Complete();
            }
        }

        public static void Validate(ECN_Framework_Entities.Communicator.BlastFieldsName blastFieldsName)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (!ECN_Framework_Common.Functions.RegexUtilities.IsValidObjectName(blastFieldsName.Name))
            {
                errorList.Add(new ECNError(Entity, Method,"Name has invalid characters"));
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

    }
}
