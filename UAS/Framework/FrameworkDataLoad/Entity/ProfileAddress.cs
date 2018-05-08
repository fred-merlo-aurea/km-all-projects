using System;
using System.Runtime.Serialization;


namespace FrameworkDataLoad.Entity
{
    [Serializable]
    [DataContract]
    public class ProfileAddress
    {
        public ProfileAddress() { }
        #region Properties
        public int ProfileAddressID { get; set; }
        public string Mail { get; set; }
        public string Address { get; set; }
        public string MailStop { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string AddressSTD { get; set; }
        public string CitySTD { get; set; }
        public string ForZipSTD { get; set; }
        public string MailStopSTD { get; set; }
        public string Plus4STD { get; set; }
        public string POBox { get; set; }
        public string PostCode { get; set; }
        public string PrimaryNumber { get; set; }
        public string PrimaryPostFix { get; set; }
        public string PrimaryPreFix { get; set; }
        public string PrimaryStreet { get; set; }
        public string PrimaryType { get; set; }
        public string RuralRouteBox { get; set; }
        public string RuralRouteNumber { get; set; }
        public string StateSTD { get; set; }
        public string UnitDescription { get; set; }
        public string UnitNumber { get; set; }
        public string ZipSTD { get; set; }
        public string FaultOrStatusCodeBestDeliveryComponent { get; set; }
        public string InfoCode { get; set; }
        public string PrimaryAddressBestDeliveryComponent { get; set; }
        public string Region1BestDeliveryComponent { get; set; }
        public string PostCode1BestDeliveryComponent { get; set; }
        public string Locality1NameBestDeliveryComponent { get; set; }
        public string InfoCodeNoneAssignmentInfoNone { get; set; }
        #endregion
    }
}
