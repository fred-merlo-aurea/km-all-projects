using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
	[Serializable]
	[DataContract]
	public class FpsCustomRule
	{
        public FpsCustomRule() { DateCreated = DateTime.Now; }
        #region Properties
        [DataMember]
		public int FpsCustomRuleId { get; set; }
		[DataMember]
		public int ClientId { get; set; }
        [DataMember]
        public string RuleName { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int ExecutionPointId { get; set; }
        [DataMember]
		public int Line { get; set; }
		[DataMember]
		public bool IsGrouped { get; set; }
		[DataMember]
		public int GroupNumber { get; set; }
		[DataMember]
		public string Link { get; set; }
		[DataMember]
		public string IncomingField { get; set; }
		[DataMember]
		public bool IsProfileField { get; set; }
		[DataMember]
		public string Operator { get; set; }
		[DataMember]
		public string ConditionValue{ get; set; }
		[DataMember]
		public string Clause { get; set; }
		[DataMember]
		public string ProductField { get; set; }
		[DataMember]
		public string ProductValue { get; set; }
		[DataMember]
		public bool  IsActive { get; set; }
        [DataMember]
        public bool IsGlobalRule { get; set; }
        [DataMember]
        public DateTime? StartDate { get; set; }
        [DataMember]
        public DateTime? EndDate { get; set; }
        [DataMember]
		public DateTime DateCreated { get; set; }
		[DataMember]
		public DateTime? DateUpdated{ get; set; }
		[DataMember]
		public int CreatedByUserId{ get; set; }
		[DataMember]
		public int UpdatedByUserId{ get; set; }
        #endregion

    }
}