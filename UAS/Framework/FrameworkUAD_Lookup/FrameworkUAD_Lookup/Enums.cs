using System;
using System.Runtime.Serialization;


namespace FrameworkUAD_Lookup
{
    public class Enums
    {
        #region UAD_Lookup tables
        #region CodeTypes
        [DataContract]
        public enum CodeType
        {
            [EnumMember]
            ACS_Type,
            [EnumMember]
            ACS_Action,
            [EnumMember]
            Action,
            [EnumMember]
            Address,
            [EnumMember]
            Address_Update_Source,
            [EnumMember]
            ADMS_Step,
            [EnumMember]
            Authorization_Mode,
            [EnumMember]
            Color_Wheel,
            [EnumMember]
            Configuration,
            [EnumMember]
            Custom_Import_Rule,
            [EnumMember]
            Credit_Card,
            [EnumMember]
            Database_Destination,
            [EnumMember]
            Database_File,
            [EnumMember]
            Data_Compare_Target,
            [EnumMember]
            Data_Compare_Cost,
            [EnumMember]
            Demographic_Custom_Attributes,
            [EnumMember]
            Demographic_Premium_Attributes,
            [EnumMember]
            Demographic_Standard_Attributes,
            [EnumMember]
            Demographic_Update,
            [EnumMember]
            Execution_Points,
            [EnumMember]
            Export,
            [EnumMember]
            Export_Format,
            [EnumMember]
            Field_Data_Type,
            [EnumMember]
            Field_Mapping,
            [EnumMember]
            File_Snippet,
            [EnumMember]
            File_Status,
            [EnumMember]
            File_Recurrence,
            [EnumMember]
            Filter_Type,
            [EnumMember]
            Mail_Message,
            [EnumMember]
            Marketing,
            [EnumMember]
            Match,
            [EnumMember]
            Operators,
            [EnumMember]
            Par3c,
            [EnumMember]
            Payment_Status,
            [EnumMember]
            Payment,
            [EnumMember]
            Postal_Service,
            [EnumMember]
            Processing_Status,
            [EnumMember]
            Procedure,
            [EnumMember]
            Profile_Premium_Attributes,
            [EnumMember]
            Profile_Standard_Attributes,
            [EnumMember]
            Qualification_Source,
            [EnumMember]
            Qualification_Source_Type,
            [EnumMember]
            Recurrence,
            [EnumMember]
            Report,
            [EnumMember]
            Requester_Flag,
            [EnumMember]
            Response_Group,
            [EnumMember]
            Rule_Action,
            [EnumMember]
            Rule_Chain,
            [EnumMember]
            Service_Response_Status,
            [EnumMember]
            Severity,
            [EnumMember]
            Special_File_Result,
            [EnumMember]
            Subscriber_Source,
            [EnumMember]
            Transformation,
            [EnumMember]
            User_Log,
            [EnumMember]
            Issue_Permission,
            [EnumMember]
            Filter_Group,
            [EnumMember]
            Report_Expression,
            [EnumMember]
            Deliver,
            [EnumMember]
            Dimension,
            [EnumMember]
            Data_Compare_Type,
            [EnumMember]
            Record_Process_Time,
            [EnumMember]
            UX_Control,
            [EnumMember]
            UAD_Field_Type
        }
        public static CodeType GetCodeType(string codeType)
        {
            try
            {
                codeType = codeType.Replace(" ", "_");
                return (CodeType) System.Enum.Parse(typeof(CodeType), codeType, true);
            }
            catch { return CodeType.Configuration; }
        }
        #endregion

        #region Codes - for CodeTypes
        [DataContract]
        public enum ACS_Actions
        {
            Kill_Address,
            Update_Address
        }
        public static ACS_Actions GetACS_Actions(string acsAction)
        {
            try
            {
                acsAction = acsAction.Replace(" ", "_");
                return (ACS_Actions) System.Enum.Parse(typeof(ACS_Actions), acsAction, true);
            }
            catch { return ACS_Actions.Kill_Address; }
        }
        [DataContract]
        public enum ACS_Types
        {
            Traditional,
            OneCode,
            Full_Service,
            Intelligent_Mail_Package_Barcode
        }
        public static ACS_Types GetACS_Types(string acsType)
        {
            try
            {
                acsType = acsType.Replace(" ", "_");
                return (ACS_Types) System.Enum.Parse(typeof(ACS_Types), acsType, true);
            }
            catch { return ACS_Types.Intelligent_Mail_Package_Barcode; }
        }
        [DataContract]
        public enum ActionTypes
        {
            [EnumMember]
            Data_Entry,
            [EnumMember]
            System_Generated
        }
        [DataContract]
        public enum AddressTypes
        {
            [EnumMember]
            Residential,
            [EnumMember]
            Residential_Business,
            [EnumMember]
            Business,
            [EnumMember]
            International
        }
        [DataContract]
        public enum UADFieldType
        {
            [EnumMember]
            Profile,
            [EnumMember]
            Custom,
            [EnumMember]
            Dimension,
            [EnumMember]
            Adhoc
        }
        
