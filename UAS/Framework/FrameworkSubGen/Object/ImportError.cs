using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;

namespace FrameworkSubGen.Object
{
    public class ImportError
    {
        public ImportError()
        {
            Method = string.Empty;
            ErrorMsg = string.Empty;
            DataRow = null;
            ImportFile = null;
            SubGenImportSubscriber = null;
            UADSubscriberTransformed = null;

        }
        #region Properties
        public string Method { get; set; }
        public string ErrorMsg { get; set; }
        public StringDictionary DataRow { get; set; }
        public FileInfo ImportFile { get; set; }
        public FrameworkSubGen.Entity.ImportSubscriber SubGenImportSubscriber { get; set; }
        public FrameworkUAD.Entity.SubscriberTransformed UADSubscriberTransformed { get; set; }
        #endregion
    }
}
