namespace ECN_Framework_Entities.Salesforce.Convertors
{
    public class UserConverter : EntityConverterBase
    {
        private const string IdPropertyName = "Id";

        public UserConverter()
        {
            AddPropertyConverter(IdPropertyName, s => s.Trim());
        }

        protected override string LastPropertyName => "Username";
    }
}
