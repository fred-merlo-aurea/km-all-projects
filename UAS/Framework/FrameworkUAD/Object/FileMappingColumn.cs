using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class FileMappingColumn
    {
        public FileMappingColumn() { }
        #region Properties
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string DataTable { get; set; }
        [DataMember]
        public string TablePrefix { get; set; }
        [DataMember]
        public string ColumnName { get; set; }
        [DataMember]
        public string DataType { get; set; }
        [DataMember]
        public bool IsDemographic { get; set; }
        [DataMember]
        public bool IsDemographicOther { get; set; }
        [DataMember]
        public bool IsDemographicDate { get; set; }
        [DataMember]
        public bool IsMultiSelect { get; set; }
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public string UxControl { get; set; }

        #endregion
    }
}
