using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml;
using System.Runtime.Serialization;
using System.Collections;
using System.Xml.Serialization;

namespace EmailMarketing.API.Models.EmailGroup
{

    [Serializable]
    [DataContract(Namespace = "")]
    [XmlRoot("Profile")]
    public class Profile 
        {
        //public static implicit operator Email(Profile profile)
        //{
        //    return new Email
        //    {
        //        EmailAddress = profile.EmailAddress
        //    };
        //}

        //public class EmailGroup
        //{
        //    /// <summary>
        //    /// Used to coordinate between the Profile and UDF table value input parameters of the 
        //    /// ManageSubsubscribersWithProfile stored procedure; filled right before that is called.
        //    /// </summary>
        //    internal Guid SubscriberInputTableRowID { get; set; }

        //    public int GroupID { get; set; }
        //    public Formats Format { get; set; }
        //    public SubscribeTypes SubscribeType { get; set; }
        //    public Dictionary<string,string> CustomFields { get; set; }

        //    public IEnumerable<Dictionary<string, string>> TransactionalCustomFields { get; set; }
        //}

        //public IEnumerable<EmailGroup> Groups { get; set; }
        /// <summary>
        /// Email Address for subscriber
        /// </summary>
        [Required]
        [DataMember]
        public string EmailAddress { get; set; }
        /// <summary>
        /// Job title
        /// </summary>
        [DataMember]
        public string Title { get; set; }
        /// <summary>
        /// Subscribers first name
        /// </summary>
        [DataMember]
        public string FirstName { get; set; }
        /// <summary>
        /// Subscribers last name
        /// </summary>
        [DataMember]
        public string LastName { get; set; }
        /// <summary>
        /// Subscribers full name
        /// </summary>
        [DataMember]
        public string FullName{ get; set; }
        /// <summary>
        /// Subscribers company
        /// </summary>
        [DataMember]
        public string Company { get; set; }
        /// <summary>
        /// Subscribers current occupation
        /// </summary>
        [DataMember]
        public string Occupation { get; set; }
        /// <summary>
        /// Subscribers address
        /// </summary>
        [DataMember]
        public string Address { get; set; }
        /// <summary>
        /// Subscribers extra address info
        /// </summary>
        [DataMember]
        public string Address2 { get; set; }
        /// <summary>
        /// Subscribers city that they live in
        /// </summary>
        [DataMember]
        public string City { get; set; }
        /// <summary>
        /// Subscribers state that they live in
        /// </summary>
        [DataMember]
        public string State { get; set; }
        /// <summary>
        /// Subscribers zip code 
        /// </summary>
        [DataMember]
        public string Zip { get; set; }
        /// <summary>
        /// Subscribers country that they live in
        /// </summary>
        [DataMember]
        public string Country { get; set; }
        /// <summary>
        /// Subscribers home phone number
        /// </summary>
        [DataMember]
        public string Voice { get; set; }
        /// <summary>
        /// Subscribers mobile number
        /// </summary>
        [DataMember]
        public string Mobile { get; set; }
        /// <summary>
        /// Subscribers fax number
        /// </summary>
        [DataMember]
        public string Fax { get; set; }
        /// <summary>
        /// Subscribers website url for the company they work at
        /// </summary>
        [DataMember]
        public string Website { get; set; }
        /// <summary>
        /// Subscribers age
        /// </summary>
        [DataMember]
        public string Age { get; set; }
        /// <summary>
        /// Subscribers income
        /// </summary>
        [DataMember]
        public string Income { get; set; }
        /// <summary>
        /// Subscribers gender
        /// </summary>
        [DataMember]
        public string Gender { get; set; }
        /// <summary>
        /// Subscribers user1 info
        /// </summary>
        [DataMember]
        public string User1 { get; set; }
        /// <summary>
        /// Subscribers user2 info
        /// </summary>
        [DataMember]
        public string User2 { get; set; }
        /// <summary>
        /// Subscribers user3 info
        /// </summary>
        [DataMember]
        public string User3 { get; set; }
        /// <summary>
        /// Subscribers user4 info
        /// </summary>
        [DataMember]
        public string User4 { get; set; }
        /// <summary>
        /// Subscribers user5 info
        /// </summary>
        [DataMember]
        public string User5 { get; set; }
        /// <summary>
        /// Subscribers user6 info
        /// </summary>
        [DataMember]
        public string User6 { get; set; }
        /// <summary>
        /// Subscribers birthday
        /// </summary>
        [DataMember]
        public DateTime? Birthdate { get; set; }
        /// <summary>
        /// Subscribers user event 1
        /// </summary>
        [DataMember]
        public string UserEvent1 { get; set; }
        /// <summary>
        /// Subscribers user event 1 date
        /// </summary>
        [DataMember]
        public DateTime? UserEvent1Date
        {
            get;
            set;
        }
        /// <summary>
        /// Subscribers user event 2
        /// </summary>
        [DataMember]
        public string UserEvent2 { get; set; }
        /// <summary>
        /// Subscribers user event 2 date
        /// </summary>
        [DataMember]
        public DateTime? UserEvent2Date { get; set; }
        /// <summary>
        /// Subscribers notes
        /// </summary>
        [DataMember]
        public string Notes { get; set; }
        /// <summary>
        /// Subscribers password
        /// </summary>
        [DataMember]
        public string Password { get; set; }
        /// <summary>
        /// Subscribers subscribe type 
        /// </summary>
        [DataMember]
        public SubscribeTypes SubscribeType { get; set; }
        /// <summary>
        /// Subscribers format type
        /// </summary>
        [DataMember]
        public Formats Format { get; set; }
        /// <summary>
        /// Subscribers UDF data
        /// </summary>
        [DataMember]
        public List<ProfileCustomField> CustomFields { get; set; }
    }
}