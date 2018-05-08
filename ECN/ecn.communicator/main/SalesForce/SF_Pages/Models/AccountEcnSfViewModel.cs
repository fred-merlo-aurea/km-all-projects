namespace ecn.communicator.main.Salesforce.SF_Pages
{
    public class AccountEcnSfViewModel : EcnSfViewModel
    {
        public AccountEcnSfViewModel(int ecnId, string sfId) : base(ecnId, sfId)
        { }

        public ItemViewModel Country { get; set; }
        public ItemViewModel Name { get; set; }
        public ItemViewModel Fax { get; set; }
    }
}