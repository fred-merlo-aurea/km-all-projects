using System;
using System.Collections.Generic;
using System.Text;

namespace KM.Common.Exceptions
{
    [Serializable]
    public class EntityException<TError, TEntityEnum, TEntityMethodEnum> : Exception
       where TError : EntityError<TEntityEnum, TEntityMethodEnum>
       where TEntityEnum : struct
       where TEntityMethodEnum : struct
    {
        protected const string InvalidLinkMessage = "We're sorry, but the link you are requesting appears to be invalid.";
        protected const string PageNotFoundMessage = "Sorry! The page you have requested does not exist.";
        protected const string DefaultErrorMessage = "Sorry! We're having trouble processing your request right now.";

        public IList<TError> ErrorList { get; }

        public EntityException(IList<TError> errorList)
            : this(null, errorList)
        {
        }

        public EntityException(string message, IList<TError> errorList)
            : this(message, null, errorList)
        {
        }

        public EntityException(string message, Exception inner, IList<TError> errorList)
            : base(message, inner)
        {
            if (errorList == null)
            {
                errorList = new List<TError>();
            }

            ErrorList = errorList;
        }

        public static string CreateErrorMessage(EntityException<TError, TEntityEnum, TEntityMethodEnum> exception)
        {
            if (exception == null)
            {
                return String.Empty;
            }

            var builder = new StringBuilder();
            foreach (var error in exception.ErrorList)
            {
                builder.AppendLine($"Entity: {error.Entity} Method: {error.Method} Message: {error.ErrorMessage}.");
            }

            return builder.ToString();
        }
    }
}
