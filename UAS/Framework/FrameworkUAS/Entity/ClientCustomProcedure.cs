using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class ClientCustomProcedure
    {
        public ClientCustomProcedure() { }
        #region Properties
        [DataMember]
        public int ClientCustomProcedureID { get; set; }
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string ProcedureType { get; set; }
        [DataMember]
        public string ProcedureName { get; set; }
        //[DataMember]
        //public bool IsRunBeforeDQM { get; set; }
        [DataMember]
        public int ExecutionOrder { get; set; }
        [DataMember]
        public int ServiceID { get; set; }
        [DataMember]
        public int? ServiceFeatureID { get; set; }
        [DataMember]
        public int ExecutionPointID { get; set; }
        [DataMember]
        public bool IsForSpecialFile { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        #endregion
    }
}
