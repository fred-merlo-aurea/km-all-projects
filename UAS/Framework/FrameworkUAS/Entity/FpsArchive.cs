using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
	[Serializable]
	[DataContract]
	public class FpsArchive
	{
        public FpsArchive() { DateCreated = DateTime.Now; }
        #region Properties
        [DataMember]
        public int FpsArchiveId { get; set; }
        [DataMember]
		public int ClientId { get; set; }
		[DataMember]
		public int SourceFileId { get; set; }
		[DataMember]
		public string ObjectType { get; set; }
		[DataMember]
		public string ObjectJson { get; set; }
		[DataMember]
		public DateTime DateCreated { get; set; }
		[DataMember]
		public int CreatedByUserId { get; set; }
		[DataMember]
		public DateTime? DateUpdated { get; set; }
		[DataMember]
		public int UpdatedByUserId { get; set; }
        #endregion

    }
}