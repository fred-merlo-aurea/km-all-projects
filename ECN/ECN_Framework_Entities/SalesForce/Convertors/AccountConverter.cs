using ECN_Framework_Common.Functions;

namespace ECN_Framework_Entities.Salesforce.Convertors
{
    public class AccountConverter : EntityConverterBase
    {
        private const string IdPropertyName = "Id";
        private const string BillingStreetPropertyName = "BillingStreet";

        public AccountConverter() : base()
        {
            AddPropertyConverter(IdPropertyName, s => s.Trim());
            AddPropertyConverter(BillingStreetPropertyName, s => JsonFunctions.GetNormalizedString(s));
        }

        protected override string LastPropertyName => "JigsawCompanyId";
    }
}
