using System;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class CodeSheetMasterCodeSheetBridge
    {
        public int Save(Entity.CodeSheetMasterCodeSheetBridge x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.CodeSheetID = DataAccess.CodeSheetMasterCodeSheetBridge.Save(x, client);
                scope.Complete();
            }

            return x.CodeSheetID;
        }

        public bool Delete(KMPlatform.Object.ClientConnections client, int codeSheetID)
        {
            bool delete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                delete = DataAccess.CodeSheetMasterCodeSheetBridge.DeleteCodeSheetID(client, codeSheetID);
                scope.Complete();
            }

            return delete;
        }
        public bool DeleteMasterID(KMPlatform.Object.ClientConnections client, int masterID)
        {
            bool delete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                delete = DataAccess.CodeSheetMasterCodeSheetBridge.DeleteMasterID(client, masterID);
                scope.Complete();
            }

            return delete;
        }
    }
}
