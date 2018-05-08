using System;
using System.Collections.Generic;

namespace UAS.Web.Models.Common
{
    public class UASException : Exception
    {
        public List<UASError> ErrorList { get; private set; }
        public UASException(List<UASError> errorList)
        {
            ErrorList = errorList;
        }
    }

    [Serializable]
    public class UASError
    {
        public string ErrorMessage { get; set; }

        public UASError()
        {
        }

        public UASError(string error)
        {
            ErrorMessage = error;
        }
    }
}