        public static AddressTypes GetAddressTypes(string xType)
        {
            try
            {
                xType = xType.Replace(" ", "_");
                return (AddressTypes) System.Enum.Parse(typeof(AddressTypes), xType, true);
            }
            catch { return AddressTypes.Residential; }
        }
        [DataContract]
        public enum AddressUpdateSourceTypes
        {
            [EnumMember]
            ACS,
            [EnumMember]
            NCOA,
            [EnumMember]
            Data_Entry,
            [EnumMember]
            File,
            [EnumMember]
            Other,
            [EnumMember]
            Geocoding
        }
        public static AddressUpdateSourceTypes GetAddressUpdateSourceTypes(string xType)
        {
            try
            {
                xType = xType.Replace(" ", "_");
                return (AddressUpdateSourceTypes) System.Enum.Parse(typeof(AddressUpdateSourceTypes), xType, true);
            }
            catch { return AddressUpdateSourceTypes.Data_Entry; }
        }
        [DataContract]
        public enum ADMS_StepType
        {
            [EnumMember]
            Downloading_File,//first step
            [EnumMember]
            File_Name_Check,
            [EnumMember]
            Executing_Custom_Code,
            [EnumMember]
            Validator_Enter,
            [EnumMember]
            Validator_Api_Begin,
            [EnumMember]
            Validator_End,
            [EnumMember]
            Validator_Set_Variables,
            [EnumMember]
            Validator_Data_Validation,
            [EnumMember]
            Validator_Create_Original_Subscribers,
            [EnumMember]
            Validator_Create_Transformed_Subscribers,
            [EnumMember]
            Validator_Transformations,
            [EnumMember]
            Validator_Dedupe_Subscribers,
            [EnumMember]
            Validator_Inserting_Original_Subscribers,
            [EnumMember]
            Validator_Api_Validation,
            [EnumMember]
            Validator_Api_End,
            [EnumMember]
            Address_Clean,
            [EnumMember]
            GeoCoding,
            [EnumMember]
            Suppression,
            [EnumMember]
            Processed,
            [EnumMember]
            Watching_for_File,
            [EnumMember]
            Qued
        }
        public static ADMS_StepType GetADMS_Step(string status)
        {
            try
            {
                status = status.Replace(" ", "_");
                return (ADMS_StepType) System.Enum.Parse(typeof(ADMS_StepType), status, true);
            }
            catch { return ADMS_StepType.Downloading_File; }
        }
        [DataContract]
        [Flags]
        public enum AuthorizationModeTypes
        {
            [EnumMember]
            User_Name_Password,
            [EnumMember]
            Access_Key
        }
        [DataContract]
        public enum Color_Wheel
        {
            [EnumMember]
            White,
            [EnumMember]
            Silver,
            [EnumMember]
            Gray,
            [EnumMember]
            Black,
            [EnumMember]
            Red,
            [EnumMember]
            Maroon,
            [EnumMember]
            Yellow,
            [EnumMember]
            Olive,
            [EnumMember]
            Lime,
            [EnumMember]
            Green,
            [EnumMember]
            Aqua,
            [EnumMember]
            Teal,
            [EnumMember]
            Blue,
            [EnumMember]
            Navy,
            [EnumMember]
            Fuchsia,
            [EnumMember]
            Purple,
            [EnumMember]
            Orange
        }
        [DataContract]
        public enum ConfigurationTypes
        {
            [EnumMember]
            USPS_URL,
            [EnumMember]
            USPS_User_Name,
            [EnumMember]
            USPS_Password,
            [EnumMember]
            ACS_Account_Number,
            [EnumMember]
            ACS_Account_Password
        }
        public static ConfigurationTypes GetConfigurationTypes(string configurationType)
        {
            try
            {
                configurationType = configurationType.Replace(" ", "_");
                return (ConfigurationTypes) System.Enum.Parse(typeof(ConfigurationTypes), configurationType, true);
            }
            catch { return ConfigurationTypes.USPS_URL; }
        }
        [DataContract]
        public enum CreditCardTypes
        {
            [EnumMember]
            American_Express,
            [EnumMember]
            Discover,
            [EnumMember]
            Master_Card,
            [EnumMember]
            Visa
        }
        [DataContract]
        public enum CustomImportRule
        {
            [EnumMember]
            Insert,
            [EnumMember]
            Update,
            [EnumMember]
            Delete,
            [EnumMember]
            ADMS
        }
        public static CustomImportRule GetCustomImportRule(string cir)
        {
            try
            {
                cir = cir.Replace(" ", "_");
                return (CustomImportRule) System.Enum.Parse(typeof(CustomImportRule), cir, true);
            }
            catch { return CustomImportRule.Insert; }
        }
        
