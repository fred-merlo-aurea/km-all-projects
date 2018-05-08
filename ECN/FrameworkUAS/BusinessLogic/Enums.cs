using System;
using System.Linq;
using System.Runtime.Serialization;

namespace KMPlatform.BusinessLogic
{
    public class Enums
    {
        #region Services and Features
        [DataContract]
        public enum FulfillmentFeatures
        {
            [EnumMember]
            Paid,
            [EnumMember]
            Suppression,
            [EnumMember]
            List_Management,
            [EnumMember]
            File_Import,
            [EnumMember]
            Address_Geocoding,
            [EnumMember]
            Address_Update
        }
        public static FulfillmentFeatures GetFulfillmentFeature(string fulfillmentFeatureName)
        {
            fulfillmentFeatureName = fulfillmentFeatureName.Replace(" ", "_");
            try
            {
                return (FulfillmentFeatures)System.Enum.Parse(typeof(FulfillmentFeatures), fulfillmentFeatureName, true);
            }
            catch { return FulfillmentFeatures.File_Import; }
        }
        [DataContract]
        public enum UADFeatures
        {
            [EnumMember]
            Standard_Files,
            [EnumMember]
            Special_Files,
            [EnumMember]
            Suppression,
            [EnumMember]
            Relational_Data,
            [EnumMember]
            AdHoc_Dimensions,
            [EnumMember]
            Aggregate_Dimensions,
            [EnumMember]
            Topic_Dimension,
            [EnumMember]
            Consensus_Dimension,
            [EnumMember]
            Multi_Column_Splits,
            [EnumMember]
            Custom_DQM_Procedures,
            [EnumMember]
            Custom_Processing,
            [EnumMember]
            Opens,
            [EnumMember]
            Clicks,
            [EnumMember]
            UAD_Api,
            [EnumMember]
            Address_Geocoding,
            [EnumMember]
            Address_Update,
            [EnumMember]
            Data_Compare,
            [EnumMember]
            Special_Split,
            [EnumMember]
            File_Import
        }
        public static UADFeatures GetUADFeature(string serviceFeatureName)
        {
            serviceFeatureName = serviceFeatureName.Replace(" ", "_");
            try
            {
                return (UADFeatures)System.Enum.Parse(typeof(UADFeatures), serviceFeatureName, true);
            }
            catch { return UADFeatures.Standard_Files; }
        }

        [DataContract]
        public enum EmailFeatures
        {

        }
        [DataContract]
        public enum FormsFeatures
        {

        }
        #endregion

        [DataContract]
        public enum Clients
        {
            [EnumMember]
            Advanstar,
            [EnumMember]
            AHACoding,
            [EnumMember]
            Anthem,
            [EnumMember]
            Atcom,
            [EnumMember]
            ATHB,
            [EnumMember]
            Babcox,
            [EnumMember]
            BelmontPublications,
            [EnumMember]
            BriefMedia,
            [EnumMember]
            BusinessJournalsInc,
            [EnumMember]
            Canon,
            [EnumMember]
            Catersource,
            [EnumMember]
            DEMO,
            [EnumMember]
            DeWittPublishing,
            [EnumMember]
            France,
            [EnumMember]
            GLM,
            [EnumMember]
            HappyDayMedia,
            [EnumMember]
            HealthForum,
            [EnumMember]
            HVCB,
            [EnumMember]
            HWPublishing,
            [EnumMember]
            KnowledgeMarketing,
            [EnumMember]
            Lebhar,
            [EnumMember]
            Medtech,
            [EnumMember]
            Meister,
            [EnumMember]
            MMA,
            [EnumMember]
            MTG,
            [EnumMember]
            NECA,
            [EnumMember]
            NoriaCorp,
            [EnumMember]
            Northstar,
            [EnumMember]
            OildomPublishing,
            [EnumMember]
            OutcomesLLC,
            [EnumMember]
            PennWell,
            [EnumMember]
            PentaVision,
            [EnumMember]
            SAETB,
            [EnumMember]
            Scranton,
            [EnumMember]
            SpecialityFoods,
            [EnumMember]
            Stamats,
            [EnumMember]
            Tabor,
            [EnumMember]
            TeamMHC,
            [EnumMember]
            TenMissions,
            [EnumMember]
            TMB,
            [EnumMember]
            TradePress,
            [EnumMember]
            UAStest,
            [EnumMember]
            UPI,
            [EnumMember]
            Vcast,
            [EnumMember]
            Watt
        }
        public static Clients GetClient(string clientName)
        {
            try
            {
                return (Clients)System.Enum.Parse(typeof(Clients), clientName, true);
            }
            catch { return Clients.KnowledgeMarketing; }
        }

