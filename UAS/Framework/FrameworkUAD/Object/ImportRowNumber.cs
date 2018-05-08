using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    public class ImportRowNumber
    {
        public ImportRowNumber() { }
        #region Properties
        [DataMember]
        public int SubscriberImportRowNumber { get; set; }
        #endregion
    }
}
