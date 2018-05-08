﻿using System;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class SubscriptionDetail
    {
        public bool DeleteMasterID(KMPlatform.Object.ClientConnections client, int masterID)
        {
            bool delete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                delete = DataAccess.SubscriptionDetail.DeleteMasterID(client, masterID);
                scope.Complete();
            }

            return delete;
        }
    }
}
