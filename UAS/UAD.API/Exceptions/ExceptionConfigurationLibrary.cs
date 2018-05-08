using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Web.Http;
//using WebResponseException = System.Web.Http.HttpResponseException;

namespace UAD.API.Exceptions
{
    public static class ExceptionConfigurationLibrary
    {

        public static IEnumerable<ExceptionConfiguration> GetExceptionHandlers(System.Web.Http.ApiController controller = null)
        {

            //TODO: add special casing to merge in per-controller definitions

            return new ExceptionConfiguration[]
            {
                new ExceptionConfiguration<System.Web.Http.HttpResponseException>()
                {
                    FriendlyMessage = (e) => ((System.Web.Http.HttpResponseException)e).Response.ReasonPhrase,
                    StatusCodeHandler = (e) => ((System.Web.Http.HttpResponseException)e).Response.StatusCode
                },
                new ExceptionConfiguration<ECN_Framework_Common.Objects.ECNException>()
                {
                    FriendlyMessage = (e) => ECN_Framework_Common.Objects.ECNException.CreateErrorMessage(e as ECN_Framework_Common.Objects.ECNException),
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "400 Bad Request"
                },
                new ExceptionConfiguration<APIAccessKeyException>()
                {
                    FriendlyMessage = (e) => APIAccessKeyException.GetAPIAccessKeyExceptionMessage(e as APIAccessKeyException),
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "401 Unauthorized"
                },
                new ExceptionConfiguration<ECN_Framework_Common.Objects.SecurityException>()
                {
                    FriendlyMessage = (e) => "You are not authorized to access the selected resource.",
                    StatusCode = HttpStatusCode.Forbidden,
                    ErrorCode = "403 Forbidden"
                },
                new ExceptionConfiguration<APIResourceNotFoundException>()
                { 
                    FriendlyMessage = (e) => APIResourceNotFoundException.GetAPIResourceNotFoundExceptionMessage((APIResourceNotFoundException)e),
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorCode = "404 Not Found"                        
                },
                new ExceptionConfiguration<System.IO.FileNotFoundException>()
                {
                    FriendlyMessage = (e) => e.Message,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorCode = "404 Not Found"
                },
                new ExceptionConfiguration<ApplicationException>()
                { 
                    // TODO: move this message over to Strings.
                    FriendlyMessage = (e) => "An internal error has occurred.  If this problem persists please contact Knowledge Marketing.", 

                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    ErrorCode = "500 Internal Server Error"
                },
                 new ExceptionConfiguration<ImageException>()
                {
                    FriendlyMessage = (e) => ImageException.GetImageExceptionMessage(e as ImageException),
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "400 Bad Request"
                },
            };
        }
    }
}