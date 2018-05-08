using System;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Activity
{
    [Serializable]
    public class EmailActivityUpdate
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Email;
        public static void UpdateEmailActivity_NoAccessCheck(
            string oldEmailAddress,
            string newEmailAddress,
            int groupID,
            int customerID,
            int formID,
            string comments,
            KMPlatform.Entity.User user)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Activity.EmailActivityUpdate.UpdateEmailActivity(oldEmailAddress, newEmailAddress, groupID, customerID, formID, comments);
                scope.Complete();
            }
        }
    }
}
