using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
	[Serializable]
	[DataContract]
	public abstract class FilterBase
	{
		[DataMember]
		public DateTime DateCreated { get; set; } = DateTime.Now;

		[DataMember]
		public DateTime? DateUpdated { get; set; }

		[DataMember]
		public int CreatedByUserID { get; set; }

		[DataMember]
		public int? UpdatedByUserID { get; set; }
	}
}