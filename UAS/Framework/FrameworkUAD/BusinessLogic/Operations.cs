using System;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class Operations
    {
        public bool RemovePubCode(KMPlatform.Object.ClientConnections client, string pubCode)
        {
            bool delete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                delete = DataAccess.Operations.RemovePubCode(client, pubCode);
                scope.Complete();
            }

            return delete;
        }

        public bool RemoveProcessCode(KMPlatform.Object.ClientConnections client, string processCode)
        {
            bool delete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                delete = DataAccess.Operations.RemoveProcessCode(client, processCode);
                scope.Complete();
            }

            return delete;
        }

        public bool QSourceValidation(KMPlatform.Object.ClientConnections client, int sourceFileID, string processCode)
        {
            return DataAccess.Operations.QSourceValidation(client, sourceFileID, processCode);
        }
        public bool FileValidator_QSourceValidation(KMPlatform.Object.ClientConnections client, int sourceFileID, string processCode)
        {
            return DataAccess.Operations.FileValidator_QSourceValidation(client, sourceFileID, processCode);
        }
    }
}
