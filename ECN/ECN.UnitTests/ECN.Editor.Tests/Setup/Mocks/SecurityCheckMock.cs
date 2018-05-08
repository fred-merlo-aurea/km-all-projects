using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECN.Editor.Tests.Setup.Interfaces;
using ECN_Framework.Common;
using Moq;
using Shim = ECN_Framework.Common.Fakes.ShimSecurityCheck;

namespace ECN.Editor.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class SecurityCheckMock:Mock<ISecurityCheck>
    {
        public SecurityCheckMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            Shim.AllInstances.CustomerID = CustomerID;
        }

        private int CustomerID(SecurityCheck instance)
        {
            return Object.CustomerID();
        }
    }
}
