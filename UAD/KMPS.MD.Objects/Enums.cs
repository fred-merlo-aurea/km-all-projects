using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace KMPS.MD.Objects
{
    public class Enums
    {
        public enum EmailStatus
        {
            Active = 1,
            Bounced = 2,
            Invalid = 3,
            MasterSupppressed = 4,
            Spam = 5,
            UnSubscribe = 6
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

        public enum ExportType
        {
            FTP = 1,
            ECN = 2,
            Campaign = 3,
            Marketo = 4
        }

        public enum SiteType
        {
            FTP,
            FTPS,
            SFTP
        }

        public enum Platforms
        {
            Windows = 1,
            Mobile = 2,
            Linux = 3,
            Mac = 4,
            Unknown = 5
        }

        public enum Clients
        {
            Outlook = 1,
            InternetExplorer = 2,
            IPhone = 3,
            IPad = 4,
            Android = 5,
            Firefox = 6,
            Chrome = 7,
            BlackBerry = 8,
            WebOS = 9,
            Symbian = 10,
            Safari = 11,
            Opera = 12,
            Thunderbird = 13,
            LotusNotes = 14,
            Unknown = 15
        }

        public enum ViewType
        {
            None,
            ConsensusView,
            ProductView,
            CrossProductView,
            RecencyView,
            RecordDetails
        }

        public enum PageType
        {
            Search,
            FilterSchedule,
            Export
        }

        public enum Marketo
        {
            BaseURL,
            ClientID,
            ClientSecret,
            Partition
        }

        public enum Results
        {
            LeadResult,
            ListResult
        }

        public enum DataCompareViewType
        {
            Consensus,
            Product,
            Brand
        }

        public enum UADFieldType
        {
            Profile,
            Custom,
            Dimension,
            Adhoc
        }

        public enum Config
        {
            CustomerLogo,
            License,
            FilterScheduleSummaryNotificationEmail
        }

        public enum Operation
        {
            AllIntersect,
            AllUnion,
            Combo,
            Individual,
            Venn
        }

        public enum ExportFieldType
        {
            Profile,
            Demo,
            Adhoc,
            All
        }

        public enum GroupExportSource
        {
            UADManualExport,
            UADScheduledExport
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

        public enum ListType
        {
            Filters,
            FilterSegmentations
        }

        // Task 47938:Filter Segmentation - cannot save over an existing Filter Segmenation
        public enum LoadFilterOptions
        {
            Filters = 0,
            FilterSegmentations = 1,
            Both = 2
        }
    }
}