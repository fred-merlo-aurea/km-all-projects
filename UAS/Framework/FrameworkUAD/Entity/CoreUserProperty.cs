using System;
using System.Runtime.Serialization;
using KM.Common.Functions;

namespace FrameworkUAD.Entity
{
	public class CoreUserProperty
	{
		public CoreUserProperty(bool defaultValue = true)
		{
			if (defaultValue)
			{
				ExternalKeyID = 0;
				FirstName = string.Empty;
				LastName = string.Empty;
				Company = string.Empty;
				Title = string.Empty;
				Occupation = string.Empty;
				Address1 = string.Empty;
				Address2 = string.Empty;
				Address3 = string.Empty;
				City = string.Empty;
				ZipCode = string.Empty;
				Plus4 = string.Empty;
				CarrierRoute = string.Empty;
				County = string.Empty;
				Country = string.Empty;
				Latitude = 0;
				Longitude = 0;
				Email = string.Empty;
				Phone = string.Empty;
				Fax = string.Empty;
				Mobile = string.Empty;
				Website = string.Empty;
				Birthdate = DateTimeFunctions.GetMinDate();
				Age = 0;
				Income = string.Empty;
				Gender = string.Empty;
				PhoneExt = string.Empty;
			}
		}

		[DataMember]
		public int ExternalKeyID { get; set; }

		[DataMember]
		public string FirstName { get; set; }

		[DataMember]
		public string LastName { get; set; }

		[DataMember]
		public string Company { get; set; }

		[DataMember]
		public string Title { get; set; }

		[DataMember]
		public string Occupation { get; set; }

		[DataMember]
		public string Address1 { get; set; }

		[DataMember]
		public string Address2 { get; set; }

		[DataMember]
		public string Address3 { get; set; }

		[DataMember]
		public string City { get; set; }

		[DataMember]
		public string ZipCode { get; set; }

		[DataMember]
		public string Plus4 { get; set; }

		[DataMember]
		public string CarrierRoute { get; set; }

		[DataMember]
		public string County { get; set; }

		[DataMember]
		public string Country { get; set; }

		[DataMember]
		public decimal Latitude { get; set; }

		[DataMember]
		public decimal Longitude { get; set; }

		[DataMember]
		public string Phone { get; set; }

		[DataMember]
		public string Fax { get; set; }

		[DataMember]
		public string Mobile { get; set; }

		[DataMember]
		public string Website { get; set; }

		[DataMember]
		public DateTime Birthdate { get; set; }

		[DataMember]
		public int Age { get; set; }

		[DataMember]
		public string Income { get; set; }

		[DataMember]
		public string Gender { get; set; }

		[DataMember]
		public string PhoneExt { get; set; }

		[DataMember]
		public string Email { get; set; }

		[DataMember(Name = "QDate")]
		public DateTime? QualificationDate { get; set; }

		[DataMember(Name = "QSourceID")]
		public int PubQSourceID { get; set; }

		[DataMember(Name = "CategoryID")]
		public int PubCategoryID { get; set; }

		[DataMember(Name = "TransactionID")]
		public int PubTransactionID { get; set; }

		[DataMember]
		public DateTime StatusUpdatedDate { get; set; }

		[DataMember]
		public string StatusUpdatedReason { get; set; }

		[DataMember]
		public DateTime DateCreated { get; set; }

		[DataMember]
		public DateTime? DateUpdated { get; set; }

		[DataMember(Name = "EmailStatus")]
		public string Status { get; set; }

		[DataMember(Name = "Name")]
		public string PubName { get; set; }

		[DataMember(Name = "PubCode")]
		public string PubCode { get; set; }

		[DataMember(Name = "PubType")]
		public string PubTypeDisplayName { get; set; }

		[DataMember]
		public int PubID { get; set; }

		[DataMember]
		public int PubSubscriptionID { get; set; }

		[DataMember]
		public int SubscriptionStatusID { get; set; }

		[DataMember(Name = "Demo7")]
		public string Demo7 { get; set; }
	}
}