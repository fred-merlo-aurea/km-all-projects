using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.BusinessLogic
{
    public class Enums
    {
        [DataContract]
        [Serializable]
        public enum EmailStatus
        {
            [EnumMember]
            Active,
            [EnumMember]
            Inactive,
            [EnumMember]
            Bounced,
            [EnumMember]
            Invalid,
            [EnumMember]
            MasterSuppressed,
            [EnumMember]
            Spam,
            [EnumMember]
            UnSubscribe,
            [EnumMember]
            Unverified
        }
        [DataContract]
        [Serializable]
        public enum EmailStatusSingleValue
        {
            [EnumMember]
            S,
            [EnumMember]
            P,
            [EnumMember]
            U,
            [EnumMember]
            M
        }
        public static EmailStatusSingleValue GetEmailStatusSingleValue(string emailStatus)
        {
            try
            {
                return (EmailStatusSingleValue) System.Enum.Parse(typeof(EmailStatusSingleValue), emailStatus.ToUpper(), true);
            }
            catch { return EmailStatusSingleValue.S; }
        }
        public static EmailStatus GetEmailStatus(string emailStatus)
        {
            try
            {
                return (EmailStatus)System.Enum.Parse(typeof(EmailStatus), emailStatus.ToUpper(), true);
            }
            catch { return EmailStatus.Active; }
        }
        public enum Operation
        {
            AllIntersect,
            AllUnion,
            Combo,
            Individual,
            Venn
        }
        public enum ViewType
        {
            None,
            ConsensusView,
            ProductView,
            CrossProductView,
            RecencyView,
            RecordDetails,
            AMSView
        }
        public enum DataCompareViewType
        {
            Consensus,
            Product,
            Brand
        }
        public enum GroupExportSource
        {
            UADManualExport,
            UADScheduledExport
        }
       
        public enum ExportType
        {
            FTP = 1,
            ECN = 2,
            Campaign = 3,
            Marketo = 4
        }
        public enum ExportFieldType
        {
            Profile,
            Demo,
            Adhoc,
            All
        }
        public enum FieldType
        {
            Varchar,
            Int
        }

        public enum FieldCase
        {
            Default,
            UpperCase,
            LowerCase,
            ProperCase,
            None
        }
        public enum FiltersType
        {
            Product = 0,
            Dimension = 1,
            Standard = 2,
            Geo = 3,
            Activity = 4,
            Adhoc = 5,
            None = 6,
            Brand = 7,
            Circulation = 8,
            DataCompare = 9
        }
    }
}
