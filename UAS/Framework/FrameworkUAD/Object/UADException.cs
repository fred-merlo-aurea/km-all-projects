using System;
using System.Collections.Generic;
using KM.Common.Exceptions;
using EntityEnum = FrameworkUAD.Object.Enums.Entity;
using static FrameworkUAD.Object.Enums;

namespace FrameworkUAD.Object
{
    public class UADException : EntityException<UADError, EntityEnum, Method>
    {
        public ExceptionLayer ExceptionLayer { get; private set; }

        public UADException(IList<UADError> errorList,
                    ExceptionLayer layer = ExceptionLayer.Business)
                    : this(null, errorList, layer)
        {
        }

        public UADException(string message, IList<UADError> errorList,
            ExceptionLayer layer = ExceptionLayer.Business)
            : this(message, null, errorList, layer)
        {
        }

        public UADException(string message, Exception inner, IList<UADError> errorList,
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
}
