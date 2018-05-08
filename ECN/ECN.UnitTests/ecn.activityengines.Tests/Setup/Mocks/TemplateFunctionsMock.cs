using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.activityengines.Tests.Setup.Interfaces;
using Shim = ecn.common.classes.Fakes.ShimTemplateFunctions;
using Moq;
using ECN_Framework_Entities.Communicator;
using System.Diagnostics.CodeAnalysis;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class TemplateFunctionsMock : Mock<ITemplateFunctions>
    {
        public TemplateFunctionsMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            this.SetReturnsDefault(string.Empty);
            Shim.LinkReWriterStringBlastStringStringString = LinkReWriter;
            Shim.EmailHTMLBodyStringStringInt32Int32Int32Int32Int32Int32Int32Int32Int32 = EmailHTMLBody;
        }

        private string EmailHTMLBody(
            string templateSource,
            string tableOptions,
            int slot1,
            int slot2,
            int slot3,
            int slot4,
            int slot5,
            int slot6,
            int slot7,
            int slot8,
            int slot9)
        {
            return Object.EmailHTMLBody(templateSource, tableOptions, slot1, slot2, slot3, slot4, slot5, slot6, slot7,
                slot8, slot9);
        }

        private string LinkReWriter(string text, Blast blast, string customerId, string virtualPath, string hostName)
        {
            return Object.LinkReWriter(text, blast, customerId, virtualPath, hostName);
        }
    }
}
