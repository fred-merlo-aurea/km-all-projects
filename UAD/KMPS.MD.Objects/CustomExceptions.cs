using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMPS.MD.Objects
{
        [Serializable]
        public class DuplicateFilterException : Exception
        {
            public DuplicateFilterException(string errorMessage) : base(errorMessage) { }
            public DuplicateFilterException(string errorMessage, Exception innerEx) : base(errorMessage, innerEx) { }
        }

        [Serializable]
        public class FilterNoRecordsException : Exception
        {
            public FilterNoRecordsException(string errorMessage) : base(errorMessage) { }
            public FilterNoRecordsException(string errorMessage, Exception innerEx) : base(errorMessage, innerEx) { }
        }

        [Serializable]
        public class InvalidZipCodeException : Exception
        {
            public InvalidZipCodeException(string errorMessage) : base(errorMessage) { }
            public InvalidZipCodeException(string errorMessage, Exception innerEx) : base(errorMessage, innerEx) { }
        }

}