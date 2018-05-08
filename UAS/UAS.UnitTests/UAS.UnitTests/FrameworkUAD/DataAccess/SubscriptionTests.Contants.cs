namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    public partial class SubscriptionTests
    {
        private const int Rows = 5;
        private const string Name = "Name";
        private const int SubscriptionID = 2;
        private const string Email = "email";
        private const bool IsClientService = true;
        private const string ProcessCode = "process-code";
        private const int SourceFileId = 6;
        private const int ProductId = 9;
        private const string Fname = "fname";
        private const string Lname = "lname";
        private const string Company = "company";
        private const string Address = "address";
        private const string State = "state";
        private const string Zip = "zip";
        private const string Phone = "phone";
        private const string Title = "title";
        private const string Xml = "xml";
        private const int UserId = 20;
        private const int UpdatedByUserId = 22;
        private const string ClientDisplayName = "client-display-name";
        private const string Add1 = "add1";
        private const string City = "city";
        private const string RegionCode = "region-code";
        private const string Country = "country";
        private const int SequenceId = 28;
        private const string Account = "account";
        private const int PublisherId = 30;
        private const int PublicationId = 31;
        private const string FirstName = "first-name";
        private const string LastName = "last-name";
        private const int Page = 34;
        private const int PageSize = 35;
        private const bool IsLocked = true;
        private const int WaveMailingID = 37;
        private const string Address1 = "address1";
        private const string ZipCode = "zip-code";
        private const string ProcExistsStandardFieldName = "select 1 from sysobjects so join syscolumns sc on so.id = sc.id where (so.name = 'Subscriptions'  or so.name = 'PubSubscriptions'  or so.name = 'QSource' or so.name = 'Transaction' or so.name = 'TransactionGroup' or so.name = 'EmailStatus') and sc.name = @Name";
        private const string ProcSelectSubscriptionId = "e_Subscription_Select_SubscriptionID";
        private const string ProcSelectEmail = "e_Subscription_Select_Email";
        private const string ProcSelect = "e_Subscription_Select";
        private const string ProcSelectInValidAddresses = "e_Subscriptions_GetInValidLatLon";
        private const string ProcSelectIDs = "e_Subscription_SelectIDs";
        private const string ProcSelectForFileAudit = "e_Subscription_SelectForFileAudit";
        private const string ProcFindMatches = "dt_Subscribers_Match";
        private const string ProcAddressUpdate = "e_Subscription_AddressUpdate";
        private const string ProcSave = "e_Subscription_Save";
        private const string ProcNcoaUpdateAddress = "job_NCOA_AddressUpdate";
        private const string ProcUpdateQDate = "e_Subscription_Update_QDate";
        private const string ProcSearch = "e_Subscription_Search_Params";
        private const string ProcSearchSuggestMatch = "e_Subscription_SuggestMatch";
        private const string ProcSelectPaging = "e_Subscription_Select_ProductID_Paging";
        private const string ProcUpdateSubscription = "e_Subscription_Update";
        private const string ProcDeleteSubscription = "e_Subscription_Delete_SubscriptionID";
        private const string ProcSelectPublication = "e_Subscription_Select_PublicationID";
        private const string ProcClearWaveMailingInfo = "e_Subscription_ClearWaveMailingInfo";
        private const string ProcSaveBulkWaveMailing = "e_Subscription_BulkUpdate_WaveMailing";
        private const string ProcSelectSequence = "e_Subscription_Select_SequenceID";
        private const string ProcSelectCount = "e_Subscription_Select_ProductID_Count";
        private const string ProcSearchAddressZip = "e_Subscription_SearchAddressZip";
        private const string ProcSaveBulkActionIDUpdate = "e_Subscription_BulkUpdate_ActionIDs";
    }
}