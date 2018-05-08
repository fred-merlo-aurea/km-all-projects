namespace ECN_Framework_Entities.Salesforce.Convertors
{
    public class ContactConverter : EntityConverterBase
    {
        private const string IdPropertyName = "Id";
        private const string NextRecordUrlPropertyName = "nextRecordsUrl";
        private const string EmailBouncedReasonPropertyName = "EmailBouncedReason";
        private const string EmailBouncedReasonMappedName = "EmailBounceReason";
        private const string EmailBouncedDatePropertyName = "EmailBouncedDate";
        private const string EmailBouncedDateMappedName = "EmailBounceDate";

        public ContactConverter()
        {
            AddPropertyConverter(IdPropertyName, s => s.Trim());
            AddPropertyConverter(NextRecordUrlPropertyName, s => s.Trim());

            AddPropertyMapping(EmailBouncedReasonPropertyName, EmailBouncedReasonMappedName);
            AddPropertyMapping(EmailBouncedDatePropertyName, EmailBouncedDateMappedName);
        }

        protected override string LastPropertyName => "Master_Suppressed__c";
    }
}
