using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using KM.Common;
using KM.Common.Functions;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class SubscriberBase
    {
        public const int MinEmailLength = 4;
        public const int NoId = -1;
        public const int ZeroId = 0;
        public const int NoInt = -1;
        public const int ZeroInt = 0;
        public const int StatusOne = 1;

        public SubscriberBase()
        {
            SourceFileID = NoId;
            Sequence = NoInt;
            PubCode = string.Empty;

            Company = string.Empty;
            City = string.Empty;
            Plus4 = string.Empty;
            ForZip = string.Empty;
            County = string.Empty;
            Country = string.Empty;
            CountryID = NoId;
            Phone = string.Empty;
            Fax = string.Empty;
            Email = string.Empty;
            CategoryID = NoId;
            TransactionID = NoId;
            QSourceID = NoId;
            RegCode = string.Empty;
            OrigsSrc = string.Empty;
            Par3C = string.Empty;
            Source = string.Empty;
            Priority = string.Empty;
            Sic = string.Empty;
            SicCode = string.Empty;
            Gender = string.Empty;
            Address3 = string.Empty;
            Home_Work_Address = string.Empty;
            Demo7 = string.Empty;
            Mobile = string.Empty;
            Latitude = NoInt;
            Longitude = NoInt;
            CreatedByUserID = NoId;
            UpdatedByUserID = NoId;
            ProcessCode = string.Empty;
            TextPermission = null;
            IsActive = true;
            ImportRowNumber = NoInt;
        }

        public SubscriberBase(string processCode) : this()
        {
            ProcessCode = processCode;
        }

        [DataMember]
        public int SourceFileID { get; set; }

        [DataMember]
        [StringLength(100)]
        public string PubCode { get; set; }

        [DataMember]
        public int Sequence { get; set; }

        [DataMember]
        [StringLength(100)]
        public string Company { get; set; }

        [DataMember]
        [StringLength(50)]
        public string City { get; set; }

        [DataMember]
        [StringLength(50)]
        public string Plus4 { get; set; }

        [DataMember]
        [StringLength(50)]
        public string ForZip { get; set; }

        [DataMember]
        [StringLength(100)]
        public string County { get; set; }

        [DataMember]
        [StringLength(100)]
        public string Country { get; set; }

        [DataMember]
        public int CountryID { get; set; }

        private string _phone;
        [DataMember]
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = StringFunctions.FormatPhoneNumber(value);
            }
        }

        private string _fax;
        [DataMember]
        public string Fax
        {
            get { return _fax; }
            set
            {
                _fax = StringFunctions.FormatPhoneNumber(value);
            }
        }
        private string _email;

        [DataMember]
        [StringLength(100)]
        public string Email
        {
            get
            {
                if ((_email?.Length ?? 0) < MinEmailLength)
                    return string.Empty;
                else
                    return _email;
            }
            set { _email = value; }
        }

        [DataMember]
        public int CategoryID { get; set; }

        [DataMember]
        public int TransactionID { get; set; }

        [DataMember]
        public int QSourceID { get; set; }

        [DataMember]
        [StringLength(5)]
        public string RegCode { get; set; }

        [DataMember]
        [StringLength(50)]
        public string OrigsSrc { get; set; }
        [DataMember]
        [StringLength(10)]
        public string Par3C { get; set; }
        [DataMember]
        [StringLength(50)]
        public string Source { get; set; }
        [DataMember]
        [StringLength(4)]
        public string Priority { get; set; }
        [StringLength(8)]
        public string Sic { get; set; }
        [DataMember]
        [StringLength(20)]
        public string SicCode { get; set; }
        [DataMember]
        [StringLength(1024)]
        public string Gender { get; set; }

        [StringLength(255)]
        public string Address3 { get; set; }

        [DataMember]
        [StringLength(10)]
        public string Home_Work_Address { get; set; }

        [DataMember]
        [StringLength(1)]
        public string Demo7 { get; set; }

        private string _mobile;
        [DataMember]
        public string Mobile
        {
            get { return _mobile; }
            set
            {
                _mobile = StringFunctions.FormatPhoneNumber(value);
            }
        }

        [DataMember]
        public decimal Latitude { get; set; }

        [DataMember]
        public decimal Longitude { get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [DataMember]
        public DateTime? DateUpdated { get; set; } = DateTimeFunctions.GetMinDate();

        [DataMember]
        public int CreatedByUserID { get; set; }

        [DataMember]
        public int? UpdatedByUserID { get; set; }

        [DataMember]
        [StringLength(50)]
        public string ProcessCode { get; set; }

        [DataMember]
        public string AccountNumber { get; set; } = string.Empty;

        [DataMember]
        public int EmailID { get; set; } = ZeroInt;

        [DataMember]
        public int Copies { get; set; } = ZeroInt;

        [DataMember]
        public int GraceIssues { get; set; } = ZeroInt;

        [DataMember]
        public bool IsComp { get; set; }

        [DataMember]
        public bool IsPaid { get; set; }

        [DataMember]
        public bool IsSubscribed { get; set; } = true;

        [DataMember]
        [StringLength(50)]
        public string Occupation { get; set; } = string.Empty;

        [DataMember]
        public int SubscriptionStatusID { get; set; } = ZeroId;

        [DataMember]
        public int SubsrcID { get; set; } = ZeroId;

        [DataMember]
        [StringLength(255)]
        public string Website { get; set; } = string.Empty;

        [DataMember]
        public bool? TextPermission { get; set; }

        [DataMember]
        public int ImportRowNumber { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int ExternalKeyId { get; set; } = ZeroId;

        protected void ClearPhoneMobileFax()
        {
            _phone = null;
            _mobile = null;
            _fax = null;
        }
    }
}
