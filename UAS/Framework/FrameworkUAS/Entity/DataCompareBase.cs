using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
	[Serializable]
	[DataContract]
	public abstract class DataCompareBase: CommonDataCompareProperties
	{
		[DataMember]
		public int ClientId { get; set; }
	}
}