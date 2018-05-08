using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;

namespace ECN.Framework.BusinessLayer.Tests.DTO
{
    public class ValidateDTO
    {
        public string EmailsToAdd { get; set; }
        public int LayoutID { get; set; }
        public bool EmailPreview { get; set; }
        public string EmailFrom { get; set; }
        public string ReplyTo { get; set; }
        public string FromName { get; set; }
        public string EmailSubject { get; set; }
        public int CustomerID { get; set; }
        public int? GroupID { get; set; }
        public string GroupName { get; set; }
        public int BaseChannelID { get; set; }
        public int? CampaignID { get; set; }
        public string CampaignName { get; set; }
        public int? CampaignItemID { get; set; }
        public string CampaignItemName { get; set; }
        public User CurrentUser { get; set; }
        public QuickTestBlastConfig QTB { get; set; }
    }
}