        [DataContract]
        public enum DataCompareOptions
        {
            Summary_Report,
            Matching_Profiles,
            Matching_Demographics,
            Like_Profiles,
            Like_Demographics,
            Other_Profiles,
            Other_Demographics,
            No_Data
        }
        public static DataCompareOptions GetDataCompareOption(string dataCompareOption)
        {
            try
            {
                dataCompareOption = dataCompareOption.Replace(" ", "_");
                return (DataCompareOptions) System.Enum.Parse(typeof(DataCompareOptions), dataCompareOption, true);
            }
            catch { return DataCompareOptions.Summary_Report; }
        }
        [DataContract]
        public enum DataCompareType
        {
            Match,
            Like
        }
        public static DataCompareType GetDataCompareType(string dcType)
        {
            try
            {
                dcType = dcType.Replace(" ", "_");
                return (DataCompareType) System.Enum.Parse(typeof(DataCompareType), dcType, true);
            }
            catch { return DataCompareType.Match; }
        }
        [DataContract]
        public enum DataCompareCost
        {
            MergePurge,
            Download
        }
        public static DataCompareCost GetDataCompareCost(string cost)
        {
            try
            {
                cost = cost.Replace(" ", "_");
                return (DataCompareCost) System.Enum.Parse(typeof(DataCompareCost), cost, true);
            }
            catch { return DataCompareCost.MergePurge; }
        }
        [DataContract]
        public enum DataCompareTargetTypes
        {
            [EnumMember]
            Product,
            [EnumMember]
            Brand,
            [EnumMember]
            Market,
            [EnumMember]
            Consensus
        }
        public static DataCompareTargetTypes GetDataCompareTargetType(string dataCompareTargetType)
        {
            try
            {
                dataCompareTargetType = dataCompareTargetType.Replace(" ", "_");
                return (DataCompareTargetTypes) System.Enum.Parse(typeof(DataCompareTargetTypes), dataCompareTargetType, true);
            }
            catch { return DataCompareTargetTypes.Consensus; }
        }
        [DataContract]
        public enum DatabaseDestinationTypes
        {
            [EnumMember]
            UAS,
            [EnumMember]
            UAD,
            [EnumMember]
            Circ,
            [EnumMember]
            UAD_Circ
        }
        [DataContract]
        public enum DeliverTypes
        {
            [EnumMember]
            Print,
            [EnumMember]
            Digital,
            [EnumMember]
            Both
        }
        [DataContract]
        public enum DemographicUpdate
        {
            [EnumMember]
            Replace,
            [EnumMember]
            Append,
            [EnumMember]
            Overwrite
        }
        public static DemographicUpdate GetDemographicUpdate(string demographicUpdate)
        {
            try
            {
                demographicUpdate = demographicUpdate.Replace(" ", "_");
                return (DemographicUpdate) System.Enum.Parse(typeof(DemographicUpdate), demographicUpdate, true);
            }
            catch { return DemographicUpdate.Replace; }
        }
        [DataContract]
        public enum DimensionTypes
        {
            [EnumMember]
            Response_Group,
            [EnumMember]
            Profile_Field
        }
        [DataContract]
        public enum ExecutionPointType
        {
            [EnumMember]
            Pre_File_Moved,
            [EnumMember]
            Post_File_Moved,
            [EnumMember]
            Pre_Validation_Process,
            [EnumMember]
            Post_Validation_Process,
            [EnumMember]
            Pre_Codesheet_Validation,
            [EnumMember]
            Post_Codesheet_Validation,
            [EnumMember]
            Pre_Column_Checks,
            [EnumMember]
            Post_Column_Checks,
            [EnumMember]
            Pre_Transformations,
            [EnumMember]
            Post_Transformations,
            [EnumMember]
            Pre_DQM,
            [EnumMember]
            Post_DQM,
            [EnumMember]
            ADMS_ClientMethods,
            [EnumMember]
            Pre_DataMatch,
            [EnumMember]
            Post_DataMatch,
            [EnumMember]
            Pre_Suppression,
            [EnumMember]
            Post_Suppression,
            [EnumMember]
            Pre_UAD_Import,
            [EnumMember]
            Post_UAD_Import,
            [EnumMember]
            Pre_ApiSaveSubscriber,
            [EnumMember]
            Post_ApiSaveSubscriber,
            [EnumMember]
            Pre_ApiValidation,
            [EnumMember]
            QDateDefault,
            [EnumMember]
            RequireValidEmail,
            [EnumMember]
            QualifiedProfile,
            [EnumMember]
            ConsensusDataRollup,
            [EnumMember]
            DataMatch,
            [EnumMember]
            Custom_Import_Rule
        }
        public static ExecutionPointType GetExecutionPointTypes(string executionPointType)
        {
            try
            {
                executionPointType = executionPointType.Replace(" ", "_");
                return (ExecutionPointType) System.Enum.Parse(typeof(ExecutionPointType), executionPointType, true);
            }
            catch { return ExecutionPointType.Pre_File_Moved; }
        }
        [DataContract]
        public enum ExportTypes
        {
            [EnumMember]
            FTP_Export,
            [EnumMember]
            ECN_Export,
            [EnumMember]
            Campaign_Export
        }
        [DataContract]
        public enum ExportFormatTypes
        {
            [EnumMember]
            csv,
            [EnumMember]
            txt,
            [EnumMember]
            xls,
            [EnumMember]
            xlsx,
            [EnumMember]
            pdf,
            [EnumMember]
            doc,
            [EnumMember]
            xml,
            [EnumMember]
            json,
            [EnumMember]
            html
        }
        [DataContract]
        public enum FileTypes
        {
            [EnumMember]
            ACS,
            [EnumMember]
            Other,
            [EnumMember]
            Audience_Data,
            [EnumMember]
            NCOA,
            [EnumMember]
            Telemarketing_Long_Form,
            [EnumMember]
            Telemarketing_Short_Form,
            [EnumMember]
            Complimentary,
            [EnumMember]
            Data_Compare,
            [EnumMember]
            Web_Forms,
            [EnumMember]
            List_Source_2YR,
            [EnumMember]
            List_Source_3YR,
            [EnumMember]
            List_Source_Other,
            [EnumMember]
            Field_Update,
            [EnumMember]
            QuickFill,
            [EnumMember]
            Paid_Transaction,
            [EnumMember]
            Web_Forms_Short_Form,
            [EnumMember]
            API
        }
        public static FileTypes GetDatabaseFileType(string databaseFileType)
        {
            try
            {
                databaseFileType = databaseFileType.Replace(" ", "_");
                return (FileTypes) System.Enum.Parse(typeof(FileTypes), databaseFileType, true);
            }
            catch { return FileTypes.Audience_Data; }
        }
        [DataContract]
        public enum FieldDataType
        {
            [EnumMember]
            Int,
            [EnumMember]
            String,
            [EnumMember]
            Date,
            [EnumMember]
            Datetime,
            [EnumMember]
            Time,
            [EnumMember]
            Float,
            [EnumMember]
            Decimal,
            [EnumMember]
            Bit,
            [EnumMember]
            Lookup,
            [EnumMember]
            Demo

        }
        public static FieldDataType GetFieldDataType(string fieldDataType)
        {
            try
            {
                fieldDataType = fieldDataType.Replace(" ", "_");
                return (FieldDataType) System.Enum.Parse(typeof(FieldDataType), fieldDataType, true);
            }
            catch { return FieldDataType.String; }
        }

