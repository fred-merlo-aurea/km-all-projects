using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class SearchData
    {
        public SearchData()
        {
            this.firstName = "";
            this.lastName = "";
            this.Company = "";
            this.Title = "";
            this.Address1 = "";
            this.City = "";
            this.State = "";
            this.Zip = "";
            this.Country = "";
            this.Email = "";
            this.Phone = "";
            this.SequenceID = 0;
            this.Account = "";
            this.PublisherID = 0;
            this.PublicationID = 0;
            this.SubscriptionID = 0;
        }
        public SearchData(SearchData sd)
        {
            this.firstName = sd.firstName;
            this.lastName = sd.lastName;
            this.Company = sd.Company;
            this.Title = sd.Title;
            this.Address1 = sd.Address1;
            this.City = sd.City;
            this.State = sd.State;
            this.Zip = sd.Zip;
            this.Country = sd.Country;
            this.Email = sd.Email;
            this.Phone = sd.Phone;
            this.SequenceID = sd.SequenceID;
            this.Account = sd.Account;
            this.PublisherID = sd.PublisherID;
            this.PublicationID = sd.PublicationID;
            this.SubscriptionID = sd.SubscriptionID;
        }
        #region Properties
        [DataMember]
        public string firstName { get; set; }
        [DataMember]
        public string lastName { get; set; }
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Address1 { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Zip { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Phone { get; set; }
        //[DataMember]
        //public int SubscriptionID { get; set; }
        [DataMember]
        public int SequenceID { get; set; }
        [DataMember]
        public string Account { get; set; }
        [DataMember]
        public int PublisherID { get; set; }
        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public int SubscriptionID { get; set; }
        #endregion

        //public override bool Equals(object obj)
        //{
        //    if (obj.GetType() != typeof(SearchData))
        //        return false;
        //    else
        //    {
        //        SearchData comparer = (SearchData)obj;
        //        foreach (PropertyInfo p in typeof(SearchData).GetProperties())
        //        {
        //            if (p.GetValue(this) != p.GetValue(comparer))
        //            {
        //                return false;
        //            }
        //        }
        //        return true;
        //    }
        //}
        #region HashSet support overrides
        //item.PubCode, item.FName, item.LName, item.Company, item.Title, item.Address, item.Email, item.Phone
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int mult = 486187739;
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * mult + firstName.GetHashCode();
                hash = hash * mult + lastName.GetHashCode();
                hash = hash * mult + Company.GetHashCode();
                hash = hash * mult + Title.GetHashCode();
                hash = hash * mult + Address1.GetHashCode();
                hash = hash * mult + City.GetHashCode();
                hash = hash * mult + State.GetHashCode();
                hash = hash * mult + Zip.GetHashCode();
                hash = hash * mult + Country.GetHashCode();
                hash = hash * mult + Email.GetHashCode();
                hash = hash * mult + Phone.GetHashCode();
                hash = hash * mult + SequenceID.GetHashCode();
                hash = hash * mult + Account.GetHashCode();
                hash = hash * mult + PublisherID.GetHashCode();
                hash = hash * mult + PublicationID.GetHashCode();
                hash = hash * mult + SubscriptionID.GetHashCode();
                return hash;
            }
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(SearchData))
                return false;

            return Equals(obj as SearchData);
        }
        public bool Equals(SearchData other)
        {

            // Check for null
            if (ReferenceEquals(other, null))
                return false;

            // Check for same reference
            if (ReferenceEquals(this, other))
                return true;

            // Check for same Values
            if (firstName == other.firstName &&
                   lastName == other.lastName &&
                   Company == other.Company &&
                   Title == other.Title &&
                   Address1 == other.Address1 &&
                   City == other.City &&
                   State == other.State &&
                   Zip == other.Zip &&
                   Country == other.Country &&
                   Email == other.Email &&
                   Phone == other.Phone &&
                   SequenceID == other.SequenceID &&
                   Account == other.Account &&
                   PublisherID == other.PublisherID &&
                   PublicationID == other.PublicationID &&
                   SubscriptionID == other.SubscriptionID)
            {
                return true;
            }
            else
                return false;
        }
        #endregion
    }
}
