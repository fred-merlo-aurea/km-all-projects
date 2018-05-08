using ECN_Framework_Entities.Communicator;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface ITemplateFunctions
    {
        string LinkReWriter(string text, Blast blast, string customerId, string virtualPath, string hostName);

        string EmailHTMLBody(
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
            int slot9);
    }
}
