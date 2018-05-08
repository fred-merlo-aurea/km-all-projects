using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
	[Serializable]
	[DataContract]
	public abstract class CommonDataCompareProperties
	{
		[DataMember]
		public int CodeTypeId { get; set; }
		[DataMember]
		public decimal CodeTypeCostModifier { get; set; }
		[DataMember]
		public DateTime DateCreated { get; set; } = DateTime.Now;
		[DataMember]
		public int CreatedByUserId { get; set; }
		[DataMember]
		public DateTime? DateUpdated { get; set; }
		[DataMember]
		public int? UpdatedByUserId { get; set; }
	}
}