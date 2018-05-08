using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class SuppressionFileDetail
    {
        public SuppressionFileDetail() 
        {
            SuppressionFileId = 0;
            FirstName = string.Empty;
            LastName = string.Empty;
            Company = string.Empty;
            Address1 = string.Empty;
            City = string.Empty;
            RegionCode = string.Empty;
            ZipCode = string.Empty;
            Phone = string.Empty;
            Fax = string.Empty;
            Email = string.Empty;
        }
        public SuppressionFileDetail(SuppressionFileDetail toClean)
        {
            SuppressionFileId = toClean.SuppressionFileId;
            FirstName = Core_AMS.Utilities.StringFunctions.CleanXMLString(toClean.FirstName);
            LastName = Core_AMS.Utilities.StringFunctions.CleanXMLString(toClean.LastName);
            Company = Core_AMS.Utilities.StringFunctions.CleanXMLString(toClean.Company);
            Address1 = Core_AMS.Utilities.StringFunctions.CleanXMLString(toClean.Address1);
            City = Core_AMS.Utilities.StringFunctions.CleanXMLString(toClean.City);
            RegionCode = Core_AMS.Utilities.StringFunctions.CleanXMLString(toClean.RegionCode);
            ZipCode = Core_AMS.Utilities.StringFunctions.CleanXMLString(toClean.ZipCode);
            Phone = Core_AMS.Utilities.StringFunctions.CleanXMLString(toClean.Phone);
            Fax = Core_AMS.Utilities.StringFunctions.CleanXMLString(toClean.Fax);
            Email = Core_AMS.Utilities.StringFunctions.CleanXMLString(toClean.Email);

        }
        #region Properties
        [DataMember]
        public int SuppressionFileId { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public string Address1 { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string RegionCode { get; set; }
        [DataMember]
        public string ZipCode { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Email { get; set; }
        #endregion
    }
}