        [DataContract]
        public enum FieldType
        {
            [EnumMember]
            Profile,
            [EnumMember]
            Demo,
            [EnumMember]
            Custom,
            [EnumMember]
            Lookup_State,
            [EnumMember]
            Lookup_Country,
            [EnumMember]
            Lookup_Code,
            [EnumMember]
            Lookup_Category,
            [EnumMember]
            Lookup_Transaction
        }
        public static FieldType GetFieldType(string fieldType)
        {
            try
            {
                fieldType = fieldType.Replace(" ", "_");
                return (FieldType) System.Enum.Parse(typeof(FieldType), fieldType, true);
            }
            catch { return FieldType.Profile; }
        }

        [DataContract]
        public enum FieldMappingTypes
        {
            [EnumMember]
            Ignored,
            [EnumMember]
            Standard,
            [EnumMember]
            Demographic,
            [EnumMember]
            Demographic_Other,
            [EnumMember]
            Demographic_Date,
            [EnumMember]
            kmTransform
        }
        [DataContract]
        public enum FileSnippetTypes
        {
            [EnumMember]
            Prefix
        }
        [DataContract]
        public enum FileStatusType
        {
            [EnumMember]
            Detected,
            [EnumMember]
            Invalid,
            [EnumMember]
            Processing,
            [EnumMember]
            Qued,
            [EnumMember]
            Failed,
            [EnumMember]
            Completed,
            [EnumMember]
            ApiProcessing
        }
        public static FileStatusType GetFileStatusType(string fileStatusType)
        {
            try
            {
                fileStatusType = fileStatusType.Replace(" ", "_");
                return (FileStatusType) System.Enum.Parse(typeof(FileStatusType), fileStatusType, true);
            }
            catch { return FileStatusType.Detected; }
        }
        [DataContract]
        public enum FileRecurrenceTypes
        {
            [EnumMember]
            One_Time,
            [EnumMember]
            Recurring
        }
        public static FileRecurrenceTypes GetFileRecurrenceTypeName(string sourceFileType)
        {
            try
            {
                sourceFileType = sourceFileType.Replace(" ", "_");
                return (FileRecurrenceTypes) System.Enum.Parse(typeof(FileRecurrenceTypes), sourceFileType, true);
            }
            catch { return FileRecurrenceTypes.One_Time; }
        }
        [DataContract]
        public enum FilterTypes
        {
            [EnumMember]
            Standard,
            [EnumMember]
            AdHoc,
            [EnumMember]
            Activity,
            [EnumMember]
            Dynamic
        }
        [DataContract]
        public enum FilterGroupTypes
        {
            [EnumMember]
            Circulation,
            [EnumMember]
            Product_View,
            [EnumMember]
            Consensus_View,
            [EnumMember]
            Market_View,
            [EnumMember]
            Add_Remove,
            [EnumMember]
            Issue_Splits
        }
        [DataContract]
        public enum IssuePermissionTypes
        {
            [EnumMember]
            Data_Entry,
            [EnumMember]
            External_Import,
            [EnumMember]
            Internal_Import,
            [EnumMember]
            Add_Remove
        }
        //Mail_Message
        [DataContract]
        public enum MailMessageType
        {
            [EnumMember]
            DataCompareComplete,
            [EnumMember]
            DataCompareEstimatedCompletionTime,
            [EnumMember]
            RejectFileErrorLimitExceeded,
            [EnumMember]
            EmailRejectCircFileLockedProducts,
            [EnumMember]
            EmailRejectCircFileTelemarketingMultipleProducts,
            [EnumMember]
            FileNotDefined,
            [EnumMember]
            RejectFile,
            [EnumMember]
            EmailFieldUpdateIssues,
            [EnumMember]
            SubGenAddressesNotAbleToUpdate,
            [EnumMember]
            ACSFileComplete,
            [EnumMember]
            NCOAFileComplete,
            [EnumMember]
            SendFileNotValidMessage,
            [EnumMember]
            EmailError,
            [EnumMember]
            Null
        }
        public static MailMessageType GetMailMessageType(string mailMessageType)
        {
            try
            {
                return (MailMessageType) System.Enum.Parse(typeof(MailMessageType), mailMessageType, true);
            }
            catch { return MailMessageType.Null; }
        }
        [DataContract]
        public enum Marketing
        {
            [EnumMember]
            Phone,
            [EnumMember]
            Text,
            [EnumMember]
            Email,
            [EnumMember]
            Fax,
            [EnumMember]
            Mail,
            [EnumMember]
            Advertising,
            [EnumMember]
            Trade_Shows,
            [EnumMember]
            XProduct
        }
        public static Marketing GetMarketing(string marketingID)
        {
            try
            {
                return (Marketing) System.Enum.Parse(typeof(Marketing), marketingID, true);
            }
            catch { return Marketing.Phone; }
        }
        [DataContract]
        public enum MatchTypes
        {
            [EnumMember]
            Any_Character,
            [EnumMember]
            Equals,
            [EnumMember]
            Not_Equals,
            [EnumMember]
            Like,
            [EnumMember]
            Has_Data,
            [EnumMember]
            Is_Null_or_Empty,
            [EnumMember]
            Default,
            [EnumMember]
            Find_and_Replace
        }
        public static MatchTypes GetMatchTypeName(string matchTypeName)
        {
            try
            {
                matchTypeName = matchTypeName.Replace(" ", "_");
                return (MatchTypes) System.Enum.Parse(typeof(MatchTypes), matchTypeName, true);
            }
            catch { return MatchTypes.Equals; }
        }
        [DataContract]
        public enum OperatorTypes
        {
            [EnumMember]
            add,
            [EnumMember]
            and,
            [EnumMember]
            contains,
            [EnumMember]
            date_range,
            [EnumMember]
            divide,
            [EnumMember]
            ends_with,
            [EnumMember]
            equal,
            [EnumMember]
            greater_than,
            [EnumMember]
            greater_than_or_equal_to,
            [EnumMember]
            in_,
            [EnumMember]
            is_,
            [EnumMember]
            is_empty,
            [EnumMember]
            is_not,
            [EnumMember]
            is_not_empty,
            [EnumMember]
            is_not_greater_than,
            [EnumMember]
            is_not_less_than,
            [EnumMember]
            is_not_null,
            [EnumMember]
            is_null,
            [EnumMember]
            xdays,
            [EnumMember]
            less_than,
            [EnumMember]
            less_than_or_equal_to,
            [EnumMember]
            modulus,
            [EnumMember]
            multiply,
            [EnumMember]
            no,
            [EnumMember]
            not_contain,
            [EnumMember]
            not_equal,
            [EnumMember]
            not_in,
            [EnumMember]
            or,
            [EnumMember]
            range,
            [EnumMember]
            starts_with,
            [EnumMember]
            subtract,
            [EnumMember]
            year,
            [EnumMember]
            yes

        }
        public static OperatorTypes GetOperatorType(string operatorText)
        {
            try
            {
                operatorText = operatorText.Replace(" ", "_");
                return (OperatorTypes) System.Enum.Parse(typeof(OperatorTypes), operatorText, true);
            }
            catch { return OperatorTypes.is_; }
        }
        [DataContract]
        public enum Par3c
        {
            [EnumMember]
            Individual_By_Name_And_Title_AndOr_Function,
            [EnumMember]
            Individuals_By_Name_Only,
            [EnumMember]
            Titles_Or_Functions_Only,
            [EnumMember]
            Company_Names_Only,
            [EnumMember]
            Bulk_Copies
        }
        [DataContract]
        public enum PaymentStatus
        {
            [EnumMember]
            Paid,
            [EnumMember]
            Unpaid,
            [EnumMember]
            Pending,
            [EnumMember]
            Non_Billed,
            [EnumMember]
            Cancelled
        }
        public static PaymentStatus GetPaymentStatus(string status)
        {
            try
            {
                status = status.Replace(" ", "_");
                return (PaymentStatus) System.Enum.Parse(typeof(PaymentStatus), status, true);
            }
            catch { return PaymentStatus.Pending; }
        }
        [DataContract]
        [Serializable]
        public enum PaymentType
        {
            [EnumMember]
            Check,
            [EnumMember]
            Credit_Card,
            [EnumMember]
            Bill_Me_Later,
            [EnumMember]
            Cash,
            [EnumMember]
            Other,
            [EnumMember]
            PayPal,
            [EnumMember]
            Imported
        }
        public static PaymentType GetPaymentType(string paymentType)
        {
            try
            {
                paymentType = paymentType.Replace(" ", "_");
                return (PaymentType) System.Enum.Parse(typeof(PaymentType), paymentType, true);
            }
            catch { return PaymentType.Credit_Card; }
        }
        [DataContract]
        public enum PostalServiceTypes
        {
            [EnumMember]
            ACS_Code,
            [EnumMember]
            Intelligent_Mail_Barcode
        }
        [DataContract]
        public enum ProcessingStatusType
        {
            [EnumMember]
            Detected,
            [EnumMember]
            Loading,
            [EnumMember]
            Ready,
            [EnumMember]
            Invalid,
            [EnumMember]
            In_Validation,
            [EnumMember]
            Validated,
            [EnumMember]
            In_Transformations,
            [EnumMember]
            Transformed,
            [EnumMember]
            In_Cleansing,
            [EnumMember]
            Cleansed,
            [EnumMember]
            In_CreatingSubscribers,
            [EnumMember]
            CreatedSubscribers,
            [EnumMember]
            In_DedupeSubscribers,
            [EnumMember]
            In_Matching,
            [EnumMember]
            Matched,
            [EnumMember]
            GeoCode,
            [EnumMember]
            Suppression,
            [EnumMember]
            Custom_File_Processed,
            [EnumMember]
            ApiValidated,
            [EnumMember]
            In_ApiMethod,
            [EnumMember]
            In_ApiValidation,
            [EnumMember]
            Processed,
            [EnumMember]
            Qued,
            [EnumMember]
            Completed,
            [EnumMember]
            Watching_for_File//final step
        }
        public static ProcessingStatusType GetProcessingStatusType(string status)
        {
            try
            {
                status = status.Replace(" ", "_");
                return (ProcessingStatusType) System.Enum.Parse(typeof(ProcessingStatusType), status, true);
            }
            catch { return ProcessingStatusType.Detected; }
        }
        [DataContract]
        public enum ProcedureTypes
        {
            [EnumMember]
            SQL,
            [EnumMember]
            NET
        }
        public static ProcedureTypes GetProcedureType(string procedureType)
        {
            try
            {
                procedureType = procedureType.Replace(" ", "_");
                return (ProcedureTypes) System.Enum.Parse(typeof(ProcedureTypes), procedureType, true);
            }
            catch { return ProcedureTypes.NET; }
        }
        [DataContract]
        public enum QualificationSourceTypes
        {
            [EnumMember]
            Personal,
            [EnumMember]
            Company,
            [EnumMember]
            Association,
            [EnumMember]
            Communication,
            [EnumMember]
            Other
        }
        [DataContract]
        public enum QualificationSource
        {
            [EnumMember]
            Personal_Written,
            [EnumMember]
            Personal_Telecom,
            [EnumMember]
            Personal_Email,
            [EnumMember]
            Company_Written,
            [EnumMember]
            Company_Telecom,
            [EnumMember]
            Company_Email,
            [EnumMember]
            Association_Individual,
            [EnumMember]
            Association_Organization,
            [EnumMember]
            Communication_Written,
            [EnumMember]
            Communication_Telecom,
            [EnumMember]
            Communication_Email,
            [EnumMember]
            Other_Roster,
            [EnumMember]
            Other_Directory,
            [EnumMember]
            Other_Report,
            [EnumMember]
            Other_License,
            [EnumMember]
            Other_List,
            [EnumMember]
            Other_Other
        }
        [DataContract]
        public enum RecordProcessTimeType
        {
            [EnumMember]
            RecordProcessTime_0_15K,
            [EnumMember]
            RecordProcessTime_15_50K,
            [EnumMember]
            RecordProcessTime_50_100K,
            [EnumMember]
            RecordProcessTime_100_Max
        }
        [DataContract]
        public enum RecordType
        {
            [EnumMember]
            New,
            [EnumMember]
            Matched,
            [EnumMember]
            Existing,
            [EnumMember]
            All_File
        }
        public static RecordType GetRecordType(string recordType)
        {
            try
            {
                recordType = recordType.Replace(" ", "_");
                return (RecordType) System.Enum.Parse(typeof(RecordType), recordType, true);
            }
            catch { return RecordType.New; }
        }
        [DataContract]
        public enum RecurrenceTypes
        {
            [EnumMember]
            Daily,
            [EnumMember]
            Weekly,
            [EnumMember]
            BiWeekly,
            [EnumMember]
            Monthly,
            [EnumMember]
            Quarterly,
            [EnumMember]
            Yearly
        }
        [DataContract]
        public enum ReportExpressionTypes
        {
            [EnumMember]
            INTERSECT,
            [EnumMember]
            UNION,
            [EnumMember]
            NOT_IN,
            [EnumMember]
            ALL_Intersect,
            [EnumMember]
            ALL_Union
        }
        [DataContract]
        public enum RequesterFlagTypes
        {
            [EnumMember]
            Requested,
            [EnumMember]
            Non_Requested,
            [EnumMember]
            Advertiser
        }
        [DataContract]
        public enum ResponseGroupTypes
        {
            [EnumMember]
            Circ_Only,
            [EnumMember]
            UAD_Only,
            [EnumMember]
            Circ_and_UAD
        }
        public static ResponseGroupTypes GetResponseGroupTypes(string responseGroupType)
        {
            try
            {
                responseGroupType = responseGroupType.Replace(" ", "_");
                return (ResponseGroupTypes) System.Enum.Parse(typeof(ResponseGroupTypes), responseGroupType, true);
            }
            catch { return ResponseGroupTypes.Circ_and_UAD; }
        }
        [DataContract]
        public enum ReportURL
        {
            [EnumMember]
            BPA_Report,
            [EnumMember]
            Add_Remove,
            [EnumMember]
            geobreakdown_demographics,
            [EnumMember]
            geoBreakdown,
            [EnumMember]
            SubFields,
            [EnumMember]
            geoBreakdown_FCI,
            [EnumMember]
            geoBreakdown_c,
            [EnumMember]
            CategorySummary,
            [EnumMember]
            subsrc,
            [EnumMember]
            CrossTabReport,
            [EnumMember]
            CrossTab,
            [EnumMember]
            DemoXQualification,
            [EnumMember]
            ListReport,
            [EnumMember]
            QSourceBreakdown,
            [EnumMember]
            Par3c,
            [EnumMember]
            geoBreakdown_d
        }
        [DataContract]
        public enum RuleAction
        {
            [EnumMember]
            Import,
            [EnumMember]
            Do_Not_Import,
            [EnumMember]
            Update_New,
            [EnumMember]
            Delete,
            [EnumMember]
            Delete_All,
            [EnumMember]
            Update_Existing_and_File,
            [EnumMember]
            Update_Existing,
            [EnumMember]
            Update_File,
            [EnumMember]
            Update_Existing_All,
            [EnumMember]
            Update_File_All,
            [EnumMember]
            Update_All,
            [EnumMember]
            Update

        }
        public static RuleAction GetRuleAction(string ra)
        {
            try
            {
                ra = ra.Replace(" ", "_");
                return (RuleAction) System.Enum.Parse(typeof(RuleAction), ra, true);
            }
            catch { return RuleAction.Import; }
        }
        [DataContract]
        public enum RuleQDateOption
        {
            [EnumMember]
            CurrentDate,
            [EnumMember]
            FirstOfCurrentMonth,
            [EnumMember]
            FirstOfPreviousMonth,
            [EnumMember]
            FirstOfNextMonth,
            [EnumMember]
            Blank
        }
        public static RuleQDateOption GetRuleQDateOption(string ruleQDateOption)
        {
            try
            {
                ruleQDateOption = ruleQDateOption.Replace(" ", "_");
                return (RuleQDateOption) System.Enum.Parse(typeof(RuleQDateOption), ruleQDateOption, true);
            }
            catch { return RuleQDateOption.CurrentDate; }
        }
        [DataContract]
        public enum RuleValueType
        {
            [EnumMember]
            FreeText,
            [EnumMember]
            Dropdown,
            [EnumMember]
            Calendar,
            [EnumMember]
            Numeric,
            [EnumMember]
            none
        }
        public static RuleValueType GetRuleValueType(string ruleValueType)
        {
            try
            {
                ruleValueType = ruleValueType.Replace(" ", "_");
                return (RuleValueType) System.Enum.Parse(typeof(RuleValueType), ruleValueType, true);
            }
            catch { return RuleValueType.none; }
        }
        [DataContract]
        public enum RuleChain
        {
            [EnumMember]
            And,
            [EnumMember]
            Or,
            [EnumMember]
            BreakGreaterThanZero,
            [EnumMember]
            BreakZero,
            [EnumMember]
            BreakTrue,
            [EnumMember]
            BreakFalse,
            [EnumMember]
            BreakAlways
        }
        public static RuleChain GetRuleChain(string ruleChain)
        {
            try
            {
                ruleChain = ruleChain.Replace(" ", "_");
                return (RuleChain) System.Enum.Parse(typeof(RuleChain), ruleChain, true);
            }
            catch { return RuleChain.BreakAlways; }
        }
        [DataContract]
        public enum RuleSystem
        {
            [EnumMember]
            CurrentDate,
            [EnumMember]
            FirstOfCurrentMonth,
            [EnumMember]
            FirstOfPreviousMonth,
            [EnumMember]
            FirstOfNextMonth,
            [EnumMember]
            Blank,
            [EnumMember]
            RequireValidEmail,
            [EnumMember]
            QualifiedName,
            [EnumMember]
            QualifiedAddress,
            [EnumMember]
            QualifiedPhone,
            [EnumMember]
            QualifiedCompany,
            [EnumMember]
            QualifiedTitle,
            [EnumMember]
            MailPermissionOverRide,
            [EnumMember]
            FaxPermissionOverRide,
            [EnumMember]
            PhonePermissionOverRide,
            [EnumMember]
            OtherProductsPermissionOverRide,
            [EnumMember]
            ThirdPartyPermissionOverRide,
            [EnumMember]
            EmailRenewPermissionOverRide,
            [EnumMember]
            TextPermissionOverRide,
            [EnumMember]
            UpdateEmail,
            [EnumMember]
            UpdatePhone,
            [EnumMember]
            UpdateFax,
            [EnumMember]
            UpdateMobile,
            FirstName, LastName, Company, Title, Address1, State, Zipcode, Phone, Email, SequenceNumber, AccountNumber
        }
        public static RuleSystem GetRuleSystem(string systemRule)
        {
            try
            {
                systemRule = systemRule.Replace(" ", "_");
                return (RuleSystem) System.Enum.Parse(typeof(RuleSystem), systemRule, true);
            }
            catch { return RuleSystem.Blank; }
        }
        [DataContract]
        public enum ServiceResponseStatusTypes
        {
            [EnumMember]
            Success,
            [EnumMember]
            Error,
            [EnumMember]
            Access_Denied,
            [EnumMember]
            Access_Validated,
            [EnumMember]
            No_Access_Key_Required
        }
        [DataContract]
        public enum SeverityTypes
        {
            [EnumMember]
            Critical,
            [EnumMember]
            Non_Critical
        }
        public static SeverityTypes GetSeverityTypes(string severityType)
        {
            try
            {
                severityType = severityType.Replace(" ", "_");
                return (SeverityTypes) System.Enum.Parse(typeof(SeverityTypes), severityType, true);
            }
            catch { return SeverityTypes.Non_Critical; }
        }
        [DataContract]
        public enum SpecialFileResultTypes
        {
            [EnumMember]
            Creates_Data_Table,
            [EnumMember]
            Creates_Mapped_File,
            [EnumMember]
            Adds_Data,
            [EnumMember]
            Updates_Data,
            [EnumMember]
            Replaces_Data,
            [EnumMember]
            Deletes_Data
        }
        public static SpecialFileResultTypes GetSpecialFileResultType(string specialFileResultType)
        {
            if (specialFileResultType.ToLower().Contains("table"))
                specialFileResultType = "Creates_Data_Table";
            else if (specialFileResultType.ToLower().Contains("mapped"))
                specialFileResultType = "Creates_Mapped_File";
            else if (specialFileResultType.ToLower().Contains("adds"))
                specialFileResultType = "Adds_Data";
            else if (specialFileResultType.ToLower().Contains("updates"))
                specialFileResultType = "Updates_Data";
            else if (specialFileResultType.ToLower().Contains("replaces"))
                specialFileResultType = "Replaces_Data";
            else if (specialFileResultType.ToLower().Contains("deletes"))
                specialFileResultType = "Deletes_Data";
            try
            {
                return (SpecialFileResultTypes) System.Enum.Parse(typeof(SpecialFileResultTypes), specialFileResultType, true);
            }
            catch { return SpecialFileResultTypes.Creates_Data_Table; }
        }
        [DataContract]
        public enum SubscriberSourceTypes
        {
            [EnumMember]
            Cover_Wrap,
            [EnumMember]
            Fax,
            [EnumMember]
            Mailing,
            [EnumMember]
            Telemarketing,
            [EnumMember]
            Web
        }
        [DataContract]
        public enum TransformationTypes
        {
            [EnumMember]
            Data_Mapping,
            [EnumMember]
            Join_Columns,
            [EnumMember]
            Split_Into_Rows,
            [EnumMember]
            Assign_Value,
            [EnumMember]
            Split_Transform
        }
        public static TransformationTypes GetTransformationType(string transformationType)
        {
            try
            {
                transformationType = transformationType.Replace(" ", "_");
                return (TransformationTypes) System.Enum.Parse(typeof(TransformationTypes), transformationType, true);
            }
            catch { return TransformationTypes.Data_Mapping; }
        }
        [DataContract]
        public enum UxControlType
        {
            [EnumMember]
            Calendar,
            [EnumMember]
            Dropdown_List,
            [EnumMember]
            Numeric,
            [EnumMember]
            Textbox
        }
        public static UxControlType GetUxControl(string uxControl)
        {
            try
            {
                uxControl = uxControl.Replace(" ", "_");
                return (UxControlType) System.Enum.Parse(typeof(UxControlType), uxControl, true);
            }
            catch { return UxControlType.Textbox; }
        }
        [DataContract]
        public enum UserLogTypes
        {
            [EnumMember]
            Add,
            [EnumMember]
            Change_Active_Flag,
            [EnumMember]
            Delete,
            [EnumMember]
            Edit,
            [EnumMember]
            Log_In,
            [EnumMember]
            Log_Out,
            [EnumMember]
            Record_Update_Edit
        }
        public static UserLogTypes GetUserLogType(string userLogType)
        {
            try
            {
                userLogType = userLogType.Replace(" ", "_");
                return (UserLogTypes) System.Enum.Parse(typeof(UserLogTypes), userLogType, true);
            }
            catch { return UserLogTypes.Edit; }
        }
        [DataContract]
        public enum SubGenQueTypes
        {
            [EnumMember]
            Change_Data_Capture
        }
        public static SubGenQueTypes GetSubGenQueTypes(string subGenQueType)
        {
            try
            {
                subGenQueType = subGenQueType.Replace(" ", "_");
                return (SubGenQueTypes) System.Enum.Parse(typeof(SubGenQueTypes), subGenQueType, true);
            }
            catch { return SubGenQueTypes.Change_Data_Capture; }
        }
        [DataContract]
        public enum SubGenEntityTypes
        {
            [EnumMember]
            customfields,
            [EnumMember]
            subscriptions,
            [EnumMember]
            subscribers,
            [EnumMember]
            addresses,
            [EnumMember]
            listdownloads
        }
        public static SubGenEntityTypes GetSubGenEntityTypes(string subGenEntityType)
        {
            try
            {
                subGenEntityType = subGenEntityType.Replace(" ", "_");
                return (SubGenEntityTypes) System.Enum.Parse(typeof(SubGenEntityTypes), subGenEntityType, true);
            }
            catch { return SubGenEntityTypes.customfields; }
        }
        #endregion

