using System.Diagnostics.CodeAnalysis;

namespace UAS.UnitTests.ADMS.Services.Validator.Common
{
    [ExcludeFromCodeCoverage]
    public static class Constants
    {
        public const int SourceFileId = 12345;
        public const int ClientId = 25;
        public const int CategoryCodeValue = 10;
        public const int TransactionCodeValue = 12;
        public const string UadWsAddSubscriberFile = "UAD_WS_AddSubscriber";
        public const string API = "API";
        public const string CircWsAddSubscriberFile = "CIRC_WS_AddSubscriber";
        public const int TransformDataMappingId = 1;
        public const int TransformJoinId = 2;
        public const int TransformSplitId = 3;
        public const int TransformAssignId = 4;
        public const int TransformSplitTransId = 5;
        public const int FieldMappingId = 10;
        public const string TransformImportFileData = "TransformImportFileData";
        public const string PubCode = "pub-code";
        public const string IncomingField = "incoming-field";
        public const string RemoveFileFromRepositoryMethod = "RemoveFileFromRepository";
        public const string SetMafFieldRowsForTransformedListMethod = "SetMafFieldRowsForTransformedList";
        public const string SetMafFieldRowsForInvalidListMethod = "SetMafFieldRowsForInvalidList";
        public const string SetIncomeFieldMethod = "SetIncomeField";
        public const string SaveAdmsLogMethod = "SaveAdmsLog";
        public const string SendXmlToDatabaseMethod = "SendXmlToDatabase";
        public const string SetRuleIsQualifiedMethod = "SetRuleIsQualified";
        public const string SetCurrentRowValuesMethod = "SetCurrentRowValues";
    }
}