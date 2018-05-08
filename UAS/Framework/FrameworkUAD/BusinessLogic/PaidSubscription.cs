using System;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class PaidSubscription
    {
        public int Save(FrameworkUAD.Object.PaidSubscription x, KMPlatform.Object.ClientConnections client)
        {
            x.SubscriberPhone = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.SubscriberPhone);
            x.SubscriberMobile = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.SubscriberMobile);
            x.SubscriberFax = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.SubscriberFax);

            //using (TransactionScope scope = new TransactionScope())
            //{
            //    x.SubscriptionId_UAD = DataAccess.PaidSubscription.Save(x, client);
            //    scope.Complete();
            //}

            return x.SubscriptionId_UAD;
        }
    }
}
