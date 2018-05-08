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
    /// <summary>
    /// Wrapper around HttpResponseException(HttpStatusCode.NotFound) to simplify
    /// customization of error responses.  <see cref="ExceptionConfiguration"/>
    /// and <see cref="ExceptionConfigurationLibrary"/> 
    /// and <see cref="Attributes.FriendlyExceptionsAttribute"/>
    /// </summary>
    public class APIResourceNotFoundException : Exception
    {
        /// <summary>
        ///  Raises a 404/Not Found such that we'll reach the Exception Filter pipeline
        /// </summary>
        /// <param name="resource">names the type of resource we didn't fine (e.g. ControllerName)</param>
        /// <param name="id">ID value for the unmatched resource</param>
        /// <returns></returns>
        public static APIResourceNotFoundException Factory(string resource, int? id=null)
        {           
            var apiException = new APIResourceNotFoundException(resource, id);
            return apiException;
        }

        public static string GetAPIResourceNotFoundExceptionMessage(APIResourceNotFoundException apiResourceNotFoundException)
        {
            return Strings.Format(
                Strings.Errors.FriendlyMessages.RESOURCE_NOT_FOUND,
                apiResourceNotFoundException.Data[Strings.Errors.APIResourceNotFoundExceptionResourceKey],
                apiResourceNotFoundException.Data[Strings.Errors.APIResourceNotFoundExceptionIDKey]??"<unknown>"
            );
        }
        // Don't use this -- use the public static constructor
        private APIResourceNotFoundException(string resource, int? id = null)
            : base()
        {
            Data[Strings.Errors.APIResourceNotFoundExceptionResourceKey] = resource;
            Data[Strings.Errors.APIResourceNotFoundExceptionIDKey] = id;
        }

    }
}