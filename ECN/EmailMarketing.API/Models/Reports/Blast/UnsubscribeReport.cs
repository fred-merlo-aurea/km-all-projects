using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace EmailMarketing.API.Models.Reports.Blast
{

    /// <summary>
    /// The Email Marketing Unsubscribe API object model.
    /// </summary>
    [Serializable]
    [DataContract]
    [XmlRoot("UnsubscribeReport")]
    public class UnsubscribeReport
    {
        public UnsubscribeReport()
        {
             EmailAddress = string.Empty;
             UnsubscribeTime = string.Empty;
             SubscriptionChange = string.Empty;
             Reason = string.Empty;
             Title = string.Empty;
             FirstName = string.Empty;
             LastName = string.Empty;
             FullName = string.Empty;
             Company = string.Empty;
             Occupation = string.Empty;
             Address = string.Empty;
             Address2 = string.Empty;
             City = string.Empty;
             State = string.Empty;
             Zip = string.Empty;
             Country = string.Empty;
             Voice = string.Empty;
             Mobile = string.Empty;
             Fax = string.Empty;
             Website = string.Empty;
             Age = -1;
             Income = -1;
             Gender = string.Empty;
             User1 = string.Empty;
             User2 = string.Empty;
             User3 = string.Empty;
             User4 = string.Empty;
             User5 = string.Empty;
             User6 = string.Empty;
             Birthdate = string.Empty;
             UserEvent1 = string.Empty;
             UserEvent1Date = string.Empty;
             UserEvent2 = string.Empty;
             UserEvent2Date = string.Empty;
             CreatedOn = string.Empty;
             LastChanged = string.Empty;
             FormatTypeCode = string.Empty;
             SubscribeTypeCode = string.Empty;
             tmp_EmailID = string.Empty;
             NickName = string.Empty;
        }

        #region Properties


        /// <summary>
        /// Unsubscriber's email address
        /// </summary>
        [DataMember]
        [XmlElement]
        public string EmailAddress { get; set; }

        /// <summary>
        /// The time the Unsubscriber unsubscribed
        /// </summary>
        [DataMember]
        [XmlElement]
        public string UnsubscribeTime { get; set; }

		/// <summary> 
		/// The subscription change
		/// </summary>
		[DataMember]
		[XmlElement]
        public string SubscriptionChange { get; set; }

		/// <summary> 
		/// Why the user unsubscribed
		/// </summary>
		[DataMember]
		[XmlElement]
        public string Reason { get; set; }

		/// <summary> 
		/// The user's title
		/// </summary>
		[DataMember]
		[XmlElement]
        public string Title { get; set; }

		/// <summary> 
		/// User's first name
		/// </summary>
		[DataMember]
		[XmlElement]
        public string FirstName { get; set; }

		/// <summary> 
		/// User's last name
		/// </summary>
		[DataMember]
		[XmlElement]
        public string LastName { get; set; }

		/// <summary> 
		/// User's full name
		/// </summary>
		[DataMember]
		[XmlElement]
        public string FullName { get; set; }

		/// <summary> 
		/// The user's company affiliation
		/// </summary>
		[DataMember]
		[XmlElement]
        public string Company { get; set; }

		/// <summary> 
		/// The user's occupation
		/// </summary>
		[DataMember]
		[XmlElement]
        public string Occupation { get; set; }

		/// <summary> 
		/// The user's residential/professional address
		/// </summary>
		[DataMember]
		[XmlElement]
        public string Address { get; set; }

		/// <summary> 
		/// A second address
		/// </summary>
		[DataMember]
		[XmlElement]
        public string Address2 { get; set; }

		/// <summary> 
		/// The city the user is located in
		/// </summary>
		[DataMember]
		[XmlElement]
        public string City { get; set; }

		/// <summary> 
		/// The state the user is located in
		/// </summary>
		[DataMember]
		[XmlElement]
        public string State { get; set; }

		/// <summary> 
		/// The zip code of the user's location
		/// </summary>
		[DataMember]
		[XmlElement]
        public string Zip { get; set; }

		/// <summary> 
		/// The country the user is located in.
		/// </summary>
		[DataMember]
		[XmlElement]
        public string Country { get; set; }

		/// <summary> 
		/// The user's phone number
		/// </summary>
		[DataMember]
		[XmlElement]
        public string Voice { get; set; }

		/// <summary> 
		/// The user's mobile phone number
		/// </summary>
		[DataMember]
		[XmlElement]
        public string Mobile { get; set; }

		/// <summary> 
		/// The user's fax number
		/// </summary>
		[DataMember]
		[XmlElement]
        public string Fax { get; set; }

		/// <summary> 
		/// The user's website url
		/// </summary>
		[DataMember]
		[XmlElement]
        public string Website { get; set; }

		/// <summary> 
		/// The user's age
		/// </summary>
		[DataMember]
		[XmlElement]
        public int Age { get; set; }

		/// <summary> 
		/// The user's income
		/// </summary>
		[DataMember]
		[XmlElement]
        public float Income { get; set; }

		/// <summary> 
		/// The user's gender
		/// </summary>
		[DataMember]
		[XmlElement]
        public string Gender { get; set; }

		/// <summary> 
		/// 
		/// </summary>
		[DataMember]
		[XmlElement]
        public string User1 { get; set; }

		/// <summary> 
		///
		/// </summary>
		[DataMember]
		[XmlElement]
        public string User2 { get; set; }

		/// <summary> 
		///
		/// </summary>
		[DataMember]
		[XmlElement]
        public string User3 { get; set; }

		/// <summary> 
		///
		/// </summary>
		[DataMember]
		[XmlElement]
        public string User4 { get; set; }

		/// <summary> 
		///
		/// </summary>
		[DataMember]
		[XmlElement]
        public string User5 { get; set; }

		/// <summary> 
		///
		/// </summary>
		[DataMember]
		[XmlElement]
        public string User6 { get; set; }

		/// <summary> 
		/// The user's birthdate
		/// </summary>
		[DataMember]
		[XmlElement]
        public string Birthdate { get; set; }

		/// <summary> 
		///
		/// </summary>
		[DataMember]
		[XmlElement]
        public string UserEvent1 { get; set; }

		/// <summary> 
		///
		/// </summary>
		[DataMember]
		[XmlElement]
        public string UserEvent1Date { get; set; }

		/// <summary> 
		///
		/// </summary>
		[DataMember]
		[XmlElement]
        public string UserEvent2 { get; set; }

		/// <summary> 
		///
		/// </summary>
		[DataMember]
		[XmlElement]
        public string UserEvent2Date { get; set; }

		/// <summary> 
		/// The date the user's account was created
		/// </summary>
		[DataMember]
		[XmlElement]
        public string CreatedOn { get; set; }

		/// <summary> 
		/// The last date the user's account was updated
		/// </summary>
		[DataMember]
		[XmlElement]
        public string LastChanged { get; set; }

		/// <summary> 
		///
		/// </summary>
		[DataMember]
		[XmlElement]
        public string FormatTypeCode { get; set; }

		/// <summary> 
		///
		/// </summary>
		[DataMember]
		[XmlElement]
        public string SubscribeTypeCode { get; set; }

		/// <summary> 
		///
		/// </summary>
		[DataMember]
		[XmlElement]
        public string tmp_EmailID { get; set; }

		/// <summary> 
		///
		/// </summary>
		[DataMember]
		[XmlElement]
        public string NickName { get; set; }

        #endregion
    }
}