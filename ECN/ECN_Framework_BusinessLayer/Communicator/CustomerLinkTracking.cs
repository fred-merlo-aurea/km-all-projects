using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;


namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class CustomerLinkTracking
    {
        public static List<ECN_Framework_Entities.Communicator.CustomerLinkTracking> GetByCustomerID(int customerID)
        {
            List<ECN_Framework_Entities.Communicator.CustomerLinkTracking> customerLinkTrackingList = new List<ECN_Framework_Entities.Communicator.CustomerLinkTracking>();
            using (TransactionScope scope = new TransactionScope())
            {
                customerLinkTrackingList = ECN_Framework_DataLayer.Communicator.CustomerLinkTracking.GetByCustomerID(customerID);
                scope.Complete();
            }
            return customerLinkTrackingList;
        }
    }
}
