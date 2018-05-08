using System;
using System.Linq;
using System.Runtime.Serialization;

namespace KMPlatform.Object
{
    [Serializable]
    [DataContract]
    public class Product
    {
        public Product() 
        {
            IsActive = true;
            IsUAD = false;
            IsCirc = false;
            AllowDataEntry = true;
            UseSubGen = false;
        }
        public Product(int id, string name, string code, int clientID, bool isActive, bool allowDataEntry, bool? uad, bool? circ, bool subGen)
        {
            ProductID = id;
            ProductName = name;
            ProductCode = code;
            ClientID = clientID;
            IsActive = isActive;
            AllowDataEntry = allowDataEntry;
            IsUAD = uad;
            IsCirc = circ;
            UseSubGen = subGen;
        }
        #region Properties
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        //[DataMember]
        //public bool IsTradeShow { get; set; }
        [DataMember]
        public string ProductCode { get; set; }
        //[DataMember]
        //public int ProductTypeID { get; set; }
        //[DataMember]
        //public int GroupID { get; set; }
        //[DataMember]
        //public bool EnableSearching { get; set; }
        //[DataMember]
        //public int Score { get; set; }
        //[DataMember]
        //public int SortOrder { get; set; }
        //[DataMember]
        //public DateTime DateCreated { get; set; }
        //[DataMember]
        //public DateTime? DateUpdated { get; set; }
        //[DataMember]
        //public int CreatedByUserID { get; set; }
        //[DataMember]
        //public int? UpdatedByUserID { get; set; }
        [DataMember]
        public int ClientID { get; set; }
        //[DataMember]
        //public string YearStartDate { get; set; }
        //[DataMember]
        //public string YearEndDate { get; set; }
        //[DataMember]
        //public DateTime? IssueDate { get; set; }
        //[DataMember]
        //public bool? IsImported { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public bool AllowDataEntry { get; set; }
        //[DataMember]
        //public int? FrequencyID { get; set; }
        [DataMember]
        public bool? KMImportAllowed { get; set; }
        [DataMember]
        public bool? ClientImportAllowed { get; set; }
        [DataMember]
        public bool? AddRemoveAllowed { get; set; }
        //[DataMember]
        //public string AcsMailerId { get; set; }
        [DataMember]
        public bool? IsUAD { get; set; }
        [DataMember]
        public bool? IsCirc { get; set; }
        [DataMember]
        public bool UseSubGen { get; set; }
        #endregion
    }
}
