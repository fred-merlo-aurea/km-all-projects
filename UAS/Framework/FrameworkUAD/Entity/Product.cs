using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class Product
    {
        public Product()
        {
            PubID = 0;
            PubName = string.Empty;
            istradeshow = false;
            PubCode = string.Empty;
            PubTypeID = 0;
            GroupID = 0;
            EnableSearching = false;
            score = 0;
            SortOrder = 0;
            CodeSheets = new List<CodeSheet>();
            ResponseGroups = new List<ResponseGroup>();
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
            ClientID = 0;
            AllowDataEntry = true;
            IsActive = true;
            IsCirc = false;
            IsUAD = false;
            HasPaidRecords = false;
            UseSubGen = false;
        }
        #region Properties
        [DataMember(Name = "ProductID")]
        public int PubID { get; set; }
        [DataMember(Name = "ProductName")]
        public string PubName { get; set; }
        [DataMember(Name = "IsTradeShow")]
        public bool istradeshow { get; set; }
        [DataMember(Name = "ProductCode")]
        public string PubCode { get; set; }
        [DataMember(Name = "ProductTypeID")]
        public int PubTypeID { get; set; }
        [DataMember(Name = "GroupID")]
        public int GroupID { get; set; }
        [DataMember(Name = "EnableSearching")]
        public bool EnableSearching { get; set; }
        [DataMember(Name = "Score")]
        public int score { get; set; }
        [DataMember]
        public int SortOrder { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        [DataMember]
        public int ClientID { get; set; }
	    [DataMember]
        public string YearStartDate { get; set; }
	    [DataMember]
        public string YearEndDate { get; set; }
	    [DataMember]
        public DateTime? IssueDate { get; set; }
	    [DataMember]
        public bool? IsImported { get; set; }
	    [DataMember]
        public bool IsActive { get; set; }
	    [DataMember]
        public bool AllowDataEntry { get; set; }
	    [DataMember]
        public int? FrequencyID { get; set; }
	    [DataMember]
        public bool? KMImportAllowed { get; set; }
	    [DataMember]
        public bool? ClientImportAllowed { get; set; }
	    [DataMember]
        public bool? AddRemoveAllowed { get; set; }
	    [DataMember]
        public int AcsMailerInfoId { get; set; }
        [DataMember]
        public bool? IsUAD { get; set; }
        [DataMember]
        public bool? IsCirc { get; set; }
        [DataMember]
        public bool? IsOpenCloseLocked { get; set; }
        [DataMember]
        public bool HasPaidRecords { get; set; }
        [DataMember]
        public bool UseSubGen { get; set; }
        #endregion

        [DataMember]
        public List<CodeSheet> CodeSheets { get; set; }
        [DataMember]
        public List<ResponseGroup> ResponseGroups { get; set; }
    }
}
