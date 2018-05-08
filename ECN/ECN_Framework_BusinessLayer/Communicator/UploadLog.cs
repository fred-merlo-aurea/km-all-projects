using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class UploadLog
    {
        public static void Save(ref ECN_Framework_Entities.Communicator.UploadLog log)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                log.UploadID = ECN_Framework_DataLayer.Communicator.UploadLog.Save(log);
                scope.Complete();
            }
        }
    }
}