        #region AMS
        [DataContract]
        public enum ActionType
        {
            [EnumMember]
            DataEntry,
            [EnumMember]
            SystemGenerated
        }
        public static ActionType GetActionType(string actionType)
        {
            try
            {
                return (ActionType) System.Enum.Parse(typeof(ActionType), actionType, true);
            }
            catch { return ActionType.SystemGenerated; }
        }

        [DataContract]
        public enum CategoryCodeType
        {
            [EnumMember]
            Qualified_Free,
            [EnumMember]
            NonQualified_Free,
            [EnumMember]
            Qualified_Paid,
            [EnumMember]
            NonQualified_Paid
        }
        public static CategoryCodeType GetCategoryCodeType(string categoryCodeType)
        {
            try
            {
                return (CategoryCodeType) System.Enum.Parse(typeof(CategoryCodeType), categoryCodeType, true);
            }
            catch { return CategoryCodeType.Qualified_Free; }
        }

        [DataContract]
        public enum SubscriberSourceType
        {
            [EnumMember]
            Cover_Wrap,
            [EnumMember]
            Mailing,
            [EnumMember]
            Telemarketing,
            [EnumMember]
            Web
        }
        public static SubscriberSourceType GetSubscriberSourceType(string subscriberSourceType)
        {
            try
            {
                return (SubscriberSourceType) System.Enum.Parse(typeof(SubscriberSourceType), subscriberSourceType, true);
            }
            catch { return SubscriberSourceType.Web; }
        }

