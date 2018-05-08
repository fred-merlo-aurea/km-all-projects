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
    public class SmartFormTracking
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.SmartFormTracking;

        public static void Insert(ECN_Framework_Entities.Communicator.SmartFormTracking sft)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.SmartFormTracking.Insert(sft);
                scope.Complete();
            }

//            return sft.SALID;
        }
    }
}