        [DataContract]
        public enum Applications
        {
            [EnumMember]
            Control_Center,
            [EnumMember]
            Circulation,
            [EnumMember]
            DQM,
            [EnumMember]
            Profile_Manager,
            [EnumMember]
            File_Mapper,
            [EnumMember]
            UAD_Dashboard,
            [EnumMember]
            UAD_Explorer,
            [EnumMember]
            Circulation_Explorer,
            [EnumMember]
            Email,
            [EnumMember]
            Forms,
            [EnumMember]
            UAD,
            [EnumMember]
            AMS_Desktop,
            [EnumMember]
            AMS_Web,
            [EnumMember]
            Data_Compare,
            [EnumMember]
            Data_Import_Export,
            [EnumMember]
            File_Mapper_Wizard,
            [EnumMember]
            ADMS_Engine,
            [EnumMember]
            AMS_Geocoding,
            [EnumMember]
            AMS_Operations,
            [EnumMember]
            Service_Que_Monitor,
            [EnumMember]
            SubGen_Integration,
            [EnumMember]
            Framework
        }
        public static Applications GetApplication(string applicationName)
        {
            try
            {
                applicationName = applicationName.Replace(" ", "_");
                return (Applications)System.Enum.Parse(typeof(Applications), applicationName, true);
            }
            catch { return Applications.UAD; }
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
                return (DataCompareOptions)System.Enum.Parse(typeof(DataCompareOptions), dataCompareOption, true);
            }
            catch { return DataCompareOptions.Summary_Report; }
        }

        #region KM Specific
        [DataContract]
        public enum KmSecurityGroups
        {
            [EnumMember]
            Account_Managers,
            [EnumMember]
            Administrators,
            [EnumMember]
            Circulation_Data_Entry,
            [EnumMember]
            DQM_Users
        }
        public static KmSecurityGroups GetKmSecurityGroups(string kmSecurityGroups)
        {
            try
            {
                kmSecurityGroups = kmSecurityGroups.Replace(" ", "_");
                return (KmSecurityGroups)System.Enum.Parse(typeof(KmSecurityGroups), kmSecurityGroups, true);
            }
            catch { return KmSecurityGroups.Circulation_Data_Entry; }
        }
        #endregion

        [DataContract]
        public enum CountriesWithRegions
        {
            [EnumMember]
            UNITED_STATES,
            [EnumMember]
            CANADA,
            [EnumMember]
            US_AND_CANADA,
            [EnumMember]
            INTERNATIONAL,
            [EnumMember]
            MEXICO
        }

        [DataContract]
        public enum SubGenControls
        {
            [EnumMember]
            Search,
            [EnumMember]
            New_Subscriber,
            [EnumMember]
            Subscriber,
            [EnumMember]
            Home,
            [EnumMember]
            Reports
        }

