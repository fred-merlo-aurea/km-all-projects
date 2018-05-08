using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
	[Serializable]
	[DataContract]
	public class FpsMap
	{
        public FpsMap() { DateCreated = DateTime.Now; }
        #region Properties
        [DataMember]
		public int FpsMapId { get; set; }
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public int SourceFileId { get; set; }
        [DataMember]
		public bool IsStandardRule { get; set; }
        [DataMember]
        public int FpsStandardRuleId { get; set; }
        [DataMember]
        public int FpsCustomRuleId { get; set; }
        [DataMember]
		public int RuleOrder { get; set; }
		[DataMember]
		public bool IsActive { get; set; }
		[DataMember]
		public DateTime DateCreated { get; set; }
		[DataMember]
		public DateTime? DateUpdated { get; set; }
		[DataMember]
		public int CreatedByUserId { get; set; }
		[DataMember]
		public int UpdatedByUserId { get; set; }
        #endregion
    }
}