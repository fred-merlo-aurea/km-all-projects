using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class Reporting
    {
        public Reporting() 
        {
            PublicationIDs = "";
            CategoryIDs = "";
            TransactionIDs = "";
            QSourceIDs = "";
            StateIDs = "";
            Regions = "";
            CountryIDs = "";
            Email = "";
            Fax = "";
            Phone = "";
            Mobile = "";
            FromDate = "";
            ToDate = "";
            Year = "";
            Media = "";
            CategoryCodes = "";
            TransactionCodes = "";
            ResponseIDs = "";
            UADResponseIDs = "";
            AdHocXML = "";
            Demo31 = "";
            Demo32 = "";
            Demo33 = "";
            Demo34 = "";
            Demo35 = "";
            Demo36 = "";
            IsMailable = "";
            EmailStatusIDs = "";
            OpenSearchType = "";
            OpenCount = "-1";
            OpenDateFrom = "";
            OpenDateTo = "";
            OpenBlastID = "";
            OpenEmailSubject = "";
            OpenEmailFromDate = "";
            OpenEmailToDate = "";
            ClickSearchType = "";
            ClickCount = "-1";
            ClickURL = "";
            ClickDateFrom = "";
            ClickDateTo = "";
            ClickBlastID = "";
            ClickEmailSubject = "";
            ClickEmailFromDate = "";
            ClickEmailToDate = "";
            Domain = "";
            VisitsURL = "";
            VisitsDateFrom = "";
            VisitsDateTo = "";
            BrandID = "";
            SearchType = "";
            RangeMaxLatMin = "";
            RangeMaxLatMax = "";
            RangeMaxLonMin = "";
            RangeMaxLonMax = "";
            RangeMinLatMin = "";
            RangeMinLatMax = "";
            RangeMinLonMin = "";
            RangeMinLonMax = "";
            WaveMail = "";
        }

        public Reporting(Reporting r)
        {
            PublicationIDs = r.PublicationIDs;
            CategoryIDs = r.CategoryIDs;
            TransactionIDs = r.TransactionIDs;
            QSourceIDs = r.QSourceIDs;
            StateIDs = r.StateIDs;
            Regions = r.Regions;
            CountryIDs = r.CountryIDs;
            Email = r.Email;
            Fax = r.Fax;
            Phone = r.Phone;
            Mobile = r.Mobile;
            FromDate = r.FromDate;
            ToDate = r.ToDate;
            Year = r.Year;
            Media = r.Media;
            CategoryCodes = r.CategoryCodes;
            TransactionCodes = r.TransactionCodes;
            ResponseIDs = r.ResponseIDs;
            UADResponseIDs = r.UADResponseIDs;
            AdHocXML = r.AdHocXML;
            Demo31 = r.Demo31;
            Demo32 = r.Demo32;
            Demo33 = r.Demo33;
            Demo34 = r.Demo34;
            Demo35 = r.Demo35;
            Demo36 = r.Demo36;
            IsMailable = r.IsMailable;
            EmailStatusIDs = r.EmailStatusIDs;
            OpenSearchType = r.OpenSearchType;
            OpenCount = r.OpenCount;
            OpenDateFrom = r.OpenDateFrom;
            OpenDateTo = r.OpenDateTo;
            OpenBlastID = r.OpenBlastID;
            OpenEmailSubject = r.OpenEmailSubject;
            OpenEmailFromDate = r.OpenEmailFromDate;
            OpenEmailToDate = r.OpenEmailToDate;
            ClickSearchType = r.ClickSearchType;
            ClickCount = r.ClickCount;
            ClickURL = r.ClickURL;
            ClickDateFrom = r.ClickDateFrom;
            ClickDateTo = r.ClickDateTo;
            ClickBlastID = r.ClickBlastID;
            ClickEmailSubject = r.ClickEmailSubject;
            ClickEmailFromDate = r.ClickEmailFromDate;
            ClickEmailToDate = r.ClickEmailToDate;
            Domain = r.Domain;
            VisitsURL = r.VisitsURL;
            VisitsDateFrom = r.VisitsDateFrom;
            VisitsDateTo = r.VisitsDateTo;
            BrandID = r.BrandID;
            SearchType = r.SearchType;
            RangeMaxLatMin = r.RangeMaxLatMin;
            RangeMaxLatMax = r.RangeMaxLatMax;
            RangeMaxLonMin = r.RangeMaxLonMin;
            RangeMaxLonMax = r.RangeMaxLonMax;
            RangeMinLatMin = r.RangeMinLatMin;
            RangeMinLatMax = r.RangeMinLatMax;
            RangeMinLonMin = r.RangeMinLonMin;
            RangeMinLonMax = r.RangeMinLonMax;
            WaveMail = r.WaveMail;
        }

        #region Standard Properties
        [DataMember]
        public string PublicationIDs { get; set; }
        [DataMember]
        public string CategoryIDs { get; set; }
        [DataMember]
        public string TransactionIDs { get; set; }
        [DataMember]
        public string QSourceIDs { get; set; }
        [DataMember]
        public string StateIDs { get; set; }
        [DataMember]
        public string Regions { get; set; }
        [DataMember]
        public string CountryIDs { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string Year { get; set; }
        [DataMember]
        public string Media { get; set; }
        [DataMember]
        public string CategoryCodes { get; set; }
        [DataMember]
        public string TransactionCodes { get; set; }
        [DataMember]
        public string ResponseIDs { get; set; }
        [DataMember]
        public string UADResponseIDs { get; set; }
        [DataMember]
        public string AdHocXML { get; set; }
        [DataMember]
        public string Demo31 { get; set; }
        [DataMember]
        public string Demo32 { get; set; }
        [DataMember]
        public string Demo33 { get; set; }
        [DataMember]
        public string Demo34 { get; set; }
        [DataMember]
        public string Demo35 { get; set; }
        [DataMember]
        public string Demo36 { get; set; }
        [DataMember]
        public string EmailStatusIDs { get; set; }
        [DataMember]
        public string IsMailable { get; set; }
        [DataMember]
        public string WaveMail { get; set; }
        #endregion
        #region Activity Properties
        [DataMember]
        public string OpenSearchType { get; set; }
        [DataMember]
        public string OpenCount { get; set; }
        [DataMember]
        public string OpenDateFrom { get; set; }
        [DataMember]
        public string OpenDateTo { get; set; }
        [DataMember]
        public string OpenBlastID { get; set; }
        [DataMember]
        public string OpenEmailSubject { get; set; }
        [DataMember]
        public string OpenEmailToDate { get; set; }
        [DataMember]
        public string OpenEmailFromDate { get; set; }
        [DataMember]
        public string ClickSearchType { get; set; }
        [DataMember]
        public string ClickCount { get; set; }
        [DataMember]
        public string ClickDateFrom { get; set; }
        [DataMember]
        public string ClickDateTo { get; set; }
        [DataMember]
        public string ClickURL { get; set; }
        [DataMember]
        public string ClickBlastID { get; set; }
        [DataMember]
        public string ClickEmailSubject { get; set; }
        [DataMember]
        public string ClickEmailFromDate { get; set; }
        [DataMember]
        public string ClickEmailToDate { get; set; }
        [DataMember]
        public string Domain { get; set; }
        [DataMember]
        public string VisitsURL { get; set; }
        [DataMember]
        public string VisitsDateFrom { get; set; }
        [DataMember]
        public string VisitsDateTo { get; set; }
        [DataMember]
        public string BrandID { get; set; }
        [DataMember]
        public string SearchType { get; set; }
        #endregion
        #region Zip Range Properties
        [DataMember]
        public string RangeMaxLatMin { get; set; }
        [DataMember]
        public string RangeMaxLatMax { get; set; }
        [DataMember]
        public string RangeMaxLonMin { get; set; }
        [DataMember]
        public string RangeMaxLonMax { get; set; }
        [DataMember]
        public string RangeMinLatMin { get; set; }
        [DataMember]
        public string RangeMinLatMax { get; set; }
        [DataMember]
        public string RangeMinLonMin { get; set; }
        [DataMember]
        public string RangeMinLonMax { get; set; }
        #endregion
    }
}
