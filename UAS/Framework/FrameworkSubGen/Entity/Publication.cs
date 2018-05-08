using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class Publication: IEntity
    {
        public Publication() { }
        #region Properties
        [DataMember]
        public int publication_id { get; set; }
        [DataMember]
        [StringLength(50, ErrorMessage = "The value cannot exceed 50 characters.")]
        public string name { get; set; }
        [DataMember]
        public int issues_per_year { get; set; }
        [DataMember]
        public int account_id { get; set; }
        [DataMember]
        public string KMPubCode { get; set; }
        [DataMember]
        public int KMPubID { get; set; }
        #endregion
    }
}
