using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.activityengines.Tests.Setup.Interfaces;
using Shim = ECN_Framework_BusinessLayer.Communicator.Fakes.ShimEmail;
using Moq;
using ECN_Framework_Entities.Communicator;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class EmailMock : Mock<IEmail>
    {
        public EmailMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            Shim.GetByEmailID_NoAccessCheckInt32 = GetByEmailIDNoAccessCheck;
            Shim.GetColumnNames = GetColumnNames;
        }
        

        private Email GetByEmailIDNoAccessCheck(int emailId)
        {
            return Object.GetByEmailIDNoAccessCheck(emailId);
        }

        private DataTable GetColumnNames()
        {
            return Object.GetColumnNames();
        }

    }
}
