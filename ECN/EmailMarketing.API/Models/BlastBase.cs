using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace EmailMarketing.API.Models
{
	public abstract class BlastBase
	{
		/// <summary>
		/// ID of the User who created the blast,
		/// </summary>
		[IgnoreDataMember]
		[Newtonsoft.Json.JsonIgnore]
		[XmlIgnore]
		public int? CreatedUserID { get; set; }

		/// <summary>
		/// Date the blast was created
		/// </summary>
		[IgnoreDataMember]
		[Newtonsoft.Json.JsonIgnore]
		[XmlIgnore]
		public DateTime? CreatedDate { get; set; }

		/// <summary>
		/// If updated, the user to put the change.  Otherwise null.
		/// </summary>
		[IgnoreDataMember]
		[Newtonsoft.Json.JsonIgnore]
		[XmlIgnore]
		public int? UpdatedUserID { get; set; }

		/// <summary>
		/// If updated, the date of the change.  Otherwise null.
		/// </summary>
		[IgnoreDataMember]
		[Newtonsoft.Json.JsonIgnore]
		[XmlIgnore]
		public DateTime? UpdatedDate { get; set; }

		/// <summary>
		/// ID for blast. This value is assigned by ECN when new blasts are posted.  
		/// Required, except for post and search where ignored.
		/// </summary>
		[DataMember]
		[XmlElement]
		public int BlastID { get; set; }

		/// <summary>
		/// Read-only, current status of the blast
		/// </summary>
		[DataMember]
		[XmlElement]
		public string StatusCode { get; set; }

		/// <summary>Only standard blasts are supported by SimpleBlast</summary>
		[DataMember]
		[XmlElement]
		public string BlastType { get; set; }

		/// <summary>Required, the ID of an existing layout</summary>
		[DataMember]
		[XmlElement]
		public int LayoutID { get; set; }

		/// <summary>
		/// Required, the ID of a recipient email group.
		/// </summary>
		[DataMember]
		[XmlElement]
		public int GroupID { get; set; }

		/// <summary>
		/// Optional, ID of a filter already posted to ECN.
		/// </summary>
		[DataMember]
		[XmlElement(IsNullable = true)]
		public int? FilterID { get; set; }

		/// <summary>Required, Subject line.  If passing emojis, they should be in unicode escape format(\uXXXX)</summary>
		[DataMember]
		[XmlElement]
		public string EmailSubject { get; set; }

		/// <summary>Required, From e-mail address</summary>
		[DataMember]
		[XmlElement]
		public string EmailFrom { get; set; }

		/// <summary>Required, From e-mail name</summary>
		[DataMember]
		[XmlElement]
		public string EmailFromName { get; set; }

		/// <summary>Required, Blast Reply To e-mail address</summary>
		[DataMember]
		[XmlElement]
		public string ReplyTo { get; set; }
	}
}