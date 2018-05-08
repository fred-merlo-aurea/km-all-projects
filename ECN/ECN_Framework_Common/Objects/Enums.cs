using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Common.Objects
{
    [Serializable]
    public partial class Enums
    {

        public enum ExceptionLayer
        {
            Business,
            WebSite,
            API
        }

        public enum SortDirection
        {
            Ascending,
            Descending
        }

        public enum ErrorMessage
        {
            ValidationError,
            SecurityError,
            HardError,
            InvalidLink,
            PageNotFound,
            Timeout,
            Unknown
        }

        public enum SecurityExceptionType
        {
            FeatureNotEnabled,
            RoleAccess,
            Security
        }

        //for ECNError object
        public enum Entity
        {
            FormsSpecificAPI,
            Image,
            ImageFolder,
            Campaign,
            CampaignItemTemplateGroup,
            CampaignItemTemplateSuppressionGroup,
            CampaignItemTemplateOptoutGroup,
            CampaignItemTemplateFilter,
            CampaignItem,
            CampaignItemBlast,
            CampaignItemBlastFilter,
            CampaignItemBlastRefBlast,
            CampaignItemSuppression,
            CampaignItemTestBlast,
            CampaignItemTemplate,
            Blast,
            BlastAB,
            BlastABMaster,
            BlastRegular,
            BlastPersonalization,
            BlastChampion,
            BlastLayout,
            BlastNoOpen,
            BlastSMS,
            BlastSocial,
            BlastFields,
            BlastFieldsName,
            ContentFilter,
            ContentFilterDetail, 
            LinkOwnerIndex, 
            LinkAlias, 
            Code,
            ReportSchedule,
            User,
            Content,
            DataFieldSets,
            DeptItemReferences,
            EmailGroup,
            Folder,
            Group,
            Layout,
            Samples,
            UserDepartment,
            Publication,
            Customer,
            CustomerContact,
            CustomerNote,
            MessageType,
            Quote,
            Email,
            GroupDataFields,
            EmailDataValues,
            SmartFormsPrePopFields,
            DomainSuppression,
            Filter,
            Role,
            RoleAction,
            Contact,
            CustomerProduct,
            CustomerConfig,
            FilterGroup,
            FilterCondition,
            CustomerTemplate,
            BaseChannel,
            Channel,
            Template,
            CustomerLicense,
            Wizard,
            Edition,
            BlastPlans,
            LayoutPlans,
            TriggerPlans,
            EditionHistory,
            AutoResponders,
            Page,
            Link,
            BlastSingle,
            SmartFormActivityLog,
            SmartFormTracking,
            BlastEnvelope,
            EditionActivityLog,
            Rule,
            DomainTracker,
            DomainTrackerFields,
            BlastLink,
            DynamicTag,
            DynamicTagRule,
            APILogging,
            BlastOptOutGroup,
            CampaignItemOptOutGroup,
            LinkTrackingSettings,
            LinkTrackingParam,
            LinkTrackingParamSettings,
            LinkTrackingParamOption,
            LandingPage,
            LandingPageOption,
            LandingPageOptionReason,
            LandingPageAssign,
            LandingPageAssignContent,
            ConversionLinks,
            ChampionAudit,
            Survey,
            None,
            RSSFeed,
            BlastRSS,
            SubscriptionManagement,
            Notification,
            UniqueLink,
            Gateway,
            GatewayValue,
            DateRange,
            EmailDirect,
            GroupConfig,
            ReportQueue,
            SmartFormsHistory,
            GroupDataFieldsDefault,
            MarketingAutomation,
            MAControl,
            MAConnector,
            QuickTestBlast
        }

        //for ECNError object
        public enum Method
        {
            Validate,
            Save,
            Delete,
            Get,
            GetList,
            HasPermission,
            Create,
            None
        }

        public enum ParameterTypes
        {
            EmailID,
            EmailAddress,
            BlastID,
            GroupID,
            BlastLinkID,
            SocialMediaID,
            CampaignItemID,
            CustomerID,
            UserName,
            Password,
            Subscribe,
            Format,
            Preview,
            SmartFormID,
            URL,
            LayoutID,
            Monitor,
            BaseChannelID,
            NewEmail,
            OldEmail,
            EmailDirectID
            //ContentID,
            //GroupID,
            //LayoutID,
            //FilterID,        
            //Link
        };
    }
}
namespace ECN_Framework_Common.Objects.Collector
{
    public class Enums
    {
        public enum MenuCode
        {   
            INDEX,
            SURVEY
        }
    }
}

namespace ECN_Framework_Common.Objects.Communicator
{
    public class Enums
    {
        

