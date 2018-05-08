using System;
using System.Runtime.Serialization;


namespace FrameworkDataLoad.Entity
{
    [Serializable]
    [DataContract]
    public class StagingColumn
    {
        public StagingColumn() { }
        #region Properties
        public string IncomingName { get; set; }
        public string MapToColumn { get; set; }
        #endregion
    }
}