        [DataContract]
        public enum TransactionCodeType
        {
            [EnumMember]
            Free_Active,
            [EnumMember]
            Free_Inactive,
            [EnumMember]
            Paid_Active,
            [EnumMember]
            Paid_Inactive
        }
        public static TransactionCodeType GetTransactionCodeType(string transactionCodeType)
        {
            try
            {
                return (TransactionCodeType) System.Enum.Parse(typeof(TransactionCodeType), transactionCodeType, true);
            }
            catch { return TransactionCodeType.Free_Active; }
        }

        [DataContract]
        public enum ReportTypes
        {
            [EnumMember]
            Add_Remove,
            [EnumMember]
            BPA,
            [EnumMember]
            Cross_Tab,
            [EnumMember]
            Category_Summary,
            [EnumMember]
            DemoXQualification,
            [EnumMember]
            Geo_Domestic_BreakDown,
            [EnumMember]
            Geo_International_BreakDown,
            [EnumMember]
            Geo_Single_Country,
            [EnumMember]
            Par3C,
            [EnumMember]
            ProfileField,
            [EnumMember]
            Qualification_BreakDown,
            [EnumMember]
            Single_Response,
            [EnumMember]
            SubFields,
            [EnumMember]
            SubscriberDetails,
            [EnumMember]
            FullSubscriberDetails,
            [EnumMember]
            SubscriberPaidDetails,
            [EnumMember]
            SubscriberResponseDetails
        }

        [DataContract]
        public enum SubscriptionStatus
        {
            [EnumMember]
            AProsp,
            [EnumMember]
            IAProsp,
            [EnumMember]
            AFree,
            [EnumMember]
            APaid,
            [EnumMember]
            IAFree,
            [EnumMember]
            IAPaid
        }

        public static SubscriptionStatus GetSubscriptionStatus(string subscriptionStatus)
        {
            try
            {
                return (SubscriptionStatus) System.Enum.Parse(typeof(SubscriptionStatus), subscriptionStatus, true);
            }
            catch { return SubscriptionStatus.AProsp; }
        }

        [DataContract]
        public enum TransactionCode
        {
            [EnumMember]
            Free_PO_Hold,
            [EnumMember]
            Free_Request,
            [EnumMember]
            Paid_Refund,
            [EnumMember]
            Paid_PO_Hold
        }

        public static TransactionCode GetTransactionCodeKill(string transactionCode)
        {
            try
            {
                return (TransactionCode) System.Enum.Parse(typeof(TransactionCode), transactionCode, true);
            }
            catch { return TransactionCode.Free_PO_Hold; }
        }
        #endregion
        #endregion
    }
}
