using Core.ADMS.Events;

namespace ADMS.ClientMethods
{
    class Canon : ClientSpecialCommon
    {
        private const string Company2DimensionGroup = "Canon_Company_Company2";
        private const string CompanyTop100DimensionGroup = "Canon_Company_Top100";
        private const string Company2Dim = "company2";
        private const string Company2FieldName = "company2";
        private const string CompanyLookupMatchFieldName = "company";
        private const string CompanyTop100MacthFieldName = "Company";
        private const string Top100Dim = "TOP100";
        private const string Top100DimensionValue = "A";

        public void CanonLookupAdHocImport(KMPlatform.Entity.Client client, FileMoved eventMessage)
        {
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(
                new FillAgGroupAndTableArgs()
                {
                    Client = client,
                    EventMessage = eventMessage,
                    AdHocDimensionGroupName = Company2DimensionGroup,
                    CreatedDimension = Company2Dim,
                    DimensionOperator = ContainsOperation,
                    DimensionValueField = Company2FieldName,
                    MatchValueField = CompanyLookupMatchFieldName,
                    StandardField = CompanyStandardField
                });
        }

        public void CannonTop100AdHocImport(KMPlatform.Entity.Client client, FileMoved eventMessage)
        {
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(
                new FillAgGroupAndTableArgs()
                {
                    Client = client,
                    EventMessage = eventMessage,
                    AdHocDimensionGroupName = CompanyTop100DimensionGroup,
                    CreatedDimension = Top100Dim,
                    DimensionOperator = ContainsOperation,
                    DimensionValue = Top100DimensionValue,
                    MatchValueField = CompanyTop100MacthFieldName,
                    StandardField = CompanyStandardField
                });
        }
    }
}
