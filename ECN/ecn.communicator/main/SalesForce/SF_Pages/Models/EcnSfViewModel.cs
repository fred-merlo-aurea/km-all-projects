namespace ecn.communicator.main.Salesforce.SF_Pages
{
    public class EcnSfViewModel
    {
        public EcnSfViewModel(int ecnId, string sfId)
        {
            EcnId = ecnId.ToString();
            SfId = sfId;
        }

        public string EcnId { get; private set; }
        public string SfId { get; private set; }
        public ItemViewModel CellPhone { get; set; }
        public ItemViewModel Phone { get; set; }
        public ItemViewModel FirstName { get; set; }
        public ItemViewModel LastName { get; set; }
        public ItemViewModel Email { get; set; }
        public ItemViewModel Address { get; set; }
        public ItemViewModel City { get; set; }
        public ItemViewModel State { get; set; }
        public ItemViewModel Zip { get; set; }
    }
}