        public enum EventType
        {
            NoOpen,
            Subscribe,
            Refer,
            Open,
            Click,
            UNKNOWN
        }

        public enum ActionTypeCode
        {
            Subscribe,
            Click,
            Refer,
            Read,
            Open,
            Abandon,
            Submit,
            UNKNOWN
        }

        public enum ActionValue
        {
            S,
            U,
            UNKNOWN
        }

        public enum ApplicationCode
        {
            CAMPAIGNITEMTYPE,
            MENUCODE,
            BLASTSTATUSCODE,
            BLASTTYPE,
            CODETYPE,
            TEMPLATESTYLECODE,
            FOLDERTYPE,
            CONTENTTYPECODE,
            UNKNOWN
        }

        
        public enum MenuCode
        {
            INDEX,
            GROUPS,
            BLASTS,
            CONTENT,
            EVENTS,
            EMAILTOFRIEND,
            FOLDERS,
            LOGIN,
            REPORTS,
            PAGEKNOWTICE,
            SALESFORCE,
            OMNITURE,
            BASECHANNEL,
            CUSTOMER
        }
        public enum BlastStatusCode
        {
            Pending,
            Active,
            Sent,
            Deleted,
            System,
            Saved,
            Cancelled,
            NoLicense,
            Unknown,
            PendingContent,
            Paused
        }

        public enum CampaignItemFormatType
        {
            HTML,
            TEXT,
            UNKNOWN
        }

        public enum CampaignItemType
        {
            Regular,
            AB,
            Champion,
            SMS,
            Social,
            Layout,
            NoOpen,
            Unknown,
            Salesforce,
            Personalization
        }

        public enum BlastType
        {
            HTML,
            TEXT,
            Sample,
            Champion,
            SMS,
            Social,
            Layout,
            NoOpen,
            Unknown,
            Personalization
        }
        public enum CodeType
        {
            BLASTCATEGORY,
            LINKTYPE
        }
        public enum TemplateStyleCode
        {
            NEWSLETTER
        }

        public enum FolderTypes
        {
            CNT,
            GRP,
            IMG
        }

        public enum ContentTypeCode
        {
            FILE,
            HTML,
            SMS,
            TEXT
        }

        public enum State
        {
            Active,
            Paused,
            Cancelled
        }

        public enum MarketingAutomationType
        {
            Simple
        }

        public enum MarketingAutomationStatus
        {
            Saved,
            // Active, Bug 36972:Remove 'Active' from status drop down list in home page 
            Paused,
            Completed,
            Archived,
            Published
        }

        public enum MarketingAutomationControlType
        {
            CampaignItem,
            Click,
            Direct_Click,
            Direct_NoOpen,
            Direct_Open,
            End,
            Form,
            FormSubmit,
            FormAbandon,
            Group,
            NoClick,
            NoOpen,
            NotSent,
            Open,
            Open_NoClick,
            Sent,
            Start,
            Subscribe,
            Suppressed,
            Unsubscribe,
            Wait

        }

        public enum MessageType
        {
            Information = 0,
            Warning = 1,
            Success = 2,
            Confirm = 3,
            Error = 4
        }
        public enum SocialMediaStatusType
        {
            Created,
            Pending,
            Sent,
            Failed
        }
    }
}

namespace ECN_Framework_Common.Objects.Accounts
{
    public class Enums
    {

        public enum CodeType
        {
            
            ChannelType,
            ContentType,
            CustomerSecurity,
            CustomerType,
            FolderType,
            FormatType,
            LicenseType,
            OwnerType,
            ProfileType,
            SubscribeType,
            TemplateStyle,
            TemplateType,
            Unknown
        }
        public enum ChannelTypeCode
        {
            accounts,
            charity,
            collector,
            communicator,
            creator,
            publisher,
            wizard
        }
        public enum LicenseOption
        {
            unlimited,
            limited,
            notavailable
        }
        public enum LicenseTypeCode
        {
            ClientInquirie,
            collectorsurvey,
            creatorlimit,
            emailblock10k
        }
        public enum LiceseType : int
        {
            Zero = 0,
            One = 1,
            Two = 2
        }
        public enum LicenseLevel
        {
            CORP,
            CUST
        }
        public enum PriceType : int
        {
            One = 1,
            Two = 2,
            Three = 3
        }
        public enum FrequencyType : int
        {
            One = 1,
            Two = 2,
            Four = 4,
            Eight = 8,
            Sixteen = 16
        }
        public enum CardType
        {
            MasterCard,
            Visa
        }
        public enum SubscriptionType
        {
            M
        }
        public enum ChannelType
        {
            Charity,
            Marketing,
            Other,
            Publishing,
            Retail,
            Tradeshow,
            Unknown
        }

