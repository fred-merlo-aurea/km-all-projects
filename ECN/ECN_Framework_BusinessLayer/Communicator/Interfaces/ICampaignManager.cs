namespace ECN_Framework_BusinessLayer.Communicator.Interfaces
{
    public interface ICampaignManager
    {
        bool Exists(int campaignID, int customerID);
        bool Exists(int campaignID, string campaignName, int customerID);
    }
}