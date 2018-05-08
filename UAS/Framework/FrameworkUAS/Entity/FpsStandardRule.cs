using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
	[Serializable]
	[DataContract]
	public class FpsStandardRule
	{
        public FpsStandardRule() { DateCreated = DateTime.Now; }
        #region Properties
        [DataMember]
		public int FpsStandardRuleId { get; set; }
		[DataMember]
		public string RuleName { get; set; }
		[DataMember]
		public string DisplayName { get; set; }
		[DataMember]
		public string Description { get; set; }
		[DataMember]
		public string RuleMethod { get; set; }
		[DataMember]
		public int ProcedureTypeId { get; set; }
		[DataMember]
		public int ExecutionPointId { get; set; }
		[DataMember]
		public bool IsActive { get; set; }
		[DataMember]
		public DateTime DateCreated { get; set; }
		[DataMember]
		public DateTime? DateUpdated { get; set; }
		[DataMember]
		public int CreatedByUserID{ get; set; }
		[DataMember]
		public int  UpdatedByUserID{ get; set; }
        #endregion
    }
}