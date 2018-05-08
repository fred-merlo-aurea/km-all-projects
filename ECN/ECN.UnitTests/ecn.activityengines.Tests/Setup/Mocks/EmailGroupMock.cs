using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.activityengines.Tests.Setup.Interfaces;
using Shim = ECN_Framework_BusinessLayer.Communicator.Fakes.ShimEmailGroup;
using Moq;
using ECN_Framework_Entities.Communicator;
using System.Diagnostics.CodeAnalysis;
using KMPlatform.Entity;
using System.Data;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class EmailGroupMock : Mock<IEmailGroup>
    {
        public EmailGroupMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            Shim.GetByEmailIDGroupID_NoAccessCheckInt32Int32 = GetByEmailIDGroupIDNoAccessCheck;
            Shim.EmailExistsInCustomerSeedListInt32Int32 = EmailExistsInCustomerSeedList;
            Shim.ImportEmails_NoAccessCheckUserInt32Int32StringStringStringStringBooleanStringString =
                ImportEmails_NoAccessCheck;
            Shim.GetEmailIDFromWhatEmail_NoAccessCheckInt32Int32String = GetEmailIDFromWhatEmail_NoAccessCheck;
        }

        private int GetEmailIDFromWhatEmail_NoAccessCheck(int groupId, int customerId, string emailAddress)
        {
            return Object.GetEmailIDFromWhatEmail_NoAccessCheck(groupId, customerId, emailAddress);
        }

        private DataTable ImportEmails_NoAccessCheck(
            User user,
            int customerId,
            int groupId,
            string xmlProfile,
            string xmlUdf,
            string formatTypeCode,
            string subscribeTypeCode,
            bool emailAddressOnly,
            string fileName,
            string source)
        {
            return Object.ImportEmails_NoAccessCheck(user, customerId, groupId, xmlProfile, xmlUdf,
                formatTypeCode, subscribeTypeCode, emailAddressOnly, fileName, source);
        }

        private bool EmailExistsInCustomerSeedList(int emailId, int customerId)
        {
            return Object.EmailExistsInCustomerSeedList(emailId, customerId);
        }

        private EmailGroup GetByEmailIDGroupIDNoAccessCheck(int emailId, int groupId)
        {
            return Object.GetByEmailIDGroupIDNoAccessCheck(emailId, groupId);
        }
    }
}
