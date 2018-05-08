using System;
using System.Runtime.Serialization;


namespace FrameworkDataLoad.Entity
{
    [Serializable]
    [DataContract]
    public class StandardizedField
    {
        public StandardizedField() { }
        #region Properties
        public int StandardizedFieldID { get; set; }
        public string DataFlag { get; set; }
        public string DataMpPriority { get; set; }
        public string AddressSTD { get; set; }
        public string CitySTD { get; set; }
        public string CompanyMatch1 { get; set; }
        public string CompanyMatch2 { get; set; }
        public string CompanySTD { get; set; }
        public string Company2 { get; set; }
        public string EmailSTD { get; set; }
        public string FirstNameMatch1 { get; set; }
        public string FirstNameMatch2 { get; set; }
        public string FirstNameMatch3 { get; set; }
        public string FirstNameMatch4 { get; set; }
        public string FirstNameMatch5 { get; set; }
        public string FirstNameMatch6 { get; set; }
        public string FirstNameSTD { get; set; }
        public string ForZipSTD { get; set; }
        public string InitialPhone { get; set; }
        public string LastNameSTD { get; set; }
        public string MailStopSTD { get; set; }
        public string Plus4STD { get; set; }
        public string POBox { get; set; }
        public string PostCode { get; set; }
        public string PrimaryNumber { get; set; }
        public string PrimaryPostFix { get; set; }
        public string PrimaryPreFix { get; set; }
        public string PrimaryStreet { get; set; }
        public string PrimaryType { get; set; }
        public string PrimaryRuralRouteBox { get; set; }
        public string PrimaryRuralRouteNumber { get; set; }
        public string StateSTD { get; set; }
        public string TitleSTD { get; set; }
        public string UnitDescription { get; set; }
        public string UnitNumber { get; set; }
        public string UscanPhone { get; set; }
        public string ZipSTD { get; set; }
        public int AssociateGroupCount { get; set; }
        public int AssociateGroupNumber { get; set; }
        public string AssociateGroupRank { get; set; }
        public string AssociateGroupSourceType { get; set; }
        public int CgroupCount { get; set; }
        public int CgroupNumber { get; set; }
        public string CgroupRank { get; set; }
        public string GenderSTD { get; set; }
        public string FaultOrStatusCodeBestDeliveryComponent { get; set; }
        public string InfoCode { get; set; }
        #endregion
    }
}
