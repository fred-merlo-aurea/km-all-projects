using System;
using System.Collections.Generic;
using KM.Common.Exceptions;
using static ECN_Framework_Common.Objects.Enums;

namespace ECN_Framework_Common.Objects
{
    public class ECNException : EntityException<ECNError, Entity, Method>
    {
        public ExceptionLayer ExceptionLayer { get; private set; }

        public ECNException(IList<ECNError> errorList, 
            ExceptionLayer layer = ExceptionLayer.Business)
            : this(null, errorList, layer)
        {
        }

        public ECNException(string message, IList<ECNError> errorList, 
            ExceptionLayer layer = ExceptionLayer.Business)
            : this(message, null, errorList, layer)
        {
        }

        public ECNException(string message, Exception inner, IList<ECNError> errorList,
            ExceptionLayer layer = ExceptionLayer.Business)
            : base(message, inner, errorList)
        {
            ExceptionLayer = layer;
        }

        public static string GetErrorText(ErrorMessage errorMessage)
        {
            var errorText = String.Empty;
            if (errorMessage == ErrorMessage.InvalidLink)
            {
                errorText = InvalidLinkMessage;
            }
            else if (errorMessage == ErrorMessage.PageNotFound)
            {
                errorText = PageNotFoundMessage;
            }
            else
            {
                errorText = DefaultErrorMessage;
            }

            return errorText;
        }
    }

    public class SecurityException : ApplicationException
    {
        // Default constructor
        public SecurityException()
            : base("SECURITY VIOLATION!")
        {
            SecurityType = Enums.SecurityExceptionType.RoleAccess;
        }

        // Constructor accepting a single string message
        public SecurityException(string message)
            : base(message)
        {
            SecurityType = Enums.SecurityExceptionType.RoleAccess;
        }

        // Constructor accepting a string message and an inner exception
        // that will be wrapped by this custom exception class
        public SecurityException(string message, Exception inner)
            : base(message, inner)
        {
            SecurityType = Enums.SecurityExceptionType.RoleAccess;
        }

        public ECN_Framework_Common.Objects.Enums.SecurityExceptionType SecurityType { get; set; }
    }

    public class DataNotFoundException : ApplicationException
    {
        // Default constructor
        public DataNotFoundException()
        {
        }

        // Constructor accepting a single string message
        public DataNotFoundException(string message)
            : base(message)
        {
        }

        // Constructor accepting a string message and an inner exception
        // that will be wrapped by this custom exception class
        public DataNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    //public class ECNBlastEngineException : ApplicationException
    //{
    //    // Default constructor
    //    public ECNBlastEngineException()
    //    {
    //    }

    //    // Constructor accepting a single string message
    //    public ECNBlastEngineException(string message)
    //        : base(message)
    //    {
    //    }

    //    // Constructor accepting a string message and an inner exception
    //    // that will be wrapped by this custom exception class
    //    public ECNBlastEngineException(string message, Exception inner)
    //        : base(message, inner)
    //    {
    //    }
    //}
}
