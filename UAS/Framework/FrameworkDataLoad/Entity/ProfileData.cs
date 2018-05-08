using System;
using System.Runtime.Serialization;


namespace FrameworkDataLoad.Entity
{
    [Serializable]
    [DataContract]
    public class ProfileData
    {
        public ProfileData() { }
        #region Properties
        public int ProfileDataID { get; set; }
        public DateTime QualificationDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string CompanySTD { get; set; }
        public string EmailSTD { get; set; }
        public string FirstNameMatch1 { get; set; }
        public string FirstNameMatch2 { get; set; }
        public string FirstNameMatch3 { get; set; }
        public string FirstNameMatch4 { get; set; }
        public string FirstNameMatch5 { get; set; }
        public string FirstNameMatch6 { get; set; }
        public string UscanPhone { get; set; }
        public string FirstNameSTD { get; set; }
        public string InitialPhone { get; set; }
        public string LastNameSTD { get; set; }
        public string TitleSTD { get; set; }
        public string GenderSTD { get; set; }
        public string Country { get; set; }
        #endregion
    }
}
