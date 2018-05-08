using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class ApplicationCode
    {
        public static bool Exists(int appCodeID, ECN_Framework_Common.Objects.Communicator.Enums.ApplicationCode codeType)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.ApplicationCode.Exists(appCodeID, codeType);
                scope.Complete();
            }
            return exists;
        }

        public static ECN_Framework_Entities.Communicator.ApplicationCode GetByCodeID(int codeID)
        {
            ECN_Framework_Entities.Communicator.ApplicationCode appCode = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                appCode = ECN_Framework_DataLayer.Communicator.ApplicationCode.GetByCodeID(codeID);
                scope.Complete();
            }
            return appCode;
        }

        public static List<ECN_Framework_Entities.Communicator.ApplicationCode> GetByCodeType(ECN_Framework_Common.Objects.Communicator.Enums.ApplicationCode ctype)
        {
            List<ECN_Framework_Entities.Communicator.ApplicationCode> appCodeList = new List<ECN_Framework_Entities.Communicator.ApplicationCode>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                appCodeList = ECN_Framework_DataLayer.Communicator.ApplicationCode.GetByCodeType(ctype);
                scope.Complete();
            }
            return appCodeList;
        }

        public static List<ECN_Framework_Entities.Communicator.ApplicationCode> GetAll()
        {
            List<ECN_Framework_Entities.Communicator.ApplicationCode> appCodeList = new List<ECN_Framework_Entities.Communicator.ApplicationCode>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                appCodeList = ECN_Framework_DataLayer.Communicator.ApplicationCode.GetAll();
                scope.Complete();
            }
            return appCodeList;
        }
    }
}