        public enum ServerVariable
        {
            [EnumMember]
            ALL_HTTP,
            [EnumMember]
            ALL_RAW,
            [EnumMember]
            APPL_MD_PATH,
            [EnumMember]
            APPL_PHYSICAL_PATH,
            [EnumMember]
            AUTH_PASSWORD,
            [EnumMember]
            AUTH_TYPE,
            [EnumMember]
            AUTH_USER,
            [EnumMember]
            CERT_COOKIE,
            [EnumMember]
            CERT_FLAGS,
            [EnumMember]
            CERT_ISSUER,
            [EnumMember]
            CERT_KEYSIZE,
            [EnumMember]
            CERT_SECRETKEYSIZE,
            [EnumMember]
            CERT_SERIALNUMBER,
            [EnumMember]
            CERT_SERVER_ISSUER,
            [EnumMember]
            CERT_SERVER_SUBJECT,
            [EnumMember]
            CERT_SUBJECT,
            [EnumMember]
            CONTENT_LENGTH,
            [EnumMember]
            CONTENT_TYPE,
            [EnumMember]
            GATEWAY_INTERFACE,
            [EnumMember]
            HTTP_,
            [EnumMember]
            HTTP_ACCEPT,
            [EnumMember]
            HTTP_ACCEPT_LANGUAGE,
            [EnumMember]
            HTTP_COOKIE,
            [EnumMember]
            HTTP_REFERER,
            [EnumMember]
            HTTP_USER_AGENT,
            [EnumMember]
            HTTPS,
            [EnumMember]
            HTTPS_KEYSIZE,
            [EnumMember]
            HTTPS_SECRETKEYSIZE,
            [EnumMember]
            HTTPS_SERVER_ISSUER,
            [EnumMember]
            HTTPS_SERVER_SUBJECT,
            [EnumMember]
            INSTANCE_ID,
            [EnumMember]
            INSTANCE_META_PATH,
            [EnumMember]
            LOCAL_ADDR,
            [EnumMember]
            LOGON_USER,
            [EnumMember]
            PATH_INFO,
            [EnumMember]
            PATH_TRANSLATED,
            [EnumMember]
            QUERY_STRING,
            [EnumMember]
            REMOTE_ADDR,
            [EnumMember]
            REMOTE_HOST,
            [EnumMember]
            REMOTE_USER,
            [EnumMember]
            REQUEST_METHOD,
            [EnumMember]
            SCRIPT_NAME,
            [EnumMember]
            SERVER_NAME,
            [EnumMember]
            SERVER_PORT,
            [EnumMember]
            SERVER_PORT_SECURE,
            [EnumMember]
            SERVER_PROTOCOL,
            [EnumMember]
            SERVER_SOFTWARE,
            [EnumMember]
            URL

        }

        #region Brought from Circulation - q.k.

        [DataContract]
        [Flags]
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

        #endregion

