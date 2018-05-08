using KMPlatform.Entity;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace Ecn.Communicator.Main.Interfaces
{
    public interface IMasterCommunicator
    {
        int GetCustomerID();
        int GetUserID();
        User GetCurrentUser();
        int? GetBaseChannelID();
        MenuCode CurrentMenuCode { get; set; }
        string SubMenu { get; set; }
        string Heading { set; }
    }
}