        public enum ChannelPartnerType
        { 
            NotInitialized = 0, 
            Silver = 1, 
            Gold = 2, 
            Platinum = 3, 
            NotApplicable = 4 
        }

        public enum CustomerType
        {
            Other,
            Technology,
            Tradeshow,
            Retail,
            Marketing,
            Hospitality,
            Publishing,
            Charity
        }
        public enum ConfigName 
        {
            MailingIP,
            MailRoute,
            PickupPath,
            Unknown
        }
        
        [DataContract]
        public enum Salutation
        {
            [EnumMember]
            Ms,
            [EnumMember]
            Mr,
            [EnumMember]
            Unknown
        }
        public enum CommunicatiorLevel : int
        {
            Three = 3,
            Two = 2,
            One = 1,
            Zero = 0,
            NegativeOne = -1
        }
        public enum CollectorLevel : int
        {
            Two = 2,
            One = 1,
            Zero = 0,
            NegativeOne = -1
        }
        public enum CreatorLevel : int
        {
            One = 1,
            Zero = 0,
            NegativeOne = -1
        }
        public enum PublisherLevel : int
        {
            One = 1,
            Zero = 0
        }
        public enum CharityLevel : int
        {
            One = 1,
            Zero = 0
        }
        public enum AccountsLevel : int
        {
            Zero = 0,
            NegativeOne = -1
        }
        public enum Selector
        {
            ecn
        }
        public enum Status
        {
            Success,
            TransError,
            Start,
            Complete,
            Started,
            Fail
        }
        public enum EngineName
        {
            LicenseEngine,
            BillingEngine
        }

        public enum MenuCode
        {
            INDEX,
            USERS,
            CUSTOMERS,
            LEADS,
            BILLINGSYSTEM,
            CHANNELS,
            REPORTS,
            PAGESETUP,
            DIGITALEDITION,
            NOTIFICATION
        }

        public enum ProductFeature
        {

        }

        public enum UserPermission
        {
            Manage_Content,
            Create_Content,
            Create_Message,
            Manage_Groups,
            Manage_Groups_Full_Access,
            Manage_Campaigns,
            Create_Regular_Blast,
            Add_Emails,
            Import_Data,
            Clean_Emails,
            Delivery_Report,
            NoCheck
        }

        [Flags]
        public enum StaffRoleEnum
        {
            CustomerService = 1,
            AccountExecutive = 2,
            AccountManager = 3,
            DemoManager = 4
        }

        public enum FrequencyTypeEnum 
        {
            Annual = 1,
            Quarterly = 2,
            Monthly = 4,
            OneTime = 8,
            BiWeekly = 16,
            Weekly = 32
        }

        public enum LicenseTypeEnum 
        { 
            AnnualTechAccess = 0, 
            EmailBlock = 1, 
            Option = 2 
        }

        public enum PriceTypeEnum 
        { 
            Recurring = 1, 
            Usage = 2, 
            OneTime = 3 
        }

        public enum QuoteStatusEnum 
        { 
            Pending = 0, 
            Approved = 1, 
            Denied = 2 
        }
    }
}

namespace ECN_Framework_Common.Objects.Activity
{
    public class Enums
    {
        public enum ActionTypes
        {
            Open,
            Click,
            Bounce,
            Conversion,
            Refer,
            Resend,
            Send,
            Suppressed,
            Unsubscribe
        }

        public enum BounceCode
        { 
            None = 1,
            addresschange,
            autoresponder,
            dnserror,
            hard,
            hardbounce,
            notify,
            resend,
            softbounce,
            SpamNofication,
            spamnotification,
            subscribe,
            transient,
            U,
            unknown,
            unsubscribe,
            blocks
        }
    }
}

namespace ECN_Framework_Common.Objects.Publisher
{
    public class Enums
    {
        public enum MenuCode
        {
            INDEX,
            PUBLICATION,
            EDITION
        }

        public enum Status
        {
            Active,
            Archieve,
            Inactive, 
            Pending
        }
    }
}

namespace ECN_Framework_Common.Objects.Creator
{
    public class Enums
    {
        public enum CodeType
        {
            MediaType,
            EventType,
            PointerType,
            PageType,
            MediaFields,
            MenuType,
            ItemType,
            TemplateType,
            Unknown
        }
    }
}

namespace ECN_Framework_Common.Objects.EmailDirect
{
    public class Enums
    {
        public enum Status
        {
            Pending,
            Sent,
            Error,
            Opened,
            Active
        }
    }
}
