using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using UAD.API;

namespace UAD.API.Exceptions
{
    public class APIAccessKeyException : HttpResponseException
    {
        public APIAccessKeyException(string message) : base(HttpStatusCode.Unauthorized)
        {
            this.Data[Strings.Errors.APIAccessKeyExceptionDataKey] = message;
        }

        public static string GetAPIAccessKeyExceptionMessage(APIAccessKeyException apiAccessKeyException)
        {
            return apiAccessKeyException.Data[Strings.Errors.APIAccessKeyExceptionDataKey].ToString();
        }
    }
}