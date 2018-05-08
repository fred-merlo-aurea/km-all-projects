using System;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class AcsShippingDetail
    {
        public int Save(Entity.AcsShippingDetail x, KMPlatform.Object.ClientConnections client)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                x.AcsShippingDetailId = DataAccess.AcsShippingDetail.Save(x, client);
                scope.Complete();
            }

            return x.AcsShippingDetailId;
        }
    }
}
