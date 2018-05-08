using ecn.activityengines.Tests.Setup.Interfaces;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class EmailDirectMock:Mock<IEmailDirect>
    {
        public EmailDirectMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            ShimEmailDirect.SaveEmailDirect = Save;
        }

        private int Save(EmailDirect email)
        {
            return Object.Save(email);
        }
    }
}