        #region UAD_Lookup tables
        #region CodeTypes
        [DataContract]
        public enum CodeTypes
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
            Authorization_Mode,
            [EnumMember]
            Color_Wheel,
            [EnumMember]
            Configuration,
            [EnumMember]
            Credit_Card,
            [EnumMember]
            Database_Destination,
            [EnumMember]
            Database_File,
            [EnumMember]
            Data_Compare_Target,
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
            Marketing,
            [EnumMember]
            Match,
            [EnumMember]
            Operators,
            [EnumMember]
            Par3c,
            [EnumMember]
            Payment,
            [EnumMember]
            Postal_Service,
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
            Response_Group,
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
            Dimension
        }
        public static CodeTypes GetCodeType(string codeType)
        {
            try
            {
                codeType = codeType.Replace(" ", "_");
                return (CodeTypes)System.Enum.Parse(typeof(CodeTypes), codeType, true);
            }
            catch { return CodeTypes.Configuration; }
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
                return (ACS_Actions)System.Enum.Parse(typeof(ACS_Actions), acsAction, true);
            }
            catch { return ACS_Actions.Kill_Address; }
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
                return (ConfigurationTypes)System.Enum.Parse(typeof(ConfigurationTypes), configurationType, true);
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
                return (DataCompareTargetTypes)System.Enum.Parse(typeof(DataCompareTargetTypes), dataCompareTargetType, true);
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
            DBF
        }
        public static FileTypes GetDatabaseFileType(string databaseFileType)
        {
            try
            {
                databaseFileType = databaseFileType.Replace(" ", "_");
                return (FileTypes)System.Enum.Parse(typeof(FileTypes), databaseFileType, true);
            }
            catch { return FileTypes.Audience_Data; }
        }
        [DataContract]
        public enum ExecutionPointTypes
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
            Post_UAD_Import
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
        public enum FieldMappingTypes
        {
            [EnumMember]
            Ignored,
            [EnumMember]
            Standard,
            [EnumMember]
            Demographic
        }
        [DataContract]
        public enum FileSnippetTypes
        {
            [EnumMember]
            Prefix
        }
        [DataContract]
        public enum FileStatusTypes
        {
            [EnumMember]
            Cleansed,
            [EnumMember]
            Detected,
            [EnumMember]
            GeoCode,
            [EnumMember]
            In_Cleansing,
            [EnumMember]
            In_Matching,
            [EnumMember]
            In_Transformations,
            [EnumMember]
            In_Validation,
            [EnumMember]
            Invalid,
            [EnumMember]
            Loading,
            [EnumMember]
            Matched,
            [EnumMember]
            Processed,
            [EnumMember]
            Ready,
            [EnumMember]
            Suppression,
            [EnumMember]
            Transformed,
            [EnumMember]
            Validated
        }
        public static FileStatusTypes GetFileStatusType(string fileStatusType)
        {
            try
            {
                fileStatusType = fileStatusType.Replace(" ", "_");
                return (FileStatusTypes)System.Enum.Parse(typeof(FileStatusTypes), fileStatusType, true);
            }
            catch { return FileStatusTypes.Processed; }
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
                return (FileRecurrenceTypes)System.Enum.Parse(typeof(FileRecurrenceTypes), sourceFileType, true);
            }
            catch { return FileRecurrenceTypes.One_Time; }
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
                return (Marketing)System.Enum.Parse(typeof(Marketing), marketingID, true);
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
                return (MatchTypes)System.Enum.Parse(typeof(MatchTypes), matchTypeName, true);
            }
            catch { return MatchTypes.Equals; }
        }
        [DataContract]
        public enum OperatorTypes
        {
            [EnumMember]
            multiply,
            [EnumMember]
            divide,
            [EnumMember]
            modulus,
            [EnumMember]
            add,
            [EnumMember]
            subtract,
            [EnumMember]
            greater_than,
            [EnumMember]
            less_than,
            [EnumMember]
            greater_than_or_equal_to,
            [EnumMember]
            less_than_or_equal_to,
            [EnumMember]
            is_not_less_than,
            [EnumMember]
            is_not_greater_than,
            [EnumMember]
            equal,
            [EnumMember]
            not_equal,
            [EnumMember]
            and,
            [EnumMember]
            or,
            [EnumMember]
            contains,
            [EnumMember]
            starts_with,
            [EnumMember]
            ends_with
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
        public enum PaymentTypes
        {
            [EnumMember]
            Check,
            [EnumMember]
            Credit_Card,
            [EnumMember]
            Bill_Me_Later
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
        public enum ProcedureTypes
        {
            [EnumMember]
            SQL,
            [EnumMember]
            NET
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
                return (SeverityTypes)System.Enum.Parse(typeof(SeverityTypes), severityType, true);
            }
            catch { return SeverityTypes.Non_Critical; }
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
            Log_Out
        }

        public static UserLogTypes GetUserLogType(string userLogType)
        {
            try
            {
                userLogType = userLogType.Replace(" ", "_");
                return (UserLogTypes)System.Enum.Parse(typeof(UserLogTypes), userLogType, true);
            }
            catch { return UserLogTypes.Edit; }
        }
        #endregion

        #region Circulation
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
                return (ActionType)System.Enum.Parse(typeof(ActionType), actionType, true);
            }
            catch { return ActionType.SystemGenerated; }
        }

        #endregion
        #endregion
    }
}
