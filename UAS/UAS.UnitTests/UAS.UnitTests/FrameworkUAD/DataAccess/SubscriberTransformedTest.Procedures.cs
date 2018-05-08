namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    public partial class SubscriberTransformedTest
    {
        private const string ProcSelect = "e_SubscriberTransformed_Select";
        private const string ProcSelectProcessCode = "e_SubscriberTransformed_Select_ProcessCode";
        private const string ProcSelectTopOne = "e_SubscriberTransformed_Select_ProcessCode_TopOne";
        private const string ProcSelectSourceFileId = "e_SubscriberTransformed_Select_SourceFileID";
        private const string ProcSelectDimensionCount = "o_SubscriberTransformed_Select_DimensionCount";
        private const string ProcSelectImportRowNumbers = "o_SubscriberTransformed_ImportRowNumber";
        private const string ProcSelectByAddressValidationSourceFileId = "e_SubscriberTransformed_Select_SourceFileID_IsLatLonValid";
        private const string ProcSelectByAddressValidationProcessCodeSourceFileId = "e_SubscriberTransformed_Select_ProcessCode_SourceFileID_IsLatLonValid";
        private const string ProcSelectByAddressValidationProcessCode = "e_SubscriberTransformed_Select_ProcessCode_IsLatLonValid";
        private const string ProcSelectByAddressValidation = "e_SubscriberTransformed_Select_IsLatLonValid";
        private const string ProcSelectForFileAudit = "e_SubscriberTransformed_SelectForFileAudit";
        private const string ProcAddressValidationPaging = "e_SubscriberTransformed_AddressValidation_Paging";
        private const string ProcSelectForGeoCoding = "e_SubscriberTransformed_SelectForGeoCoding";
        private const string ProcSelectForGeoCodingSourceFileId = "e_SubscriberTransformed_SelectForGeoCoding_SourceFileID";
        private const string ProcCountAddressValidationSourceFileId = "o_CountAddressValidation_SourceFileID_IsLatLonValid";
        private const string ProcCountAddressValidationProcessCode = "o_CountAddressValidation_ProcessCode_IsLatLonValid";
        private const string ProcCountAddressValidation = "o_CountAddressValidation_IsLatLonValid";
        private const string ProcCountForGeoCoding = "o_CountForGeoCoding";
        private const string ProcCountForGeoCodingSourceFileId = "o_CountForGeoCoding_SourceFileID";
        private const string ProcGetDistinctPubCodes = "o_SubscriberTransformed_GetPubCodes";
        private const string ProcAddressUpdateBulkSql = "e_SubscriberTransformed_SaveAddressValidation";
        private const string ProcStandardRollUpToMaster = "job_Standard_RollUpToMaster";
        private const string ProcDataMatching = "job_DataMatching";
        private const string ProcSequenceDataMatching = "job_SequenceDataMatching";
        private const string ProcDataMatchingMultiple = "job_DataMatch_multiple";
        private const string ProcDataMatchingSingle = "job_DataMatch_single";
        private const string ProcAddressValidExisting = "e_SubscriberTransformed_AddressValidateExisting";
        private const string ProcRevertXmlFormattingAfterBulkInsert = "e_SubscriberTransformed_RevertXmlFormattingAfterBulkInsert";
    }
}