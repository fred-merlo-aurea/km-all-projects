using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface IEmailGroup
    {
        EmailGroup GetByEmailIDGroupIDNoAccessCheck(int emailId, int groupId);

        bool EmailExistsInCustomerSeedList(int emailId, int customerId);

        DataTable ImportEmails_NoAccessCheck(
            User user,
            int customerId,
            int groupId,
            string xmlProfile,
            string xmlUdf,
            string formatTypeCode,
            string subscribeTypeCode,
            bool emailAddressOnly,
            string fileName,
            string source);

        int GetEmailIDFromWhatEmail_NoAccessCheck(int groupId, int customerId, string emailAddress);
    }
}
