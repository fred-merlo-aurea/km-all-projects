namespace ECN_Framework_Entities.Salesforce.Convertors
{
    public class CampaignMemberStatusConverter : EntityConverterBase
    {
        private const string IdPropertyName = "Id";

        public CampaignMemberStatusConverter()
        {
            AddPropertyConverter(IdPropertyName, s => s.Trim());
        }

        protected override string LastPropertyName => "SystemModstamp";
    }
}
