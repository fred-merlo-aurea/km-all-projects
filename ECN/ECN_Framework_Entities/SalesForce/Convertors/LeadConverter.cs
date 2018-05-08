namespace ECN_Framework_Entities.Salesforce.Convertors
{
    public class LeadConverter : EntityConverterBase
    {
        private const string IdPropertyName = "Id";
        private const string EmailBouncedDatePropertyName = "EmailBouncedDate";
        private const string EmailBouncedReasonPropertyName = "EmailBouncedReason";
        private const string LastViewedDatePropertyName = "LastViewedDate";

        private const string EmailBouncedReasonMappedName = "EmailBounceReason";
        private const string EmailBouncedDateMappedName = "EmailBounceDate";
        private const string LastViewedDateMappedName = "LastReferencedDate";

        public LeadConverter()
        {
            AddPropertyConverter(IdPropertyName, s => s.Trim());

            AddPropertyMapping(EmailBouncedReasonPropertyName, EmailBouncedReasonMappedName);
            AddPropertyMapping(EmailBouncedDatePropertyName, EmailBouncedDateMappedName);
            AddPropertyMapping(LastViewedDatePropertyName, LastViewedDateMappedName);
        }

        protected override string LastPropertyName => "EmailBouncedDate";
    }
}
