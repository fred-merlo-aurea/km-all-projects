using System;
using System.Runtime.Serialization;

namespace Core_AMS.Utilities
{
    public partial class Enums
    {
        [DataContract]
        public enum TrueFalse
        {
            [EnumMember]
            True,
            [EnumMember]
            False
        }

        [DataContract]
        public enum YesNo
        {
            [EnumMember]
            Yes,
            [EnumMember]
            No
        }

        [DataContract]
        public enum MAFFieldStandardFields
        {
            [EnumMember]
            ACCOUNTNUMBER,
            [EnumMember]
            ADDRESS1,
            [EnumMember]
            ADDRESS2,
            [EnumMember]
            ADDRESS3,
            [EnumMember]
            CITY,
            [EnumMember]
            COMPANY,
            [EnumMember]
            COPIES,
            [EnumMember]
            COUNTRY,
            [EnumMember]
            COUNTRYID,
            [EnumMember]
            COUNTY,
            [EnumMember]
            DEMO7,
            [EnumMember]
            EMAIL,
            [EnumMember]
            EMAILID,
            [EnumMember]
            EMAILRENEWPERMISSION,
            [EnumMember]
            EMAILSTATUSID,
            [EnumMember]
            EXTERNALKEYID,
            [EnumMember]
            FAX,
            [EnumMember]
            FAXPERMISSION,
            [EnumMember]
            FIRSTNAME,
            [EnumMember]
            GENDER,
            [EnumMember]
            GRACEISSUES,
            [EnumMember]
            ISCOMP,
            [EnumMember]
            ISPAID,
            [EnumMember]
            ISSUBSCRIBED,
            [EnumMember]
            LASTNAME,
            [EnumMember]
            LATITUDE,
            [EnumMember]
            LONGITUDE,
            [EnumMember]
            MAILPERMISSION,
            [EnumMember]
            MOBILE,
            [EnumMember]
            OCCUPATION,
            [EnumMember]
            ORIGSSRC,
            [EnumMember]
            OTHERPRODUCTSPERMISSION,
            [EnumMember]
            PAR3CID,
            [EnumMember]
            PHONE,
            [EnumMember]
            PHONEPERMISSION,
            [EnumMember]
            PLUS4,
            [EnumMember]
            PUBCATEGORYID,
            [EnumMember]
            PUBCODE,
            [EnumMember]
            PUBQSOURCEID,
            [EnumMember]
            PUBTRANSACTIONDATE,
            [EnumMember]
            PUBTRANSACTIONID,
            [EnumMember]
            QUALIFICATIONDATE,
            [EnumMember]
            REGIONCODE,
            [EnumMember]
            SEQUENCEID,
            [EnumMember]
            SUBGENSUBSCRIBERID,
            [EnumMember]
            SUBSCRIBERSOURCECODE,
            [EnumMember]
            SUBSCRIPTIONSTATUSID,
            [EnumMember]
            SUBSRCID,
            [EnumMember]
            TEXTPERMISSION,
            [EnumMember]
            THIRDPARTYPERMISSION,
            [EnumMember]
            TITLE,
            [EnumMember]
            VERIFY,
            [EnumMember]
            WEBSITE,
            [EnumMember]
            ZIPCODE
        }

        #region OLD SUBSCRIPTION STANDARD FIELDS
        //OLD SUBSCRIPTION WAY
        //[DataContract]
        //public enum MAFFieldStandardFields
        //{
        //    [EnumMember]
        //    Address,
        //    [EnumMember]
        //    Address3,
        //    [EnumMember]
        //    CategoryID,
        //    [EnumMember]
        //    CITY,
        //    [EnumMember]
        //    Company,
        //    [EnumMember]
        //    Country,
        //    [EnumMember]
        //    County,
        //    [EnumMember]
        //    Ctry,
        //    [EnumMember]
        //    Demo31,
        //    [EnumMember]
        //    Demo32,
        //    [EnumMember]
        //    Demo33,
        //    [EnumMember]
        //    Demo34,
        //    [EnumMember]
        //    Demo35,
        //    [EnumMember]
        //    Demo36,
        //    [EnumMember]
        //    Demo7,
        //    [EnumMember]
        //    Email,
        //    [EnumMember]
        //    EmailExists,
        //    [EnumMember]
        //    EmailStatus,
        //    [EnumMember]
        //    Fax,
        //    [EnumMember]
        //    FName,
        //    [EnumMember]
        //    ForZip,
        //    [EnumMember]
        //    FullName,
        //    [EnumMember]
        //    Gender,
        //    [EnumMember]
        //    Home_Work_Address,
        //    [EnumMember]
        //    LName,
        //    [EnumMember]
        //    MailStop,
        //    [EnumMember]
        //    Mobile,
        //    [EnumMember]
        //    Notes,
        //    [EnumMember]
        //    OrigsSrc,
        //    [EnumMember]
        //    Par3C,
        //    [EnumMember]
        //    Phone,
        //    [EnumMember]
        //    Plus4,
        //    [EnumMember]
        //    PubCode,
        //    [EnumMember]
        //    QDate,
        //    [EnumMember]
        //    QSourceID,
        //    [EnumMember]
        //    RegCode,
        //    [EnumMember]
        //    Sequence,
        //    [EnumMember]
        //    Sic,
        //    [EnumMember]
        //    State,
        //    [EnumMember]
        //    SubSrc,
        //    [EnumMember]
        //    Title,
        //    [EnumMember]
        //    TransactionDate,
        //    [EnumMember]
        //    TransactionID,
        //    [EnumMember]
        //    Verified,
        //    [EnumMember]
        //    Zip
        //}
        #endregion

        [DataContract]
        public enum DialogResponses
        {
            [EnumMember]
            Print,
            [EnumMember]
            Save,
            [EnumMember]
            Cancel,
            [EnumMember]
            Copy
        }

        [DataContract]
        public enum FileExtensions
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
            dbf,
            [EnumMember]
            zip
        }
        public static FileExtensions GetFileExtensions(string extension)
        {
            try
            {
                return (FileExtensions)Enum.Parse(typeof(FileExtensions), extension, true);
            }
            catch { return FileExtensions.csv; }
        }

        [DataContract]
        public enum Windows
        {
            [EnumMember]
            AMS_Desktop_Windows_Home,
            [EnumMember]
            Circulation_Windows_Popout,
            [EnumMember]
            WpfControls_Popout
        }
    
        [DataContract]
        public enum FileRemovalOption
        {
            [EnumMember]
            Product_Code,
            [EnumMember]
            Process_Code
        }

        [DataContract]
        public enum FileAuditLocations
        {
            [EnumMember]
            Original,
            [EnumMember]
            Transformed,
            [EnumMember]
            Invalid,
            [EnumMember]
            Archive,
            [EnumMember]
            Final,
            [EnumMember]
            Subscription
        }
        public static FileAuditLocations GetFileAuditLocations(string delimiter)
        {
            try
            {
                return (FileAuditLocations)Enum.Parse(typeof(FileAuditLocations), delimiter, true);
            }
            catch { return FileAuditLocations.Original; }
        }

        [DataContract]
        public enum DashboardReportName
        {
            [EnumMember]
            _InvalidReport,
            [EnumMember]
            _TransformedReport,
            [EnumMember]
            _DuplicatesReport,
            [EnumMember]
            _IgnoredReport,
            [EnumMember]
            _ProcessedReport,
            [EnumMember]
            _DimensionErrorsSummaryReport,
            [EnumMember]
            _DimensionSubscriber,
        }
    }
}
