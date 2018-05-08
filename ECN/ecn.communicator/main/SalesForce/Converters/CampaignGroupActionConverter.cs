namespace ecn.communicator.main.Salesforce.SF_Pages.Converters
{
    public class CampaignGroupActionConverter : ActionConverter
    {
        public CampaignGroupActionConverter() : base()
        {
            AddOrOverrideMapping("T", new ActionModel("Total Records in the File", 1));
        }
    }
}