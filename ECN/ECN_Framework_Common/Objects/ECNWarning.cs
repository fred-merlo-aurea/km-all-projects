using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECN_Framework_Common.Objects
{
    public partial class ECNWarning : Exception
    {
        public List<ECNWarning> ErrorList { get; private set; }
        public Enums.ExceptionLayer ExceptionLayer { get; private set; }
        //public bool Notify{ get; private set; }

        // Default constructor
        public ECNWarning(List<ECNWarning> errorList, Enums.ExceptionLayer layer = Enums.ExceptionLayer.Business)
        {
            ErrorList = errorList;
            ExceptionLayer = layer;
        }

        // Constructor accepting a single string message
        public ECNWarning(string message, List<ECNWarning> errorList, Enums.ExceptionLayer layer = Enums.ExceptionLayer.Business)
            : base(message)
        {
            ErrorList = errorList;
            ExceptionLayer = layer;
        }

        // Constructor accepting a string message and an inner exception
        // that will be wrapped by this custom exception class
        public ECNWarning(string message, Exception inner, List<ECNWarning> errorList, Enums.ExceptionLayer layer = Enums.ExceptionLayer.Business)
            : base(message, inner)
        {
            ErrorList = errorList;
            ExceptionLayer = layer;
        }

        public static string CreateErrorMessage(ECNWarning exception)
        {
            string message = string.Empty;
            foreach (ECNWarning error in exception.ErrorList)
            {
                message += "Entity: " + error.Entity + " Method: " + error.Method + " Message: " + error.WarningMessage + ". ";
            }

            return message;
        }
    }
}
