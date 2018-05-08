using KMPlatform.Object;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    public partial class SubscriberFinalTests
    {
        private const int Rows = 5;
        private const string ProcessCode = "process-code";
        private const int SourceFileId = 3;
        private const bool IsLatLonValid = true;
        private const bool IsIgnored = true;
        private const string Xml = "xml";
        private const string FileType = "file-type";
        private const string Address = "address";
        private const string Mailstop = "mailstop";
        private const string City = "city";
        private const string State = "state";
        private const string Zip = "zip";
        private const int ProductId = 16;
        private const int BrandId = 17;
        private const int GroupId = 18;
        private const string ProcSelectByAddressValidationSourceId = "e_SubscriberFinal_Select_ProcessCode_SourceFileID_IsLatLonValid";
        private const string ProcSelectByAddressValidation = "e_SubscriberFinal_Select_IsLatLonValid";
        private const string ProcSelect = "e_SubscriberFinal_Select";
        private const string ProcSelectProcessCode = "e_SubscriberFinal_Select_ProcessCode";
        private const string ProcSelectForFileAudit = "e_SubscriberFinal_SelectForFileAudit";
        private const string ProcSelectForFieldUpdate = "e_SubscriberFinal_SelectForFieldUpdate";
        private const string ProcSelectForIgnoredReport = "e_SubscriberFinal_Select_ProcessCode_Ignored";
        private const string ProcSelectRecordIdentifiers = "o_SubscriberFinal_Select_RecordIdentifiers";
        private const string ProcSelectResultCount = "o_SubscriberFinal_Select_AdmsResultCount";
        private const string ProcSelectResultCountAfterProcessToLive = "o_SubscriberFinal_Select_AdmsResultCount_AfterProcessToLive";
        private const string ProcSaveBulkUpdate = "e_SubscriberFinal_SaveBulkUpdate";
        private const string ProcSaveBulkInsert = "e_SubscriberFinal_SaveBulkInsert";
        private const string ProcSaveDqmClean = "e_SubscriberFinal_SaveDQMClean";
        private const string ProcNullifyKmpsGroupEmails = "job_NullifyKMPSGroupEmails";
        private const string ProcSetMissingMaster = "job_SubscriberFinal_SetMissingMaster";
        private const string ProcSetOneMaster = "job_SetOneMaster";
        private const string ProcAddressSearch = "e_SubscriberFinal_AddressSearch";
        private const string ProcSetPubCode = "e_SubscriberFinal_SetPubCode";
        private const string ProcGetEmailListFromEcn = "sp_getEmailsListFromGroup";
        private const string ProcEcnThirdPartySuppresion = "ccp_Meister_Demo35OptOuts";
        private const string ProcEcnOtherProductsSuppression = "ccp_Meister_Demo34OptOuts";
        private static readonly ClientConnections Client = new ClientConnections();
    }
}