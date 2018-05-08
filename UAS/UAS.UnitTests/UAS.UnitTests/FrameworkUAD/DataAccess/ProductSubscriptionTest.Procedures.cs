namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    public partial class ProductSubscriptionTest
    {
        private const string ProcSelect = "e_ProductSubscription_Select_SubscriptionID";
        private const string ProcSelectSfRecodeIdentifier = "e_ProductSubscription_Select_SFRecordIdentifier";
        private const string ProcSelectSequenceIdPubId = "e_ProductSubscription_Select_SequenceNum_PubID";
        private const string ProcSelectProductSubscription = "e_ProductSubscription_Select_PubSubscriptionID";
        private const string ProcSelectProcessCode = "e_ProductSubscription_Select_ProcessCode";
        private const string ProcUpdateRequesterFlags = "e_PubSubscriptions_Update_ReqFlags";
        private const string ProcUpdateQDate = "e_ProductSubscription_Update_QDate";
        private const string ProcSearch = "e_ProductSubscription_Search_Params";
        private const string ProcSearchSuggestMatch = "e_ProductSubscription_SuggestMatch";
        private const string ProcSelectPaging = "e_ProductSubscription_Select_ProductID_Paging";
        private const string ProcUpdateSubscription = "e_ProductSubscription_Update";
        private const string ProcDeleteSubscription = "e_ProductSubscription_Delete_ProductSubscriptionID";
        private const string ProcSelectPublication = "e_ProductSubscription_Select_PublicationID";
        private const string ProcClearWaveMailingInfo = "e_ProductSubscription_ClearWaveMailingInfo";
        private const string ProcClearImbSeq = "e_ProductSubscription_Clear_IMB";
        private const string ProcSaveBulkWaveMailing = "e_ProductSubscription_BulkUpdate_WaveMailing";
        private const string ProcSelectSequence = "e_ProductSubscription_Select_SequenceID";
        private const string ProcSelectSequenceWhereClause = "e_ProductSubscription_Select_SequenceIdWhereClause";
        private const string ProcSelectCount = "e_ProductSubscription_Select_ProductID_Count";
        private const string ProcSearchAddressZip = "e_Subscriber_SearchAddressZip";
        private const string ProcSaveBulkActionIdUpdate = "e_ProductSubscription_BulkUpdate_ActionIDs";
        private const string ProcSelectForExport = "e_PubSubscriptions_Select_For_Export";
        private const string ProcSelectPubSubscriptionsForExportStatic = "e_PubSubscriptions_Select_For_Export_Static";
        private const string ProcSelectIssueArchiveSubscriptionForExportStatic = "e_IssueArchiveSubscription_SelectForExportStatic_IssueID";
        private const string ProcSelectForUpdate = "e_ProductSubscription_SelectForUpdate";
        private const string ProcRecordUpdate = "sp_RecordUpdate";
        private const string ProcSelectProductId = "e_ActionProductSubscription_SelectByProductID";
        private const string ProcSelectProductIdIssueId = "e_ActionProductSubscription_SelectByProductID_IssueID";
        private const string ProcSelectAllActiveIDs = "e_ProductSubscriptions_Select_AllActiveIDs";
        private const string ProcCountryRegionCleanse = "job_CountryRegionCleanse";
        private const string MethodGetIntList = "GetIntList";
        private const string ProcUpdateMasterDb = "e_ImportFromUAS";
    }
}
