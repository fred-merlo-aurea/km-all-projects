namespace ECN_Framework_Entities.Salesforce.Convertors
{
    public class CampaignMemberConverter : EntityConverterBase
    {
        private const string IdPropertyName = "Id";
        private const string LeadIdPropertyName = "LeadId";
        private const string StatusPropertyName = "Status";

        public CampaignMemberConverter()
        {
            AddPropertyConverter(IdPropertyName, s => s.Trim());
            AddPropertyConverter(LeadIdPropertyName, s => s.Trim());
            AddPropertyConverter(StatusPropertyName, s => s.Trim());
        }

        protected override string LastPropertyName => "LastModifiedDate";
    }
}
